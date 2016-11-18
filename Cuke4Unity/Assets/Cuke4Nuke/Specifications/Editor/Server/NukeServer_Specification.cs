using System.IO;

using Cuke4Nuke.Core;
using Cuke4Nuke.Server;
using Cuke4Nuke.Specifications.Core;
using UnityEngine;

using NUnit.Framework;

namespace Cuke4Nuke.Specifications.Server
{
    [TestFixture]
    public class NukeServer_Specification
    {
        MockListener _listener;
        /*StringWriter _outputWriter;*/

        [SetUp]
        public void SetUp()
        {
            _listener = new MockListener();
            /*_outputWriter = new StringWriter();*/
        }

        [Test]
        public void Start_without_help_option_should_start_the_listener()
        {
            var server = new NukeServer(_listener, new Options());
            server.Start();

            Assert.That(_listener.HasMessageLoggedListeners());
            Assert.That(_listener.StartCalled);
            // Stop used to be called immediately on the async server,
            // but this caused it to not receive or respond to requests.
            // So, I've simplified this test to only check that start was called.
            //Assert.That(_listener.StopCalled);    
        }

        class MockListener : Listener
        {
            internal bool StartCalled;
            internal bool StopCalled;

            public MockListener()
                : base(new Processor(new Loader(new ObjectFactory()), null), 0)
            {
            }

            public override void Start()
            {
                StartCalled = true;
            }

            public override void Stop()
            {
                StopCalled = true;
            }

            internal bool HasMessageLoggedListeners()
            {
                return MessageLogged != null;
            }
        }
    }
}