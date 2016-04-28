using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;
using System.Text;
using System.Numerics;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Linq;


/*
 * Initially I thought returning a hash on each packethandler would work, I'm not so sure about this anymore.
 * What may be better is to creat very ~basic, generic~ types, IE: Entity/Aisling (this may include spells, skills,
 * map class's and mappathingfinding classes -- and is why this must be seriously thought)
 * 
 * Is it possible to make the packethandlers return the data to the main program? The actual call is on the serverEndReceive,
 * if we look into the actual socket and learn a bit we may be able to do something like this.
 * 
 * Is this even the ideal method of doing this? Or would writing all logic on the clientless bot be just as fast?
 * I believe we should get into a trend of only reading client packets and writing server ones (again, we don't need to modify
 * any client information (i dont think..))
 * 
 *1) So, do we create a class much like "DarkAgesBase" that contains all of the info that the packet handlers pull up?
 * Then can interact with said info either through our main program, or directly on the DLL.. probably off the DLL for portability?
 * 
 * or 2) read below... Do we really wann try this... message buffer idea?
 ---To make this a true DLL we need to make a message buffer that the main program will have to iterate through and clear on it's own (obviously we'll have to manage
 * it on this end as well).
 * 
 * IE: The DLL should NOT open a form, that is TEMPORARY. The main program will control all form opening, the DLL will control nothing but clientless communication
 * with the DA server
 * 
 * DO NOT FORGET, we SEND clientpackets, READ serverpackets.
 * What does this mean for walking?That we wait for a response instead of sending the serverpacket
 * 
 * This is INTENDED (when the FUCK are we going to read a packet that were sending, and write a packet that were receiving?)
 * ---- Maybe when were making hidden players appear? NO DUMBFUCK WE HAVE NO CLIENT.
 * (There is one issue, hidden players names are obfusicated by the server and require parsing to be read correctly as text
 * The fix to this issue is.... DUMDUM.. We READ the server messages, and once we have received all data, we PARSE IT ON THE DLL.
 * WE DO NOT PARSE PACKETS MID STREAM. (I think....))
 
 */

namespace ConsoleDA
{
    public delegate void ServerPacketHandler(ServerPacket packet);
    public delegate void ClientPacketHandler(ClientPacket packet);

    public class Client
    {
        public bool sentVersion;
        public byte Seed { get; set; }
        public byte[] Key { get; set; }
        public byte[] KeyTable { get; set; }
        public string keySalt { get; set; }
        public bool access = true;

        public bool Connected { get; private set; }
        public Server Server { get; private set; }

        public ClientTab Tab { get; private set; }
        public DarkagesBase Base { get; private set; }

        public bool EditingMap { get; set; }
        public bool ShouldUpdateMap { get; set; }
        //public event OnClientDisConnected OnDisconnected;

        public int LastMap { get; set; }
        public int WalkCounter { get; set; }

        public List<Client> Bashers
        {
            get
            {
                return Enumerable.ToList<Client>(Enumerable.Where<Client>((IEnumerable<Client>)Server.Clients, (Func<Client, bool>)(i => !i.Base.Caster)));
            }
        }

        public Client Me
        {
            get
            {
                return this;
            }
        }

        public List<Client> Others
        {
            get
            {
                return Enumerable.ToList<Client>(Enumerable.Where<Client>((IEnumerable<Client>)Server.Clients, (Func<Client, bool>)(i => (int)i.Base.Serial != (int)this.Base.Serial)));
            }
        }

        public List<Client> Casters
        {
            get
            {
                return Enumerable.ToList<Client>(Enumerable.Where<Client>((IEnumerable<Client>)Server.Clients, (Func<Client, bool>)(i => !i.Base.Caster)));
            }
        }

        private static short daVersion = 741;
        private Socket socket;
        public ServerInfo server;
        public ConsoleView console;

        private byte[] recvBuffer = new byte[4096];
        private List<byte> fullRecvBuffer = new List<byte>();
        private byte clientOrdinal;
        private Dictionary<byte, ServerPacketHandler> serverPacketHandlers;
        private Dictionary<byte, ClientPacketHandler> clientPacketHandlers;

        public bool loggedIn { get; set; }

        private Crypto crypto;

        public string username;
        public string password;

        private DateTime lastUpdate;

        public Client(string username, string password)
        {
            this.Server = MainForm.Server;
            this.loggedIn = false;
            this.console = new ConsoleView(this);
            this.username = username;
            this.password = password;
            this.Base = new DarkagesBase(this);
            this.Tab = new ClientTab(this);
            //this.Key = Encoding.ASCII.GetBytes("UrkcnItnI");//"UrkånIt£I"
            this.Key = Encoding.ASCII.GetBytes("UrkånIt£I");//"UrkånIt£I"
            this.KeyTable = new byte[1024];
            crypto = new Crypto();
            
            clientPacketHandlers = new Dictionary<byte, ClientPacketHandler>();
            serverPacketHandlers = new Dictionary<byte, ServerPacketHandler>();
            serverPacketHandlers[0x00] = PacketHandler_0x00_Encryption;
            serverPacketHandlers[0x02] = PacketHandler_0x02_LoginMessage;
            serverPacketHandlers[0x03] = PacketHandler_0x03_Redirect;
            serverPacketHandlers[0x04] = PacketHandler_0x04_LocationUpdated;
            serverPacketHandlers[0x05] = PacketHandler_0x05_UserID;
            serverPacketHandlers[0x07] = PacketHandler_0x07_Sprites;
            serverPacketHandlers[0x0E] = PacketHandler_0x0E_SpriteRemoved;
            serverPacketHandlers[0x0C] = PacketHandler_0x0C_SpriteWalked;
            serverPacketHandlers[0x08] = PacketHandler_0x08_StatsUpdated;
            serverPacketHandlers[0x0A] = PacketHandler_0x0A_SystemMessage;
            serverPacketHandlers[0x0B] = PacketHandler_0x0B_ClientWalk;
            serverPacketHandlers[0x0D] = PacketHandler_0x0D_Chat;
            serverPacketHandlers[0x15] = PacketHandler_0x15_MapInfo;
            serverPacketHandlers[0x3B] = PacketHandler_0x3B_PingA;
            serverPacketHandlers[0x33] = PacketHandler_0x33_PlayerAdded;
            serverPacketHandlers[0x4C] = PacketHandler_0x4C_EndingSignal;
            serverPacketHandlers[0x68] = PacketHandler_0x68_PingB;
            serverPacketHandlers[0x7E] = PacketHandler_0x7E_Welcome;
        }

