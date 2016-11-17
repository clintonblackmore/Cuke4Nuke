using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;

namespace Cuke4Nuke.Server
{
    [Serializable]
    public class Options
    {
        public static readonly int DefaultPort = 3901;

        public int Port = 3901;
        //public ICollection<string> AssemblyPaths { get; set; }
    }
}