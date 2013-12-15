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
}
