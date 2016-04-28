// Decompiled with JetBrains decompiler
// Type: Dean.DarkagesBase
// Assembly: Dean, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Dean\Dean.exe

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
//using System.ServiceModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleDA
{
    public class DarkagesBase
    {
        public Dictionary<ushort, SpellAnimation> AllAnimations = new Dictionary<ushort, SpellAnimation>();
        public List<SpellAnimation> Animations = new List<SpellAnimation>();
        public Dictionary<ushort, ConsoleDA.SpellBar> Bar = new Dictionary<ushort, ConsoleDA.SpellBar>();
        public List<BarMessage> BarMessages = new List<BarMessage>();
        public Dictionary<int,List<Location>> Blocks = new Dictionary<int,List<Location>>();
        public bool Caster = false;
        public Dictionary<uint, Entity> Entitys = new Dictionary<uint, Entity>();
        public List<ConsoleDA.Item> Inventory = new List<ConsoleDA.Item>();
        public string currentDialogResponse { get; set; }
        public int dialogNumber { get; set; }
        
        public Dictionary<uint, Aisling> Aislings = new Dictionary<uint, Aisling>();
        public Dictionary<uint, Aisling> Players = new Dictionary<uint, Aisling>();
        public Dictionary<string, SKill> Skills = new Dictionary<string, SKill>();
        public List<ushort> SpellBar = new List<ushort>();
        public Dictionary<string, Spell> Spells = new Dictionary<string, Spell>();
        public Dictionary<string, User> UsersOnline = new Dictionary<string, User>();
        public Location LastEntityRemovedLocation = new Location();
        public Location LastPlayerRemovedLocation = new Location();

        public ConcurrentBag<ushort> threadSafeSpellBar = new ConcurrentBag<ushort>();
        public ConcurrentDictionary<uint, Entity> threadSafeEntitys = new ConcurrentDictionary<uint, Entity>();
        public ConcurrentDictionary<uint, Aisling> threadSafeAislings = new ConcurrentDictionary<uint,Aisling>();
        
        public bool LoggedIn = false;
        public Aisling Me = new Aisling();
        public Location MyPosition = new Location();
        public Location MyServerPosition = new Location();
        public int WaypointIndex = 0;
        public int MapIndex = 0;
        public Dictionary<int, List<Location>> Waypoints = new Dictionary<int, List<Location>>();
        public Dictionary<int, List<Location>> Doors = new Dictionary<int, List<Location>>();

        public ConsoleDA.Map DaMap { get; set; }

        public Client client { get; set; }

        public uint Serial { get; set; }

        public DateTime LastLoggedIn { get; set; }

        public DateTime LastRefreshed { get; set; }

        public int BodySwings { get; set; }

        public int Swings { get; set; }

        public Entity CurrentTarget { get; set; }

        public Entity LastTarget { get; set; }

        public DateTime LastDionAttempt { get; set; }

        public DateTime LastEntityRemoved { get; set; }

        public bool LightNeck { get; set; }

        public bool OmniNeck { get; set; }

        public bool Casting { get; set; }

        public ushort CurrentStaffID { get; set; }

        public string CurrentStaffName = "no name";

        public bool IsRefreshing
        {
            get
            {
                return DateTime.Now - this.LastRefreshed < new TimeSpan(0, 0, 0, 0, 500);
            }
        }

        public bool IsSwitchingItems { get; set; }

        public bool IconDebuffStatus()
        {
            lock (this.threadSafeSpellBar)
            {
                return (Enumerable.Count<ushort>(Enumerable.Where<ushort>((IEnumerable<ushort>)this.threadSafeSpellBar, (Func<ushort, bool>)
                        (i => (ushort)i == (ushort)spellIcon.cradh
                        || (ushort)i == (ushort)spellIcon.mor_cradh
                        || (ushort)i == (ushort)spellIcon.ard_cradh
                        || (ushort)i == (ushort)spellIcon.Wolf_Fang_Fist
                        || (ushort)i == (ushort)spellIcon.Sleep
                        || (ushort)i == (ushort)spellIcon.Poison
                        || (ushort)i == (ushort)spellIcon.Dark_Seal)
                        )) > 0);
            }
        }

        public bool IconBuffStatus()
        {
            lock (this.threadSafeSpellBar)
            {
                return Enumerable.All<ushort>((IEnumerable<ushort>)this.threadSafeSpellBar, (Func<ushort, bool>)
                    (i => (ushort)i == (ushort)spellIcon.naomh_aite
                    || (ushort)i == (ushort)spellIcon.fas_nadur
                    || (ushort)i == (ushort)spellIcon.beag_cradh));
            }
        }
        public Entity FindNpcByNamedArray(string name)
        {
            Entity[] entityArray = new Entity[Entitys.Count];
            Array.Copy((Array)Enumerable.ToArray<Entity>((IEnumerable<Entity>)Entitys.Values), (Array)entityArray, entityArray.Length);

            lock (Program.SyncObj)
            {
                foreach (Entity item_1 in entityArray)
                    if (item_1.Name.Equals(name) && item_1.Type == EntityType.NPC)
                    {
                        return item_1;
                    }
            }
            return null;
        }

        public Aisling FindPlayerByNamedArray(string name)
        {
            Entity[] aislingArray = new Entity[Entitys.Count];
            Array.Copy((Array)Enumerable.ToArray<Entity>((IEnumerable<Entity>)Entitys.Values), (Array)aislingArray, aislingArray.Length);

            lock (Program.SyncObj)
            {
                foreach (Entity item_1 in aislingArray)
                    if(item_1 != null)
                        if(item_1.Name != null)
                            if (item_1.Name.Equals(name))
                            {
                                return item_1 as Aisling;
                            }
            }
            return null;
        }

        public T FindPlayerByName<T>(string name) where T : Aisling
        {
            lock (this.Players)
            {
                foreach (Entity item_0 in this.Players.Values)
                {
                    if (item_0 != null && (item_0.Name.Equals(name)))
                        return (T)item_0;
                }
            }
            return default(T);
        }

        public T FindNPCByName<T>(string name) where T : Entity
        {
            lock (this.Entitys)
            {
                foreach (Entity item_0 in this.Entitys.Values)
                {
                    if(item_0 != null)
                        if (item_0.Type == EntityType.NPC)
                            if(item_0.Name.Equals(name))
                                return (T)item_0;
                }
            }
            return default(T);
        }

        public Tile[,] Map
        {
            get
            {
                if (!this.LoggedIn || this.DaMap == null)
                    return (Tile[,])null;
                this.DaMap.Matrix = new Tile[this.DaMap.Width/* + 1*/, this.DaMap.Height/* + 1*/];
                try
                {
                    for (int index1 = 0; index1 < this.DaMap.Height/*- 1*/ ; ++index1)
                    {
                        for (int index2 = 0; index2 < this.DaMap.Width/*- 1*/; ++index2)
                            this.DaMap.Matrix[index2, index1] = this.DaMap.Grid[index2, index1];
                    }
                    Entity[] entityArray = new Entity[this.Entitys.Count];
                    Array.Copy((Array)Enumerable.ToArray<Entity>((IEnumerable<Entity>)this.Entitys.Values), (Array)entityArray, entityArray.Length);
                    
                    //becuase of the way we changed the player array to being the aisling array (ie it keeps track of all players, and  players
                    //screen are kept track of as entities we may need to do some changes to the way entities are read
                    //this could be as simple as a check that the entity isn't in the peristing players array
                    //dont forget the goal is to build a library of player and entity data
                    
                    Aisling[] aislingArray = new Aisling[this.Aislings.Count];
                    Array.Copy((Array)Enumerable.ToArray<Aisling>((IEnumerable<Aisling>)this.Aislings.Values), (Array)aislingArray, aislingArray.Length);
                    
                    lock (Program.SyncObj)
                    {
                        try
                        {
                            foreach (Entity item_2 in entityArray)
                            {
                                if (item_2.Type == EntityType.Monster)
                                    this.DaMap.Matrix[(int)item_2.Position.X, (int)item_2.Position.Y] = Tile.Monster;
                                if (item_2.Type == EntityType.NPC)
                                    this.DaMap.Matrix[(int)item_2.Position.X, (int)item_2.Position.Y] = Tile.Npc;
                                if (item_2.Type == EntityType.Pet)
                                    this.DaMap.Matrix[(int)item_2.Position.X, (int)item_2.Position.Y] = Tile.Empty;
                                if (item_2.Type == EntityType.SpecialMonster)
                                    this.DaMap.Matrix[(int)item_2.Position.X, (int)item_2.Position.Y] = Tile.Special;
                                if (item_2.Type == EntityType.Item)
                                    this.DaMap.Matrix[(int)item_2.Position.X, (int)item_2.Position.Y] = Tile.Empty;
                            }
                        }
                        catch
                        {
                        }
                    }
                    lock (Program.SyncObj)
                    {
                        foreach (Client item_3 in this.client.Server.Clients)
                        {
                            foreach (KeyValuePair<int, List<Location>> blocks in this.Blocks)
                            {
                                if (blocks.Key == this.DaMap.Number)
                                {
                                    foreach (Location block in blocks.Value)
                                    {
                                        item_3.Base.DaMap.Matrix[(int)block.X, (int)block.Y] = Tile.Wall;
                                    }
                                    break;
                                }
                            }
                            //foreach (Location item_4 in item_3.Base.Blocks)
                            //    item_3.Base.DaMap.Matrix[(int)item_4.X, (int)item_4.Y] = Tile.Wall;
                        }
                    }
                    foreach (Client client in this.client.Server.Clients)
                        this.DaMap.Matrix[(int)client.Base.MyPosition.X, (int)client.Base.MyPosition.Y] = Tile.Player;
                    lock (Program.SyncObj)
                    {
                        foreach (Aisling item_1 in aislingArray)
                        {
                            if (item_1.Map == this.DaMap.Number)
                                this.DaMap.Matrix[(int)item_1.Position.X, (int)item_1.Position.Y] = Tile.Player;
                        }
                    }
                    this.DaMap.Matrix[(int)this.client.Base.MyPosition.X, (int)this.client.Base.MyPosition.Y] = Tile.Me;
                    return this.DaMap.Matrix;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                }
                return this.DaMap.Matrix;
            }
        }

        public Entity this[uint id]
        {
            get
            {
                if (this.Entitys.ContainsKey(id))
                    return this.Entitys[id];
                return (Entity)null;
            }
            set
            {
                this.Entitys[id] = value;
            }
        }
        #region Find Targets
        public Entity FindNearestTarget()
        {
            try
            {
                return Enumerable.FirstOrDefault<Entity>((IEnumerable<Entity>)Enumerable.OrderBy<Entity, double>(Enumerable.Where<Entity>((IEnumerable<Entity>)this.client.Base.Entitys.Values, (Func<Entity, bool>)(i => i.Type == EntityType.Monster || i.Type == EntityType.SpecialMonster)), (Func<Entity, double>)(i => this.client.Base.DistanceFrom(i.Position))));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return (Entity)null;
        }

        public List<Entity> MonstersInRange(double range)
        {
            try
            {
                return Enumerable.ToList<Entity>(Enumerable.Where<Entity>((IEnumerable<Entity>)this.Monsters, (Func<Entity, bool>)(i => this.client.Base.DistanceFrom(i.Position) <= range)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return null;
        }
        
        public ConcurrentBag<Entity> Monsters
        {
            get
            {
                try
                {
                    //ConcurrentBag<Entity> returnBag = new ConcurrentBag<Entity>(); /*List<Entity> temp = Enumerable.ToList<Entity>(IEnumerable<Entity>)(Enumerable.Where<Entity>((IEnumerable<Entity>)this.threadSafeEntitys.Values, (Func<Entity, bool>)(i => i.Type == EntityType.Monster || i.Type == EntityType.SpecialMonster)));
                   
                    return new ConcurrentBag<Entity>((IEnumerable<Entity>)(Enumerable.Where<Entity>((IEnumerable<Entity>)this.threadSafeEntitys.Values, (Func<Entity, bool>)(i => i.Type == EntityType.Monster || i.Type == EntityType.SpecialMonster)))); // original was
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                }
                return null;
                
            }
        }

        public List<Entity> Items
        {
            get
            {
                return Enumerable.ToList<Entity>(Enumerable.Where<Entity>((IEnumerable<Entity>)this.Entitys.Values, (Func<Entity, bool>)(i => i.Type == EntityType.Item)));
            }
        }

        public List<Entity> Npcs
        {
            get
            {
                return Enumerable.ToList<Entity>(Enumerable.Where<Entity>((IEnumerable<Entity>)this.Entitys.Values, (Func<Entity, bool>)(i => i.Type == EntityType.NPC)));
            }
        }
        #endregion


        public DateTime LastCast { get; set; }

        public bool IsCasting { get; set; }

        public uint LastClickID { get; set; }

        public DarkagesBase(Client _client)
        {
            this.client = _client;
        }

        public void UseSkill(SKill skill)
        {
            try
            {
                if (skill == null || this.client == null)
                    return;
                ClientPacket msg = new ClientPacket((byte)62);
                msg.WriteByte(skill.Slot);
                this.client.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                if (skill != null)
                    skill.LastUsed = DateTime.Now;
            }
        }

        public void UseSkill(string skillname)
        {
            try
            {
                SKill skill = Enumerable.FirstOrDefault<KeyValuePair<string, SKill>>((IEnumerable<KeyValuePair<string, SKill>>)this.Skills, (Func<KeyValuePair<string, SKill>, bool>)(i => i.Value.Name.StartsWith(skillname))).Value;
                if (skill == null || this.client == null)
                    return;
                ClientPacket msg = new ClientPacket((byte)62);
                msg.WriteByte(skill.Slot);
                this.client.Send(msg);
                skill.LastUsed = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void SendPacketToServer(byte[] Data)
        {
            try
            {
                this.client.Send(Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void SendPacketToServer(ClientPacket packet)
        {
            try
            {
                this.client.Send(packet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void SendPacketToClient(ServerPacket packet)
        {
            try
            {
                //this.client.SendClient(packet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public ushort GetWeaponID()
        {
            return this.CurrentStaffID;
        }
        public string GetWeaponName()
        {
            return this.CurrentStaffName;
        }

        public bool WearingLightNeck()
        {
            return this.LightNeck;
        }

        /*public string GetWeaponName()
        {
            try
            {
                    for (int index = 0; index < this.Inventory.Count; ++index)
                    {
                        //this.client.SendMessage(this.Inventory[index].IconSet + " vs " + this.GetWeaponID(), (byte)0);
                        //this.client.SendMessage(this.Inventory[index].Icon + " vs " + this.GetWeaponID(), (byte)0);
                        //this.client.SendMessage(this.Inventory[index].Slot + " vs " + this.GetWeaponID(), (byte)0);

                        if (this.Inventory[index].IconSet == this.GetWeaponID())
                        {
                            return this.Inventory[index].Name;
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return "Not found";
        }
        */
        public bool SwitchStaff(string staff)
        {
            try
            {
                if (this.IsSwitchingItems)
                    return false;
                if (!this.Bar.ContainsKey((ushort)50) && !this.Bar.ContainsKey((ushort)101))
                {
                    for (int index = 0; index < this.Inventory.Count; ++index)
                    {
                        if (this.Inventory[index].Name.Contains(staff))
                        {
                            this.UseItem(this.Inventory[index]);
                            Thread.Sleep(200);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }

        public bool UseSpell(uint serial, Spell spell)
        {
            try
            {
                return this.client.CastBuffOn(serial, spell);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        public bool UseSpell(Entity Target, Spell spell)
        {
            try
            {
                return this.client.CastSpell(Target, spell);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }

        public bool UseHolyDiana()
        {
            try
            {
                return this.SwitchStaff("Holy Diana");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }

        public bool UseAndorStaff()
        {
            try
            {
                return this.SwitchStaff("Andor Staff");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }

        public bool UseGlimmeringWand()
        {
            try
            {
                return this.SwitchStaff("Glimmering Wand");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }

        public void UseItem(string ItemName)
        {
            try
            {
                for (int index = 0; index < this.Inventory.Count; ++index)
                {
                    if (this.Inventory[index].Name.Contains(ItemName))
                    {
                        this.UseItem(this.Inventory[index]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void LootGold()
        {
            Dictionary<uint, Entity> dictionary = this.client.Base.Entitys;
            try
            {
                foreach (KeyValuePair<uint, Entity> keyValuePair1 in dictionary)
                {
                    KeyValuePair<uint, Entity> entity = keyValuePair1;
                    KeyValuePair<uint, Entity> keyValuePair2 = entity;
                    int num;
                    if (keyValuePair2.Value.Type == EntityType.Item)
                    {
                        keyValuePair2 = entity;
                        if (this.DistanceFrom(keyValuePair2.Value.Position) <= 5.0)
                        {
                            keyValuePair2 = entity;
                            num = (int)keyValuePair2.Value.SpriteID != 32908 ? 1 : 0;
                            goto label_7;
                        }
                    }
                    num = 1;
                label_7:
                    if (num == 0)
                        new Thread((ThreadStart)(() =>
                        {
                            for (int index = 0; index < 5; ++index)
                            {
                                ClientPacket msg = new ClientPacket((byte)7);
                                msg.WriteByte((byte)index);
                                msg.WriteInt16((short)entity.Value.Position.X);
                                msg.WriteInt16((short)entity.Value.Position.Y);
                                msg.WriteByte((byte)0);
                                msg.WriteByte((byte)7);
                                this.client.Send(msg);
                            }
                        })).Start();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        public void UseItem(ConsoleDA.Item item)
        {
            try
            {
                ClientPacket msg = new ClientPacket((byte)28);
                msg.WriteByte(item.Slot);
                this.client.Send(msg);
                item.LastUsed = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void Travel()
        {
            try
            {
                if (this.Waypoints.Count > 0 && this.Waypoints.ContainsKey((int)this.client.Base.DaMap.Number) && this.Waypoints[(int)this.client.Base.DaMap.Number].Count != 0)
                {
                    if (this.Waypoints[(int)this.client.Base.DaMap.Number].Count < this.WaypointIndex)
                        this.WaypointIndex = 0;
                    Location location = this.Waypoints[(int)this.client.Base.DaMap.Number][this.WaypointIndex];
                    List<PathFinder.PathFinderNode> path = PathFinder.FindPath(this.Map, this.MyPosition, location);
                    if (location == null)
                        this.WaypointIndex = 0;
                    else if ((int)this.MyPosition.X == (int)location.X && (int)this.MyPosition.Y == (int)location.Y)
                    {
                        ++this.WaypointIndex;
                        if (this.WaypointIndex >= this.Waypoints[(int)this.client.Base.DaMap.Number].Count)
                            this.WaypointIndex = 0;
                    }
                    else if ((path = this.FindPath(location))!= null)
                        this.WalkPlayerThroughPath(path,0);
                    else
                        ++this.WaypointIndex;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public bool IsNextToEntity(Location Location)
        {
            try
            {
                if ((int)this.MyPosition.X == (int)Location.X && (int)this.MyPosition.Y + 1 == (int)Location.Y || (int)this.MyPosition.X == (int)Location.X && (int)this.MyPosition.Y - 1 == (int)Location.Y || (int)this.MyPosition.X + 1 == (int)Location.X && (int)this.MyPosition.Y == (int)Location.Y)
                    return true;
                if ((int)this.MyPosition.X - 1 == (int)Location.X && (int)this.MyPosition.Y == (int)Location.Y)
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }

        public bool IsFacingEntity(Location Location)
        {
            try
            {
                switch (this.MyPosition.Facing)
                {
                    case Direction.Up:
                        return (int)this.MyPosition.X == (int)Location.X && (int)this.MyPosition.Y - 1 == (int)Location.Y;
                    case Direction.Right:
                        return (int)this.MyPosition.X + 1 == (int)Location.X && (int)this.MyPosition.Y == (int)Location.Y;
                    case Direction.Down:
                        return (int)this.MyPosition.X == (int)Location.X && (int)this.MyPosition.Y + 1 == (int)Location.Y;
                    case Direction.Left:
                        return (int)this.MyPosition.X - 1 == (int)Location.X && (int)this.MyPosition.Y == (int)Location.Y;
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }

        public bool FaceEntity(Direction direction)
        {
            try
            {
                ClientPacket msg = new ClientPacket((byte)17);
                msg.WriteByte((byte)direction);
                this.client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }

        public void CheckIfFacing(Location Location)
        {
            if (!this.IsNextToEntity(Location))
                throw new Exception();
            if ((int)this.MyPosition.X + 1 == (int)Location.X && (int)this.MyPosition.Y == (int)Location.Y)
                this.FaceEntity(Direction.Right);
            if ((int)this.MyPosition.X - 1 == (int)Location.X && (int)this.MyPosition.Y == (int)Location.Y)
                this.FaceEntity(Direction.Left);
            if ((int)this.MyPosition.X == (int)Location.X && (int)this.MyPosition.Y - 1 == (int)Location.Y)
                this.FaceEntity(Direction.Up);
            if ((int)this.MyPosition.X != (int)Location.X || (int)this.MyPosition.Y + 1 != (int)Location.Y)
                return;
            this.FaceEntity(Direction.Down);
        }
        public bool IsPathFullyPassable(List<PathFinder.PathFinderNode> path)
        {
            for (int i = 0; i < path.Count; i++)
                if (this.DaMap.isBlock((ushort)path[i].X, (ushort)path[i].Y))
                    return false;
            return true;
        }
        public void WalkPlayerToEntity(Location Position)
        {
            try
            {
                if (this.IsRefreshing)
                    Thread.Sleep(1000);
                Location Target = new Location()
                {
                    X = Position.X,
                    Y = Position.Y
                };
                if (this.IsNextToEntity(Position))
                {
                    this.CheckIfFacing(Position);
                }
                else
                {
                    List<PathFinder.PathFinderNode> list = (List<PathFinder.PathFinderNode>)null;
                    for (int index = 0; index < 4; ++index)
                    {
                        if (this.IsRefreshing)
                            Thread.Sleep(1000);
                        list = this.FindPath(Target);
                        if (list == null)
                        {
                            if (index == 3)
                                throw new WalkingException();
                        }
                        else if (list.Count == 1)
                        {
                            if (index == 3)
                                throw new WalkingException();
                            this.Refresh();
                        }
                        else
                            break;
                    }
                    for (int index1 = 1; index1 < list.Count - 1; ++index1)
                    {
                        Tile[,] map;
                        if ((int)Position.X != (int)Target.X || (int)Position.Y != (int)Target.Y)
                        {
                            Target = new Location()
                            {
                                X = Position.X,
                                Y = Position.Y
                            };
                            for (int index2 = 0; index2 < 4; ++index2)
                            {
                                map = this.Map;
                                list = this.FindPath(Target);
                                if (list == null)
                                    throw new WalkingException();
                                if (list.Count == 1)
                                    this.Refresh();
                                if (list.Count == 2)
                                    return;
                                if (index2 == 3)
                                    throw new WalkingException();
                            }
                            index1 = 1;
                        }
                        foreach (Entity entity in this.Entitys.Values)
                        {
                            if (entity.Type == EntityType.Monster && ((int)entity.Position.X == list[index1].X && (int)entity.Position.Y == list[index1].Y))
                            {
                                for (int index2 = 0; index2 < 4; ++index2)
                                {
                                    if (this.IsRefreshing)
                                        Thread.Sleep(1000);
                                    map = this.Map;
                                    list = this.FindPath(Position);
                                    if (list == null)
                                        throw new WalkingException();
                                    if (list.Count == 1)
                                        this.Refresh();
                                    if (list.Count == 2)
                                        return;
                                    if (index2 == 3)
                                        throw new WalkingException();
                                }
                                index1 = 1;
                            }
                        }
                        Direction direction;
                        if (list[index1].X == (int)this.MyPosition.X && list[index1].Y == (int)this.MyPosition.Y - 1)
                            direction = Direction.Up;
                        else if (list[index1].X == (int)this.MyPosition.X && list[index1].Y == (int)this.MyPosition.Y + 1)
                            direction = Direction.Down;
                        else if (list[index1].X == (int)this.MyPosition.X - 1 && list[index1].Y == (int)this.MyPosition.Y)
                        {
                            direction = Direction.Left;
                        }
                        else
                        {
                            if (list[index1].X != (int)this.MyPosition.X + 1 || list[index1].Y != (int)this.MyPosition.Y)
                                throw new WalkingException();
                            direction = Direction.Right;
                        }
                        this.client.Walk(direction);
                        Thread.Sleep(this.client.Tab.walkMsDelay);
                        if (this.IsNextToEntity(Position))
                        {
                            this.CheckIfFacing(Position);
                            return;
                        }
                    }
                    if (!this.IsNextToEntity(Position))
                        throw new WalkingException();
                    this.CheckIfFacing(Position);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void WalkPlayerToEntity(Entity Entity)
        {
            if (this.IsRefreshing)
                Thread.Sleep(1000);
            try
            {
                Location Target = new Location()
                {
                    X = Entity.Position.X,
                    Y = Entity.Position.Y
                };
                if (this.IsNextToEntity(Entity.Position))
                {
                    this.CheckIfFacing(Entity.Position);
                }
                else
                {
                    List<PathFinder.PathFinderNode> list = (List<PathFinder.PathFinderNode>)null;
                    for (int index = 0; index < 4; ++index)
                    {
                        if (this.IsRefreshing)
                            Thread.Sleep(1000);
                        list = this.FindPath(Target);
                        if (list == null)
                        {
                            if (index == 3)
                                throw new WalkingException();
                        }
                        else if (list.Count == 1)
                        {
                            if (index == 3)
                                throw new WalkingException();
                            this.Refresh();
                        }
                        else
                            break;
                    }
                    for (int index1 = 1; index1 < list.Count - 1; ++index1)
                    {
                        if (this[Entity.Serial] == null)
                            return;
                        if ((int)Entity.Position.X != (int)Target.X || (int)Entity.Position.Y != (int)Target.Y)
                        {
                            Target = new Location()
                            {
                                X = Entity.Position.X,
                                Y = Entity.Position.Y
                            };
                            for (int index2 = 0; index2 < 4; ++index2)
                            {
                                if (this[Entity.Serial] == null)
                                    return;
                                list = this.FindPath(Target);
                                if (list == null)
                                    throw new WalkingException();
                                if (list.Count == 1)
                                    this.Refresh();
                                if (list.Count == 2)
                                    return;
                                if (index2 == 3)
                                    throw new WalkingException();
                            }
                            index1 = 1;
                        }
                        if (this[Entity.Serial] == null)
                            return;
                        foreach (Entity entity in this.Entitys.Values)
                        {
                            if (entity.Type == EntityType.Monster && ((int)entity.Position.X == list[index1].X && (int)entity.Position.Y == list[index1].Y))
                            {
                                for (int index2 = 0; index2 < 4; ++index2)
                                {
                                    if (this.IsRefreshing)
                                        Thread.Sleep(1000);
                                    if (this[Entity.Serial] == null)
                                        return;
                                    list = this.FindPath(Entity.Position);
                                    if (list == null)
                                        throw new WalkingException();
                                    if (list.Count == 1)
                                        this.Refresh();
                                    if (list.Count == 2)
                                        return;
                                    if (index2 == 3)
                                        throw new WalkingException();
                                }
                                index1 = 1;
                            }
                        }
                        Direction direction;
                        if (list[index1].X == (int)this.MyPosition.X && list[index1].Y == (int)this.MyPosition.Y - 1)
                            direction = Direction.Up;
                        else if (list[index1].X == (int)this.MyPosition.X && list[index1].Y == (int)this.MyPosition.Y + 1)
                            direction = Direction.Down;
                        else if (list[index1].X == (int)this.MyPosition.X - 1 && list[index1].Y == (int)this.MyPosition.Y)
                        {
                            direction = Direction.Left;
                        }
                        else
                        {
                            if (list[index1].X != (int)this.MyPosition.X + 1 || list[index1].Y != (int)this.MyPosition.Y)
                                throw new WalkingException();
                            direction = Direction.Right;
                        }
                        this.client.Walk(direction);
                        Thread.Sleep(this.client.Tab.walkMsDelay);
                        if (this[Entity.Serial] == null)
                            return;
                        if (this.IsNextToEntity(Entity.Position))
                        {
                            this.CheckIfFacing(Entity.Position);
                            return;
                        }
                    }
                    if (!this.IsNextToEntity(Entity.Position))
                        throw new WalkingException();
                    this.CheckIfFacing(Entity.Position);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void WalkPlayerThroughPath(List<PathFinder.PathFinderNode> path, int distance)
        {
            /*this.client.SendMessage( this.DistanceFrom(new Location()
                {
                    X = (ushort)path[path.Count - 1].X,
                    Y = (ushort)path[path.Count - 1].Y
                }).ToString(),(byte)0);*/
            for (int i = 0; i < path.Count; i++)
            {
                //this.client.SendMessage(path[i].X + ", " + path[i].Y,(byte)0);
            }
                //if (this.IsRefreshing)
                //    Thread.Sleep(1000);
            
                if (path == null)
                    throw new Exception();
                if (path.Count <= 0)
                {
                    this.Refresh();
                    throw new Exception();
                }
                if (path.Count == 1)
                    throw new Exception();
                int iNode = 0;

                
            for (PathFinder.PathFinderNode node = path[iNode]; 
                    /*this.DistanceFrom(new Location() { 
                        X = (ushort)path[path.Count - 1].X, 
                        Y = (ushort)path[path.Count - 1].Y 
                    }) >= distance*/iNode < path.Count - 1;
                   node = path[iNode])
                /*for (int i = 0; this.DistanceFrom(new Location()
                {
                    X = (ushort)path[path.Count - 1].X,
                    Y = (ushort)path[path.Count - 1].Y
                }) > distance; i++)*/
                {
                    //lock (this.client.Tab.syncObj) { }
                    //this.client.SendMessage(node.NextNode.X + ", " + node.NextNode.Y, (byte)0);
                    //this.WalkPlayerToward(new Location() { X = (ushort)path[i].X, Y = (ushort)path[i].Y });
                    Direction direction;
                    if (node.NextNode.X == (int)this.MyPosition.X && node.NextNode.Y == (int)this.MyPosition.Y - 1)
                        direction = Direction.Up;
                    else if (node.NextNode.X == (int)this.MyPosition.X && node.NextNode.Y == (int)this.MyPosition.Y + 1)
                        direction = Direction.Down;
                    else if (node.NextNode.X == (int)this.MyPosition.X - 1 && node.NextNode.Y == (int)this.MyPosition.Y)
                    {
                        direction = Direction.Left;
                    }
                    else
                    {
                        if (node.NextNode.X != (int)this.MyPosition.X + 1 || node.NextNode.Y != (int)this.MyPosition.Y)
                            throw new Exception();
                        direction = Direction.Right;
                    }
                    //node = node.NextNode;
                    this.client.Walk(direction);
                    iNode++;
                    Thread.Sleep(this.client.Tab.walkMsDelay);
                }      
        }

        public void WalkPlayerThroughServerPath(List<PathFinder.PathFinderNode> path, int distance)
        {
            /*this.client.SendMessage( this.DistanceFrom(new Location()
                {
                    X = (ushort)path[path.Count - 1].X,
                    Y = (ushort)path[path.Count - 1].Y
                }).ToString(),(byte)0);*/
            for (int i = 0; i < path.Count; i++)
            {
                //this.client.SendMessage(path[i].X + ", " + path[i].Y,(byte)0);
            }
            //if (this.IsRefreshing)
            //    Thread.Sleep(1000);

            if (path == null)
                throw new Exception();
            if (path.Count <= 0)
            {
                this.Refresh();
                throw new Exception();
            }
            if (path.Count == 1)
                throw new Exception();
            int iNode = 0;
            for (PathFinder.PathFinderNode node = path[iNode];
                /*this.DistanceFrom(new Location() { 
                    X = (ushort)path[path.Count - 1].X, 
                    Y = (ushort)path[path.Count - 1].Y 
                }) >= distance*/iNode < path.Count - 1;
               node = path[iNode])
            /*for (int i = 0; this.DistanceFrom(new Location()
            {
                X = (ushort)path[path.Count - 1].X,
                Y = (ushort)path[path.Count - 1].Y
            }) > distance; i++)*/
            {
                //this.client.SendMessage(node.NextNode.X + ", " + node.NextNode.Y, (byte)0);
                //this.WalkPlayerToward(new Location() { X = (ushort)path[i].X, Y = (ushort)path[i].Y });
                Direction direction;
                if (node.NextNode.X == (int)this.MyServerPosition.X && node.NextNode.Y == (int)this.MyServerPosition.Y - 1)
                    direction = Direction.Up;
                else if (node.NextNode.X == (int)this.MyServerPosition.X && node.NextNode.Y == (int)this.MyServerPosition.Y + 1)
                    direction = Direction.Down;
                else if (node.NextNode.X == (int)this.MyServerPosition.X - 1 && node.NextNode.Y == (int)this.MyServerPosition.Y)
                {
                    direction = Direction.Left;
                }
                else
                {
                    if (node.NextNode.X != (int)this.MyServerPosition.X + 1 || node.NextNode.Y != (int)this.MyServerPosition.Y)
                        throw new Exception();
                    direction = Direction.Right;
                }
                //node = node.NextNode;
                this.client.Walk(direction);
                iNode++;
                Thread.Sleep(this.client.Tab.walkMsDelay);
            }
        }

        public void WalkPlayerToward(Location Location)
        {
            if (this.IsRefreshing)
                Thread.Sleep(1000);
            List<PathFinder.PathFinderNode> path = this.FindPath(Location);
            if (path == null)
                throw new Exception();
            if (path.Count <= 0)
            {
                this.Refresh();
                throw new Exception();
            }
            if (path.Count == 1)
                throw new Exception();
            Direction direction;
            if (path[1].X == (int)this.MyPosition.X && path[1].Y == (int)this.MyPosition.Y - 1)
                direction = Direction.Up;
            else if (path[1].X == (int)this.MyPosition.X && path[1].Y == (int)this.MyPosition.Y + 1)
                direction = Direction.Down;
            else if (path[1].X == (int)this.MyPosition.X - 1 && path[1].Y == (int)this.MyPosition.Y)
            {
                direction = Direction.Left;
            }
            else
            {
                if (path[1].X != (int)this.MyPosition.X + 1 || path[1].Y != (int)this.MyPosition.Y)
                    throw new Exception();
                direction = Direction.Right;
            }
            this.client.Walk(direction);
            Thread.Sleep(this.client.Tab.walkMsDelay);
        }

        public void Refresh()
        {
            try
            {
                this.client.Send(new byte[2]
        {
          (byte) 56,
          (byte) 0
        });
                //Thread.Sleep(200);
                this.LastRefreshed = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void Assail()
        {
            try
            {
                this.client.Send(new byte[2]
        {
          (byte) 19,
          (byte) 1
        });
                ++this.client.Base.Swings;
                if (this.client.Base.BodySwings >= 20 && (DateTime.Now - this.LastRefreshed).TotalSeconds >= 3.0)
                {
                    this.client.Base.Refresh();
                    this.client.Base.BodySwings = 0;
                }
                if (this.client.Base.Swings < 45 || (DateTime.Now - this.LastRefreshed).TotalSeconds < 10.0)
                    return;
                this.client.Base.Refresh();
                this.client.Base.Swings = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public List<PathFinder.PathFinderNode> FindPath(Location Target)
        {
            try
            {
                this.Map[(int)this.MyPosition.X, (int)this.MyPosition.Y] = Tile.Empty;
                this.Map[(int)Target.X, (int)Target.Y] = Tile.Empty;
                return PathFinder.FindPath(this.Map, this.MyPosition, Target);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return (List<PathFinder.PathFinderNode>)null;
        }

        public double DistanceFrom(Location Location)
        {
            double num1 = (double)Math.Abs((int)Location.X - (int)this.MyPosition.X);
            double num2 = (double)Math.Abs((int)Location.Y - (int)this.MyPosition.Y);
            return num1 > num2 ? num1 : num2;
        }

        public bool WithinRange(Location Location, double MaxDistance)
        {
            return this.DistanceFrom(Location) <= MaxDistance;
        }

        public bool OnScreenOf(Location Location)
        {
            return Math.Abs((int)Location.X - (int)this.MyPosition.X) < 7 && Math.Abs((int)Location.Y - (int)this.MyPosition.Y) < 7;
        }

        public bool ShouldSpecialLoot()
        {
            try
            {
                return Enumerable.Count<Entity>((IEnumerable<Entity>)Enumerable.OrderBy<Entity, double>(Enumerable.Where<Entity>((IEnumerable<Entity>)this.Entitys.Values, (Func<Entity, bool>)(i => i.Type == EntityType.Item && ((int)i.SpriteID == 34272 || (int)i.SpriteID == 34261))), (Func<Entity, double>)(i => this.DistanceFrom(i.Position)))) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }

        public void PullObjectsToStack(byte[] data)
        {
            this.LoggedIn = true;
            try
            {
                ushort num1 = (ushort)(((uint)data[0] << 8) + (uint)data[1]);
                int num2 = 0;
                for (int index = 0; index < (int)num1; ++index)
                {
                    Entity ent = new Entity(true);
                    ent.Position.X = (ushort)(((uint)data[num2 + 2] << 8) + (uint)data[num2 + 3]);
                    ent.Position.Y = (ushort)(((uint)data[num2 + 4] << 8) + (uint)data[num2 + 5]);
                    ent.Serial = (uint)(((int)data[num2 + 6] << 24) + ((int)data[num2 + 7] << 16) + ((int)data[num2 + 8] << 8)) + (uint)data[num2 + 9];
                    ent.SpriteID = (ushort)(((uint)data[num2 + 10] << 8) + (uint)data[num2 + 11]);
                    if ((int)ent.SpriteID > 32768 && (int)ent.SpriteID < 36864)
                    {
                        ent.Type = EntityType.Item;
                        num2 += 13;
                    }
                    else
                    {
                        ent.Position.Facing = (Direction)data[num2 + 16];
                        ushort num3 = (ushort)(((uint)data[num2 + 17] << 8) + (uint)data[num2 + 18]);
                        if ((int)num3 == 1 || (int)num3 == 0)
                        {
                            ent.Type = (int)num3 == 0 ? EntityType.Monster : EntityType.Pet;
                            if ((int)ent.SpriteID == 16650)
                                ent.Type = EntityType.SpecialMonster;
                            if ((int)ent.SpriteID == 16657)
                                ent.Type = EntityType.SpecialMonster;
                            num2 += 17;
                        }
                        else
                        {
                            int count = (int)data[num2 + 19];
                            ent.Type = EntityType.NPC;
                            ent.Name = Encoding.GetEncoding(949).GetString(data, num2 + 20, count);
                            num2 += 18 + count;
                        }
                    }
                    ent.Active = true;
                    ent.fased = false;
                    ent.Cursed = false;
                    this.AddEntity(ent);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void Attack()
        {
            try
            {
                if (!this.client.Base.Bar.ContainsKey((ushort)53) && (DateTime.Now - this.client.Base.LastDionAttempt).TotalSeconds >= 2.0)
                {
                    this.client.CastBuffOn(this.client.Base.Serial, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("dion"))));
                    this.client.Base.LastDionAttempt = DateTime.Now;
                }
                else
                {
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("eagle strike"))));
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("cyclone blade"))));
                }
                this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("ground stomp"))));
                Thread.Sleep(10);
                this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("animal roar"))));
                Thread.Sleep(10);
                if (this.client.Base.CurrentTarget == null)
                    return;
                if (this.client.Base.HaveCasterAround() != null)
                {
                    if ((int)this.client.Base.CurrentTarget.HPPercent < 100 && this.client.Base.CurrentTarget.Cursed && this.client.Base.CurrentTarget.fased)
                    {
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("beag suain"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("lull"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("cyclone kick"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("wolf fang fist"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("wheel kick"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("strikedown"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("dune swipe"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("poison punch"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("kelberoth strike"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("wind blade"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("mass strike"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("double rake"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("raging attack"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("pounce"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("tail sweep"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("venom attack"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("draco tail kick"))));
                        Thread.Sleep(10);
                        this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("mantis kick"))));
                        Thread.Sleep(10);
                    }
                }
                else
                {
                    if (this.client.Base.CurrentTarget == null || (int)this.client.Base.CurrentTarget.HPPercent >= 100)
                        return;
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("beag suain"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("lull"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("cyclone kick"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("wolf fang fist"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("wheel kick"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("strikedown"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("dune swipe"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("poison punch"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("kelberoth strike"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("wind blade"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("mass strike"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("double rake"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("raging attack"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("pounce"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("tail sweep"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("venom attack"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("draco tail kick"))));
                    Thread.Sleep(10);
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("mantis kick"))));
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                Thread.Sleep(100);
            }
        }

        public void AttackPureWar(string[] skills)
        {
            if (!this.client.Base.Bar.ContainsKey((ushort)94))
                this.client.CastBuffOn(this.client.Base.Serial, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("Aegis Sphere"))));
            if (!this.client.Base.Bar.ContainsKey((ushort)54))
            {
                this.client.CastBuffOn(this.client.Base.Serial, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("asgall faileas"))));
            }
            else
            {
                this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("eagle strike"))));
                this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("cyclone blade"))));
            }
            if ((int)this.client.Base.CurrentTarget.HPPercent < 100)
            {
                for (int k = 0; k < skills.Length; ++k)
                    this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith(skills[k].ToLower()))));
            }
            else
                this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith("wind blade"))));
        }

        public void Dion(string DionSpellName)
        {
            if (this.client.Base.Bar.ContainsKey((ushort)53) || (DateTime.Now - this.client.Base.LastDionAttempt).TotalSeconds < 2.0)
                return;
            this.client.CastBuffOn(this.client.Base.Serial, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith(DionSpellName))));
            this.client.Base.LastDionAttempt = DateTime.Now;
        }

        public void AttackWith(params string[] args)
        {
            if (!this.client.Base.Bar.ContainsKey((ushort)53) && (DateTime.Now - this.client.Base.LastDionAttempt).TotalSeconds >= 2.0)
            {
                this.client.CastBuffOn(this.client.Base.Serial, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("dion"))));
                this.client.Base.LastDionAttempt = DateTime.Now;
            }
            if ((int)this.client.Base.CurrentTarget.HPPercent >= 100 || !this.client.Base.CurrentTarget.Cursed || !this.client.Base.CurrentTarget.fased)
                return;
            for (int j = 0; j < args.Length; ++j)
                this.client.Base.UseSkill(Enumerable.ToList<SKill>((IEnumerable<SKill>)this.client.Base.Skills.Values).Find((Predicate<SKill>)(i => i.Name.ToLower().StartsWith(args[j]))));
        }

        

        public void FasOnEntity(Entity entity)
        {
            if (entity.fased)
                return;
            this.client.CastSpell(entity, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("ard fas nadur"))));
        }

        public Spell FindSpell(string spellname)
        {
            return Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith(spellname)));
        }

        public void CurseOnEntity(Entity entity)
        {
            if (entity.Cursed)
                return;
            this.client.CastSpell(entity, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("ard cradh"))));
        }

        public void FasBashersTarget(Client basher)
        {
            if (basher.Base.CurrentTarget.fased)
                return;
            this.client.CastSpell(basher.Base.CurrentTarget, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("ard fas nadur"))));
        }

        public void CastOnBashersTarget(Client basher, string spell)
        {
            if (basher.Base.CurrentTarget.fased)
                return;
            this.client.CastSpell(basher.Base.CurrentTarget, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith(spell))));
        }

        public void CurseBashersTarget(Client basher)
        {
            if (basher.Base.CurrentTarget.Cursed)
                return;
            this.client.CastSpell(basher.Base.CurrentTarget, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("ard cradh"))));
        }

        public void PramhBashersTarget(Client basher)
        {
            if (!basher.Base.CurrentTarget.Cursed || !basher.Base.CurrentTarget.fased || (DateTime.Now - basher.Base.CurrentTarget.LastAttacked).TotalSeconds < 2.0)
                return;
            this.client.CastSpell(basher.Base.CurrentTarget, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("pramh"))));
        }

        public void PramhTarget(Client basher)
        {
            if (!basher.Base.CurrentTarget.Cursed || !basher.Base.CurrentTarget.fased)
                return;
            this.client.CastSpell(basher.Base.CurrentTarget, this.FindSpell("pramh"));
        }

        public void PramhBashersTargetNonStop(Client basher)
        {
            if (!basher.Base.CurrentTarget.Cursed || !basher.Base.CurrentTarget.fased)
                return;
            this.client.CastSpell(basher.Base.CurrentTarget, Enumerable.ToList<Spell>((IEnumerable<Spell>)this.client.Base.Spells.Values).Find((Predicate<Spell>)(i => i.Name.StartsWith("pramh"))));
        }

        public Client HaveBasherAround()
        {
            return Enumerable.FirstOrDefault<Client>((IEnumerable<Client>)this.client.Others, (Func<Client, bool>)(i => !i.Base.Caster));
        }

        public Client HaveCasterAround()
        {
            return Enumerable.FirstOrDefault<Client>((IEnumerable<Client>)this.client.Others, (Func<Client, bool>)(i => i.Base.Caster));
        }

        public void UseDark()
        {
            try
            {
                if (!this.client.Base.OmniNeck && !this.client.Base.LightNeck)
                    return;
                ConsoleDA.Item obj = this.client.Base.Inventory.Find((Predicate<ConsoleDA.Item>)(i => i.Name.Contains("Thief's Dark Necklace")));
                if (obj != null && (DateTime.Now - obj.LastUsed).TotalMilliseconds > 2.0)
                {
                    this.client.Base.UseItem(obj);
                    Thread.Sleep(800);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void UseLight()
        {
            try
            {
                if (this.client.Base.LightNeck)
                    return;
                ConsoleDA.Item obj = this.client.Base.Inventory.Find((Predicate<ConsoleDA.Item>)(i => i.Name.Contains("Light Necklace")));
                if (obj != null && (DateTime.Now - obj.LastUsed).TotalMilliseconds > 2.0)
                {
                    this.client.Base.UseItem(obj);
                    Thread.Sleep(800);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void UseOmni()
        {
            try
            {
                if (this.client.Base.LightNeck && this.client.Base.OmniNeck)
                    return;
                ConsoleDA.Item obj = this.client.Base.Inventory.Find((Predicate<ConsoleDA.Item>)(i => i.Name.Contains("Omni")));
                if (obj != null && (DateTime.Now - obj.LastUsed).TotalMilliseconds > 2.0)
                {
                    this.client.Base.UseItem(obj);
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void AddEntity(Entity ent)
        {
            if (this.Entitys.ContainsKey(ent.Serial))
                return;
            ent.DateAdded = DateTime.Now;
            this.Entitys.Add(ent.Serial, ent);
        }

        public void RemoveEntity(uint id)
        {
            if (!this.Entitys.ContainsKey(id))
                return;
            if ((DateTime.Now - this.Entitys[id].LastAttacked).TotalMilliseconds <= 200.0)
            {
                this.Swings = 0;
                this.BodySwings = 0;
            }
            this.Entitys.Remove(id);
        }
    }
}
