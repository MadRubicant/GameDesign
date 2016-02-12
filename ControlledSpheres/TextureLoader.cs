using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Utilities;

namespace ControlledSpheres {
    public enum TextureNames {
        Debug,
        ExplosionOneYellow, ExplosionOneOrange, ExplosionOneGreen, ExplosionOneRed, ExplosionOneBlue,
        ExplosionTwoYellow, ExplosionTwoGreen, ExplosionTwoRed, ExplosionTwoBlue,
        ExplosionThreeOrangeLeft, ExplosionThreeOrangeRight, ExplosionThreeOrangeUp, ExplosionThreeOrangeDown,
        ExplosionThreeGreenLeft, ExplosionThreeGreenRight, ExplosionThreeGreenUp, ExplosionThreeGreenDown,
        ExplosionThreeRedLeft, ExplosionThreeRedRight, ExplosionThreeRedUp, ExplosionThreeRedDown,
        ExplosionThreeBlueLeft, ExplosionThreeBlueRight, ExplosionThreeBlueUp, ExplosionThreeBlueDown,
    }

    class TextureManager {
        ContentManager Content;
        GraphicsDevice Graphics;
        public static Dictionary<TextureNames, Texture2D[]> TextureAtlas = new Dictionary<TextureNames, Texture2D[]>();

        public TextureManager(ContentManager content, GraphicsDevice graphics) {
            Content = content;
            Graphics = graphics;
        }

