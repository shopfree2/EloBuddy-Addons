using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Drawing;

namespace UBQSS
{
    class UBQSS
    {
        private static Menu Menu;
        private static Menu CC;
        private static Menu Spells;
        private static Item QSS;
        private static Item Mercurial;

        private static bool Check { get { return Menu["Check"].Cast<KeyBind>().CurrentValue; } }
        private static bool Draw { get { return Menu["Draw"].Cast<CheckBox>().CurrentValue; } }
        private static bool Show { get { return Menu["Show"].Cast<CheckBox>().CurrentValue; } }
        private static bool Noenemy { get { return CC["EnemyManager"].Cast<CheckBox>().CurrentValue; } }

        private static bool Taunt { get { return CC["Taunt"].Cast<CheckBox>().CurrentValue; } }
        private static bool Stun { get { return CC["Stun"].Cast<CheckBox>().CurrentValue; } }
        private static bool Snare { get { return CC["Snare"].Cast<CheckBox>().CurrentValue; } }
        private static bool Polymorph { get { return CC["Polymorph"].Cast<CheckBox>().CurrentValue; } }
        private static bool Blind { get { return CC["Blind"].Cast<CheckBox>().CurrentValue; } }
        private static bool Fear { get { return CC["Fear"].Cast<CheckBox>().CurrentValue; } }
        private static bool Charm { get { return CC["Charm"].Cast<CheckBox>().CurrentValue; } }
        private static bool Suppression { get { return CC["Suppression"].Cast<CheckBox>().CurrentValue; } }
        private static bool Silence { get { return CC["Silence"].Cast<CheckBox>().CurrentValue; } }
        private static bool Knockup { get { return CC["Knockup"].Cast<CheckBox>().CurrentValue; } }

        private static bool Ignite { get { return Spells["Ignite"].Cast<CheckBox>().CurrentValue; } }
        private static bool Exhaust { get { return Spells["Exhaust"].Cast<CheckBox>().CurrentValue; } } 
        private static bool TristanaE { get { return Spells["TristanaE"].Cast<CheckBox>().CurrentValue; } }
        private static bool TahmkenchW { get { return Spells["TahmkenchW"].Cast<CheckBox>().CurrentValue; } }
        private static bool ZedUlt { get { return Spells["ZedUlt"].Cast<CheckBox>().CurrentValue; } }
        private static bool VladUlt { get { return Spells["VladUlt"].Cast<CheckBox>().CurrentValue; } }
        private static bool FizzUlt { get { return Spells["FizzUlt"].Cast<CheckBox>().CurrentValue; } }
        private static bool MordUlt { get { return Spells["MordUlt"].Cast<CheckBox>().CurrentValue; } }
        private static bool FioraUlt { get { return Spells["FioraUlt"].Cast<CheckBox>().CurrentValue; } }
        private static bool Damage { get { return Spells["Damage"].Cast<CheckBox>().CurrentValue; } }
        private static bool TFUlt { get { return Spells["TFsight"].Cast<CheckBox>().CurrentValue; } }
        private static bool NocUlt { get { return Spells["Nocturnesight"].Cast<CheckBox>().CurrentValue; } }

        public static int MinDurCC { get { return CC["CCDelay"].Cast<Slider>().CurrentValue; } }
        public static int MinDurSpells { get { return Spells["SpellsDelay"].Cast<Slider>().CurrentValue; } }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Dattenosa;
        }

