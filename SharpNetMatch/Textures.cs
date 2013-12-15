using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
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

        public static SpriteFont Arial16;

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

            // Loads a sprite font
            // The [Arial16.xml] file is defined with the build action [ToolkitFont] in the project
            Arial16 = Content.Load<SpriteFont>("Arial16");
        }
    }
}
