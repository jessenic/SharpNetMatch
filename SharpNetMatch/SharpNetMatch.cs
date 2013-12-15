using System;
using System.Text;
using SharpDX;


namespace SharpNetMatch
{
    // Use these namespaces here to override SharpDX.Direct3D11
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    using System.IO;
    using SharpCompress.Archive.Rar;
    using SharpCompress.Reader;
    using System.Diagnostics;

    /// <summary>
    /// Simple SharpNetMatch game using SharpDX.Toolkit.
    /// </summary>
    public class SharpNetMatch : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        internal SpriteBatch spriteBatch;
        internal SpriteFont arial16Font;

        internal KeyboardManager keyboard;
        internal KeyboardState keyboardState;

        internal MouseManager mouse;
        internal MouseState mouseState;
        NmClient cbn = new NmClient();

        Map map;

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
            keyboard = new KeyboardManager(this);

            // Initialize input mouse system
            mouse = new MouseManager(this);
        }

        protected override void Initialize()
        {
            // Modify the title of the window
            Window.Title = "SharpNetMatch";

            base.Initialize();
        }
        string tmp = "";
        protected override void LoadContent()
        {
            // Instantiate a SpriteBatch
            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            // Loads a sprite font
            // The [Arial16.xml] file is defined with the build action [ToolkitFont] in the project
            arial16Font = Content.Load<SpriteFont>("Arial16");

            cbn.InitClient("ci.dy.fi", 29929);
            cbn.Login();

            string file = @"Content\Luna.mpc";
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
                    tmp += f.FilePath + "\n";

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
                    using (var ffs = File.OpenWrite(Path.GetDirectoryName(file) + "\\" + f.FilePath))
                    {
                        f.WriteTo(ffs);
                        ffs.Flush();
                    }
                    if (Path.GetExtension(f.FilePath).ToLower().Equals(".pxi"))
                    {
                        DotPxi.ConvertToBitmap(Path.GetDirectoryName(file) + "\\" + f.FilePath);
                    }
                }
            }
            map = new Map(this, "Luna");
            map.LoadContent();
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Get the current state of the keyboard
            keyboardState = keyboard.GetState();

            // Get the current state of the mouse
            mouseState = mouse.GetState();

            map.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Use time in seconds directly
            var time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.Black);


            map.Draw(gameTime);


            // ------------------------------------------------------------------------
            // Draw the some 2d text
            // ------------------------------------------------------------------------
            spriteBatch.Begin();
            spriteBatch.DrawString(arial16Font, tmp, new Vector2(16, 16), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
