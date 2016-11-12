﻿using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Cuke4Nuke.Core;
using UnityEngine;

// This is a component that knows how to speak Cucumber's 'Wire' Protocol

// Much of this is based on "Asynchronous Server Socket Example"
// https://msdn.microsoft.com/en-us/library/fx6588te(v=vs.110).aspx

public class CucumberWireServer : MonoBehaviour 
{
    public int port = 3901;
    public string[] assemblyPaths;

    public static ManualResetEvent awaitingConnection = new ManualResetEvent(false);


    // These are details about the client that is talking to our Wire service
    class WireClientState
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
        public IProcessor processor;
    }

    class ListenerThreadData
    {
        public int port;
        public IProcessor processor;
        public Socket initialListener = null;
    }

	void Start() 
    {
        var objectFactory = new ObjectFactory();
        var loader = new Loader(assemblyPaths, objectFactory);
        var processor = new Processor(loader, objectFactory);
        //var listener = new Listener(processor, port);

        Thread listeningThread = new Thread(StartListening);
        ListenerThreadData listenerThreadData = 
            new ListenerThreadData() { port = port, processor = processor };
        listeningThread.Start(listenerThreadData);

	}
	

    // Listen for a client to connect
    public static void StartListening(object input)
    {
        ListenerThreadData parameters = input as ListenerThreadData;

        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, parameters.port);

        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and listen for incoming connections.
        try {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true) 
            {
                awaitingConnection.Reset();

                Debug.Log("Waiting for wire client to connect.");

                parameters.initialListener = listener;
                listener.BeginAccept( 
                    new AsyncCallback(AcceptCallback),
                    parameters);

                // Wait until a connection is made before continuing.
                awaitingConnection.WaitOne();
            }

        } 
        catch (Exception e) 
        {
            Debug.Log(e.ToString());
        }
    }

    // Accepts the client connect and starts a worker listening to it
    public static void AcceptCallback(IAsyncResult ar) 
    {
        // Tell the listener thread it may continue listening for clients
        awaitingConnection.Set();

        // Get the socket that handles the client request.
        ListenerThreadData parameters = ar.AsyncState as ListenerThreadData;
        Socket listener = parameters.initialListener;
        Socket handler = listener.EndAccept(ar);
        Debug.LogFormat("Accepted connection from {0}", handler);

        WireClientState state = new WireClientState();
        state.workSocket = handler;
        state.processor = parameters.processor;
        state.workSocket.BeginReceive(
            state.buffer, 0, WireClientState.BufferSize, 0,
            new AsyncCallback(ReadCallback), state);

        // Create a thread to send back replies
        Thread responseThread = new Thread(ResponseThread);
        responseThread.Start(state);
    }

    public static void ReadCallback(IAsyncResult ar) 
    {
        // Retrieve the state object and the handler socket
        // from the asynchronous state object.
        WireClientState state = ar.AsyncState as WireClientState;
        Socket handler = state.workSocket;

        // Read data from the client socket. 
        int bytesRead = handler.EndReceive(ar);

        if (bytesRead > 0) 
        {
            // Store the data received so far
            state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

            // We receive one request at a time, and it ends with a newline
            // If we can read a full line of text, we have our request
            string message = new StreamReader(state.sb.ToString()).ReadLine();
            if (message != null)
            {
                // Send a message to the other thread
                state.processor.request.Message = message;
            }
        }
    }

    // We spawn up a thread to listen for replies
    // and then we pass them back over the network
    public static void ResponseThread(object stateInput)
    {
        WireClientState state = stateInput as WireClientState;
        IntraThreadMessage reply = state.processor.reply;
        Socket handler = state.workSocket;

        try
        {
            while (true)
            {
                // Wait for data
                reply.DataAvailable.WaitOne();
                byte[] byteData = Encoding.UTF8.GetBytes(reply.Message);
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
        }
        catch (Exception e) 
        {
            Debug.Log(e.ToString());
        }
    }

    private static void SendCallback(IAsyncResult ar) 
    {
        try 
        {
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = handler.EndSend(ar);
            Debug.LogFormat("Sent {0} bytes to client.", bytesSent);
        } 
        catch (Exception e) 
        {
            Debug.Log(e.ToString());
        }
    }
}
