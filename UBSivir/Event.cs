using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;

namespace UBSivir
{
    class Event
    {
       private static List<AIHeroClient> Enemies = new List<AIHeroClient>();
       public static void Resetaa()
       {
           Orbwalker.OnPostAttack += OrbwalkerOnPostAttack;
       }
       private static void OrbwalkerOnPostAttack(AttackableUnit target, EventArgs args)
       {
           if (!Spells.W.IsReady())
           {
               return;
           }
           if (Config.ComboMenu["useWCombo"].Cast<CheckBox>().CurrentValue )
           {
              if (target is AIHeroClient)
                 {
                     Spells.W.Cast();
                     Orbwalker.ResetAutoAttack();
                     Player.IssueOrder(GameObjectOrder.AttackUnit, target);                      
                     return;
                 }
           }
       }
       public static void AutoQ()
       {
           foreach (
                     var enemy in
                         Enemies.Where(
                             enemy => enemy.IsValidTarget(Spells.Q.Range) && !CanMove(enemy)))
           {
               Spells.Q.Cast(enemy.Position);
               return;
           }
       }
       public static bool CanMove(Obj_AI_Base target)
       {
           if (target.HasBuffOfType(BuffType.Stun) 
            || target.HasBuffOfType(BuffType.Snare)
            || target.HasBuffOfType(BuffType.Knockup) 
            || target.HasBuffOfType(BuffType.Charm) 
            || target.HasBuffOfType(BuffType.Fear) 
            || target.HasBuffOfType(BuffType.Knockback) 
            || target.HasBuffOfType(BuffType.Taunt) 
            || target.HasBuffOfType(BuffType.Suppression) 
            || target.IsStunned 
            && !target.IsMoving)
           {
               return false;
           }
           return true;
       }
    }
}
