using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;


namespace UBSivir
{
    class Config
    {
        public static Menu Menu { get; private set; }
        public static Menu ComboMenu { get; private set; }
        public static Menu ShieldMenu { get; private set; }
        public static Menu ShieldMenu2 { get; private set; }
        public static Menu HarassMenu { get; private set; }
        public static Menu LaneClear { get; private set; }
        public static Menu JungleClear { get; private set; }
        public static Menu LasthitMenu { get; private set; }
        public static Menu MiscMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }

        public static void Dattenosa()
        {
            // Menu
            Menu = MainMenu.AddMenu("UB Sivir", "UBSivir");
            Menu.AddGroupLabel("Made by Uzumaki Boruto");
            Menu.AddLabel("Dattenosa");

            //ComboMenu
            ComboMenu = Menu.AddSubMenu("Combo");
            {
                ComboMenu.AddGroupLabel("Combo Settings");
                ComboMenu.Add("useQCombo", new CheckBox("Use Q"));
                ComboMenu.Add("AutoQ", new CheckBox("Auto Q if target in immobilize(soon)", false));
                ComboMenu.Add("useWCombo", new CheckBox("Use W"));
                ComboMenu.Add("useRCombo", new CheckBox("Use R"));
                ComboMenu.Add("RHitCombo", new Slider("Use R if buff {0} ally & you in combo mode", 4, 1, 4));
                ComboMenu.Add("sep", new Separator());
                ComboMenu.AddLabel("Use spells");
                ComboMenu.Add("useheal", new CheckBox("Use Heal"));
                ComboMenu.Add("manageheal", new Slider("If my HP below {0}% use Heal", 15, 1, 80));
                ComboMenu.Add("usehealally", new CheckBox("Use heal to save ally"));
                ComboMenu.Add("managehealally", new Slider("If ally's HP below {0}% use Heal to save", 10, 1, 80));
            }
            //Shield Menu
            ShieldMenu = Menu.AddSubMenu("ShieldMenu");
            {
                ShieldMenu.AddGroupLabel("Block Options");
                ShieldMenu.Add("blockSpellsE", new CheckBox("Auto-Block Spells (E)"));
                ShieldMenu.Add("Evade", new CheckBox("Evade is Integration"));
                ShieldMenu.AddSeparator();

                ShieldMenu.AddGroupLabel("Enemies spells to block");
            }
            //Shield Menu2
            ShieldMenu2 = Menu.AddSubMenu("ShieldMenu2");
            {
                ShieldMenu2.AddGroupLabel("Core Options");
                ShieldMenu2.Add("BlockChalleningE", new CheckBox("Auto use E to Block Channeling Spells"));
                ShieldMenu2.AddSeparator();

                ShieldMenu2.AddGroupLabel("Enemies spells to block");
            }

            //HarassMenu
            HarassMenu = Menu.AddSubMenu("Harass");
            {
                HarassMenu.AddGroupLabel("Harass Settings");
                HarassMenu.Add("useQHr", new CheckBox("Use Q"));
                HarassMenu.Add("useWHr", new CheckBox("Use W"));
                HarassMenu.Add("autoWHr", new CheckBox("Auto W if maybe hit enemy(soon)", false));
                HarassMenu.Add("HrManage", new Slider("If mana percent below {0} stop harass", 50));
            }

            //LaneClear Menu
            LaneClear = Menu.AddSubMenu("LaneClear");
            {
                LaneClear.AddGroupLabel("Laneclear Settings");
                LaneClear.Add("useQLc", new CheckBox("Use Q to laneclear", false));
                LaneClear.Add("useWLc", new CheckBox("Use W to laneclear", false));
                LaneClear.Add("WHitLc", new Slider("Only Use W if hit {0} minion(s)", 6, 1, 30));
                LaneClear.Add("sep4", new Separator());
                LaneClear.Add("LcManager", new Slider("If mana percent below {0} stop use skill to laneclear", 50));
            }
            //JungleClear Menu
            JungleClear = Menu.AddSubMenu("JungleClear");
            {
                JungleClear.AddGroupLabel("Jungleclear Settings");
                JungleClear.Add("useQJc", new CheckBox("Use Q to jungleclear"));
                JungleClear.Add("useWJc", new CheckBox("Use W to jungleclear"));
                JungleClear.Add("WHitJc", new Slider("Use W if hit {0} monster(s)", 2, 1, 4));
                JungleClear.Add("useEJc", new CheckBox("Use E to jungleclear", false));
                JungleClear.Add("JcManager", new Slider("If mana percent below {0} stop use skill to jungleclear", 50));
            }

            //LasthitMenu
            LasthitMenu = Menu.AddSubMenu("Lasthit");
            {
                LasthitMenu.Add("useQLh", new CheckBox("Use Q to lasthit"));
                LasthitMenu.Add("useWLh", new CheckBox("Use W to lasthit"));
                LasthitMenu.Add("LhManager", new Slider("If mana percent below {0} stop use skill to lasthit", 50));
            }

            //DrawMenu
            DrawMenu = Menu.AddSubMenu("Drawings");
            {
                DrawMenu.Add("drawQ", new CheckBox("Draw Q", false));
                DrawMenu.Add("drawR", new CheckBox("Draw R", false));
            }

            //MiscMenu          
            MiscMenu = Menu.AddSubMenu("MiscMenu");
            {
                MiscMenu.AddGroupLabel("Misc Settings");
                MiscMenu.AddLabel("Killsteal Settings");
                MiscMenu.Add("useQKS", new CheckBox("Use Q to KS"));

                MiscMenu.AddLabel("Activator Item");
                MiscMenu.Add("item.1", new CheckBox("Auto use Bilgewater Cutlass"));
                MiscMenu.Add("item.1MyHp", new Slider("My HP less than {0}%", 95));
                MiscMenu.Add("item.1EnemyHp", new Slider("Enemy HP less than {0}%", 70));
                MiscMenu.Add("item.sep", new Separator());

                MiscMenu.Add("item.2", new CheckBox("Auto use Blade of the Ruined King"));
                MiscMenu.Add("item.2MyHp", new Slider("My HP less than {0}%", 80));
                MiscMenu.Add("item.2EnemyHp", new Slider("Enemy HP less than {0}%", 70));
                MiscMenu.Add("sep7", new Separator());
            }
        }
    }
}