        // Until I standardize all my textures, I'll just have to hardcode each one
        // blech
        public void LoadExplosion1() {
            Texture2D tex = Content.Load<Texture2D>("explosionTilesheet1");
            Color[] ColorData = new Color[tex.Width * tex.Height];
            tex.GetData<Color>(ColorData);
            for (int i = 0; i < ColorData.Length; i++) {
                if (ColorData[i] == Color.Black) {
                    ColorData[i] = Color.Transparent;
                }
            }
            tex.SetData<Color>(ColorData);


            Texture2D[][] Explosions = new Texture2D[5][];
            for (int j = 0; j < 5; j++) {
                Explosions[j] = new Texture2D[14];
                for (int i = 0; i < 14; i++) {
                    Texture2D Frame = new Texture2D(Graphics, 30, 30);
                    ColorData = new Color[30 * 30];
                    Rectangle FramePosition = new Rectangle(22 + 31 * i, 28 + 39 * j , 30, 30);
                    tex.GetData<Color>(0, FramePosition, ColorData, 0, 30 * 30);
                    Frame.SetData<Color>(ColorData);

                    Explosions[j][i] = Frame;
                }
            }
            TextureAtlas.Add(TextureNames.ExplosionOneYellow, Explosions[0]);
            TextureAtlas.Add(TextureNames.ExplosionOneOrange, Explosions[1]);
            TextureAtlas.Add(TextureNames.ExplosionOneGreen, Explosions[2]);
            TextureAtlas.Add(TextureNames.ExplosionOneRed, Explosions[3]);
            TextureAtlas.Add(TextureNames.ExplosionOneBlue, Explosions[4]);
            // Time to move to the second set

            Explosions = new Texture2D[4][];
            for (int i = 0; i < 4; i++) {
                Explosions[i] = new Texture2D[8];
                for (int j = 0; j < 8; j++) {
                    Texture2D Frame = new Texture2D(Graphics, 30, 30);
                    ColorData = new Color[30 * 30];
                    Rectangle FramePosition = new Rectangle(22 + 31 * j, 238 + 36 * i, 30, 30);
                    tex.GetData<Color>(0, FramePosition, ColorData, 0, 30 * 30);
                    Frame.SetData<Color>(ColorData);
                    Explosions[i][j] = Frame;
                }
            }
            TextureAtlas.Add(TextureNames.ExplosionTwoYellow, Explosions[0]);
            TextureAtlas.Add(TextureNames.ExplosionTwoGreen, Explosions[1]);
            TextureAtlas.Add(TextureNames.ExplosionTwoRed, Explosions[2]);
            TextureAtlas.Add(TextureNames.ExplosionTwoBlue, Explosions[3]);

            Explosions = new Texture2D[4][];
            for (int i = 0; i < 4; i++) {
                Explosions[i] = new Texture2D[6];
                for (int j = 0; j < 6; j++) {
                    Texture2D Frame = new Texture2D(Graphics, 30, 30);
                    ColorData = new Color[30 * 30];
                    Rectangle FramePosition = new Rectangle(22 + 31 * j, 400 + 37 * i, 30, 30);
                    tex.GetData<Color>(0, FramePosition, ColorData, 0, 30 * 30);
                    Frame.SetData<Color>(ColorData);
                    Explosions[i][j] = Frame;
                }
            }

            TextureAtlas.Add(TextureNames.ExplosionThreeOrangeLeft, Explosions[0]);
            TextureAtlas.Add(TextureNames.ExplosionThreeOrangeRight, Explosions[1]);
            TextureAtlas.Add(TextureNames.ExplosionThreeOrangeUp, Explosions[2]);
            TextureAtlas.Add(TextureNames.ExplosionThreeOrangeDown, Explosions[3]);

            Explosions = new Texture2D[4][];
            for (int i = 0; i < 4; i++) {
                Explosions[i] = new Texture2D[6];
                for (int j = 0; j < 6; j++) {
                    Texture2D Frame = new Texture2D(Graphics, 30, 30);
                    ColorData = new Color[30 * 30];
                    Rectangle FramePosition = new Rectangle(225 + 31 * j, 400 + 37 * i, 30, 30);
                    tex.GetData<Color>(0, FramePosition, ColorData, 0, 30 * 30);
                    Frame.SetData<Color>(ColorData);
                    Explosions[i][j] = Frame;
                }
            }

            TextureAtlas.Add(TextureNames.ExplosionThreeGreenLeft, Explosions[0]);
            TextureAtlas.Add(TextureNames.ExplosionThreeGreenRight, Explosions[1]);
            TextureAtlas.Add(TextureNames.ExplosionThreeGreenUp, Explosions[2]);
            TextureAtlas.Add(TextureNames.ExplosionThreeGreenDown, Explosions[3]);

            Explosions = new Texture2D[4][];
            for (int i = 0; i < 4; i++) {
                Explosions[i] = new Texture2D[6];
                for (int j = 0; j < 6; j++) {
                    Texture2D Frame = new Texture2D(Graphics, 30, 30);
                    ColorData = new Color[30 * 30];
                    Rectangle FramePosition = new Rectangle(22 + 31 * j, 559 + 37 * i, 30, 30);
                    tex.GetData<Color>(0, FramePosition, ColorData, 0, 30 * 30);
                    Frame.SetData<Color>(ColorData);
                    Explosions[i][j] = Frame;
                }
            }

            TextureAtlas.Add(TextureNames.ExplosionThreeRedLeft, Explosions[0]);
            TextureAtlas.Add(TextureNames.ExplosionThreeRedRight, Explosions[1]);
            TextureAtlas.Add(TextureNames.ExplosionThreeRedUp, Explosions[2]);
            TextureAtlas.Add(TextureNames.ExplosionThreeRedDown, Explosions[3]);

            Explosions = new Texture2D[4][];
            for (int i = 0; i < 4; i++) {
                Explosions[i] = new Texture2D[6];
                for (int j = 0; j < 6; j++) {
                    Texture2D Frame = new Texture2D(Graphics, 30, 30);
                    ColorData = new Color[30 * 30];
                    Rectangle FramePosition = new Rectangle(225 + 31 * j, 559 + 37 * i, 30, 30);
                    tex.GetData<Color>(0, FramePosition, ColorData, 0, 30 * 30);
                    Frame.SetData<Color>(ColorData);
                    Explosions[i][j] = Frame;
                }
            }

            TextureAtlas.Add(TextureNames.ExplosionThreeBlueLeft, Explosions[0]);
            TextureAtlas.Add(TextureNames.ExplosionThreeBlueRight, Explosions[1]);
            TextureAtlas.Add(TextureNames.ExplosionThreeBlueUp, Explosions[2]);
            TextureAtlas.Add(TextureNames.ExplosionThreeBlueDown, Explosions[3]);
            Texture2D[] te = new Texture2D[1];
            
            te[0] = tex;
            TextureAtlas.Add(TextureNames.Debug, te);
            Point Base = new Point(22, 28);

        }

        public void WriteStandardizedTextures() {
            FileStream WriteStream;
            
            string path = "Content" + System.IO.Path.DirectorySeparatorChar + "Art" + System.IO.Path.DirectorySeparatorChar;
            foreach (var NameTexturePair in TextureAtlas) {
                Texture2D[] TextureArray = NameTexturePair.Value;
                string textureName = Enum.GetName(typeof(TextureNames), NameTexturePair.Key);
                for (int i = 0; i < TextureArray.Length; i++) {
                    string filename = path + textureName + i.ToString() + ".png";
                    WriteStream = File.OpenWrite(filename);
                    TextureArray[i].SaveAsPng(WriteStream, TextureArray[i].Width, TextureArray[i].Height);
                }
            }
        }
        
    }
}
