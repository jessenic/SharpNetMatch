
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public Vector2 NextPosition;
        public short Angle;//Kulma
        public WeaponType HeldWeapon;//Käytössä oleva ase
        public int Health;//Terveys
        public short Kills;//Tappojen lukumäärä
        public short Deaths;//Kuolemien lukumäärä
        public float KillRatio;//Tapposuhde
        public bool IsDead;//Onko pelaaja kuollut
        public byte Zombie;//Onko tämä pelaaja botti
        public byte Team;//Joukkue
        public int Visible;//Ukon vilkuttaminen syntymän jälkeen
        public byte IsProtected;

        public byte HasAmmo;


        public Player()
        {
            Position = Vector2.Zero;
            NextPosition = Vector2.Zero;
            HeldWeapon = WeaponType.Pistol;
            Angle = 0;
            Health = 100;
            Kills = 0;
            Deaths = 0;
            KillRatio = 0;
            IsDead = false;
            Zombie = 1;
            Team = 0;
            Visible = 0;
            IsProtected = 0;
            HasAmmo = 0;
        }

        public void Draw(GameTime gameTime, Map map)
        {
            map.parent.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, null, null, null, null, map.parent.Cam.Transformation);
            Vector2 pos = map.GetTile(Position);

            if (Team == 2)
            {
                map.parent.spriteBatch.Draw(Textures.PlayerPistol2, pos, null, Color.White, DegreeToRadian(Angle), new Vector2(Textures.PlayerPistol2.Width / 2, Textures.PlayerPistol2.Height / 2), 1, SpriteEffects.None, 0);
            }
            else
            {
                map.parent.spriteBatch.Draw(Textures.PlayerPistol1, pos, null, Color.White, DegreeToRadian(Angle), new Vector2(Textures.PlayerPistol1.Width / 2, Textures.PlayerPistol1.Height / 2), 1, SpriteEffects.None, 0);
            }
            Textures.Arial15.DrawText(map.spriteBatch, this.Name + " " + Health, pos + new Vector2(0, Textures.PlayerPistol1.Height), Color.Yellow);
            map.spriteBatch.End();
        }

        private bool lastPressed = false;

        public void Update(GameTime gameTime, Map map)
        {
            if (Vector2.Distance(Position, NextPosition) > 20)
            {
                Position = NextPosition;
            }
            NextPosition = Position;
            byte pickedItem = 0;
            if (Health > 0)
            {
                var newPos = Position;

                if (map.parent.keyboardState.IsKeyDown(Keys.A))
                {
                    newPos.X -= 4;
                }

                if (map.parent.keyboardState.IsKeyDown(Keys.D))
                {
                    newPos.X += 4;
                }

                if (map.parent.keyboardState.IsKeyDown(Keys.W))
                {
                    newPos.Y += 4;
                }

                if (map.parent.keyboardState.IsKeyDown(Keys.S))
                {
                    newPos.Y -= 4;
                }
                if (!map.IsWall(newPos))
                {
                    Position = newPos;
                }
                Vector2 pos = map.parent.Cam.MouseCursorInWorld - map.GetTile(Position);
                this.Angle = RadianToDegree(Math.Atan2(pos.Y, pos.X));
                while (Angle > 360)
                {
                    Angle -= 360;
                }
                //Camera.Location = Position;
                map.parent.Cam.Pos = map.GetTile(Position);
                HasAmmo = 1;

                foreach (var i in map.parent.cbn.Items.Values)
                {
                    if (this.Bounding.Intersects(i.Bounding))
                    {
                        pickedItem = i.Id;
                        break;
                    }
                }

                if (map.parent.mouseState.LeftButton == ButtonState.Pressed && !lastPressed)
                {
                    Weapon w = Weapon.WeaponList[(WeaponType)HeldWeapon];
                    // Tarkastetaan, onko panoksia
                    //ammoExists = False
                    //If aWeapon(player\weapon, WPNF_AMMO) > 0 Then ammoExists = True
                    //// Tutkitaan voidaanko lähettää tieto ampumisesta
                    //shootNow = 0
                    //If (MouseDown(1) = True Or KeyDown(cbKeySpace) = True) And (player\weapon<>WPN_PISTOL Or gNotShotYet = True) And gConsoleMode = False And gDevConsole = False And gSessionComplete = False And player\spawnTime = 0 And player\team <> 0 Then
                    //    // Onko ase latingissa
                    //    If player\lastShoot + aWeapon(player\weapon, WPNF_RELOADTIME) < Timer() And player\health > 0 Then
                    //        // Onko ammuksia
                    //        If aWeapon(player\weapon, WPNF_AMMO) > 0 Or aWeapon(player\weapon, WPNF_AMMO_MAX) = 0 Then
                    //            // Latingissa on, vähennetään ammuksia
                    //            aWeapon(player\weapon, WPNF_AMMO) = aWeapon(player\weapon, WPNF_AMMO) - 1
                    //            aWeaponAmmos(player\weapon) = aWeapon(player\weapon, WPNF_AMMO) * 3
                    //            player\lastShoot = Timer()
                    //            shootNow = True
                    //        Else
                    //            PlayGameSound(SND_EMPTY, ObjectX(player\obj), ObjectY(player\obj))
                    //            player\lastShoot = Timer()
                    //        EndIf
                    //        gNotShotYet = False // Ammuttiin jo. Pistoolit eivät ammu ennen kuin klikkaa uudestaan
                    //    EndIf
                    //EndIf

                }
            }
            lastPressed = map.parent.mouseState.LeftButton == ButtonState.Pressed;

            if (gameTime.TotalGameTime - map.parent.lastUpdate > TimeSpan.FromMilliseconds(100))
            {
                map.parent.cbn.UpdatePlayer(lastPressed ? (byte)1 : (byte)0, pickedItem);
                map.parent.lastUpdate = gameTime.TotalGameTime;
            }
        }
        public static float DegreeToRadian(short angle)
        {
            return (float)((Math.PI / 180.0) * (360 - angle));
        }
        public static short RadianToDegree(double angle)
        {
            return (short)(360 - (180.0 / Math.PI * angle));
        }

        public Rectangle Bounding
        {
            get
            {
                return new Rectangle((int)(Position.X), (int)(Position.Y), Textures.PlayerPistol1.Width, Textures.PlayerPistol1.Height);
            }
        }
    }
}
