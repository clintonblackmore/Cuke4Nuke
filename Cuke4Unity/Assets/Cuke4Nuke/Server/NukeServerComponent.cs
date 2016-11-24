using UnityEngine;
using System;
using System.Collections;

using Cuke4Nuke.Core;
using UnityEngine.UI;
using System.Net;
using System.Text;

namespace Cuke4Nuke.Server
{
   public class NukeServerComponent : MonoBehaviour 
   {
        public Cuke4Nuke.Server.Options options;
        public Text display;
        NukeServer server;
        int timesLoaded = 0;

    	// Use this for initialization
    	void OnEnable() 
        {
            Debug.Log("OnEnable called");

            var objectFactory = new ObjectFactory();
            var loader = new Loader(objectFactory);
            var processor = new Processor(loader, objectFactory);
            var listener = new AsyncListener(processor, options.Port);
 
            server = new NukeServer(listener);
            server.Start();

            if (display != null)
            {
                string timesCompiledMessage = "";
                if (timesLoaded == 1)
                {
                    timesCompiledMessage = string.Format("Recompiled once");
                } 
                else if (timesLoaded > 1)
                {
                    timesCompiledMessage = string.Format("Recompiled {0} times", timesLoaded);
                }

                display.text = string.Format("Listening on port {0} at \n{1}\n\n{2}", options.Port, GetIPAddresses(), timesCompiledMessage);
            }

            timesLoaded++;
    	}

        void OnDisable()
        {
            Debug.Log("OnDisable called");
            server.Stop();
            if (display != null)
            {
                display.text = string.Format("Server stopped.");
            }
        }

        private string GetIPAddresses()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return "Not connected";
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            StringBuilder sb = new StringBuilder();
            foreach (var ip in host.AddressList)
            {
                sb.AppendFormat("{0} {1}\n", ip, ip.AddressFamily);
            }
            return sb.ToString();
        }
    }
}