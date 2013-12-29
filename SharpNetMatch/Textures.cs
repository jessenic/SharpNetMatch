using BmFont;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    public static class Textures
    {
        public static Texture2D BazookaItem;
        public static Texture2D ChainsawItem;
        public static Texture2D HealthItem;
        public static Texture2D LauncherItem;
        public static Texture2D MachinegunItem;
        public static Texture2D ShotgunItem;

        public static Texture2D PlayerPistol1;
        public static Texture2D PlayerPistol2;

        public static BitmapFont Arial15;
        public static BitmapFont Impact30;

        public static void LoadContent(ContentManager Content)
        {
            BazookaItem = Content.Load<Texture2D>("bazooka_item");
            ChainsawItem = Content.Load<Texture2D>("chainsaw_item");
            HealthItem = Content.Load<Texture2D>("health_item");
            LauncherItem = Content.Load<Texture2D>("launcher_item");
            MachinegunItem = Content.Load<Texture2D>("machinegun_item");
            ShotgunItem = Content.Load<Texture2D>("shotgun_item");


            PlayerPistol1 = Content.Load<Texture2D>("player1");
            PlayerPistol2 = Content.Load<Texture2D>("player1_2");

            Arial15 = new BitmapFont("Arial_15", Content);
            Impact30 = new BitmapFont("Impact_30", Content);
        }
    }
    public class BitmapFont
    {
        public FontFile FFile { get; private set; }
        public Texture2D Texture { get; private set; }
        private Dictionary<char, FontChar> _characterMap;

        public BitmapFont(string FileName, ContentManager Content)
        {
            FFile = FontLoader.Load(Path.Combine(Content.RootDirectory, FileName + ".fnt"));
            Texture = Content.Load<Texture2D>(FileName + "_0.png");
            _characterMap = new Dictionary<char, FontChar>();

            foreach (var fontCharacter in FFile.Chars)
            {
                char c = (char)fontCharacter.ID;
                _characterMap.Add(c, fontCharacter);
            }
        }

        public void DrawText(SpriteBatch spriteBatch, string text, Vector2 pos, Color color)
        {
                foreach(char c in text)
                {
                        FontChar fc;
                        if(_characterMap.TryGetValue(c, out fc))
                        {
                                var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                                var position = pos + new Vector2(fc.XOffset,fc.YOffset);

                                spriteBatch.Draw(Texture, position, sourceRectangle, color);
                                pos.X += fc.XAdvance;
                        }
                }
        }
    }
}
