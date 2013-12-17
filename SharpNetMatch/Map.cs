using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    class Map
    {
        public readonly SharpNetMatch parent;
        public string mapname;


        private Texture2D Tilemap;
        private Texture2D Background;

        public Map(SharpNetMatch parent, string mapname)
        {
            this.parent = parent;
            this.mapname = mapname;
        }

        internal SpriteBatch spriteBatch { get { return parent.spriteBatch; } }
        GraphicsDevice GraphicsDevice { get { return parent.GraphicsDevice; } }

        internal int tileWidth;
        internal int tileHeight;
        internal int mapWidth;
        internal int mapHeight;

        byte[][,] map;

        public void LoadContent()
        {
            Tilemap = Texture2D.New(GraphicsDevice, Image.Load("Content\\" + mapname + ".pxi.bmp"));

            if (File.Exists("Content\\" + mapname + "_back.pxi.bmp"))
            {
                Background = Texture2D.New(GraphicsDevice, Image.Load("Content\\" + mapname + "_back.pxi.bmp"));
            }

            using (var fs = File.OpenRead("Content\\" + mapname + ".map"))
            {
                var w = fs.ReadByte();
                var h = fs.ReadByte();
                var tw = fs.ReadByte();
                var th = fs.ReadByte();
                mapWidth = w;
                mapHeight = h;
                tileWidth = tw;
                tileHeight = th > 0 ? th : tw;
                map = new byte[4][,];
                for (int i = 0; i < map.Length; i++)
                {
                    map[i] = new byte[w, h];
                }
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        for (byte l = 0; l < 4; l++)
                        {
                            map[l][x, y] = (byte)fs.ReadByte();
                        }
                    }
                }
                var maskR = fs.ReadByte();
                var maskG = fs.ReadByte();
                var maskB = fs.ReadByte();



                while (fs.CanRead)
                {
                    var animTile = fs.ReadByte();
                    if (animTile == 0)
                    {
                        break;
                    }
                    var animLength = fs.ReadByte();
                    var animDelay = fs.ReadByte();
                }
            }

            //file = OpenToRead(_fileName$ + ".map")
            //    o = 0
            //    w = ReadByte(file)
            //    h = ReadByte(file)
            //    tw = ReadByte(file) / 8
            //    th = ReadByte(file) / 8
            //    map = MakeMap(w, h, tw, th)
            //    For y = 1 To h
            //        For x = 1 To w
            //            For l = 0 To 3
            //                EditMap map, l, x, y, ReadByte(file)
            //            Next l
            //        Next x
            //    Next y

            //    img = LoadDotPxi(_fileName$ + ".pxi")
            //    ResizeImage img, ImageWidth(img)/8, ImageHeight(img)/8
            //    If img <> 0 Then
            //        PaintObject map, img
            //        DeleteImage img
            //    EndIf

            //    maskR = ReadByte(file)
            //    maskG = ReadByte(file)
            //    maskB = ReadByte(file)
            //    MaskObject map, maskR, maskG, maskB
            //    SetMap map, OFF, OFF

            //    While True
            //        animTile = ReadByte(file)
            //        If animTile = 0 Then Exit
            //        animLength = ReadByte(file)
            //        animDelay = ReadByte(file)
            //        SetTile animTile, animLength, animDelay
            //    Wend
            //CloseFile file
        }
        public void Draw(GameTime gameTime)
        {
            //int firstX = (int)(Camera.Location.X / tileWidth);
            //int firstY = (int)(Camera.Location.Y / tileHeight);

            //int offsetX = (int)(Camera.Location.X % tileWidth);
            //int offsetY = (int)(Camera.Location.Y % tileHeight);

            if (Background != null)
            {
                var fp = parent.Cam.ScreenPosInWorld(Vector2.Zero);
                var lp = parent.Cam.ScreenPosInWorld(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
                //tile -= GetTile(new Vector2(parent.GraphicsDevice.Viewport.Width * 0.5f, parent.GraphicsDevice.Viewport.Height * 0.5f));
                //spriteBatch.Begin(SpriteSortMode.FrontToBack, GraphicsDevice.BlendStates.Opaque, GraphicsDevice.SamplerStates.LinearWrap, GraphicsDevice.DepthStencilStates.Default, GraphicsDevice.RasterizerStates.CullNone);
                spriteBatch.Begin(SpriteSortMode.BackToFront, GraphicsDevice.BlendStates.AlphaBlend, null, null, null, null, parent.Cam.Transformation);
                for (int x = (int)fp.X / Background.Width - 1; x < (int)lp.X / Background.Width + 1; x++)
                {
                    for (int y = (int)fp.Y / Background.Height - 1; y < (int)lp.Y / Background.Height + 1 + 1; y++)
                    {
                        spriteBatch.Draw(Background, new Vector2((x * Background.Width), (y * Background.Height)), Color.White);
                    }
                }
                spriteBatch.End();
            }
            spriteBatch.Begin(SpriteSortMode.BackToFront, GraphicsDevice.BlendStates.AlphaBlend, null, null, null, null, parent.Cam.Transformation);
            for (byte l = 0; l < 2; l++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    for (int y = 0; y < mapHeight; y++)
                    {
                        if (map[l][x, y] > 0)
                        {
                            spriteBatch.Draw(
                                Tilemap, new Vector2((x * tileWidth), (y * tileHeight)),
                                GetSourceRectangle(map[l][x, y]),
                                Color.White);
                        }
                    }
                }
            }
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (parent.keyboardState.IsKeyDown(Keys.Left))
            {
                parent.Cam.Move(new Vector2(-5, 0));
            }

            if (parent.keyboardState.IsKeyDown(Keys.Right))
            {
                parent.Cam.Move(new Vector2(5, 0));
            }

            if (parent.keyboardState.IsKeyDown(Keys.Up))
            {
                parent.Cam.Move(new Vector2(0, -5));
            }

            if (parent.keyboardState.IsKeyDown(Keys.Down))
            {
                parent.Cam.Move(new Vector2(0, 5));
            }
        }

        public Rectangle GetSourceRectangle(int tileIndex)
        {
            tileIndex--;
            return new Rectangle(tileIndex % (Tilemap.Width / tileWidth) * tileWidth, tileIndex / (Tilemap.Width / tileWidth) * tileHeight, tileWidth, tileHeight);
        }

        public Vector2 GetTile(Vector2 pos)
        {
            pos.X = (float)(pos.X + 0.5 * mapWidth * tileWidth);
            pos.Y = (float)(-pos.Y + 0.5 * mapHeight * tileHeight);
            return pos;
        }

        public bool DoesHit(Rectangle source, Rectangle target)
        {
            return source.Intersects(target);
        }

        public bool IsWall(Vector2 pos)
        {
            int tileX = (int)Math.Ceiling((pos.X + this.mapWidth * this.tileWidth / 2) / this.tileWidth) - 1;
            int tileY = (int)Math.Ceiling((-pos.Y + this.mapHeight * this.tileHeight / 2) / this.tileHeight) - 1;


            if (tileX < 0 || tileX >= this.mapWidth || tileY < 0 || tileY >= this.mapHeight)
            {
                // Ollaan kartan ulkopuolella, eli törmätään.
                return true;
            }
            return map[2][tileX, tileY] == 1;
        }

        internal bool InMap(Vector2 Position)
        {
            Position.X = (float)Math.Ceiling((Position.X + this.mapWidth * this.tileWidth / 2) / this.tileWidth) - 1;
            Position.Y = (float)Math.Ceiling((-Position.Y + this.mapHeight * this.tileHeight / 2) / this.tileHeight) - 1;
            return Position.X > 0 && Position.X <= this.mapWidth && Position.Y > 0 && Position.Y <= this.mapHeight;
        }
    }
}
