using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace UBSivir
{
    class Spells
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }
        public static Spell.Active heal { get; private set; }

        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }

        public static void InitSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1250, SkillShotType.Linear, 250, 1350, 90)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W, 750);
            E = new Spell.Active(SpellSlot.E, 0);
            R = new Spell.Active(SpellSlot.R, 1000);           
            if (HasSpell("summonerheal"))
                heal = new Spell.Active(ObjectManager.Player.GetSpellSlotFromName("summonerheal"), 850);
        }
    }
}
