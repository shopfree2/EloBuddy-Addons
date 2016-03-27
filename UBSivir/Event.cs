using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;

namespace UBSivir
{
    class Event
    {
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
    }
}
