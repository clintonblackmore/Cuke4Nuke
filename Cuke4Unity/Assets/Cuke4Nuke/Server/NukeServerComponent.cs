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

    	// Use this for initialization
    	void Start () 
        {
            var objectFactory = new ObjectFactory();
            var loader = new Loader(objectFactory);
            var processor = new Processor(loader, objectFactory);
            var listener = new AsyncListener(processor, options.Port);
 
            new NukeServer(listener, options).Start();

            if (display != null)
            {
                display.text = string.Format("Listening on port {0} at \n{1}", options.Port, GetIPAddresses());
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