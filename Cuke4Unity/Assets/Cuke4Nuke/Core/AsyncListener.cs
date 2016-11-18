using System.Threading;
using UnityEngine;

namespace Cuke4Nuke.Core
{
    public class AsyncListener : Listener
    {
        readonly Thread _thread;

        public int ReadTimeout { get; set; }

        public AsyncListener(IProcessor processor, int port)
            : base(processor, port)
        {
            _thread = new Thread(Run) { Name = "AsyncListener" };
        }

        public override void Start()
        {
            _thread.Start();
            Debug.LogFormat("Waiting one, on thread {0}", Thread.CurrentThread.Name);
            Started.WaitOne();
            Debug.Log("Waited one");
        }

        public override void Stop()
        {
            base.Stop();

            Log("Waiting for stopped event.");
            Stopped.WaitOne();

            Log("Waiting for listener thread to complete.");
            _thread.Join();
        }
    }
}