using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Events;
using SharpDX;

namespace UBSivir
{
    class More
    {
    public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Sivir") return;

            Config.Dattenosa();           
            _E.Initialize();
            _E_Advance.Initialize();
            Spells.InitSpells();
            Items.InitItems();   
            InitEvents();           
        }

        private static void InitEvents()
        {           
            Game.OnTick += GameOnTick;
            Drawing.OnDraw += OnDraw;
            Game.OnUpdate += On_Update;
        }
        private static void On_Update(EventArgs args)
        {         
            Mode.Useheal();        
            if (Spells.Q.IsReady())
            {
                Event.AutoQ();
                Mode.Killsteal();
            }          
        }
        private static void GameOnTick(EventArgs args)
        {
            Orbwalker.ForcedTarget = null;
            if (Player.Instance.IsDead) return;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            { Mode.Combo(); }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            { Mode.Harass(); }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            { Mode.LaneClear(); }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            { Mode.Lasthit(); }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            { Mode.JungleClear(); }

            if (ObjectManager.Player.SkinId != Config.MiscMenu["Modskinid"].Cast<Slider>().CurrentValue)
            {
                if (Config.MiscMenu["Modskin"].Cast<CheckBox>().CurrentValue)
                {
                    Player.SetSkinId(Config.MiscMenu["Modskinid"].Cast<Slider>().CurrentValue);
                }
            }
      
        }
        private static void OnDraw(EventArgs args)
        {
            if (Config.DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Spells.Q.IsLearned ? Color.HotPink : Color.Zero , Spells.Q.Range, Player.Instance.Position);
            }
            if (Config.DrawMenu["drawR"].Cast<CheckBox>().CurrentValue)
            {
                Circle.Draw(Spells.R.IsLearned ? Color.Yellow : Color.Zero, Spells.R.Range, Player.Instance.Position);
            }
        }

    }
}
