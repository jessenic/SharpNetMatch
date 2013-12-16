using System;
using System.Collections.Generic;
using System.IO;
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
            Items = new Dictionary<byte, Item>();
        }

        public Dictionary<byte, Player> Players { get; set; }
        public Dictionary<byte, Item> Items { get; set; }

        public byte PlayerId;

        public Player LocalPlayer
        {
            get
            {
                if (!Players.ContainsKey(PlayerId))
                {
                    Players.Add(PlayerId, new Player() { Id = PlayerId });
                }
                return Players[PlayerId];
            }
        }

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
                            if (!Players.ContainsKey(id))
                            {
                                Players.Add(id, new Player());
                            }
                            Player player = Players[id];
                            player.Id = id;
                            player.Name = name;
                            player.Zombie = zombie;
                            player.Team = team;
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

                            if (!Players.ContainsKey(id))
                            {
                                Players.Add(id, new Player());
                            }
                            Player player = Players[id];
                            player.Id = id;
                            if (id == PlayerId)
                            {
                                player.NextPosition.X = x;
                                player.NextPosition.Y = y;
                            }
                            else
                            {
                                player.Position.X = x;
                                player.Position.Y = y;
                            }
                            player.Angle = angle;

                            player.HeldWeapon = (byte)((b << 28) >> 28);     // Ase (bitit 0-3)
                            player.HasAmmo = (byte)((b << 27) >> 31);     // Onko ammuksia (bitti 4)
                            player.Team = (byte)((b << 25) >> 31 + 1); // Joukkue (bitti 6)
                            player.IsProtected = (byte)((b << 24) >> 31);     // Haavoittumaton (bitti 7)

                            player.Health = health > 128 ? health - 256 : health;
                            player.Kills = kills;
                            player.Deaths = deaths;
                            player.KillRatio = kills != 0 && deaths != 0 ? kills / deaths : 0;
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
                            if (!Items.ContainsKey(itemid))
                            {
                                Items.Add(itemid, new Item());
                            }
                            Item item = Items[itemid];
                            item.Id = itemid;
                            item.ItemType = itemtype;
                            item.Position.X = x;
                            item.Position.Y = y;
                            break;
                        }
                    case PacketType.MapChange:
                        {
                            MapName = e.p.GetString();
                            MapCRC = e.p.GetInt();
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

        public string MapServerUrl { get; set; }
        public string MapName { get; set; }
        public int MapCRC { get; set; }

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
                    MapName = p.GetString();
                    MapCRC = p.GetInt();
                    MapServerUrl = p.GetString();
                    p = new Packet();
                    p.PutByte(PacketType.PlayerName);        // Pyydetään kaikki tiedot
                    p.PutByte(PlayerId);      // Pelaajatunnus
                    p.PutByte(PacketType.End);               // Viestin loppu
                    ClientSend(p);
                    IsLoggedIn = true;
                    return;
                    break;
            }
        }

        public void Login()
        {
            Packet p = new Packet();
            p.PutByte((byte)PacketType.Login);     // Login-viesti
            p.PutString("v2.5"); // Ohjelmaversio
            p.PutString("SharpNM" + new Random().Next(int.MaxValue));    // Nimi
            ClientSend(p);                // Lähetys
            while (!IsLoggedIn)
            {
                NmClient_PacketReceived(null, new PacketEventArgs(ClientRead()));
            }
            ClientReadInternal();
        }

        public void UpdatePlayer(byte shootNow)
        {
            Packet p = new Packet();
            p.PutByte((byte)PacketType.Player);
            p.PutByte(LocalPlayer.Id);
            p.PutShort((short)LocalPlayer.Position.X);
            p.PutShort((short)LocalPlayer.Position.Y);
            p.PutShort(LocalPlayer.Angle);

            if (LocalPlayer.HasAmmo == 0 || LocalPlayer.HeldWeapon == 0)
            {
                shootNow = 0;
            }

            // Tungetaan useampi muuttuja yhteen tavuun:
            byte b = (byte)(((LocalPlayer.HeldWeapon % 16) << 0)  // Ase (bitit 0-3)
              + (LocalPlayer.HasAmmo << 4)     // Onko ammuksia (bitti 4)
              + (shootNow << 5));          // Ammutaanko (bitti 5)

            p.PutByte(b);
            byte pickedId = 0;
            p.PutByte(pickedId);
            ClientSend(p);
        }
    }
}
