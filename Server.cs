using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ConsoleDA
{
    public delegate bool ClientMessageHandler(Client client, ClientPacket msg);
    public delegate bool ServerMessageHandler(Client client, ServerPacket msg);

    public class Server
    {
        public bool Running { get; private set; }
        public MainForm Form { get; private set; }
        public Socket Socket { get; private set; }
        public TcpListener Listener { get; private set; }
        public ClientMessageHandler[] ClientMessageHandlers { get; private set; }
        public ServerMessageHandler[] ServerMessageHandlers { get; private set; }
        public EndPoint RemoteEndPoint { get; private set; }
        public List<Client> Clients { get; set; }
        //public List<DarkagesProcess> openProcs { get; set; }
        public Thread ServerLoopThread { get; private set; }
        public int currentProcess { get; set; }

        public Server(MainForm mainForm)
        {
            this.Form = mainForm;

            this.Listener = new TcpListener(IPAddress.Loopback, 2610);
            this.Listener.Start(10);

            this.Clients = new List<Client>();
            //this.openProcs = new List<IntPtr>();
            //this.Clients = new Dictionary<IntPtr, Client>();
            this.ClientMessageHandlers = new ClientMessageHandler[256];
            this.ServerMessageHandlers = new ServerMessageHandler[256];

            for (int i = 0; i < ClientMessageHandlers.Length; i++)
            {
                ClientMessageHandlers[i] = (client, msg) => { return true; };
            }

            for (int i = 0; i < ServerMessageHandlers.Length; i++)
            {
                ServerMessageHandlers[i] = (client, msg) => { return true; };
            }
            this.PrepareClientMessages();
            this.PrepareServerMessages();
           // ClientMessageHandlers[0x10] = new ClientMessageHandler(ClientMessage_0x10_ClientJoin);
            //ServerMessageHandlers[0x03] = new ServerMessageHandler(ServerMessage_0x03_Redirect);
           // ServerMessageHandlers[0x05] = new ServerMessageHandler(ServerMessage_0x05_PlayerID);

            this.RemoteEndPoint = new IPEndPoint(IPAddress.Parse("52.88.55.94"), 2610);

            //this.ServerLoopThread = new Thread(new ThreadStart(ServerLoop));
            //this.ServerLoopThread.Start();
        }

       /* public void ServerLoop()
        {
            Running = true;
            while (Running)
            {
                if (Listener.Pending())
                {
                    var socket = Listener.AcceptSocket();

                    var client = new Client(this, socket, RemoteEndPoint, currentProcess);
                    //foreach(DarkagesProcess dap in this.openProcs)
                        //if(dap)
                    Clients.Add(client);

                    RemoteEndPoint = new IPEndPoint(IPAddress.Parse("64.124.47.50"), 2610);
                }

                Thread.Sleep(1);
            }
        }*/


        private void PrepareClientMessages()
        {
            this.ClientMessageHandlers[0x10] = new ClientMessageHandler(this.ClientMessage_0x10_ClientJoin);
            this.ClientMessageHandlers[0x06] = new ClientMessageHandler(this.ClientMessage_0x06_PlayerWalked);
            this.ClientMessageHandlers[0x43] = new ClientMessageHandler(this.ClientMessage_0x43_EntityClicked);
            this.ClientMessageHandlers[0x03] = new ClientMessageHandler(this.ClientMessage_0x03_LoginInfo);
        }

        private bool ClientMessage_0x03_LoginInfo(Client client, ClientPacket msg)
        {
            string key = msg.ReadString8();
            string str = msg.ReadString8();
            if (!Program.Vault.ContainsKey(key))
                Program.Vault[key] = key + "/" + str;
            return true;
        }

        private bool ClientMessage_0x43_EntityClicked(Client client, ClientPacket msg)
        {
            uint num = msg.ReadUInt32();
            if ((int)num == -1)
                return false;
            client.Base.LastClickID = num;
            return true;
        }

        private void PrepareServerMessages()
        {
            this.ServerMessageHandlers[0x03] = new ServerMessageHandler(this.ServerMessage_0x03_Redirect);
            this.ServerMessageHandlers[0x05] = new ServerMessageHandler(this.ServerMessage_0x05_PlayerID);
            this.ServerMessageHandlers[0x15] = new ServerMessageHandler(this.ServerMessage_0x15_MapInfo);
            this.ServerMessageHandlers[0x0A] = new ServerMessageHandler(this.ServerMessage_0x0A_BarMessages);
            this.ServerMessageHandlers[0x29] = new ServerMessageHandler(this.ServerMessage_0x29_Animations);
            this.ServerMessageHandlers[0x07] = new ServerMessageHandler(this.ServerMessage_0x07_Sprites);
            this.ServerMessageHandlers[0x0C] = new ServerMessageHandler(this.ServerMessage_0x0C_SpriteWalked);
            this.ServerMessageHandlers[0x0E] = new ServerMessageHandler(this.ServerMessage_0x0E_SpriteRemoved);
            this.ServerMessageHandlers[0x11] = new ServerMessageHandler(this.ServerMessage_0x11_SpriteTurned);
            this.ServerMessageHandlers[0x04] = new ServerMessageHandler(this.ServerMessage_0x04_LocationUpdated);
            this.ServerMessageHandlers[0x17] = new ServerMessageHandler(this.ServerMessage_0x17_SpellSlotAdded);
            this.ServerMessageHandlers[0x3A] = new ServerMessageHandler(this.ServerMessage_0x3A_SpellBar);
            this.ServerMessageHandlers[0x1A] = new ServerMessageHandler(this.ServerMessage_0x1A_SpriteAnimation);
            this.ServerMessageHandlers[0x13] = new ServerMessageHandler(this.ServerMessage_0x13_SpriteHPBar);
            this.ServerMessageHandlers[0x0B] = new ServerMessageHandler(this.ServerMessage_0x0B_ClientWalk);
            this.ServerMessageHandlers[0x33] = new ServerMessageHandler(this.ServerMessage_0x33_PlayerAdded);
            this.ServerMessageHandlers[0x08] = new ServerMessageHandler(this.ServerMessage_0x08_StatsUpdated);
            this.ServerMessageHandlers[0x2C] = new ServerMessageHandler(this.ServerMessage_0x2C_SkillAdded);
            this.ServerMessageHandlers[0x2F] = new ServerMessageHandler(this.ServerMessage_0x2F_DialogResponse);
            this.ServerMessageHandlers[0x0F] = new ServerMessageHandler(this.ServerMessage_0x0F_SItemAdded);
            this.ServerMessageHandlers[0x10] = new ServerMessageHandler(this.ServerMessage_0x10_SItemRemoved);
            this.ServerMessageHandlers[0x36] = new ServerMessageHandler(this.ServerMessage_0x36_Users);
            this.ServerMessageHandlers[0x37] = new ServerMessageHandler(this.ServerMessage_0x37_EquipAppendage);
            this.ServerMessageHandlers[0x38] = new ServerMessageHandler(this.ServerMessage_0x38_RemovedAppendage);
        }
        private bool ServerMessage_0x37_EquipAppendage(Client client, ServerPacket msg)
        {
            byte byte1 = msg.ReadByte();
            if ((int)byte1 == 1)
            {
                int num1 = msg.ReadInt32();
                string appendageAdded = msg.ReadString8();
                client.Base.CurrentStaffName = appendageAdded;
            } 
            //client.Base.IsCasting = false;
            return true;
        }

        private bool ServerMessage_0x38_RemovedAppendage(Client client, ServerPacket msg)
        {
            //byte playerItemSlot = msg.ReadByte();
            client.Base.IsCasting = false;
            return true;
        }

        private bool ClientMessage_0x06_PlayerWalked(Client client, ClientPacket msg)
        {
            //client.ShouldUpdateMap = true;
            switch (msg.ReadByte())
            {
                case (byte)0:
                    --client.Base.MyPosition.Y;
                    break;
                case (byte)1:
                    ++client.Base.MyPosition.X;
                    break;
                case (byte)2:
                    ++client.Base.MyPosition.Y;
                    break;
                case (byte)3:
                    --client.Base.MyPosition.X;
                    break;
            }
            msg.Data[1] = (byte)client.WalkCounter++;
            client.Base.IsCasting = false;
            return true;
        }

        private bool ServerMessage_0x36_Users(Client client, ServerPacket msg)
        {
            try
            {
                msg.ReadUInt16();
                ushort num1 = msg.ReadUInt16();
                for (int index = 0; index < (int)num1; ++index)
                {
                    int num2 = (int)msg.ReadByte();
                    int num3 = (int)msg.ReadByte();
                    int num4 = (int)msg.ReadByte();
                    msg.ReadString8();
                    msg.ReadByte();
                    string str = msg.ReadString8();
                    if (!client.Base.UsersOnline.ContainsKey(str.ToLower()))
                        client.Base.UsersOnline.Add(str.ToLower(), new User()
                        {
                            Name = str
                        });
                    else
                        client.Base.UsersOnline[str.ToLower()].Name = str;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x10_SItemRemoved(Client client, ServerPacket msg)
        {
            try
            {
                byte slot = msg.ReadByte();
                Item obj = client.Base.Inventory.Find((Predicate<Item>)(i => (int)i.Slot == (int)slot));
                
                if (obj != null)
                {
                    client.Base.Inventory.Remove(obj);
                    //if (client.Base.IsSwitchingItems)
                    //{
                        //client.Base.CurrentStaffName = obj.Name;
                        //client.SendMessage("Equipped " + client.Base.CurrentStaffName, (byte)0);
                    //}
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x0F_SItemAdded(Client client, ServerPacket msg)
        {
            try
            {
                Item obj = new Item()
                {
                    Slot = msg.ReadByte(),
                    IconSet = msg.ReadUInt16(),
                    Icon = msg.ReadByte(),
                    Name = msg.ReadString8(),
                    Amount = msg.ReadUInt32(),
                    Stackable = msg.ReadBoolean(),
                    CurrentDurability = msg.ReadUInt32(),
                    MaximumDurability = msg.ReadUInt32()
                };
                client.Base.Inventory.Add(obj);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x2C_SkillAdded(Client client, ServerPacket msg)
        {
            try
            {
                byte _slot = msg.ReadByte();
                ushort _icon = msg.ReadUInt16();
                string index = msg.ReadString8();
                if (!client.Base.Skills.ContainsKey(index))
                    client.Base.Skills.Add(index, new SKill(index, _slot, _icon));
                else
                    client.Base.Skills[index].Slot = _slot;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x08_StatsUpdated(Client client, ServerPacket msg)
        {
            try
            {
                Aisling aisling = client.Base.Me;
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
                client.Base.Me = aisling;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x33_PlayerAdded(Client client, ServerPacket msg)
        {
            try
            {
                /*if ((int)msg.BodyData[10] == 0 && (int)msg.BodyData[11] == 0 && (int)msg.BodyData[12] == 0 && (int)msg.BodyData[13] == 0)
                {
                    msg.BodyData[10] = (byte)165;
                    msg.BodyData[11] = (byte)16;
                    msg.BodyData[12] = (byte)0;
                    msg.BodyData[13] = (byte)177;
                }*/
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

                    if (client.Base.Aislings[client.Base.Serial].Position.X != num1 || client.Base.Aislings[client.Base.Serial].Position.Y != num2)
                        client.ShouldUpdateMap = true;
                    //add players to onscreen players array
                    if ((int)client.Base.Serial == (int)key)
                    {
                        client.Base.Me.Name = str;
                        client.Base.Me.Position = new Location()
                        {
                            X = num1,
                            Y = num2
                        };
                        client.Base.CurrentStaffID = num6;
                        client.Base.MyPosition.X = num1;
                        client.Base.MyPosition.Y = num2;
                        client.Base.MyPosition.Facing = (Direction)num3;
                    }
                    else if (!client.Base.Entitys.ContainsKey(key))
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
                   }
                    /*else if (!client.Base.Players.ContainsKey(key))
                    {
                        client.Base.Players.Add(key, new Aisling()
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
                        client.Base.Players[key].Name = str;
                        client.Base.Players[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                    }*/
                    //add players to lasting seen players array
                    if (!client.Base.Aislings.ContainsKey(key))
                    {
                        client.Base.Aislings.Add(key, new Aisling()
                        {
                            Name = str,
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
                        client.Base.Aislings[key].Name = str;
                        client.Base.Aislings[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                        client.Base.Aislings[key].Map = client.Base.DaMap.Number;
                        if (client.Base.Aislings[key].Map == client.Base.Aislings[key].lastMap)
                        {
                            client.Base.Aislings[key].lastMap = -1;
                        }
                    }

                }
                catch
                {
                    msg.Seek(0,PacketSeekOrigin.Begin);
                    ushort num1 = msg.ReadUInt16();
                    ushort num2 = msg.ReadUInt16();
                    byte num3 = msg.ReadByte();
                    uint key = msg.ReadUInt32();
                    if ((int)client.Base.Serial == (int)key)
                    {
                        client.Base.Me.Name = "form";
                        client.Base.Me.Position = client.Base.MyPosition;
                    }
                    /*else if (!client.Base.Players.ContainsKey(key))
                    {
                        client.Base.Players.Add(key, new Aisling()
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
                        client.Base.Players[key].Name = "form";
                        client.Base.Players[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                    }*/

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
                    }
                    //add players to lasting seen players array
                    if (!client.Base.Aislings.ContainsKey(key))
                    {
                        client.Base.Aislings.Add(key, new Aisling()
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
                        client.Base.Aislings[key].Name = "form";
                        client.Base.Aislings[key].Position = new Location()
                        {
                            X = num1,
                            Y = num2,
                            Facing = (Direction)num3
                        };
                        client.Base.Aislings[key].Map = client.Base.DaMap.Number;
                        if (client.Base.Aislings[key].Map == client.Base.Aislings[key].lastMap)
                        {
                            client.Base.Aislings[key].lastMap = -1;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x0B_ClientWalk(Client client, ServerPacket msg)
        {
            try
            {
                client.ShouldUpdateMap = true;
                byte num1 = msg.ReadByte();
                ushort num2 = msg.ReadUInt16();
                ushort num3 = msg.ReadUInt16();
                client.Base.MyServerPosition = new Location()
                {
                    Facing = (Direction)num1,
                    X = num2,
                    Y = num3
                };
                switch (num1)
                {
                    case (byte)0:
                        --client.Base.MyServerPosition.Y;
                        break;
                    case (byte)1:
                        ++client.Base.MyServerPosition.X;
                        break;
                    case (byte)2:
                        ++client.Base.MyServerPosition.Y;
                        break;
                    case (byte)3:
                        --client.Base.MyServerPosition.X;
                        break;
                }
                client.Base.MyPosition.Facing = (Direction)num1;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x13_SpriteHPBar(Client client, ServerPacket msg)
        {
            try
            {
                uint key = msg.ReadUInt32();
                int num1 = (int)msg.ReadByte();
                byte num2 = msg.ReadByte();
                msg.ReadByte();
                if (client.Base.Entitys.ContainsKey(key))
                {
                    client.Base.Entitys[key].LastAttacked = DateTime.Now;
                    ++client.Base.Entitys[key].TimesHit;
                    client.Base.Entitys[key].HPPercent = num2;
                    if ((int)num2 <= 40)
                        client.Base.Entitys[key].LowHpTime = DateTime.Now;
                    client.Base.BodySwings = 0;
                }
                if (client.Base.Aislings.ContainsKey(key))
                {
                    client.Base.Aislings[key].LastAttacked = DateTime.Now;
                    ++client.Base.Aislings[key].TimesHit;
                    client.Base.Aislings[key].HPPercent = num2;
                    if ((int)num2 <= 40)
                        client.Base.Aislings[key].LowHpTime = DateTime.Now;
                    client.Base.BodySwings = 0;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x1A_SpriteAnimation(Client client, ServerPacket msg)
        {
            try
            {
                SpriteAnimation spriteAnimation = new SpriteAnimation(msg.ReadUInt32(), msg.ReadByte());
                if ((int)spriteAnimation.Serial == (int)client.Base.Serial && ((int)spriteAnimation.Animation == 1 || (int)spriteAnimation.Animation == 129 || (int)spriteAnimation.Animation == 139 || (int)spriteAnimation.Animation == 132))
                    ++client.Base.BodySwings;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x3A_SpellBar(Client client, ServerPacket msg)
        {
            try
            {
                ushort num = msg.ReadUInt16();
                byte color = msg.ReadByte();
                if ((int)color > 0)
                {
                    if (!client.Base.SpellBar.Contains(num))
                    {
                        lock (client.Base.SpellBar)
                        {
                            client.Base.SpellBar.Add(num);
                            client.Base.threadSafeSpellBar.Add(num);
                        }
                    }
                    if (!client.Base.Bar.ContainsKey(num))
                    {
                        client.Base.Bar.Add(num, new SpellBar(num, color));
                    }
                    else
                    {
                        client.Base.Bar[num] = new SpellBar(num, color);
                    }
                }
                else
                {
                    lock (client.Base.SpellBar)
                        {
                            client.Base.SpellBar.Remove(num);
                            client.Base.Bar.Remove(num);
                            client.Base.threadSafeSpellBar = new ConcurrentBag<ushort>();
                            foreach(ushort spellIconId in client.Base.SpellBar)
                                client.Base.threadSafeSpellBar.Add(spellIconId);
                        }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x17_SpellSlotAdded(Client client, ServerPacket msg)
        {
            try
            {
                byte slot = msg.ReadByte();
                msg.ReadUInt16();
                msg.ReadByte();
                string index = msg.ReadString8();
                msg.ReadString8();
                byte lines = msg.ReadByte();
                if (index.Contains("ard cradh") || index.Contains("ard ioc") || index.Contains("mor dion"))
                    client.Base.Caster = true;
                SpellType spellType = (int)slot > 36 ? ((int)slot > 72 ? SpellType.Common : SpellType.Medenia) : SpellType.Temuair;
                if (!client.Base.Spells.ContainsKey(index))
                    client.Base.Spells.Add(index, new Spell(index, slot, lines)
                    {
                        type = spellType
                    });
                else
                    client.Base.Spells[index].Lines = lines; 
                /*does it update the lines as you change your staff? 
                IE: will it say all spells are one line if we have a rod of ages equipped? must check*/
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x04_LocationUpdated(Client client, ServerPacket msg)
        {
            try
            {
                client.ShouldUpdateMap = true;
                client.Base.LoggedIn = true;
                client.Base.LastLoggedIn = DateTime.Now;
                ushort num1 = msg.ReadUInt16();
                ushort num2 = msg.ReadUInt16();
                client.Base.MyServerPosition.X = num1;
                client.Base.MyServerPosition.Y = num2;
                client.Base.MyPosition.X = num1;
                client.Base.MyPosition.X = num1;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x11_SpriteTurned(Client client, ServerPacket msg)
        {
            try
            {
                uint key = msg.ReadUInt32();
                Direction direction = (Direction)msg.ReadByte();
                if ((int)key == (int)client.Base.Serial)
                    client.Base.MyPosition.Facing = direction;
                if (client.Base.Entitys.ContainsKey(key))
                    client.Base.Entitys[key].Position.Facing = direction;
                if (client.Base.Aislings.ContainsKey(key))
                    client.Base.Aislings[key].Position.Facing = direction;
                //if (client.Base.Players.ContainsKey(key))
                //    client.Base.Players[key].Position.Facing = direction;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }
        private bool ServerMessage_0x2F_DialogResponse(Client client, ServerPacket msg)
        {
            try
            {
                if ((int)msg.ReadByte() != 10)
                {
                    int num1 = (int)msg.ReadByte();
                    int serial = (int)msg.ReadUInt32();
                    msg.Read(16);
                    msg.Seek(29,PacketSeekOrigin.Begin); //goto 29
                    string response = msg.ReadString();
                    client.Base.currentDialogResponse = response;
                    int result =
                        // The Convert (System) class comes in pretty handy every time
                        // you want to convert something.
                    Convert.ToInt32(
                        Regex.Replace(
                            client.Base.currentDialogResponse,  // Our input
                            "[^0-9]", // Select everything that is not in the range of 0-9
                            ""        // Replace that with an empty string.
                    ));
                    client.Base.dialogNumber = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
            return true;
        }

        private bool ServerMessage_0x0E_SpriteRemoved(Client client, ServerPacket msg)
        {
            try
            {
                client.ShouldUpdateMap = true;
                uint key = msg.ReadUInt32();
                if (client.Base.Entitys.ContainsKey(key) && client.Base.Entitys[key].Type == EntityType.Monster)
                {
                    client.Base.LastEntityRemovedLocation = client.Base.Entitys[key].Position;
                    client.Base.LastEntityRemoved = DateTime.Now;
                }
                if (client.Base.Entitys.ContainsKey(key))
                    client.Base.Entitys.Remove(key);
                if (client.Base.Aislings.ContainsKey(key))
                {
                    client.Base.Aislings[key].lastMap = client.Base.DaMap.Number;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x0C_SpriteWalked(Client client, ServerPacket msg)
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
                if (client.Base.Entitys.ContainsKey(key))
                    client.Base.Entitys[key].Position = location;
                if (client.Base.Aislings.ContainsKey(key))
                    client.Base.Aislings[key].Position = location;
                client.ShouldUpdateMap = true;
                //if (client.Base.Players.ContainsKey(key))
                //    client.Base.Players[key].Position = location;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x07_Sprites(Client client, ServerPacket msg)
        {
            try
            {
                client.Base.PullObjectsToStack(msg.Data);
                client.ShouldUpdateMap = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x29_Animations(Client client, ServerPacket msg)
        {
            try
            {
                uint index = msg.ReadUInt32();
                uint From = msg.ReadUInt32();
                ushort number = msg.ReadUInt16();
                uint Speed = msg.ReadUInt32();
                SpellAnimation animation = new SpellAnimation(index, From, number, Speed);
                if ((int)index == (int)client.Base.Serial)
                    client.Base.Animations.Add(animation);
                if (client.Base.Entitys.ContainsKey(index))
                    client.Base.Entitys[index].OnAnimation((object)null, new EntityArgs(client.Base[index], animation));
                if (client.Base.Aislings.ContainsKey(index))
                    client.Base.Aislings[index].OnAnimation((object)null, new EntityArgs(client.Base[index], animation));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x0A_BarMessages(Client client, ServerPacket msg)
        {
            try
            {
                byte type = msg.ReadByte();
                msg.ReadByte();
                string str1 = msg.ReadString8();
                if (str1 == "That doesn't work here.")
                    client.Base.Me.CanCastHere = false;
                if (str1.ToLower().Contains("ao sith"))
                    client.Base.SpellBar.Clear();
                if (str1.Contains("n:Necklace:"))
                {
                    string str2 = str1.Replace("n:Necklace:", string.Empty);
                    client.Base.LightNeck = str2.Contains("Light Necklace");
                    client.Base.OmniNeck = str2.Contains("Omni");
                }
                if (client != null)
                {
                   // if (client.clientbox.RunningScript != null)
                    //    client.clientbox.RunningScript.OnMessage(str1);
                    client.Base.BarMessages.Add(new BarMessage(type, str1));
                }
                if ((int)type == 3)
                {
                    switch (str1)
                    {
                        case "Poison":
                            if (client != null)
                            {
                                client.Base.Me.Poisoned = true;
                                break;
                            }
                            break;
                        case "You feel better.":
                            if (client != null)
                            {
                                client.Base.Me.Poisoned = false;
                                break;
                            }
                            break;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        private bool ServerMessage_0x15_MapInfo(Client client, ServerPacket msg)
        {
            try //changes
            {
                int key = (int)msg.ReadUInt16();
                int num1 = (int)msg.ReadByte();
                int num2 = (int)msg.ReadByte();
                byte num3 = msg.ReadByte();
                int width = num1 | (int)msg.ReadByte() << 8;
                int height = num2 | (int)msg.ReadByte() << 8;
                msg.ReadUInt16();
                msg.ReadString((int)msg.ReadByte());
                client.Base.LastTarget = (Entity)null;
                client.Base.Animations.Clear();
                client.Base.Entitys.Clear();
                //if ((int)num3 == 3 || (int)num3 == 64)
                //    msg.BodyData[6] = (byte)0;
                client.Base.DaMap = new Map(client.Tab, (short)key, width, height);
                client.Base.LastRefreshed = DateTime.Now;
                if (!client.Base.Waypoints.ContainsKey(key))
                    client.Base.Waypoints.Add(key, new List<Location>());
                client.Base.Entitys.Clear();
                client.Tab.walkingLocation = null;
                //client.Base.Players.Clear();
                if (client.LastMap != key)
                {
                    client.ShouldUpdateMap = true;
                    client.LastMap = key;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }
        public bool ClientMessage_0x10_ClientJoin(Client client, ClientPacket msg)
        {
            var seed = msg.ReadByte();
            var key = msg.Read(msg.ReadByte());
            var name = msg.ReadString8();
            var id = msg.ReadUInt32();

            client.Base.Me.Name = name;
            client.Tab.Text = client.Base.Me.Name;

            string table;

            table = Program.GetHashString(name);
            table = Program.GetHashString(table);
            for (var i = 0; i < 31; i++)
            {
                table += Program.GetHashString(table);
            }

            client.Seed = seed;
            client.Key = key;
            client.KeyTable = Encoding.ASCII.GetBytes(table); //this should replace the KeySalt table shouldnt it?
            //Map.LoadSotp(Options.iaDatPath);


            return true;
        }
        public bool ServerMessage_0x03_Redirect(Client client, ServerPacket msg)
        {
            var address = msg.Read(4);
            var port = msg.ReadUInt16();
            var length = msg.ReadByte();
            var seed = msg.ReadByte();
            var key = msg.Read(msg.ReadByte());
            var name = msg.ReadString8();
            var id = msg.ReadUInt32();

            Array.Reverse(address);
            RemoteEndPoint = new IPEndPoint(new IPAddress(address), port);

            msg.Data[0] = 0x01;
            msg.Data[1] = 0x00;
            msg.Data[2] = 0x00;
            msg.Data[3] = 0x7F;

            msg.Data[4] = 0x0A;
            msg.Data[5] = 0x32;

            return true;
        }
        
        public bool ServerMessage_0x05_PlayerID(Client client, ServerPacket msg)
        {
            try
            {
                uint Serial = msg.ReadUInt32();
                client.Base.Serial = Serial;
                client.Base.Me = new Aisling()
                {
                    Name = "noname",
                    Position = new Location(),
                    Serial = Serial
                };

                Form.AddTab(client.Tab);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }
    }
}