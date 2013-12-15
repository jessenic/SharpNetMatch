using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    class Player
    {
        public int Id;//Pelaajan tunnus välillä 1-MAX_PLAYERS
        public string Name;//Pelaajan nimimerkki
        public Vector2 Position { get; set; } //Sijainti
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

        public Map map { get; set; }

        public Player(Map map)
        {
            Position = Vector2.Zero;
            this.map = map;
        }

        public void Draw(GameTime gameTime)
        {
            map.spriteBatch.Begin();
            map.spriteBatch.DrawString(map.parent.arial16Font, this.Name, Camera.Location - this.Position, Color.White);
            map.spriteBatch.End();
        }

    }
}
