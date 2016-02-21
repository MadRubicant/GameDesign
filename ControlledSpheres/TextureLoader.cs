using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Utilities;

namespace ControlledSpheres.IO {
    class TextureManager {
        ContentManager Content;
        GraphicsDevice Graphics;
        private Dictionary<string, Texture2D[]> TextureDict;
        private static HashSet<string> CachedFilenames;
        Texture2D[] MagentaBlackErrorTexture;
        DirectoryInfo ContentDirInfo;

        public TextureManager(ContentManager content, GraphicsDevice graphics) {
            Content = content;
            Graphics = graphics;
            TextureDict = new Dictionary<string, Texture2D[]>();
            ContentDirInfo = new DirectoryInfo("Content" + Path.DirectorySeparatorChar);

            if (CachedFilenames == null) 
                EnumerateTextureDirectory();
        }

        /// <summary>
        /// Returns the <see cref="Texture2D"/> array associated with the passed Key
        /// </summary>
        /// <param name="Key">A <see cref="String"/> associated with an animation or sprite</param>
        /// <returns>The <see cref="Texture2D"/> associated with Key</returns>
        public Texture2D[] this[string Key] {
            get {
                if (TextureDict.ContainsKey(Key)) {
                    return TextureDict[Key];
                }
                else if (CachedFilenames.Contains(Key)) {
                    return null;
                }
                else return getMagentaBlackErrorTexture();
            }
        }

        private Texture2D[] getMagentaBlackErrorTexture() {
            if (MagentaBlackErrorTexture == null) {
                Texture2D tex = new Texture2D(Graphics, 64, 64);
                MagentaBlackErrorTexture = new Texture2D[1];
                Color[] ColorData = new Color[64 * 64];
                bool Black = false;
                for (int x = 0; x < 64; x++) {
                    for (int y = 0; y < 64; y++) {
                        if (Black == true)
                            ColorData[x * 64 + y] = Color.Black;
                        else 
                            ColorData[x * 64 + y] = Color.Magenta;
                        if (y % 8 == 0) {
                            Black = !Black;
                        }
                    }
                    if (x % 8 == 0)
                        Black = !Black;
                }

                tex.SetData<Color>(ColorData);
                MagentaBlackErrorTexture[0] = tex;
            }
            return MagentaBlackErrorTexture;
        }

        public void LoadAllTextures() {
            // Get the list of files in Content/Art, then split out the Content/Art part and drop the .xnb
            foreach (string s in CachedFilenames) {
                Content.Load<Texture2D>("Art" + Path.DirectorySeparatorChar + s);
                Console.WriteLine(s);
                
            }

        }

        public void LoadSpecificTexture(string name) {
            char[] splitchar = {'.'};
            var FilenameList = ContentDirInfo.EnumerateFiles("Art" + Path.DirectorySeparatorChar + name + "*.xnb", SearchOption.AllDirectories).Select<FileInfo, string>(x => x.Name).Select<string, string>(
                x => x.Split(splitchar).First<string>());
            Regex re = new Regex("[0-9]+");
            FilenameList = FilenameList.OrderBy<string, int>((x => int.Parse(re.Match(x).Value)));
            foreach (var s in FilenameList) {
                Console.WriteLine(s);
            }
            int TexCount = FilenameList.Count<string>();
            Texture2D[] TexArray = new Texture2D[TexCount];
            int i = 0;
            foreach (string s in FilenameList) {
                TexArray[i] = Content.Load<Texture2D>("Art" + Path.DirectorySeparatorChar + s);
                i++;
            }
            TextureDict.Add(name, TexArray);
        }

        public void WriteStandardizedTextures() {
            FileStream WriteStream;
            string path = "Uncompressed" + System.IO.Path.DirectorySeparatorChar + "Content" + System.IO.Path.DirectorySeparatorChar + "Art" + System.IO.Path.DirectorySeparatorChar;
            foreach (var NameTexturePair in TextureDict) {
                Texture2D[] TextureArray = NameTexturePair.Value;
                string textureName = NameTexturePair.Key;
                for (int i = 0; i < TextureArray.Length; i++) {
                    string filename = path + textureName + i.ToString() + ".png";
                    WriteStream = File.OpenWrite(filename);
                    TextureArray[i].SaveAsPng(WriteStream, TextureArray[i].Width, TextureArray[i].Height);
                }
            }
        }

        private void EnumerateTextureDirectory() {
            string[] FilenameList = Directory.EnumerateFiles("Content" + Path.DirectorySeparatorChar + "Art", "*.xnb", SearchOption.AllDirectories).ToArray<string>();
            FilenameList = FilenameList.Select<string, string>(x => x.Split(Path.DirectorySeparatorChar).Last<string>()).Select<string, string>(x => x.Split('.').First<string>()).ToArray<string>();
            FilenameList.OrderBy<string, string>(x => x);
            CachedFilenames = new HashSet<string>(FilenameList);
            foreach (string s in CachedFilenames) {
                //Console.WriteLine(s);
            }
        }
    }
}