        public static void Dattenosa(EventArgs args)
        {
            //Item
            QSS = new Item(ItemId.Quicksilver_Sash);
            if (Game.MapId == GameMapId.CrystalScar) { Mercurial = new Item(ItemId.Dervish_Blade); }
            else { Mercurial = new Item(ItemId.Mercurial_Scimitar); }

            var Zed = EntityManager.Heroes.Enemies.Any(x => x.ChampionName == "Zed");
            var Vladimir = EntityManager.Heroes.Enemies.Any(x => x.ChampionName == "Vladimir");
            var Fiora = EntityManager.Heroes.Enemies.Any(x => x.ChampionName == "Fiora");
            var Fizz = EntityManager.Heroes.Enemies.Any(x => x.ChampionName == "Fizz");
            var Mordekaiser = EntityManager.Heroes.Enemies.Any(x => x.ChampionName == "Mordekaiser");
            var Tristana = EntityManager.Heroes.Enemies.Any(x => x.ChampionName == "Tristana");
            var Tahmkench = EntityManager.Heroes.Enemies.Any(x => x.ChampionName == "Tahm Kench");
            var TF = EntityManager.Heroes.Enemies.Any(x => x.ChampionName == "Twisted Fate");
            var Nocturne = EntityManager.Heroes.Enemies.Any(x => x.ChampionName == "Nocturne");
            

            // Menu
            Menu = MainMenu.AddMenu("UB QSS", "UBQSS");
            Menu.AddGroupLabel("Made by Uzumaki Boruto");
            Menu.AddLabel("Dattenosa");
            Menu.AddSeparator();
            Menu.Add("Check", new KeyBind("Auto QSS", true, KeyBind.BindTypes.PressToggle, '.'));
            Menu.Add("Draw", new CheckBox("Draw QSS status"));
            Menu.Add("Show", new CheckBox("Show Delay QSS time on screen"));

            //CC Menu
            CC = Menu.AddSubMenu("Crowd control");
            {
                CC.AddGroupLabel("Auto QSS if :");
                CC.Add("Blind", new CheckBox("Blind", false));
                CC.Add("Charm", new CheckBox("Charm"));
                CC.Add("Fear", new CheckBox("Fear"));
                CC.Add("Knockup", new CheckBox("Knockup"));
                CC.Add("Polymorph", new CheckBox("Polymorph"));
                CC.Add("Taunt", new CheckBox("Taunt"));
                CC.Add("Stun", new CheckBox("Stun"));
                CC.Add("Snare", new CheckBox("Snare"));
                CC.Add("Suppression", new CheckBox("Suppression"));
                CC.Add("Silence", new CheckBox("Silence", false));
                CC.Add("CCDelay", new Slider("Delay for CC", 250, 0, 1000));
                CC.Add("EnemyManager", new CheckBox("Use QSS despite of no enemy around", false));
            }

            //Spells Menu
            Spells = Menu.AddSubMenu("Spells Manager");
            {
                Spells.AddGroupLabel("Spells");
                Spells.Add("Ignite", new CheckBox("Ignite"));
                Spells.Add("Exhaust", new CheckBox("Exhaust"));
                Spells.AddSeparator();
                Spells.AddLabel("Champion's Abilities");
                if (Tristana)
                {
                    Spells.Add("TristanaE", new CheckBox("Tristana E", false));
                }
                if (Tahmkench)
                {
                    Spells.Add("TahmkenchW", new CheckBox("Enemy Tahm Kench W", false));
                }
                if (TF)
                {
                    Spells.Add("TFsight", new CheckBox("Remove your sight from Twisted Fate R", false));
                }
                if (Nocturne)
                {
                    Spells.Add("Nocturnesight", new CheckBox("Remove the limit sight when Nocturne R", false));
                }
                if (Zed)
                {
                    Spells.Add("ZedUlt", new CheckBox("Zed Ultimate"));
                }
                if (Vladimir)
                {
                    Spells.Add("VladUlt", new CheckBox("Vladimir Ultimate"));
                }
                if (Fizz)
                {
                    Spells.Add("FizzUlt", new CheckBox("Fizz Ultimate"));
                }
                if (Mordekaiser)
                {
                    Spells.Add("MordUlt", new CheckBox("Mordekaiser Ultimate"));
                }
                if (Fiora)
                {
                    Spells.Add("FioraUlt", new CheckBox("Fiora Ultimate"));
                }

                Spells.Add("Damage", new CheckBox("Remove the bleed damage if it can kill your champion"));
                Spells.AddSeparator();
                Spells.Add("SpellsDelay", new Slider("Delay for spells", 800, 0, 2000));

                Obj_AI_Base.OnBuffGain += OnBuffGain;
                Drawing.OnDraw += OnDraw;
            }
        }

