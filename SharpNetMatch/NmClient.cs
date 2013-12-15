using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    class NmClient : CbNetwork
    {
        public NmClient()
            : base()
        {
            base.PacketReceived += NmClient_PacketReceived;
            Players = new Dictionary<byte, Player>();
        }

        public Dictionary<byte, Player> Players { get; set; }

        public byte PlayerId;

        public bool IsLoggedIn { get; private set; }

        void NmClient_PacketReceived(object sender, PacketEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("r " + BitConverter.ToString(e.p.memBlock));
            while (e.p.BytesLeft > 1)
            {
                var netmsg = e.p.GetByte(); // Luetaan viestiyyppi
                switch ((PacketType)netmsg)
                {
                    case PacketType.Login:
                        HandleLogin(e.p);
                        break;
                    case PacketType.PlayerName:
                        {
                            var id = e.p.GetByte();
                            var name = e.p.GetString();
                            var zombie = e.p.GetByte();
                            var team = e.p.GetByte();
                            System.Diagnostics.Debug.WriteLine(name);
                            if (!Players.ContainsKey(id)) {
                                //TODO: Players.Add(id, new Player());
                            }
                            Player player = Players[id];
                            break;
                        }
                    case PacketType.Player:
                        {
                            var id = e.p.GetByte();
                            var x = e.p.GetShort();
                            var y = e.p.GetShort();
                            var angle = e.p.GetShort();

                            var b = e.p.GetByte();

                            var health = e.p.GetByte();
                            var kills = e.p.GetShort();
                            var deaths = e.p.GetShort();
                            break;
                        }
                    case PacketType.Radar:
                        {
                            var angle = e.p.GetByte();
                            var team = e.p.GetByte();
                            break;
                        }
                    case PacketType.Item:
                        {
                            var itemid = e.p.GetByte();
                            var itemtype = e.p.GetByte();
                            var x = e.p.GetShort();
                            var y = e.p.GetShort();
                            break;
                        }
                    case PacketType.MapChange:
                        {
                            var mapName = e.p.GetString();
                            var mapCRC = e.p.GetInt();
                            break;
                        }
                    case PacketType.SessionTime:
                        {
                            var periodLength = e.p.GetInt();
                            var played = e.p.GetInt();
                            var sessionComplete = e.p.GetByte();
                            break;
                        }
                    case PacketType.End:
                        break;
                    case PacketType.NewBullet:
                        {
                            var bulletId = e.p.GetShort();
                            var playerId = e.p.GetByte();
                            var b = e.p.GetByte();

                            var x = e.p.GetShort();
                            var y = e.p.GetShort();
                            var angle = e.p.GetShort();
                            break;
                        }
                    case PacketType.BulletHit:
                        {
                            var bulletId = e.p.GetShort();
                            var playerId = e.p.GetByte();

                            var x = e.p.GetShort();
                            var y = e.p.GetShort();
                            var weapon = e.p.GetByte();
                            break;
                        }
                    case PacketType.KillMessage:
                        {
                            var killer = e.p.GetByte();     // Tappaja
                            var killed = e.p.GetByte();      // Tapettu
                            var weapon = e.p.GetByte();         // Ase
                            var killerKills = e.p.GetShort();   // Tappajan tapot
                            var killerDeaths = e.p.GetShort();  // Tappajan kuolemat
                            var killedKills = e.p.GetShort();  // Uhrin tapot
                            var killedDeaths = e.p.GetShort(); // Uhrin kuolemat

                            break;
                        }
                    default:
                        System.Diagnostics.Debug.WriteLine("Unhandled packet " + ((PacketType)netmsg).ToString());
                        return;
                        break;
                }
                System.Diagnostics.Debug.WriteLine("Handled " + ((PacketType)netmsg).ToString());
            }
        }

        private void HandleLogin(Packet p)
        {
            var netmsg = p.GetByte(); // Luetaan kirjautumispyynnön vastaus
            switch ((PacketType)netmsg)
            {
                case PacketType.LoginFailed:
                    {
                        // Kirjautuminen epäonnistui
                        // Luetaan syy
                        var reason = p.GetByte();
                        switch ((PacketType)reason)
                        {
                            case PacketType.WrongVersion:
                                // Väärä ohjelmaversio
                                // Luetaan palvelimen ohjelmaversio
                                throw new Exception("Wrong version! Server expected version " + p.GetString());
                            case PacketType.TooManyPlayers:
                                // Liikaa pelaajia
                                throw new Exception("Server is full");
                            case PacketType.NicknameInUse:
                                throw new Exception("Nickname in use");
                            default:
                                throw new Exception("Unexpected error!");
                        }
                    }
                case PacketType.LoginOk:
                    Uid = p.ClientId;
                    PlayerId = p.GetByte();
                    var playMode = p.GetByte();
                    var serverMap = p.GetString();
                    var mapCRC = p.GetInt();
                    var mapServerUrl = p.GetString();
                    p = new Packet();
                    p.PutByte(PacketType.PlayerName);        // Pyydetään kaikki tiedot
                    p.PutByte(PlayerId);      // Pelaajatunnus
                    p.PutByte(PacketType.End);               // Viestin loppu
                    ClientSend(p);
                    return;
                    break;
            }
        }

        public void Login()
        {
            Packet p = new Packet();
            p.PutByte((byte)PacketType.Login);     // Login-viesti
            p.PutString("v2.5"); // Ohjelmaversio
            p.PutString("SharpNM");    // Nimi
            ClientSend(p);                // Lähetys
        }
    }
}
