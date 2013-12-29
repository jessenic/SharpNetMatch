
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    class Item
    {
        public Item()
        {
            this.Position = Vector2.Zero;
        }
        public byte Id;
        public ItemType Type;
        public Vector2 Position;
        public void Draw(GameTime gameTime, Map map)
        {
            map.parent.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, null, null, null, null, map.parent.Cam.Transformation);
            switch (Type)
            {
                case ItemType.Rocket:
                    map.parent.spriteBatch.Draw(Textures.BazookaItem, map.GetTile(Position), Color.White);
                    break;
                case ItemType.Fuel:
                    map.parent.spriteBatch.Draw(Textures.ChainsawItem, map.GetTile(Position), Color.White);
                    break;
                case ItemType.Healthpack:
                    map.parent.spriteBatch.Draw(Textures.HealthItem, map.GetTile(Position), Color.White);
                    break;
                case ItemType.Launcher:
                    map.parent.spriteBatch.Draw(Textures.LauncherItem, map.GetTile(Position), Color.White);
                    break;
                case ItemType.Ammo:
                    map.parent.spriteBatch.Draw(Textures.MachinegunItem, map.GetTile(Position), Color.White);
                    break;
                case ItemType.Shotgun:
                    map.parent.spriteBatch.Draw(Textures.ShotgunItem, map.GetTile(Position), Color.White);
                    break;
                default:
                    Textures.Arial15.DrawText(map.spriteBatch, Type.ToString(), map.GetTile(Position), Color.White);
                    break;
            }
            map.parent.spriteBatch.End();
        }

        public Rectangle Bounding
        {
            get
            {
                int w = 0;
                int h = 0;
                switch (Type)
                {
                    case ItemType.Rocket:
                        w = Textures.BazookaItem.Width;
                        h = Textures.BazookaItem.Height;
                        break;
                    case ItemType.Fuel:
                        w = Textures.ChainsawItem.Width;
                        h = Textures.ChainsawItem.Height;
                        break;
                    case ItemType.Healthpack:
                        w = Textures.HealthItem.Width;
                        h = Textures.HealthItem.Height;
                        break;
                    case ItemType.Launcher:
                        w = Textures.LauncherItem.Width;
                        h = Textures.LauncherItem.Height;
                        break;
                    case ItemType.Ammo:
                        w = Textures.MachinegunItem.Width;
                        h = Textures.MachinegunItem.Height;
                        break;
                    case ItemType.Shotgun:
                        w = Textures.ShotgunItem.Width;
                        h = Textures.ShotgunItem.Height;
                        break;
                }
                return new Rectangle((int)(Position.X), (int)(Position.Y), w, h);
            }

        }
    }
}
