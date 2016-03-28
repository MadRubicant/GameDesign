using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Timers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Utilities;

using ExtensionMethods;

namespace ControlledSpheres.Graphics {
    class TextureManager {
        ContentManager Content;
        GraphicsDevice Graphics;
        private ConcurrentDictionary<string, Texture2D[]> TextureDict;
        private static ConcurrentBag<string> BadFilenames;
        Texture2D[] MagentaBlackErrorTexture;
        Texture2D[] _blankTexture;
        public Texture2D[] BlankTexture {
            get {
                if (_blankTexture != null)
                    return _blankTexture;
                else {
                    _blankTexture = getBlankTexture();
                    return _blankTexture;
                }
            }
            private set {
                _blankTexture = value;
            }
        }
        DirectoryInfo ContentDirInfo;
        ConcurrentQueue<string> RequestedTextures;
        Thread TextureLoadingThread;
        public volatile bool LoadingTextures;

        /// <summary>
        /// MainManager is for global access to textures for the current level, etc
        /// </summary>
        public static TextureManager MainManager;
        /// <summary>
        /// MiscManager is for global access to textures that need to stick around between levels
        /// It's for menus, loading screens, and the like
        /// </summary>
        public static TextureManager MiscManager;
        public TextureManager(ContentManager content, GraphicsDevice graphics) {
            Content = content;
            Graphics = graphics;
            TextureDict = new ConcurrentDictionary<string, Texture2D[]>();
            ContentDirInfo = new DirectoryInfo("Content" + Path.DirectorySeparatorChar);
            RequestedTextures = new ConcurrentQueue<string>();
            if (BadFilenames == null)
                BadFilenames = new ConcurrentBag<string>();
            TextureManager.MainManager = this;
        }

