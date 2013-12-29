using System;
using System.Text;
using Microsoft.Xna.Framework;


namespace SharpNetMatch
{
    // Use these namespaces here to override SharpDX.Direct3D11
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System.IO;
    using SharpCompress.Archive.Rar;
    using SharpCompress.Reader;
    using System.Diagnostics;
    using System.Net;
    using System.Collections.Generic;

    /// <summary>
    /// Simple SharpNetMatch game using Microsoft.Xna.Framework.
    /// </summary>
    public class SharpNetMatch : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        internal SpriteBatch spriteBatch;

        //internal KeyboardManager keyboard;
        internal KeyboardState keyboardState;

        //internal MouseManager mouse;
        internal MouseState mouseState;
        public Camera Cam;
        internal NmClient cbn = new NmClient();

        internal Map map;

        int prevMapCRC = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharpNetMatch" /> class.
        /// </summary>
        public SharpNetMatch()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Initialize input keyboard system
            //keyboard = new KeyboardManager(this);

            // Initialize input mouse system
            //mouse = new MouseManager(this);
        }

        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "SharpNetMatch";
            lastUpdate = new TimeSpan();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Instantiate a SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Textures.LoadContent(Content);
            Weapon.Load();

            cbn.InitClient("ci.dy.fi", 29929);
            cbn.Login();
            cbn.ClientReadInternal();
            LoadMap();
            Cam = new Camera(this);
            base.LoadContent();
        }
        public void LoadMap()
        {
            map = null;
            string file = Path.Combine(Content.RootDirectory, cbn.MapName + ".mpc");

            if (!File.Exists(file))
            {
                WebClient wc = new WebClient();
                wc.DownloadFile(new Uri(cbn.MapServerUrl + "/" + cbn.MapName + "/" + cbn.MapName + ".mpc"), file);
            }

            File.Copy(file, file + ".rar", true);
            using (var stream = File.Open(file + ".rar", FileMode.Open, FileAccess.Write))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.WriteByte(0x52);
                stream.WriteByte(0x61);
                stream.WriteByte(0x72);
                stream.Flush();
            }
            using (var arc = RarArchive.Open(file + ".rar"))
            {
                foreach (var f in arc.Entries)
                {

                    //using (var stream = f.OpenEntryStream())
                    //{
                    //    int read = 0;
                    //    byte[] buffer = new byte[(int)f.Size];
                    //    while (read < buffer.Length)
                    //    {
                    //        read += stream.Read(buffer, read, (int)f.Size - read);
                    //    }
                    //    Debugger.Break();
                    //}
                    using (var ffs = File.OpenWrite(Path.Combine(Content.RootDirectory, f.FilePath)))
                    {
                        f.WriteTo(ffs);
                        ffs.Flush();
                    }
                    if (Path.GetExtension(f.FilePath).ToLower().Equals(".pxi"))
                    {
                        DotPxi.ConvertToBitmap(Path.Combine(Content.RootDirectory, f.FilePath));
                    }
                }
            }

            map = new Map(this, cbn.MapName);
            map.LoadContent(Content);
            prevMapCRC = cbn.MapCRC;
        }
        internal TimeSpan lastUpdate;

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (map == null)
            {
                return;
            }
            if (prevMapCRC != cbn.MapCRC)
            {
                cbn.Logout();
                LoadMap();
                cbn.Login();
            }

            // Get the current state of the keyboard
            keyboardState = Keyboard.GetState();

            // Get the current state of the mouse
            mouseState = Mouse.GetState();
            foreach (var w in Weapon.WeaponList)
            {
                if (keyboardState.IsKeyDown(w.Value.Key))
                {
                    cbn.LocalPlayer.HeldWeapon = w.Key;
                }
            }

            map.Update(gameTime);
            cbn.LocalPlayer.Update(gameTime, map);
            Cam.Zoom = 1 + ((float)mouseState.ScrollWheelValue / 1200f);
            List<short> removeBullets = new List<short>();
            foreach (var b in cbn.Bullets)
            {
                if (b.Value.Remove)
                {
                    removeBullets.Add(b.Key);
                }
                else
                {
                    b.Value.Update(gameTime, map);
                }
            }
            foreach (var b in removeBullets)
            {
                cbn.Bullets.Remove(b);
            }
        }

        public Vector2 GetMouseXY()
        {
            return new Vector2(mouseState.X, mouseState.Y);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.Black);

            if (map == null)
            {
                spriteBatch.Begin();
                Textures.Impact30.DrawText(spriteBatch, "Loading map " + cbn.MapName, new Vector2(10, 10), Color.Yellow);
                spriteBatch.End();
                return;
            }
            map.Draw(gameTime);

            foreach (var i in cbn.Items.Values)
            {
                i.Draw(gameTime, map);
            }

            foreach (var b in cbn.Bullets.Values)
            {
                b.Draw(gameTime, map);
            }

            foreach (var p in cbn.Players.Values)
            {
                p.Draw(gameTime, map);
            }

            spriteBatch.Begin();
            spriteBatch.Draw(Textures.LauncherItem, GetMouseXY(), Color.White);
            spriteBatch.End();

            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------
            spriteBatch.Begin();
            Textures.Impact30.DrawText(spriteBatch, cbn.LocalPlayer.HeldWeapon.ToString(), new Vector2(10, 10), Color.Yellow);
            Textures.Arial15.DrawText(spriteBatch, "Time: " + cbn.TimePlayed + "/" + cbn.RoundLength, new Vector2(GraphicsDevice.Viewport.Width / 2, 10), Color.Yellow);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