        delegate void ClientlessDelegate();
        public static void Run(string username, string password, ref Thread latent, Action<Client> loopCallback, out Client console)
        {
            var client = new Client(username, password);
            console = client;
            client.Connect();
            //latent = new Thread(() => { loopCallback(client); });
            //latent.Start();
        }

        private void Connect()
        {
            Connect(ServerInfo.Login);
        }
        private void Connect(ServerInfo serverInfo)
        {
            Connect(serverInfo.Address, serverInfo.Port);
        }
        private void Connect(IPAddress address, int port)
        {
            var server = ServerInfo.FromIPAddress(address, port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            while (true)
            {

                this.console.WriteLine("Connecting to " + server.Name);

                try { socket.Connect(address, port); }
                catch (SocketException)
                {
                    this.console.WriteLine("failed, trying again.");
                    continue;
                }
                catch
                {
                    this.console.WriteLine("unexpected error, aborting.");
                    return;
                }
                break;
            }

            this.console.WriteLine("success!");

            this.server = server;
            socket.BeginReceive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None, EndReceive, socket);
        }

        public void Disconnect()
        {
            socket.Close();
            Disconnected();
        }
        private void Disconnected()
        {
            this.console.WriteLine("Disconnected from " + server.Name);

            ConnectionClosed();
            fullRecvBuffer.Clear();
            clientOrdinal = 0;
            sentVersion = false;
        }
        private void ConnectionClosed()
        {
            if (server == ServerInfo.Login)
                DisconnectedFromLogin();

            if (server == ServerInfo.Temuair ||
                server == ServerInfo.Medenia)
                DisconnectedFromWorld();

            server = null;
        }

        public void Reconnect()
        {
            Disconnect();
            Connect();
        }

        public void Send(byte[] data)
        {
            ClientPacket packet = (ClientPacket)null;
            foreach (byte opcode in data)
            {
                if (packet == null)
                    packet = new ClientPacket(opcode);
                else
                    packet.WriteByte(opcode);
            }

            packet.WriteByte(0);
            Tab.LogOutgoingPacket("Send> {0}", packet);

            if (packet.ShouldEncrypt)
            {
                packet.Ordinal = clientOrdinal++;
                packet.Encrypt(crypto);
            }
            try { socket.Send((byte[])packet); }
            catch (ObjectDisposedException) { }
            catch { }
        }

        public void Send(ClientPacket packet)
        {
            Tab.LogOutgoingPacket("Send> {0}", packet);

            if (packet.ShouldEncrypt)
            {
                packet.Ordinal = clientOrdinal++;
                packet.Encrypt(crypto);
            }
            try { socket.Send((byte[])packet); }
            catch (ObjectDisposedException) { }
            catch { }
        }
        private void EndReceive(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;

            int count = 0;

            try { count = socket.EndReceive(ar); }
            catch (ObjectDisposedException) { return; }
            catch { }

            if (count == 0)
            {
                Reconnect();
                return;
            }

            byte[] trimmed = new byte[count];
            Array.Copy(recvBuffer, trimmed, count);
            fullRecvBuffer.AddRange(trimmed);

            while (fullRecvBuffer.Count > 3)
            {
                if (fullRecvBuffer[0] != 0xAA)
                {
                    Disconnect();
                    Connect();
                    return;
                }

                int length = fullRecvBuffer[1] << 8 | fullRecvBuffer[2] + 3;

                if (length > fullRecvBuffer.Count) break;

                var range = fullRecvBuffer.GetRange(0, length);
                var buffer = range.ToArray();

                fullRecvBuffer.RemoveRange(0, length);

                var packet = new ServerPacket(buffer);

                if (packet.ShouldEncrypt)
                    packet.Decrypt(crypto);
                /*if (this.Server.ServerMessageHandlers[packet.Opcode)
                    packetHandlers[packet.Opcode](packet);*/
                Tab.LogIncomingPacket("Recv> {0}", packet);
                if (serverPacketHandlers.ContainsKey(packet.Opcode))
                    serverPacketHandlers[packet.Opcode](packet);
            }

            try { socket.BeginReceive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None, EndReceive, socket); }
            catch (ObjectDisposedException) { return; }
            catch { }
        }

        private void ConnectedToLogin()
        {
            Login(username, password);
        }
        private void ConnectedToWorld()
        {
            this.loggedIn = true;
            this.console.WriteLine("Logged in to " + server.Name + " as " + username);
            Send(new ClientPacket(0x2D));
        }

