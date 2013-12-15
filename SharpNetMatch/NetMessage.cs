//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
///**
// * @fileOverview Viestien säilytykseen liittyvät toiminnot
// */
//namespace SharpNetMatch
//{
//    public class NetMessage
//    {


//public NetMessage() {
//}


///**
// * Lisää data-pakettiin yksittäiselle pelaajalle kuuluvat viestit oikein jäsenneltynä.
// * Kts. cbNetwork-node toteutus luokasta <a href="http://vesq.github.com/cbNetwork-node/doc/symbols/Packet.html">Packet</a>.
// *
// * @param {Player} toPlayer  Kenen viestit haetaan
// * @param {Packet} data      Mihin pakettiin tiedot lisätään
// */
//public void Handle(Packet p){
//  // Tämän viestin data laitetaan d-muuttujaan, jotta tarvitsisi kirjoittaa vähemmän.
//  d = this.data[toPlayer.id][0];
//  while (d) {
//    if (!d.hasOwnProperty('msgType')) {
//      log.error('Virheellistä dataa NetMessages-objektissa!');
//      console.dir(d);
//      continue;
//    }

//    // Lisätään dataa riippuen siitä minkälaista dataa pitää lähettää.
//    switch (d.msgType) {
//      case NET.LOGIN:
//        // Joku on liittynyt peliin
//        data.putByte(d.msgType);
//        data.putByte(d.player.id);      // Kuka liittyi
//        data.putString(d.msgText);      // Liittymisteksti
//        data.putByte(d.player.zombie);  // Oliko liittynyt botti?
//        break;

//      case NET.LOGOUT:
//        // Joku on poistunut pelistä
//        data.putByte(d.msgType);
//        data.putByte(d.player.id);
//        break;

//      case NET.NEWBULLET:
//        // Uusi ammus on ammuttu
//        if (d.weapon === WPN.CHAINSAW) {
//          // Moottorisahalla "ammutaan"
//          data.putByte(d.msgType);
//          data.putShort(d.bullet.id); // Ammuksen tunnus
//          data.putByte(d.player.id);  // Kuka ampui

//          // Tungetaan samaan tavuun useampi muuttuja:
//          b = ((d.weapon % 16) << 0)
//            + (d.sndPlay << 4);
//          data.putByte(b);

//          // Ammuksen sijainti
//          data.putShort(d.x);
//          data.putShort(d.y);
//          data.putShort(0);  // Ammuksen kulma, mutta koska moottirisahalla ei ole kulmaa, on tämä 0
//        } else {
//          // Jokin muu kuin moottorisaha
//          if ('undefined' !== typeof d.bullet) {
//            data.putByte(d.msgType);
//            data.putShort(d.bullet.id);
//            data.putByte(d.player.id);

//            // Tungetaan samaan tavuun useampi muuttuja:
//            b = ((d.weapon % 16) << 0)  // Millä aseella (mod 16 ettei vie yli 4 bittiä)
//              + (d.sndPlay << 4)        // Soitetaanko ääni
//              + (d.handShooted << 5);   // Kummalla kädellä ammuttiin
//            data.putByte(b);

//            // Ammuksen sijainti
//            data.putShort(d.bullet.x);
//            data.putShort(d.bullet.y);
//            data.putShort(d.bullet.angle);
//          }
//        }
//        break;

//      case NET.TEXTMESSAGE:
//        // Tsättiviesti
//        data.putByte(d.msgType);
//        data.putByte(d.player.id);
//        data.putString(d.msgText);
//        break;

//      case NET.SERVERMSG:
//        // Palvelimen generoima viesti
//        data.putByte(d.msgType);
//        data.putString(d.msgText);
//        break;

//      case NET.BULLETHIT:
//        // Osumaviesti
//        data.putByte(d.msgType);
//        data.putShort(d.bullet.id);   // Ammuksen tunnus
//        if (d.player) {
//          data.putByte(d.player.id);  // Keneen osui
//        } else {
//          data.putByte(0);
//        }
//        data.putShort(d.x);           // Missä osui
//        data.putShort(d.y);           // Missä osui
//        data.putByte(d.weapon);       // Mistä aseesta ammus on
//        break;

//      case NET.ITEM:
//        // Tavaraviesti
//        data.putByte(d.msgType);
//        data.putByte(d.itemId);     // Tavaran tunnus
//        data.putByte(d.itemType);   // Tavaran tyyppi
//        data.putShort(d.x);         // Missä tavara on
//        data.putShort(d.y);         // Missä tavara on
//        break;

//      case NET.KILLMESSAGE:
//        // Tappoviesti! Buahahahaaaa
//        data.putByte(d.msgType);
//        data.putByte(d.player.id);       // Tappaja
//        data.putByte(d.player2.id);      // Tapettu
//        data.putByte(d.weapon);          // Ase
//        data.putShort(d.player.kills);   // Tappajan tapot
//        data.putShort(d.player.deaths);  // Tappajan kuolemat
//        data.putShort(d.player2.kills);  // Uhrin tapot
//        data.putShort(d.player2.deaths); // Uhrin kuolemat
//        break;

//      case NET.KICKED:
//        // Pelaaja potkittiin
//        data.putByte(d.msgType);
//        if (d.player) {
//          data.putByte(d.player.id); // Kuka potkaisi
//        } else {
//          data.putByte(0);           // Palvelin potkaisi
//        }
//        data.putByte(d.player2.id);  // Kenet potkittiin
//        data.putString(d.msgText);   // Potkujen syy
//        break;

//      case NET.TEAMINFO:
//        // Lähetetään pelaajan joukkue
//        data.putByte(d.msgType);
//        data.putByte(d.player.id);   // Pelaaja
//        data.putByte(d.player.team); // Pelaajan joukkue
//        break;

//      case NET.SPEEDHACK:
//        // Tämä client on haxor!
//        data.putByte(d.msgType);
//        // UNIMPLEMENTED
//        // Login( False, gCurrentPlayerId )
//        break;

//      case NET.DEBUGDRAWING:
//        // Debug-piirtelytavaraa tulossa
//        data.putByte(d.msgType);
//        data.putByte(d.drawType);
//        if (d.drawVars) {
//          for (var i=0; i < d.drawVars.length; i++) {
//            if (i === 4) {
//              // Viides parametri on pakko olla byte
//              data.putByte(d.drawVars[i]);
//              break;
//            }
//            data.putShort(d.drawVars[i]);
//          }
//        }
//        break;

//      default:
//        log.error('VIRHE: Pelaajalle %0 (%1) oli osoitettu tuntematon paketti:',
//          toPlayer.name.green, String(toPlayer).id.magenta);
//        console.dir(d);
//    }

//    // Poistetaan viesti muistista
//    this.data[toPlayer.id].splice(0, 1);

//    // Siirrytään seuraavaan viestiin
//    d = this.data[toPlayer.id][0];
//  }
//};


//module.exports = NetMessages;


//    }
//}
