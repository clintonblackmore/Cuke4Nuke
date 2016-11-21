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
            Started.WaitOne();
        }

        public override void Stop()
        {
            base.Stop();

            Stopped.WaitOne();
            _thread.Join();
        }
    }
}