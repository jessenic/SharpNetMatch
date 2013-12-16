using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    public class Weapon
    {
        //TODO: Change to right types and remove unused!
        //--------------------------------------------------------------------------------
        // Aseiden ominaisuudet
        //--------------------------------------------------------------------------------
        public int Character;     // Pelihahmon objekti
        public int ReloadTime;     // Aseen latausaika
        public int Bullet;     // Ammusobjekti
        public int ShootSound;     // Ampumisen ääni
        public int HitSound;     // Osuman ääni
        public int BulletSpeed;     // Ammuksen lentonopeus
        public int BulletForth;     // Ammuksen lähtöpaikka pelaajan etupuolella
        public int BulletYaw;     // Ammuksen lähtöpaikka sivusuunnassa
        public int Damage;     // Ammuksen aiheuttama tuho
        public int DamageRange;    // Tuhoalueen laajuus
        public int Spread;    // Hajonta asteina
        public int AnimImage;    // Animaatiokuva kun osuu
        public int AnimLength;    // Animaation pituus
        public int AnimDelay;    // Animaation viive
        public int Image;    // Aseen infokuva
        public int Ammo;    // Aseessa olevat ammukset
        public int AmmoMax;    // Ammusten maksimimäärä
        public int Fire;    // Suuliekkianimaatio
        public int FirePos;    // Missä kohdassa suuliekki näytetään (pituussuunnassa)
        public int Icon;    // Pieni ikoni tappoviesteihin
        public int PickCount;    // Kuinka paljon tavaraa saa poimittaessa
        public int Key;    // Näppäin jolla tämä ase valitaan
        public int SafeRange;    // Etäisyys jonka alle kohteesta oleva botti ei ammu
        public int ShootRange;    // Etäisyys jonka alle kohteesta oleva botti ampuu
        public int Character2;    // Pelihahmon objekti (tiimi 2)
        public int Weight;    // Aseen paino, vaikuttaa liikkumisen nopeuteen. 100=normaali
        public int Count;    // Tietoalkioiden lukumäärä


        //--------------------------------------------------------------------------------
        // Asetaulukko ja sen alustus
        //--------------------------------------------------------------------------------
        public static Dictionary<WeaponType, Weapon> WeaponList = new Dictionary<WeaponType, Weapon>();

        public static void Load()
        {
            // Pistooli
            var w = new Weapon();

            w.ReloadTime = 250;
            w.BulletSpeed = 1200;
            w.BulletForth = 33;
            w.BulletYaw = 10;
            w.Damage = 19;
            w.DamageRange = 0;
            w.Spread = 0;
            w.Ammo = 0;
            w.AmmoMax = 0;
            //w.IMAGE)         = IMG_WEAPON1
            //w.BULLET)        = OBJ_AMMO1
            //w.CHARACTER)     = OBJ_PLAYER1
            //w.CHARACTER2)    = OBJ_PLAYER1_2
            //w.SHOOTSOUND)    = SND_SHOOT1
            //w.HITSOUND)      = SND_BULLETHIT1
            //w.FIRE)          = OBJ_FIRE1
            w.FirePos = 33;
            //w.ICON)          = IMG_SWEAPON1
            //w.KEY)           = cbKey1
            w.SafeRange = 100;
            w.ShootRange = 500;
            w.Weight = 100;
            WeaponList.Add(WeaponType.Pistol, w);

            // Konekivääri
            w = new Weapon();
            w = new Weapon(); ;
            w.ReloadTime = 100;
            w.BulletSpeed = 1000;
            w.BulletForth = 29;
            w.BulletYaw = 8;
            w.Damage = 17;
            w.DamageRange = 0;
            w.Spread = 2;
            w.Ammo = 0;
            w.AmmoMax = 150;
            //;w.Image           = IMG_WEAPON2
            //;w.Bullet          = OBJ_AMMO2
            //;w.Character       = OBJ_PLAYER2
            //;w.Character2      = OBJ_PLAYER2_2
            //;w.ShootSound      = SND_SHOOT2
            //;w.HitSound        = SND_BULLETHIT2
            //;w.Fire            = OBJ_FIRE2;
            w.FirePos = 29;
            //;w.Icon            = IMG_SWEAPON2;
            w.PickCount = 50;
            //;w.Key             = cbKey2;
            w.SafeRange = 200;
            w.ShootRange = 500;
            w.Weight = 100;
            WeaponList.Add(WeaponType.MachineGun, w);

            // Sinko;
            w = new Weapon();
            w.ReloadTime = 1500;
            w.BulletSpeed = 900;
            w.BulletForth = 30;
            w.BulletYaw = 8;
            w.Damage = 150;
            w.DamageRange = 250;
            w.Spread = 0;
            w.Ammo = 0;
            w.AmmoMax = 10;
            //;w.Image        = IMG_WEAPON3
            //;w.Bullet       = OBJ_AMMO3
            //;w.Character    = OBJ_PLAYER3
            //;w.Character2   = OBJ_PLAYER3_2
            //;w.ShootSound   = SND_SHOOT3
            //;w.HitSound     = SND_BULLETHIT3
            //;w.Fire         = OBJ_FIRE3;
            w.FirePos = -25;
            //;w.Icon         = IMG_SWEAPON3;
            w.PickCount = 5;
            //;w.Key          = cbKey3;
            w.SafeRange = 300;
            w.ShootRange = 500;
            w.Weight = 115;
            WeaponList.Add(WeaponType.Bazooka, w);

            // Haulikko;
            w = new Weapon();
            w.ReloadTime = 1000;
            w.BulletSpeed = 900;
            w.BulletForth = 33;
            w.BulletYaw = 10;
            w.Damage = 20;
            w.DamageRange = 0;
            w.Spread = 15;
            w.Ammo = 0;
            w.AmmoMax = 20;
            //;w.Image        = IMG_WEAPON5
            //;w.Bullet       = OBJ_AMMO1
            //;w.Character    = OBJ_PLAYER5
            //;w.Character2   = OBJ_PLAYER5_2
            //;w.ShootSound   = SND_SHOOT5
            //;w.HitSound     = SND_BULLETHIT1
            //;w.Fire         = OBJ_FIRE2;
            w.FirePos = 33;
            //;w.Icon         = IMG_SWEAPON5;
            w.PickCount = 10;
            //;w.Key          = cbKey4;
            w.SafeRange = 150;
            w.ShootRange = 300;
            w.Weight = 100;
            WeaponList.Add(WeaponType.Shotgun, w);

            // Moottorisaha;
            w = new Weapon();
            w.ReloadTime = 100;
            w.BulletSpeed = 0;
            w.BulletForth = 45;
            w.BulletYaw = 9;
            w.Damage = 70;
            w.DamageRange = 60;
            w.Spread = 0;
            w.Ammo = 0;
            w.AmmoMax = 100;
            //;w.Image       = IMG_WEAPON4
            //;w.Bullet      = OBJ_NULL
            //;w.Character   = OBJ_PLAYER4
            //;w.Character2  = OBJ_PLAYER4_2
            //;w.ShootSound  = SND_SHOOT4;
            w.HitSound = 0;
            //;w.Fire        = OBJ_NULL
            //;w.Icon        = IMG_SWEAPON4;
            w.PickCount = 50;
            //;w.Key         = cbKey6;
            w.SafeRange = 60;
            w.ShootRange = 150;
            w.Weight = 90;
            WeaponList.Add(WeaponType.Chainsaw, w);

            // Kranaatinlaukaisin;
            w = new Weapon();
            w.ReloadTime = 1000;
            w.BulletSpeed = 400;
            w.BulletForth = 32;
            w.FirePos = 36;
            w.BulletYaw = 8;
            w.Damage = 200;
            w.DamageRange = 150;
            w.Spread = 5;
            w.Ammo = 0;
            w.AmmoMax = 6;
            //;w.Image       = IMG_WEAPON6
            //;w.Bullet      = OBJ_AMMO6
            //;w.Character   = OBJ_PLAYER6
            //;w.Character2  = OBJ_PLAYER6_2
            //;w.ShootSound  = SND_LAUNCHER
            //;w.HitSound    = SND_BULLETHIT3
            //;w.Fire        = OBJ_NULL
            //;w.Icon        = IMG_SWEAPON6;
            w.PickCount = 2;
            //;w.Key         = cbKey5;
            w.SafeRange = 300;
            w.ShootRange = 400;
            w.Weight = 110;
            WeaponList.Add(WeaponType.Launcher, w);
        }
    }
}
