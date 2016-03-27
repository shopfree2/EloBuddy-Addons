using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace UBSivir
{
    class Damages
    {   
        public static float QDamage(Obj_AI_Base target)
        {
            var damage = Player.Instance.GetAutoAttackDamage(target);
            if (Spells.Q.IsReady())
                damage += ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q);
            return damage;
        }      
    }
}
