﻿using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace UBSivir
{
    class Items
    {
        public static Item BilgewaterCutlass { get; private set; }
        public static Item BladeOfTheRuinedKing { get; private set; }

        public static void InitItems()
        {
            BilgewaterCutlass = new Item(ItemId.Bilgewater_Cutlass);
            BladeOfTheRuinedKing = new Item(ItemId.Blade_of_the_Ruined_King);          

            Game.OnTick += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;

            var target = Orbwalker.LastTarget as AIHeroClient;
            if (target == null) return;

            if (BilgewaterCutlass.IsOwned())
            {
                UseItem1(BilgewaterCutlass, target);
            }

            if (BladeOfTheRuinedKing.IsOwned())
            {
                UseItem2(BladeOfTheRuinedKing, target);
            }
        }

        private static void UseItem1(Item item, AIHeroClient target)
        {
            if (!target.IsValidTarget(550)
                || !Config.MiscMenu["item.1"].Cast<CheckBox>().CurrentValue
                || Config.MiscMenu["item.1MyHp"].Cast<Slider>().CurrentValue < Player.Instance.HealthPercent
                || Config.MiscMenu["item.1EnemyHP"].Cast<Slider>().CurrentValue < target.HealthPercent)
            {
                return;
            }

            var slot1 = Player.Instance.InventoryItems.FirstOrDefault(x => x.Id == item.Id);
            if (slot1 != null && Player.GetSpell(slot1.SpellSlot).IsReady)
            {
                Player.CastSpell(slot1.SpellSlot, target);
            }
        }
        private static void UseItem2(Item item, AIHeroClient target)
        {
            if (!target.IsValidTarget(550)
                || !Config.MiscMenu["item.2"].Cast<CheckBox>().CurrentValue
                || Config.MiscMenu["item.2MyHp"].Cast<Slider>().CurrentValue < Player.Instance.HealthPercent
                || Config.MiscMenu["item.2EnemyHP"].Cast<Slider>().CurrentValue < target.HealthPercent)
            {
                return;
            }

            var slot2 = Player.Instance.InventoryItems.FirstOrDefault(x => x.Id == item.Id);
            if (slot2 != null && Player.GetSpell(slot2.SpellSlot).IsReady)
            {
                Player.CastSpell(slot2.SpellSlot, target);
            }
        }    
    }
}
