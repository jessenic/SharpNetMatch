using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    public enum PacketType
    {
        Login = 1, // Kirjautuminen peliin
        Logout = 2, // Poistuminen
        LoginFailed = 3, // Kirjautuminen epäonnistui
        LoginOk = 4, // Kirjautuminen onnistui
        WrongVersion = 5, // Väärä ohjelmaversio
        TooManyPlayers = 6, // Pelissä on liikaa pelaajia
        NoLogin = 7, // Pelaaja ei ole kirjautunut oikein
        Player = 8, // Pelaajadataa
        NewBullet = 9, // Uusi ammus
        PlayerName = 10, // Pelaajan nimi
        TextMessage = 11, // Tekstiviesti
        BulletHit = 12, // Ammus osui johonkin pelaajaan
        Radar = 13, // Tutka
        Item = 14, // Poimittava tavara
        KillMessage = 15, // Tappoviesti
        SessionTime = 16, // Pelisession aikatiedot
        Banned = 17, // Client on bannattu
        MapChange = 18, // Kartan vaihto
        Kicked = 19, // Pelaajan potkiminen servulta
        ServerMsg = 20, // Palvelimen generoima viesti
        NicknameInUse = 21, // Liittyessä ei saa olla sama nimimerkki jo käytössä
        ServerClosing = 22, // Lähetetään kaikille tieto että palvelin sammutetaan.
        TeamInfo = 23, // Pelaajan joukkueesta tieto.
        Speedhack = 24, // Nopeuden huijaus havaittu
        DebugDrawing = 25, // Piirtelyjä debug-tarkoituksia varten tulossa
        End = 255, // Viestin loppu
    }


    /**
     * @namespace Aseet
     */
    public enum WeaponType
    {
        /** Pistooli */
        Pistol = 1,


        /** Konepistooli */
        MachineGun = 2,


        /** Sinko */
        Bazooka = 3,


        /** Haulikko */
        Shotgun = 4,


        /** Kranaatinlaukaisin */
        Launcher = 5,


        /** Moottorisaha */
        Chainsaw = 6,


        /** Aseiden lukumäärä */
        WeaponCount = 6
    };


    /**
     * @namespace Itemit omana kokoelmanaan eroteltuna OBJ kokoelmasta.
     */
    public enum ItemType
    {
        /** Healthpack */
        Healthpack = 18,


        /** Konekiväärin ammuksia */
        Ammo = 19,


        /** Singon ammuksia */
        Rocket = 20,


        /** Moottorisahan bensaa */
        Fuel = 21,


        /** Haulikon ammuksia */
        Shotgun = 33,


        /** Kranaatteja */
        Launcher = 34
    };


    /**
     * @namespace Piirrettävien tavaroiden tyypit, debuggailua varten.
     */
    enum DRAW
    {
        /** Viiva */
        LINE = 1,


        /** Origokeskinen ympyrä */
        CIRCLE = 2,


        /** Laatikko */
        BOX = 3,


        /** Clientille käsky tyhjentää piirrokset muistista */
        CLEAR = 127
    };


    /**
     * @namespace Pelaajiin liittyvät rajoitukset
     */
    enum PLR
    {
        /** Nopeus eteen (pikseliä sekunnissa) */
        FORWARD_SPEED = 250,


        /** Nopeus taakse (pikseliä sekunnissa) */
        BACKWARD_SPEED = 150,


        /** Nopeus sivulle (pikseliä sekunnissa) */
        SIDESTEP_SPEED = 160,


        /** Kuinka iso on ympyrä-tilekartta törmäyksen ympyrän säde */
        COL_RADIUS = 25
    };

}