        private void DisconnectedFromLogin()
        {

        }
        private void DisconnectedFromWorld()
        {

        }
        #region PacketHandlers
        private void PacketHandler_0x00_Encryption(ServerPacket packet)
        {
            byte type = packet.ReadByte();

            if (type == 1)
            {
                daVersion--;
                this.console.WriteLine("Invalid DA version, possibly too high. Trying again with " + daVersion);
                return;
            }

            if (type == 2)
            {
                short version = packet.ReadInt16();
                packet.ReadByte();
                string patchUrl = packet.ReadString8();

                daVersion = version;
                this.console.WriteLine("Your DA version is too low. Setting DA version to " + version);

                Reconnect();

                return;
            }

            uint serverTableCrc = packet.ReadUInt32();
            byte seed = packet.ReadByte();
            string key = packet.ReadString8();

            this.Key = Encoding.ASCII.GetBytes(key);
            this.Seed = seed;
            crypto = new Crypto(seed, key);

            var x57 = new ClientPacket(0x57);
            x57.WriteUInt32(uint.MinValue);
            Send(x57);
        }
        private void PacketHandler_0x02_LoginMessage(ServerPacket packet)
        {
            byte code = packet.ReadByte();
            string message = packet.ReadString8();

            switch (code)
            {
                case 0:
                    this.console.WriteLine("success!");
                    break;
                case 3: // invalid name or password
                case 14: // non-existent name
                case 15: // incorrect password
                    this.console.WriteLine(message);
                    break;
                default:
                    this.console.WriteLine(message);
                    Login(username, password);
                    break;
            }
        }
        private void PacketHandler_0x03_Redirect(ServerPacket packet)
        {
            byte[] address = packet.Read(4);
            ushort port = packet.ReadUInt16();
            byte remaining = packet.ReadByte();
            byte seed = packet.ReadByte();
            string key = packet.ReadString8();
            string name = packet.ReadString8();
            uint id = packet.ReadUInt32();

            crypto = new Crypto(seed, key, name);
            this.Seed = seed;
            this.Key = Encoding.ASCII.GetBytes(key);
            this.keySalt = ConsoleDA.Client.GenerateKeySalt(name);

            Array.Reverse(address);

            this.console.WriteLine("Disconnecting from " + server.Name);

            socket.Close();
            ConnectionClosed();
            this.clientOrdinal = 0;
            Connect(new IPAddress(address), port);

            var x10 = new ClientPacket(0x10);
            x10.WriteByte(seed);
            x10.WriteString8(key);
            x10.WriteString8(name);
            x10.WriteUInt32(id);
            x10.WriteByte(0x00);
            Send(x10);

            if (server == ServerInfo.Login)
                ConnectedToLogin();
            //else if(server == ServerInfo.Temuair || server == ServerInfo.Medenia)
            //    ConnectedToWorld();
        }
        private void PacketHandler_0x04_LocationUpdated(ServerPacket msg)
        {
            try
            {
                //client.ShouldUpdateMap = true;
                this.Base.LoggedIn = true;
                this.Base.LastLoggedIn = DateTime.Now;
                ushort num1 = msg.ReadUInt16();
                ushort num2 = msg.ReadUInt16();
                this.Base.MyServerPosition.X = num1;
                this.Base.MyServerPosition.Y = num2;
                this.Base.MyPosition.X = num1;
                this.Base.MyPosition.X = num1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void PacketHandler_0x05_UserID(ServerPacket packet)
        {
            try
            {
                Server.Form.AddTab(Tab);
                ConnectedToWorld();
                uint Serial = packet.ReadUInt32();
                this.Base.Serial = Serial;
                this.Base.Me = new Aisling()
                {
                    Name = "noname",
                    Position = new Location(),
                    Serial = Serial
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void PacketHandler_0x0E_SpriteRemoved(ServerPacket msg)
        {
            try
            {
                
                //client.ShouldUpdateMap = true;
                uint key = msg.ReadUInt32();
                if (this.Base.Entitys.ContainsKey(key) && this.Base.Entitys[key].Type == EntityType.Monster)
                {
                    this.Base.LastEntityRemovedLocation = this.Base.Entitys[key].Position;
                    this.Base.LastEntityRemoved = DateTime.Now;
                }
                if (this.Base.Entitys.ContainsKey(key))
                    this.Base.Entitys.Remove(key);
                if (this.Base.Aislings.ContainsKey(key))
                {
                    this.Base.Aislings[key].lastMap = this.Base.DaMap.Number;
                }
                if (this.Base.Players.ContainsKey(key))
                    this.Base.Players.Remove(key);
                //Aisling temp;
                //if (this.Base.threadSafePlayers.ContainsKey(key))
                //    while (!this.Base.threadSafePlayers.TryRemove(key, out temp))
                //        Thread.Sleep(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void PacketHandler_0x0C_SpriteWalked(ServerPacket msg)
        {
            try
            {
                uint key = msg.ReadUInt32();
                ushort num1 = msg.ReadUInt16();
                ushort num2 = msg.ReadUInt16();
                byte num3 = msg.ReadByte();
                Location location = new Location()
                {
                    X = num1,
                    Y = num2,
                    Facing = (Direction)num3
                };
                switch (location.Facing)
                {
                    case Direction.Up:
                        --location.Y;
                        break;
                    case Direction.Right:
                        ++location.X;
                        break;
                    case Direction.Down:
                        ++location.Y;
                        break;
                    case Direction.Left:
                        --location.X;
                        break;
                }
                if (this.Base.Entitys.ContainsKey(key))
                    this.Base.Entitys[key].Position = location;
                if (this.Base.Aislings.ContainsKey(key))
                    this.Base.Aislings[key].Position = location;
                //if (this.Base.threadSafePlayers.ContainsKey(key))
                //    this.Base.threadSafePlayers[key].Position = location;
                //client.ShouldUpdateMap = true;
                if (this.Base.Players.ContainsKey(key))
                    this.Base.Players[key].Position = location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void PacketHandler_0x07_Sprites(ServerPacket packet)
        {
            try
            {
                this.Base.PullObjectsToStack(packet.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void PacketHandler_0x08_StatsUpdated(ServerPacket msg)
        {
            try
            {
                Aisling aisling = this.Base.Me;
                byte num1 = msg.ReadByte();
                if (((int)num1 & 32) == 32)
                {
                    msg.Read(3);
                    aisling.Level = (int)msg.ReadByte();
                    aisling.Ability = (int)msg.ReadByte();
                    aisling.MaximumHP = msg.ReadUInt32();
                    aisling.MaximumMP = msg.ReadUInt32();
                    aisling.Str = (int)msg.ReadByte();
                    aisling.Int = (int)msg.ReadByte();
                    aisling.Wis = (int)msg.ReadByte();
                    aisling.Con = (int)msg.ReadByte();
                    aisling.Dex = (int)msg.ReadByte();
                    bool flag = (int)msg.ReadByte() != 0;
                    aisling.AvailablePoints = (int)msg.ReadByte();
                    aisling.MaximumWeight = (int)msg.ReadUInt16();
                    aisling.CurrentWeight = (int)msg.ReadUInt16();
                    msg.Read(4);
                }
                if (((int)num1 & 16) == 16)
                {
                    aisling.CurrentHP = msg.ReadUInt32();
                    aisling.CurrentMP = msg.ReadUInt32();
                }
                if (((int)num1 & 8) == 8)
                {
                    aisling.Experience = msg.ReadUInt32();
                    aisling.ToNextLevel = msg.ReadUInt32();
                    aisling.AbilityExp = msg.ReadUInt32();
                    aisling.ToNextAbility = msg.ReadUInt32();
                    aisling.GamePoints = msg.ReadUInt32();
                    aisling.Gold = msg.ReadUInt32();
                }
                if (((int)num1 & 4) == 4)
                {
                    aisling.BitMask = (int)msg.ReadUInt16();
                    int num2 = (int)msg.ReadByte();
                    aisling.AttackElement2 = (int)msg.ReadByte();
                    aisling.DefenseElement2 = (int)msg.ReadByte();
                    aisling.AttackElement = (Aisling.Elements)msg.ReadByte();
                    aisling.DefenseElement = (Aisling.Elements)msg.ReadByte();
                    aisling.MagicResistance = (int)msg.ReadByte();
                    aisling.ArmorClass = (int)msg.ReadInt16();
                    aisling.Damage = (int)msg.ReadSByte();
                    aisling.Hit = (int)msg.ReadSByte();
                }
                if (aisling.BitMask == 8)
                    msg.Data[0] = (byte)0;
                this.Base.Me = aisling;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void PacketHandler_0x0A_SystemMessage(ServerPacket packet)
        {
            byte channel = packet.ReadByte();
            string message = packet.ReadString16();

            Match match;

            switch (channel)
            {
                case 0: // Whisper
                    match = Regex.Match(message, "^([a-zA-Z]+)\" (.*)"); // incoming
                    if (match.Success)
                    {
                        string name = match.Groups[1].Value;
                        string whisper = match.Groups[2].Value;
                    }

                    match = Regex.Match(message, "^([a-zA-Z]+)> (.*)"); // outgoing
                    if (match.Success)
                    {
                        string name = match.Groups[1].Value;
                        string whisper = match.Groups[2].Value;
                    }

                    match = Regex.Match(message, "^([a-zA-Z]+)( .*)$"); // offline/can't hear/etc
                    if (match.Success)
                    {
                        string name = match.Groups[1].Value;
                        string error = match.Groups[2].Value;
                    }

                    break;
                case 1:
                case 2:
                case 4:
                case 5:
                case 6: // BarOnly
                    break;
                case 3: // BarTopLeft
                    match = Regex.Match(message, @"^\[([a-zA-Z]+)\]: (.*)$"); // world shout
                    if (match.Success)
                    {
                        string name = match.Groups[1].Value;
                        string shout = match.Groups[2].Value;
                    }
                    break;
                case 7: // Settings
                    break;
                case 8:
                case 9: // InfoBox
                    break;
                case 10: // SignBox
                    break;
                case 11: // GroupMessage
                    match = Regex.Match(message, @"^[\!([a-zA-Z]+)] (.*)");
                    if (match.Success)
                    {
                        string name = match.Groups[1].Value;
                        string whisper = match.Groups[2].Value;
                    }
                    break;
                case 12: // GuildMessage
                    match = Regex.Match(message, @"^<\!([a-zA-Z]+)> (.*)");
                    if (match.Success)
                    {
                        string name = match.Groups[1].Value;
                        string whisper = match.Groups[2].Value;
                    }
                    break;
                case 18: // TopRight
                    break;
            }
        }
        private void PacketHandler_0x0B_ClientWalk(ServerPacket msg)
        {
            try
            {
                //this.UpdateMap = true;
                byte num1 = msg.ReadByte();
                ushort num2 = msg.ReadUInt16();
                ushort num3 = msg.ReadUInt16();
                this.Base.MyServerPosition = new Location()
                {
                    Facing = (Direction)num1,
                    X = num2,
                    Y = num3
                };
                switch (num1)
                {
                    case (byte)0:
                        --this.Base.MyServerPosition.Y;
                        break;
                    case (byte)1:
                        ++this.Base.MyServerPosition.X;
                        break;
                    case (byte)2:
                        ++this.Base.MyServerPosition.Y;
                        break;
                    case (byte)3:
                        --this.Base.MyServerPosition.X;
                        break;
                }
                this.Base.MyPosition.Facing = (Direction)num1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void PacketHandler_0x0D_Chat(ServerPacket packet)
        {
            byte type = packet.ReadByte();
            uint id = packet.ReadUInt32();
            string message = packet.ReadString8();
            if (type == 2)
            {
                return;
            }  // blue caption

            var match = Regex.Match(message, "^([a-zA-Z]+)[:!] (.*)");
            if (match.Success)
            {
                string name = match.Groups[1].Value;
                string text = match.Groups[2].Value;

                this.Tab.controlChatText.Text += string.Format("{0}: {1}", name, text);
                this.Tab.controlChatText.Text += Environment.NewLine;
            }
        }
        private void PacketHandler_0x15_MapInfo(ServerPacket msg)
        {
            try
            {
                int key = (int)msg.ReadUInt16();
                int num1 = (int)msg.ReadByte();
                int num2 = (int)msg.ReadByte();
                byte num3 = msg.ReadByte();
                int width = num1 | (int)msg.ReadByte() << 8;
                int height = num2 | (int)msg.ReadByte() << 8;
                msg.ReadUInt16();
                msg.ReadString8();
                this.Base.LastTarget = (Entity)null;
                this.Base.Animations.Clear();
                this.Base.Entitys.Clear();
                //if ((int)num3 == 3 || (int)num3 == 64)
                //    msg.BodyData[6] = (byte)0;
                this.Base.DaMap = new Map(Tab,(short)key, width, height);
                this.Base.LastRefreshed = DateTime.Now;
                //if (!this.Base.Waypoints.ContainsKey(key)) add this back in later but may slow it all down... this is why
                //    this.Base.Waypoints.Add(key, new List<Location>()); this sort of processing should be done on client
                this.Base.Entitys.Clear(); //FIGURE OUT HOW TO DO THAT!!!
                //client.Tab.walkingLocation = null;
                //client.Base.Players.Clear();
                /*if (this.LastMap != key)
                {
                    this.ShouldUpdateMap = true;
                    this.LastMap = key;
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void PacketHandler_0x33_PlayerAdded(ServerPacket msg)
        {
            try
            {
                
                try
                {
                    ushort num1 = msg.ReadUInt16();
                    ushort num2 = msg.ReadUInt16();
                    byte num3 = msg.ReadByte();
                    uint key = msg.ReadUInt32();
                    msg.ReadUInt16();
                    int num4 = (int)msg.ReadUInt16();
                    int num5 = (int)msg.ReadUInt32();
                    msg.ReadByte();
                    ushort num6 = msg.ReadUInt16();
                    msg.ReadByte();
                    int num7 = (int)msg.ReadByte();
                    msg.ReadUInt16();
                    int num8 = (int)msg.ReadInt16();
                    msg.ReadUInt16();
                    int num9 = (int)msg.ReadUInt32();
                    msg.ReadByte();
                    msg.ReadUInt16();
                    int num10 = (int)msg.ReadUInt16();
                    msg.ReadByte();
                    int num11 = (int)msg.ReadUInt16();
                    string str = msg.ReadString8();
                    msg.ReadString8();
                    if (string.IsNullOrEmpty(str))
                        str = "faggot";
                    if (string.IsNullOrEmpty(str) && this.Base.Players.ContainsKey(key))
                      {
                        str = this.Base.Players[key].Name;
                      }
                    if ((int)msg.Data[10] == 0 && (int)msg.Data[11] == 0 && (int)msg.Data[12] == 0 && (int)msg.Data[13] == 0)
                    {
                        if (this.Base.Players.ContainsKey(key))
                        {
                            str = this.Base.Players[key].Name;
                        }
                    }
                    this.console.WriteLine("Player " + str + " Serial: " + key);
                    //if (this.Base.Aislings[this.Base.Serial].Position.X != num1 || this.Base.Aislings[this.Base.Serial].Position.Y != num2)
                      //  this.ShouldUpdateMap = true;
                    //add players to onscreen players array
                    if ((int)this.Base.Serial == (int)key)
                    {
                        this.Base.Me.Name = str;
                        this.Base.Me.Position = new Location()
                        {
                            X = num1,
                            Y = num2
                        };
                        this.Base.CurrentStaffID = num6;
                        this.Base.MyPosition.X = num1;
                        this.Base.MyPosition.Y = num2;
                        this.Base.MyPosition.Facing = (Direction)num3;
                    }
                    /*else if (!client.Base.Entitys.ContainsKey(key))
                    {
                       client.Base.Entitys.Add(key, new Aisling()
                       {
                           Name = str,
                           Position = new Location()
                           {
                               Facing = (Direction)num3,
                               X = num1,
                               Y = num2,
                           },
                           Serial = key,
                           Map = client.Base.DaMap.Number
                       });
                   }
                   else
                   {
                       client.Base.Entitys[key].Name = str;
                       client.Base.Entitys[key].Position = new Location()
                       {
                           X = num1,
                           Y = num2,
                           Facing = (Direction)num3
                       };
                       client.Base.Entitys[key].Map = client.Base.DaMap.Number;
                   }*/
                    else if (!this.Base.Players.ContainsKey(key))
                    {
                        this.Base.Players.Add(key, new Aisling()
                        {
                            Name = str,
                            Position = new Location()
                            {
                                Facing = (Direction)num3,
                                X = num1,
                                Y = num2
                            },
                            Serial = key
                        });
                    }
                    else
                    {
                        this.Base.Players[key].Name = str;
                        this.Base.Players[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                    }
                    /*if (!this.Base.threadSafePlayers.ContainsKey(key))
                    {
                        this.Base.threadSafePlayers.TryAdd(key, new Aisling()
                        {
                            Name = str,
                            Position = new Location()
                            {
                                Facing = (Direction)num3,
                                X = num1,
                                Y = num2
                            },
                            Serial = key,
                            Map = this.Base.DaMap.Number
                        });
                    }
                    else
                    {
                        this.Base.threadSafePlayers[key].Name = str;
                        this.Base.threadSafePlayers[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                        this.Base.threadSafePlayers[key].Map = this.Base.DaMap.Number;
                        if (this.Base.threadSafePlayers[key].Map == this.Base.Aislings[key].lastMap)
                        {
                            this.Base.threadSafePlayers[key].lastMap = -1;
                        }
                    }*/
                    //add players to lasting seen players array
                    if (!this.Base.Aislings.ContainsKey(key))
                    {
                        this.Base.Aislings.Add(key, new Aisling()
                        {
                            Name = str,
                            Position = new Location()
                            {
                                Facing = (Direction)num3,
                                X = num1,
                                Y = num2
                            },
                            Serial = key,
                            Map = this.Base.DaMap.Number
                        });
                    }
                    else
                    {
                        this.Base.Aislings[key].Name = str;
                        this.Base.Aislings[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                        this.Base.Aislings[key].Map = this.Base.DaMap.Number;
                        if (this.Base.Aislings[key].Map == this.Base.Aislings[key].lastMap)
                        {
                            this.Base.Aislings[key].lastMap = -1;
                        }
                    }

                }
                catch
                {
                    ushort num1 = msg.ReadUInt16();
                    ushort num2 = msg.ReadUInt16();
                    byte num3 = msg.ReadByte();
                    uint key = msg.ReadUInt32();
                    if ((int)this.Base.Serial == (int)key)
                    {
                        this.Base.Me.Name = "form";
                        this.Base.Me.Position = this.Base.MyPosition;
                    }
                    else if (!this.Base.Players.ContainsKey(key))
                    {
                        this.Base.Players.Add(key, new Aisling()
                        {
                            Name = "form",
                            Position = new Location()
                            {
                                Facing = (Direction)num3,
                                X = num1,
                                Y = num2
                            },
                            Serial = key
                        });
                    }
                    else
                    {
                        this.Base.Players[key].Name = "form";
                        this.Base.Players[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                    }
                    /*
                    else if (!client.Base.Entitys.ContainsKey(key))
                    {
                        client.Base.Entitys.Add(key, new Aisling()
                        {
                            Name = "form",
                            Position = new Location()
                            {
                                Facing = (Direction)num3,
                                X = num1,
                                Y = num2
                            },
                            Serial = key,
                            Map = client.Base.DaMap.Number
                        });
                    }
                    else
                    {
                        client.Base.Entitys[key].Name = "form";
                        client.Base.Entitys[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                        client.Base.Entitys[key].Map = client.Base.DaMap.Number;
                    }*/
                    /*if (this.Base.threadSafePlayers == null)
                        this.Base.threadSafePlayers = new System.Collections.Concurrent.ConcurrentDictionary<uint, Aisling>();
                    if (!this.Base.threadSafePlayers.ContainsKey(key))
                    {
                        this.Base.threadSafePlayers.GetOrAdd(key, new Aisling()
                        {
                            Name = "form",
                            Position = new Location()
                            {
                                Facing = (Direction)num3,
                                X = num1,
                                Y = num2
                            },
                            Serial = key,
                            Map = this.Base.DaMap.Number
                        });
                    }
                    else
                    {
                        this.Base.threadSafePlayers[key].Name = "form";
                        this.Base.threadSafePlayers[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                        this.Base.threadSafePlayers[key].Map = this.Base.DaMap.Number;
                        if (this.Base.threadSafePlayers[key].Map == this.Base.Aislings[key].lastMap)
                        {
                            this.Base.threadSafePlayers[key].lastMap = -1;
                        }
                    }*/
                    //add players to lasting seen players array
                    if (!this.Base.Aislings.ContainsKey(key))
                    {
                        this.Base.Aislings.Add(key, new Aisling()
                        {
                            Name = "form",
                            Position = new Location()
                            {
                                Facing = (Direction)num3,
                                X = num1,
                                Y = num2
                            },
                            Serial = key,
                            Map = this.Base.DaMap.Number
                        });
                    }
                    else
                    {
                        this.Base.Aislings[key].Name = "form";
                        this.Base.Aislings[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                        this.Base.Aislings[key].Map = this.Base.DaMap.Number;
                        if (this.Base.Aislings[key].Map == this.Base.Aislings[key].lastMap)
                        {
                            this.Base.Aislings[key].lastMap = -1;
                        }
                    }
                    if (!this.Base.Players.ContainsKey(key))
                    {
                        this.Base.Players.Add(key, new Aisling()
                        {
                            Name = "form",
                            Position = new Location()
                            {
                                Facing = (Direction)num3,
                                X = num1,
                                Y = num2
                            },
                            Serial = key
                        });
                    }
                    else
                    {
                        this.Base.Players[key].Name = "form";
                        this.Base.Players[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void PacketHandler_0x3B_PingA(ServerPacket packet)
        {

            byte hiByte = packet.ReadByte();
            byte loByte = packet.ReadByte();

            var x45 = new ClientPacket(0x45);
            x45.WriteByte(loByte);
            x45.WriteByte(hiByte);
            Send(x45);
        }
        private void PacketHandler_0x4C_EndingSignal(ServerPacket packet)
        {
            var x0B = new ClientPacket(0x0B);
            x0B.WriteBoolean(false);
            Send(x0B);
        }
        private void PacketHandler_0x68_PingB(ServerPacket packet)
        {
            int timestamp = packet.ReadInt32();

            var x75 = new ClientPacket(0x75);
            x75.WriteInt32(timestamp);
            x75.WriteInt32(Environment.TickCount);
            Send(x75);
        }
        private void PacketHandler_0x7E_Welcome(ServerPacket packet)
        {
            if (sentVersion)
            {
                return;
            }

            var x62 = new ClientPacket(0x62);
            x62.WriteByte(0x34);
            x62.WriteByte(0x00);
            x62.WriteByte(0x0A);
            x62.WriteByte(0x88);
            x62.WriteByte(0x6E);
            x62.WriteByte(0x59);
            x62.WriteByte(0x59);
            x62.WriteByte(0x75);
            Send(x62);

            var x00 = new ClientPacket(0x00);
            x00.WriteInt16(daVersion);
            x00.WriteByte(0x4C);
            x00.WriteByte(0x4B);
            x00.WriteByte(0x00);
            Send(x00);

            sentVersion = true;
        }
        #endregion

        #region Packets
        public void DropGold(int amount)
        {
            // 24 00 00 00 01 00 37 00 0F 00 24 FD ED 31
            ClientPacket msg = new ClientPacket(0x24);
            msg.WriteInt32(amount);
            msg.WriteUInt16(this.Base.MyServerPosition.X);
            msg.WriteUInt16(this.Base.MyServerPosition.Y);
            this.Send(msg);
        }

        public void Pickup(int x, int y)
        {
            int num = int.MinValue;
            for (int index = 0; index < this.Base.Inventory.Count; ++index)
            {
                if (this.Base.Inventory[index] == null)
                {
                    num = index + 1;
                    break;
                }
            }
            if (num == int.MinValue)
                return;
            ClientPacket msg = new ClientPacket((byte)7);
            msg.WriteByte((byte)num);
            msg.WriteUInt16((ushort)x);
            msg.WriteUInt16((ushort)y);
            msg.WriteByte((byte)0);
            this.Send(msg);
        }

        public void CancelTrade(uint target)
        {
            ClientPacket msg = new ClientPacket(0x4A);
            msg.WriteByte(0x04);
            msg.WriteUInt32(target);
            this.Send(msg);
        }

        public void Say(string message)
        {
            var x0E = new ClientPacket(0x0E);
            x0E.WriteBoolean(false);
            x0E.WriteString8(message);
            Send(x0E);
        }
        public void Shout(string message)
        {
            var x0E = new ClientPacket(0x0E);
            x0E.WriteBoolean(true);
            x0E.WriteString8(message);
            Send(x0E);
        }
        public void Whisper(string name, string message)
        {
            var x19 = new ClientPacket(0x19);
            x19.WriteString8(name);
            x19.WriteString8(message);
            x19.WriteByte(0x00);
            Send(x19);
        }
        public void StartTradeGold(int amount, uint target)
        {
            ClientPacket msg1 = new ClientPacket(0x2A);
            msg1.WriteInt32(amount);
            msg1.WriteUInt32(target);
            ClientPacket msg2 = new ClientPacket(0x4A);
            msg2.WriteByte(0);
            msg2.WriteUInt32(target);
            ClientPacket msg3 = new ClientPacket(0x4A);
            msg3.WriteByte(0x03);
            msg3.WriteUInt32(target);
            msg3.WriteInt32(amount);

            this.Send(msg1);
            this.Send(msg2);
            this.Send(msg3);
        }

        public void AcceptTrade(uint target)
        {
            ClientPacket msg = new ClientPacket(0x4A);
            msg.WriteByte(0x05);
            msg.WriteUInt32(target);
            this.Send(msg);
        }
        public void Walk(Direction direction)
        {
            ClientPacket msg1 = new ClientPacket(0x06);
            msg1.WriteByte((byte)direction);
            msg1.WriteByte((byte)this.WalkCounter++);
            msg1.WriteByte((byte)0);
           /*ServerPacket msg2 = new ServerPacket(0x0B);
            msg2.WriteUInt32(this.Base.Serial);
            msg2.WriteUInt16(this.Base.MyPosition.X);
            msg2.WriteUInt16(this.Base.MyPosition.Y);
            msg2.WriteByte((byte)direction);
            msg2.WriteByte((byte)0);*/
            this.Send(msg1);
            switch (direction)
            {
                case Direction.Up:
                    --this.Base.MyPosition.Y;
                    break;
                case Direction.Right:
                    ++this.Base.MyPosition.X;
                    break;
                case Direction.Down:
                    ++this.Base.MyPosition.Y;
                    break;
                case Direction.Left:
                    --this.Base.MyPosition.X;
                    break;
            }
        }

        public void Dialog0x39(uint npcid, byte[] dilogueid, string item = null, int count = 0)
        {
            ClientPacket msg = new ClientPacket((byte)57);
            byte[] t0 = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
            msg.Write(t0);
            msg.WriteUInt32(npcid);
            msg.Write(dilogueid);
            if (dilogueid.Length == 5)
            {
                msg.WriteString(count.ToString());
            }
            else if (item != null)
            {
                msg.WriteString(item);
                msg.WriteByte(0x02);
                msg.WriteString(count.ToString());
            }


            msg.Write(new byte[0]);
            this.Send(msg);
        }
        public bool CastBuffOn(uint Serial, Spell spell) //lets make it return false if they miss/deflect/etc
        {
            try
            {
                if (spell == null)
                    return false;
                else if (!this.Base.Entitys.ContainsKey(Serial) && this.Base.Serial != Serial)
                {
                    this.Base.LastTarget = (Entity)null;
                    return false;
                }
                else
                {
                    this.Send(new byte[3]
                {
                  (byte) 77,
                  spell.Lines,
                  (byte) 0
                });
                    this.Base.Casting = true;
                    for (int index = 0; index < (int)spell.Lines; ++index)
                        Thread.Sleep(1000);
                    this.Base.Casting = false;
                    byte[] bytes = BitConverter.GetBytes(Serial);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse((Array)bytes);
                    this.Send(new byte[12]
                {
                  (byte) 15,
                  spell.Slot,
                  bytes[0],
                  bytes[1],
                  bytes[2],
                  bytes[3],
                  (byte) 0,
                  (byte) 10,
                  (byte) 0,
                  (byte) 6,
                  (byte) 0,
                  (byte) 15
                });

                    this.Base.IsSwitchingItems = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return true; //lets make it return false if they miss/deflect/etc
        }

        public bool CastSpell(Entity target, Spell spell)
        {
            try
            {
                if (spell == null)
                    return false;
                this.Base.LastCast = DateTime.Now;
                if (target == null)
                {
                    this.Base.LastTarget = (Entity)null;
                    return false;
                }
                else
                {
                    this.Base.Casting = true;
                    this.Base.LastTarget = target;
                    /*this.SendServer(new byte[3]
          {
            (byte) 77,
            spell.Lines,
            (byte) 0
          });*/
                    ClientPacket msg1 = new ClientPacket((byte)77);
                    msg1.WriteByte((byte)spell.Lines);
                    this.Send(msg1);

                    for (int index = 0; index < (int)spell.Lines; ++index)
                        Thread.Sleep(1000);
                    byte[] bytes = BitConverter.GetBytes(target.Serial);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse((Array)bytes);
                    ClientPacket msg = new ClientPacket((byte)15);
                    msg.WriteByte((byte)spell.Slot);
                    msg.WriteUInt32(target.Serial);
                    msg.WriteUInt16(target.Position.X);
                    msg.WriteUInt16(target.Position.Y);
                    this.Send(msg);
                    /*this.SendServer(new byte[12]
          {
            (byte) 15,
            spell.Slot,
            bytes[0],
            bytes[1],
            bytes[2],
            bytes[3],
            (byte) 0,
            (byte) 10,
            (byte) 0,
            (byte) 6,
            (byte) 0,
            (byte) 15
          });*/
                    target.EntityTargeted(spell);
                    this.Base.Casting = false;
                    Thread.Sleep(200);
                    foreach (BarMessage barMessage in Enumerable.Where<BarMessage>((IEnumerable<BarMessage>)Enumerable.ToList<BarMessage>((IEnumerable<BarMessage>)this.Base.BarMessages), (Func<BarMessage, bool>)(i => i.Message != "")))
                    {
                        if (barMessage.TimeElapsed < new TimeSpan(0, 0, 0, 200) && (int)barMessage.Type == 3)
                        {
                            if (barMessage.Message.ToLower() == "you already cast that spell." && spell.Name.ToLower().Contains("fas"))
                                target.fased = true;
                            if (Regex.IsMatch(barMessage.Message, "Another curse afflicts thee. \\[[(a-zA-z\\s]*\\]"))
                                target.Cursed = true;
                            // use animation soon!!!!
                        }
                    }
                    this.Base.BarMessages.Clear();
                    this.Base.IsSwitchingItems = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return true;
        }
        #endregion

        private string FindUint32ClientID()
        {
            try
            {
                using (RegistryKey key32 = Registry.ClassesRoot.OpenSubKey("NXKRI.Ctrl.1"))
                {
                    if (key32 != null)
                    {
                        Object o = key32.GetValue("CLSID");


                        if (o != null)
                        {
                            switch (key32.GetValueKind("CLSID"))
                            {
                                case RegistryValueKind.String:
                                case RegistryValueKind.ExpandString:
                                    this.console.WriteLine("ValueE = " + o.ToString());
                                    break;
                                case RegistryValueKind.Binary:
                                    foreach (byte b in (byte[])o)
                                    {
                                        this.console.WriteLine( b.ToString());
                                    }
                                    //DAConsole.WriteLine();
                                    break;
                                case RegistryValueKind.DWord:
                                    this.console.WriteLine("ValueD = " + Convert.ToString((Int32)o));
                                    break;
                                case RegistryValueKind.QWord:
                                    this.console.WriteLine("ValueQ = " + Convert.ToString((Int64)o));
                                    break;
                                case RegistryValueKind.MultiString:
                                    foreach (string s in (string[])o)
                                    {
                                        this.console.WriteLine(s.ToString());
                                    }
                                    break;
                                default:
                                    this.console.WriteLine("Value = (Unknown)");
                                    break;
                            }
                            //"as" because it's REG_SZ...otherwise ToString() might be safe(r)
                            //do what you like with version
                            return Convert.ToString((Int32)o);//Convert.ToString(BigInteger.Parse(o as string));//
                        }
                    }
                }
            }
            catch (Exception ex)  //just for demonstration...it's always best to handle specific exceptions
            {
                //react appropriately

            }
            return "nothing";
        }
        private string FindUint16ClientID()
        {
            try
            {
                using (RegistryKey key16 = Registry.ClassesRoot.OpenSubKey("KRIHC.Ctrl.1"))
                {
                    if (key16 != null)
                    {
                        Object o = key16.GetValue("CLSID");
                        if (o != null)
                        {
                              //"as" because it's REG_SZ...otherwise ToString() might be safe(r)
                            //do what you like with version
                            switch (key16.GetValueKind("CLSID"))
                            {
                                case RegistryValueKind.String:
                                case RegistryValueKind.ExpandString:
                                    this.console.WriteLine("ValueE = " + o);
                                    break;
                                case RegistryValueKind.Binary:
                                    foreach (byte b in (byte[])o)
                                    {
                                        this.console.WriteLine(b.ToString());
                                    }
                                    break;
                                case RegistryValueKind.DWord:
                                    this.console.WriteLine("ValueD = " + Convert.ToString((Int32)o));
                                    break;
                                case RegistryValueKind.QWord:
                                    this.console.WriteLine("ValueQ = " + Convert.ToString((Int64)o));
                                    break;
                                case RegistryValueKind.MultiString:
                                    foreach (string s in (string[])o)
                                    {
                                        this.console.WriteLine(s.ToString());
                                    }
                                    break;
                                default:
                                    this.console.WriteLine("Value = (Unknown)");
                                    break;
                            }
                            return Convert.ToString((Int32)o);
                        }
                    }
                }
            }
            catch (Exception ex)  //just for demonstration...it's always best to handle specific exceptions
            {
                //react appropriately
                
            }
            return "nothing";
        }


        public static byte[] GetBigEndianBytes(UInt32 val, bool isLittleEndian)
        {
            UInt32 bigEndian = val;
           /* if (isLittleEndian)
            {
                bigEndian = (val & 0x000000FFU) << 24 | (val & 0x0000FF00U) << 8 |
                     (val & 0x00FF0000U) >> 8 | (val & 0xFF000000U) >> 24;
            }*/
            return BitConverter.GetBytes(bigEndian);
        }
        public static byte[] GetBigEndianBytes(UInt32 val)
        {
            return GetBigEndianBytes(val, BitConverter.IsLittleEndian);
        }

        public byte[] GenerateFake32Bits()
        {
            byte[] buffer = new byte[4];
            Random rand = new Random(Environment.TickCount);
            rand.NextBytes(buffer);
            this.console.WriteLine("random bytes: " + buffer.ToString());
            return buffer;
        }
        public int GenerateFake32Int(byte[] buffer)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            int i = BitConverter.ToInt32(buffer, 0);
            this.console.WriteLine("fakeInt32: " + i.ToString("X"));
            return i;
        }

        public int GenerateFake16Int(byte[] buffer)
        {
            ushort crc = NexonCRC16.Calculate(buffer, 0, buffer.Length);
            this.console.WriteLine("crc: " + crc.ToString("X"));
            return crc;
        }

        private void Login(string username, string password)
        {
            this.console.WriteLine("Logging in as " + username);
            var x03 = new ClientPacket(0x03);

            x03.WriteString8(username);
            x03.WriteString8(password);
            var rand = new Random();

            byte key1 = (byte)rand.Next();
            byte key2 = (byte)rand.Next();

            //uint clientId1 = (uint)int.Parse(FindUint32ClientID());
            //this.console.WriteLine("regkey: " + clientId1.ToString("X"));

            byte clientId1Key = (byte)(key2 + 138);
            

            //uint clientId2 = (uint)int.Parse(FindUint16ClientID());
            //UInt32 regKey = clientId2 ^ 0xAFAEADAC;
            //uint clientId2 = (uint)Int32.Parse(FindUint16ClientID());
            byte clientId2Key = (byte)(key2 + 94);

            //ushort temp1 = (ushort)(regKey & 0x000000FF);
            //ushort temp2 = (ushort)((regKey & 0x00FF0000) >> 8);

            byte[] bits = GenerateFake32Bits();
            uint clientId1 = (uint)GenerateFake32Int(bits);
            UInt16 clientId = (ushort)GenerateFake16Int(bits);
            //UInt16 clientId = (ushort)(temp1 + temp2);
            clientId1 ^= (uint)(clientId1Key++ | (clientId1Key++ << 8) | (clientId1Key++ << 16) | (clientId1Key << 24));
            clientId ^= (ushort)(clientId2Key++ | (clientId2Key << 8));
            //clientId ^= (ushort)((clientId2Key+1) << 8);

            //ushort clientIdUInt16 = (ushort)GenerateUint16ClientId(clientId2);

            uint randomVal = (ushort)rand.Next();
            byte randomValKey = (byte)(key2 + 115);
            randomVal ^= (uint)(randomValKey++ | (randomValKey++ << 8) | (randomValKey++ << 16) | (randomValKey << 24));
            

            x03.WriteByte(key1);
            x03.WriteByte((byte)(key2 ^ (key1 + 59)));


            x03.WriteUInt32(clientId1);
            x03.WriteUInt16((ushort)clientId);
            x03.WriteUInt32(randomVal);

            ushort crc = NexonCRC16.Calculate(x03.Data, username.Length + password.Length + 2, 12);
            byte crcKey = (byte)(key2 + 165);
            crc ^= (ushort)(crcKey++ | (crcKey << 8));

            x03.WriteUInt16(crc);
            x03.WriteUInt16(0x0100);

            this.console.WriteLine("\nSend ");
            this.console.WriteLine("uint32: " + clientId1.ToString("X") + " uint16: " + clientId.ToString("X"));
            this.console.WriteLine(x03.ToString());
            Send(x03);
        }

        #region GenerateKey
        public byte[] GenerateKey(ushort bRand, byte sRand)
        {
            var key = new byte[9];

            for (var i = 0; i < 9; ++i)
            {
                key[i] = KeyTable[(i * (9 * i + sRand * sRand) + bRand) % 1024];
            }

            return key;
        }
        public string generateKey(ushort a, byte b)
        {
            char[] key = new char[9];

            for (var i = 0; i < 9; ++i)
            {
                int saltIndex = (i * (9 * i + b * b) + a) % keySalt.Length;
                key[i] = keySalt[saltIndex];
            }

            return new string(key);
        }

        public static string GenerateKeySalt(string seed)
        {
            string keySalt;
            keySalt = GetHashString(seed, "MD5");
            keySalt = GetHashString(keySalt, "MD5");
            for (var i = 0; i < 31; i++)
            {
                keySalt += GetHashString(keySalt, "MD5");
            }
            return keySalt;
        }

        public static string GetHashString(string value, string hashName)
        {
            var algo = HashAlgorithm.Create(hashName);
            var buffer = Encoding.ASCII.GetBytes(value);
            var hash = algo.ComputeHash(buffer);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        #endregion
    }

    public class ServerInfo
    {
        private const string darkages_ip = "52.88.55.94";
        public IPAddress Address { get; private set; }
        public int Port { get; private set; }
        public string Name { get; private set; }
        public string FriendlyName { get; private set; }
        private ServerInfo(IPAddress address, int port, string name, string friendlyName)
        {
            Address = address;
            Port = port;
            Name = name;
            FriendlyName = friendlyName;
        }
        public static ServerInfo FromIPEndPoint(IPEndPoint endPoint)
        {
            return FromIPAddress(endPoint.Address, endPoint.Port);
        }
        public static ServerInfo FromIPAddress(IPAddress address, int port)
        {
            string endPointStr = string.Format("{0}:{1}", address, port);
            switch (endPointStr)
            {
                case darkages_ip+":2610": return Login;
                case darkages_ip+":2615": return Temuair;
                case darkages_ip+":2617": return Medenia;
            }
            return new ServerInfo(address, port, endPointStr, endPointStr);
        }
        public static readonly ServerInfo Login = new ServerInfo(IPAddress.Parse(darkages_ip), 2610, "Login Server", "Login Server");
        public static readonly ServerInfo Temuair = new ServerInfo(IPAddress.Parse(darkages_ip), 2615, "Temuair Server", "Temuair");
        public static readonly ServerInfo Medenia = new ServerInfo(IPAddress.Parse(darkages_ip), 2617, "Medenia Server", "Medenia");
    }
}
