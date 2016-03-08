using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Microsoft.Xna.Framework;

namespace ControlledSpheres.IO {
    class LevelSetup {
        /* Ok, the way this is going to work is that I pull a file off of disk that describes the level
         * First it'll have a list of float pairs that describe the path of creeps through the level.
         * I'll implement more stuff later
         * 
         * 
         * 
         */
        StreamReader InputStream;
        Regex NumberSplitter = new Regex("\\b[0-9]+\\.?[0-9]*\\b");
        public LevelSetup(string path) {
            InputStream = new StreamReader(File.Open(path, FileMode.Open));
        }

        public void ChangeFile(string path) {
            InputStream.Close();
            InputStream = new StreamReader(File.Open(path, FileMode.Open));
        }

        public Vector2[] ReadPathData() {
            string RawLine = InputStream.ReadLine();
            Console.WriteLine(RawLine);
            MatchCollection RawNumbers = NumberSplitter.Matches(RawLine);
            float[] FloatArray = new float[RawNumbers.Count];
            int count = 0;
            foreach (Match number in RawNumbers) {
                FloatArray[count] = float.Parse(number.Value);
                count++;
            }

            Vector2[] VectorArray = new Vector2[RawNumbers.Count / 2];
            for (int i = 0; i < RawNumbers.Count; i += 2) {
                VectorArray[i / 2] = new Vector2(FloatArray[i], FloatArray[i + 1]);
            }

            return VectorArray;
        }
    }
}