        private static void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe) return;
            var Type = args.Buff.Type;
            var Duration = args.Buff.EndTime - Game.Time;
            var Name = args.Buff.Name.ToLower();

            //CC
            if (Type == BuffType.Taunt && Taunt)
            {              
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }
            if (Type == BuffType.Stun && Stun)
            {
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }
            if (Type == BuffType.Snare && Snare)
            {
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }
            if (Type == BuffType.Polymorph && Polymorph)
            {
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }
            if (Type == BuffType.Blind && Blind)
            {
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }
            if (Type == BuffType.Flee && Fear)
            {
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }
            if (Type == BuffType.Charm && Charm)
            {
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }
            if (Type == BuffType.Suppression && Suppression)
            {
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }
            if (Type == BuffType.Silence && Silence)
            {
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }
            if (Type == BuffType.Knockup && Knockup)
            {
                if (Noenemy == true && Duration >= MinDurCC)
                {
                    UseQSS();
                }
                else if (Noenemy == false && (ObjectManager.Player.CountEnemiesInRange(1500) > 0) && Duration >= MinDurCC)
                {
                    UseQSS();
                }
            }


            //Spell
            if (Name == "twistedfatedestiny" && TFUlt)
            {
                SpellsQSS();
            }
            if (Name == "nocturneparanoia" && NocUlt)
            {
                SpellsQSS();
            }
            if (Name == "zedrdeathmark" && ZedUlt)
            {
                SpellsQSS();
            }
            if (Name == "vladimirhemoplague" && VladUlt)
            {
                SpellsQSS();
            }
            if (Name == "fizzmarinerdoom" && FizzUlt)
            {
                SpellsQSS();
            }
            if (Name == "mordekaiserchildrenofthegrave" && MordUlt)
            {
                SpellsQSS();
            }
            if (Name == "fioragrandchallenge" && FioraUlt)
            {
                SpellsQSS();
            }
            if (Name == "TristanaECharge" && TristanaE)
            {
                SpellsQSS();
            }
            if (Name == "TahmKenchW" && TahmkenchW)
            {
                SpellsQSS();
            }
            if (Name == "summonerdot" && Ignite)
            {
                SpellsQSS();
            }
            if (Name == "summonerexhaust" && Exhaust)
            {
                SpellsQSS();
            }

            //Bleeding Damage
            if (Type == BuffType.Damage && Damage && Player.Instance.HealthPercent <= 8)
            {
                UseQSS();
            }
            if (Type == BuffType.Poison && Damage && Player.Instance.HealthPercent <= 8)
            {
                UseQSS();
            }
        }
        
        private static void UseQSS()
        {
            if (!Check) return;

            if (QSS.IsOwned() && QSS.IsReady())
            {
                Core.DelayAction(() => QSS.Cast(), CC["CCDelay"].Cast<Slider>().CurrentValue);
            }

            if (Mercurial.IsOwned() && Mercurial.IsReady())
            {
                Core.DelayAction(() => Mercurial.Cast(), CC["CCDelay"].Cast<Slider>().CurrentValue);
            }
        }
        private static void SpellsQSS()
        {
            if (!Check) return;

            if (QSS.IsOwned() && QSS.IsReady())
            {
                Core.DelayAction(() => QSS.Cast(), Spells["SpellsDelay"].Cast<Slider>().CurrentValue);
            }

            if (Mercurial.IsOwned() && Mercurial.IsReady())
            {
                Core.DelayAction(() => Mercurial.Cast(), Spells["SpellsDelay"].Cast<Slider>().CurrentValue);
            }
        }

        //Draw
        private static void OnDraw(EventArgs args)
        {
            if (Draw)
            {
                var pos = Drawing.WorldToScreen(Player.Instance.Position);
                Drawing.DrawText(pos.X - 40, pos.Y + 50, Check ? Color.White : Color.Empty , Check ? "QSS ON" : "");
            }
            if (Show)
            {
                Drawing.DrawText(Drawing.Width - 200, 100, Color.White, "Delay for CC: " + MinDurCC);
                Drawing.DrawText(Drawing.Width - 200, 120, Color.White, "Delay for Spells: " + MinDurSpells);
            }

        }
    }
}
