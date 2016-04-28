// Decompiled with JetBrains decompiler
// Type: Russia.Aisling
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

namespace ConsoleDA
{
    public class Aisling : Entity
    {
        //public string Name { get; set; }

        //public Location Position { get; set; }

        //public uint Serial { get; set; }
        public int lastMap { get; set; }

        public int Level { get; set; }

        public int Ability { get; set; }

        public uint MaximumHP { get; set; }

        public uint MaximumMP { get; set; }

        public int Str { get; set; }

        public int Int { get; set; }

        public int Wis { get; set; }

        public int Con { get; set; }

        public int Dex { get; set; }

        public int AvailablePoints { get; set; }

        public int MaximumWeight { get; set; }

        public int CurrentWeight { get; set; }

        public uint CurrentHP { get; set; }

        public uint CurrentMP { get; set; }

        public uint Experience { get; set; }

        public uint ToNextLevel { get; set; }

        public uint AbilityExp { get; set; }

        public uint ToNextAbility { get; set; }

        public uint GamePoints { get; set; }

        public uint Gold { get; set; }

        public Aisling.Elements AttackElement { get; set; }

        public Aisling.Elements DefenseElement { get; set; }

        public int AttackElement2 { get; set; }

        public int DefenseElement2 { get; set; }

        public int MagicResistance { get; set; }

        public int ArmorClass { get; set; }

        public int Damage { get; set; }

        public int Hit { get; set; }

        public int BitMask { get; set; }

        public bool CanCastHere { get; set; }

        public bool Poisoned { get; set; }

        public string Password { get; set; }

        public enum Elements
        {
            None,
            Fire,
            Sea,
            Wind,
            Earth,
            Light,
            Dark,
            Wood,
            Metal,
            Undead,
        }
    }
}
