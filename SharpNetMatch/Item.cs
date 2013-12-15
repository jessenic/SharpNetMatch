using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
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
        public int Id;
        public byte ItemType;
        public Vector2 Position;
        public void Draw(GameTime gameTime, Map map)
        {
            map.parent.spriteBatch.Begin(SpriteSortMode.BackToFront, map.parent.GraphicsDevice.BlendStates.AlphaBlend, null, null, null, null, map.parent.Cam.get_transformation(map.parent.GraphicsDevice));
            switch ((ItemType)ItemType)
            {
                case global::SharpNetMatch.ItemType.Rocket:
                    map.parent.spriteBatch.Draw(Textures.BazookaItem, map.GetTile(Position), Color.White);
                    break;
                case global::SharpNetMatch.ItemType.Fuel:
                    map.parent.spriteBatch.Draw(Textures.ChainsawItem, map.GetTile(Position), Color.White);
                    break;
                case global::SharpNetMatch.ItemType.Healthpack:
                    map.parent.spriteBatch.Draw(Textures.HealthItem, map.GetTile(Position), Color.White);
                    break;
                case global::SharpNetMatch.ItemType.Launcher:
                    map.parent.spriteBatch.Draw(Textures.LauncherItem, map.GetTile(Position), Color.White);
                    break;
                case global::SharpNetMatch.ItemType.Ammo:
                    map.parent.spriteBatch.Draw(Textures.MachinegunItem, map.GetTile(Position), Color.White);
                    break;
                case global::SharpNetMatch.ItemType.Shotgun:
                    map.parent.spriteBatch.Draw(Textures.ShotgunItem, map.GetTile(Position), Color.White);
                    break;
                default:
                    map.parent.spriteBatch.DrawString(Textures.Arial16, ((ItemType)ItemType).ToString(), map.GetTile(Position), Color.White);
                    break;
            }
            map.parent.spriteBatch.End();
        }
    }
}
