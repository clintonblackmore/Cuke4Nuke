using System;
using System.IO;
using UnityEngine;

using Cuke4Nuke.Core;

namespace Cuke4Nuke.Server
{
    public class NukeServer
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Listener _listener;

        public NukeServer(Listener listener)
        {
            _listener = listener;
        }

        public void Start()
        {
            Run();
        }

        public void Stop()
        {
            _listener.MessageLogged -= listener_LogMessage;
            try
            {
                _listener.Stop();
            }
            catch (Exception e)
            {
                string message = "Unable to gracefully stop listener. Exception:\n\n" + e.Message;
                Debug.LogError(message);
            }
        }

        void Run()
        {
            _listener.MessageLogged += listener_LogMessage;
            try
            {
                _listener.Start();
            }
            catch (Exception ex)
            {
                string message = "Unable to start listener. Exception:\n\n" + ex.Message;
                Debug.LogError(message);
            }
        }

        void listener_LogMessage(object sender, LogEventArgs e)
        {
            Debug.Log(e.Message);
        }

        void Log(string message)
        {
            Debug.Log(message);
        }
    }
}