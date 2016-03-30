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
        public static Spell.Skillshot E2 { get; private set; }
        public static Spell.Active R { get; private set; }
        public static Spell.Active heal { get; private set; }

        public static void InitSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1250, SkillShotType.Linear, 250, 1350, 90)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W, 750);
            E = new Spell.Active(SpellSlot.E);
            E.CastDelay = (int)1.25f;
            E2 = new Spell.Skillshot(SpellSlot.E, 750, SkillShotType.Linear, 500, 3200, 70);
            R = new Spell.Active(SpellSlot.R, 1000);
            var slot = ObjectManager.Player.GetSpellSlotFromName("summonerheal");
            if (slot != SpellSlot.Unknown)
            {
                heal = new Spell.Active(slot, 800);
            }
        }
        public static bool CastE2(Obj_AI_Base target)
        {
            if (target == null || !target.IsValidTarget(E2.Range))
                //return SpellManager.W.Cast(Game.CursorPos);
                return E2.Cast(Player.Instance.Position);

            var cast = E2.GetPrediction(target);
            var castPos = E2.IsInRange(cast.CastPosition) ? cast.CastPosition : target.ServerPosition;

            return E2.Cast(castPos);
        }
    }
}
