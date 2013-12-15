using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    class Player
    {
        public byte Id;//Pelaajan tunnus välillä 1-MAX_PLAYERS
        public string Name = "";//Pelaajan nimimerkki
        public Vector2 Position; //Sijainti
        public short Angle;//Kulma
        public byte Weapon;//Käytössä oleva ase
        public int Health;//Terveys
        public short Kills;//Tappojen lukumäärä
        public short Deaths;//Kuolemien lukumäärä
        public float KillRatio;//Tapposuhde
        public bool IsDead;//Onko pelaaja kuollut
        public byte Zombie;//Onko tämä pelaaja botti
        public byte Team;//Joukkue
        public int Visible;//Ukon vilkuttaminen syntymän jälkeen


        public Player()
        {
            Position = Vector2.Zero;
        }

        public void Draw(GameTime gameTime, Map map)
        {
            map.parent.spriteBatch.Begin(SpriteSortMode.BackToFront, map.parent.GraphicsDevice.BlendStates.AlphaBlend, null, null, null, null, map.parent.Cam.get_transformation(map.parent.GraphicsDevice));
            Vector2 pos = map.GetTile(Position);

            if (Team == 2)
            {
                map.parent.spriteBatch.Draw(Textures.PlayerPistol2, pos, null, Color.White, DegreeToRadian(Angle), Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            else
            {
                map.parent.spriteBatch.Draw(Textures.PlayerPistol1, pos, null, Color.White, DegreeToRadian(Angle), Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            map.spriteBatch.DrawString(Textures.Arial16, this.Name, pos + new Vector2(0, Textures.PlayerPistol1.Height), Color.White);
            map.spriteBatch.End();
        }

        public void Update(GameTime gameTime, Map map)
        {
            if (map.parent.keyboardState.IsKeyDown(Keys.A))
            {
                Position.X -= 10;
            }

            if (map.parent.keyboardState.IsKeyDown(Keys.D))
            {
                Position.X += 10;
            }

            if (map.parent.keyboardState.IsKeyDown(Keys.W))
            {
                Position.Y -= 10;
            }

            if (map.parent.keyboardState.IsKeyDown(Keys.S))
            {
                Position.Y += 10;
            }
            //Camera.Location = Position;
            //map.parent.Cam.Pos = Position;
        }
        private float DegreeToRadian(short angle)
        {
            return (float)((Math.PI / 180) * (360 - angle));
        }

    }
}
