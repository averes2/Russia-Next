// Decompiled with JetBrains decompiler
// Type: Russia.Entity
// Assembly: Russia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9CDC6A05-A243-44CB-91B4-4F42717742BC
// Assembly location: C:\Users\GOON\Desktop\Russia\Russia.exe

using System;
using System.Collections.Generic;

namespace ConsoleDA
{
    public class Entity : IComparable<Entity>
    {
        public int Map { get; set; }
        public Dictionary<ushort, SpellAnimation> Animations = new Dictionary<ushort, SpellAnimation>();
        public Spell LastUserCastedSpell = (Spell)null;
        public Location Position = new Location();
        public bool Active;
        public bool Cursed;
        public DateTime DateAdded;
        public DateTime DateEvent;
        public DateTime DateTargeted;
        public DateTime DateUpdated;
        public byte HPPercent;
        public DateTime LastAttacked;
        public string Name;
        public uint Serial;
        public ushort SpriteID;
        public int TimesHit;
        public EntityType Type;
        public bool fased;

        public EventHandler<EntityArgs> OnSpellAnimation { get; set; }

        public DateTime LowHpTime { get; set; }

        public Entity(bool RaiseEvents = true)
        {
            this.DateAdded = DateTime.Now;
            if (!RaiseEvents)
                return;
            this.OnSpellAnimation += new EventHandler<EntityArgs>(this.OnAnimation);
        }

        public int CompareTo(Entity other)
        {
            Entity entity = other;
            if (this.Serial < entity.Serial)
                return 1;
            return this.Serial > entity.Serial ? -1 : 0;
        }

        public void EntityTargeted(Spell spell)
        {
            this.DateTargeted = DateTime.Now;
            this.LastUserCastedSpell = spell;
        }

        public void Update()
        {
            this.DateUpdated = DateTime.Now;
        }

        public void OnAnimation(object sender, EntityArgs args)
        {
            if ((int)args.Animation.Number == 273)
                this.fased = true;
            if ((int)args.Animation.Number == 257)
                this.Cursed = true;
            if ((int)args.Animation.Number == 104)
                this.Cursed = true;
            if ((int)args.Animation.Number == 243)
                this.Cursed = true;
            if ((int)args.Animation.Number == 82)
                this.Cursed = true;
            if (!this.Animations.ContainsKey(args.Animation.Number))
            {
                this.DateEvent = DateTime.Now;
                this.Animations.Add(args.Animation.Number, args.Animation);
            }
            else
                this.Animations[args.Animation.Number].Time = DateTime.Now;
        }

        public void Clear()
        {
            this.Animations.Clear();
        }
    }
}
