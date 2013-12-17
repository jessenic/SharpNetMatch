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
    class Bullet
    {
        public WeaponType WeaponFrom;
        public bool Moved;
        public Vector2 Position;
        public Vector2 PrevPos;
        public int ShotTime;
        public bool Remove;
        public byte PlayerId;
        public short Angle;

        public Bullet()
        {
            Position = Vector2.Zero;
            PrevPos = Vector2.Zero;
            Angle = 0;
        }

        public void Update(GameTime gameTime, Map map)
        {
            PrevPos = Position;
            // Nopeus riippuu siitä millä aseella se on ammuttu
            var speed = Weapon.WeaponList[WeaponFrom].BulletSpeed;
            if (!Moved)
            {
                Moved = true;
                speed = 0;
            }
            // Jos ammus on ammuttu singolla niin tehdään savuvana
            if (WeaponFrom == WeaponType.Bazooka)
            {
                //If InScreen(ObjectX(bullet\obj), ObjectY(bullet\obj), 100) = True Then
                //    expl.EXPL_ANIMS = New(EXPL_ANIMS)
                //    expl\x      = ObjectX(bullet\obj)
                //    expl\y      = ObjectY(bullet\obj)
                //    expl\frame  = 0
                //    expl\tStamp = Timer()
                //    expl\frames = 16
                //    expl\img    = IMG_SMOKEANIM
                //    expl\w      = 20
                //    expl\h      = 20
                //EndIf
            }
            var RotationAngle = MathHelper.ToRadians(Angle);
            Vector2 direction = new Vector2((float)Math.Cos(RotationAngle),
                                    (float)Math.Sin(RotationAngle));
            direction.Normalize();
            Position += direction * speed;

            //UpdateGame2()
            //bx# = ObjectX(bullet\obj)
            //by# = ObjectY(bullet\obj)
            var hit = false;

            // Jos on ammuttu kranaatinlaukaisimella niin tutkitaan aikaviive
            if (WeaponFrom == WeaponType.Launcher)
            {
                if (ShotTime + 1000 < gameTime.TotalGameTime.Milliseconds)
                {
                    hit = true;
                }
            }

            // Osuiko seinään tai meniko kartalta ulos
            if (!hit && map.IsWall(Position) || !map.InMap(Position) || WeaponFrom == WeaponType.Chainsaw)
            {
                hit = true;
                //            bounce = False
                //            If bullet\weapon = WPN_LAUNCHER Then bounce = True
                // Osui seinään tai meni ulos
                //PlayGameSound(aWeapon(bullet\weapon, WPNF_HITSOUND), bx, by)
                //// Jos ammus oli singosta niin luodaan räjähdysanimaatio
                //If bullet\weapon = WPN_BAZOOKA Or bullet\weapon = WPN_LAUNCHER Then
                //    If InScreen(bx, by, 200) = True Then
                //        expl.EXPL_ANIMS = New(EXPL_ANIMS)
                //        expl\x      = bx
                //        expl\y      = by
                //        expl\frame  = 0
                //        expl\tStamp = Timer()
                //        expl\frames = 16
                //        expl\img    = IMG_EXPLOSION1
                //        expl\w      = 128
                //        expl\h      = 128
                //    EndIf
                //Else
                //    // Kipinöinti seinälle jos ammus on jostain muusta kuin singosta
                //    Sparking(bx, by)
                //EndIf
                //            If bounce = False Then hit = True
                // Kimmotus
                //            If bounce = True Then
                //                // Tutkitaan ammuksen suuntaa ja törmäyskulmaa ja niiden perusteella 
                //                // asetetaan uusi suunta koska ammus kimpoaa seinästä.
                //                a# = CollisionAngle(bullet\obj, 1)
                //                ba# = ObjectAngle(bullet\obj)
                //                vx# = -Cos(ba)
                //                vy# = Sin(ba)
                //                If a = 0 And vx > 0 Then vx = vx * -1
                //                If a = 180 And vx < 0 Then vx = vx * -1
                //                If a = 90 And vy < 0 Then vy = vy * -1
                //                If a = 270 And vy > 0 Then vy = vy * -1
                //                //If a = 0 Or a = 180 Then vx = vx * -1
                //                //If a = 90 Or a = 270 Then vy = vy * -1
                //                a = GetAngle(vx, vy, 0, 0)
                //                RotateObject bullet\obj, a
                //                MoveObject bullet\obj, 10
                //            EndIf
            }
            if (WeaponFrom == WeaponType.Chainsaw)
            {
                hit = true;
            }

            if (hit)
            {
                Remove = true;
            }

        }

        public void Draw(GameTime gameTime, Map map)
        {
            Vector2 pos = map.GetTile(Position);
            map.spriteBatch.Begin(SpriteSortMode.BackToFront, map.parent.GraphicsDevice.BlendStates.AlphaBlend, null, null, null, null, map.parent.Cam.Transformation);
            map.spriteBatch.DrawString(Textures.Arial16, PlayerId.ToString(), pos, Color.Yellow);
            map.spriteBatch.End();
        }
    }
}
