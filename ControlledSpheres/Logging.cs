using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Reflection;

namespace ControlledSpheres {
    class Logging {
        StreamWriter CurrentStream;
        StreamReader CurrentStreamReadView;
        string DirectoryPath = "Logs" + Path.DirectorySeparatorChar;
        string LogFileName = "DebugLog";
        HashSet<GameObject> LoggedObjects;
        public Logging() {
            CurrentStream = new StreamWriter(File.Open(DirectoryPath + LogFileName, FileMode.OpenOrCreate));
            CurrentStreamReadView = new StreamReader(CurrentStream.BaseStream);
        }

        public void DebugLog(GameObject Obj, string message = "") {
            Type ObjectType = Obj.GetType();
            
        }
    }
}
