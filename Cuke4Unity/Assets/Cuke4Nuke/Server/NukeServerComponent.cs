using UnityEngine;
using System;
using System.Collections;

using Cuke4Nuke.Core;

namespace Cuke4Nuke.Server
{
   public class NukeServerComponent : MonoBehaviour 
   {
        public Cuke4Nuke.Server.Options options;

    	// Use this for initialization
    	void Start () 
        {
            var objectFactory = new ObjectFactory();
            var loader = new Loader(objectFactory);
            var processor = new Processor(loader, objectFactory);
            var listener = new Listener(processor, options.Port);
 
            new NukeServer(listener, options).Start();
    	}
    }

}