        /// <summary>
        /// Returns the <see cref="Texture2D"/> array associated with the passed Key
        /// </summary>
        /// <param name="Key">A <see cref="String"/> associated with an animation or sprite</param>
        /// <returns>The <see cref="Texture2D"/> associated with Key</returns>
        public Texture2D[] this[string Key] {
            get {
                // If it's loaded, take it
                if (TextureDict.ContainsKey(Key)) {
                    return TextureDict[Key];
                }
                // If it's not loaded but it hasn't been tried yet, have an error tex for now but I'll try loading it
                else if (!BadFilenames.Contains(Key)) {
                    requestTextureLoad(Key);
                    Console.WriteLine("Texture array named {0} requested, attempting to load from disk", Key);
                    return getMagentaBlackErrorTexture();
                }
                // Unloaded and we've tried loading it before
                else {
                    Console.WriteLine("Texture array named {0} requested and not found on disk", Key);
                    return getMagentaBlackErrorTexture();
                }
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

        private Texture2D[] getBlankTexture() {
            if (_blankTexture == null) {
                Texture2D tex = new Texture2D(Graphics, 1, 1);
                tex.SetData<Color>(new Color[] { Color.Transparent });
                return new Texture2D[] { tex };
            }
            else return _blankTexture;
        }
        public void requestTextureLoad(string TextureName) {
            if (!TextureDict.ContainsKey(TextureName))
                RequestedTextures.Enqueue(TextureName);
        }

        public void requestTextureLoad(IEnumerable<string> TextureNames) {
            foreach (string s in TextureNames) {
                requestTextureLoad(s);
            }
        }

        public void LoadAllTextures() {
            Console.WriteLine("Loading {0} textures on background thread", RequestedTextures.Count);
            bool DequeueSucceeded = true;
            while (DequeueSucceeded == true) {
                string texname;
                DequeueSucceeded = RequestedTextures.TryDequeue(out texname);
                if (DequeueSucceeded == true) {
                    var Tex = LoadSpecificTexture(texname);
                    TextureDict[texname] = Tex;
                }
            }
            LoadingTextures = false;
        }

        public void BeginLoadTextures() {
            if (LoadingTextures == false) {
                LoadingTextures = true;
                TextureLoadingThread = new Thread(new ThreadStart(this.LoadAllTextures));
                TextureLoadingThread.Start();
            }
        }

        public int WaitingRequestedTextures() {
            return RequestedTextures.Count;
        }

        private Texture2D[] LoadSpecificTexture(string name){
            char[] splitchar = {'.'};
            var FilenameList = ContentDirInfo.EnumerateFiles("Art" + Path.DirectorySeparatorChar + name + "*.xnb", SearchOption.AllDirectories).Select<FileInfo, string>(x => x.Name).Select<string, string>(
                x => x.Split(splitchar).First<string>());
            int TexCount = FilenameList.Count<string>();
            if (TexCount == 0) {
                // TODO add logging
                BadFilenames.Add(name);
                return getMagentaBlackErrorTexture();
            }
            if (TexCount == 1) {
                Texture2D Tex = Content.Load<Texture2D>("Art" + Path.DirectorySeparatorChar + FilenameList.First<string>());
                Texture2D[] texarray = new Texture2D[1];
                texarray[0] = Tex;
                return texarray;
            }
            Regex re = new Regex("[0-9]+");
            FilenameList = FilenameList.OrderBy<string, int>((x => int.Parse(re.Match(x).Value)));
            Texture2D[] TexArray = new Texture2D[TexCount];
            int i = 0;
            foreach (string s in FilenameList) {
                TexArray[i] = Content.Load<Texture2D>("Art" + Path.DirectorySeparatorChar + s);
                i++;
            }
            return TexArray;
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

        public void SplitLaserTexture() {
            Texture2D LaserSheet = TextureDict["BulletSprites"][0];
            Color[] LaserColorData = new Color[LaserSheet.Width * LaserSheet.Height];
            LaserSheet.GetData<Color>(LaserColorData);
            for (int i = 0; i < LaserColorData.Length; i++) {
                if (LaserColorData[i] == Color.Black)
                    LaserColorData[i] = Color.Transparent;
            }
            LaserSheet.SetData<Color>(LaserColorData);
            Texture2D[] LaserYellow = new Texture2D[4];
            LaserYellow[0] = LaserSheet.SubSprite(new Rectangle(11, 187, 8, 8));
            LaserYellow[1] = LaserSheet.SubSprite(new Rectangle(23, 187, 13, 8));
            LaserYellow[2] = LaserSheet.SubSprite(new Rectangle(40, 187, 19, 8));
            LaserYellow[3] = LaserSheet.SubSprite(new Rectangle(63, 187, 30, 8));
            TextureDict["LaserYellow"] = LaserYellow;
            Texture2D[] LaserGreen = new Texture2D[4];
            LaserGreen[0] = LaserSheet.SubSprite(new Rectangle(11, 205, 8, 8));
            LaserGreen[1] = LaserSheet.SubSprite(new Rectangle(23, 205, 13, 8));
            LaserGreen[2] = LaserSheet.SubSprite(new Rectangle(40, 205, 19, 8));
            LaserGreen[3] = LaserSheet.SubSprite(new Rectangle(63, 205, 30, 8));
            TextureDict["LaserGreen"] = LaserGreen;
            Texture2D[] LaserBlue = new Texture2D[4];
            LaserBlue[0] = LaserSheet.SubSprite(new Rectangle(11, 223, 8, 8));
            LaserBlue[1] = LaserSheet.SubSprite(new Rectangle(23, 223, 13, 8));
            LaserBlue[2] = LaserSheet.SubSprite(new Rectangle(40, 223, 19, 8));
            LaserBlue[3] = LaserSheet.SubSprite(new Rectangle(63, 223, 30, 8));
            TextureDict["LaserBlue"] = LaserBlue;
            Texture2D[] LaserPink = new Texture2D[4];
            LaserPink[0] = LaserSheet.SubSprite(new Rectangle(11, 241, 8, 8));
            LaserPink[1] = LaserSheet.SubSprite(new Rectangle(23, 241, 13, 8));
            LaserPink[2] = LaserSheet.SubSprite(new Rectangle(40, 241, 19, 8));
            LaserPink[3] = LaserSheet.SubSprite(new Rectangle(63, 241, 30, 8));
            TextureDict["LaserPink"] = LaserPink;
        }
    }
}
