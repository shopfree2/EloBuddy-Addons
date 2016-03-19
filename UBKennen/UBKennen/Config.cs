using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;


namespace UBKennen
{
    public class Config
    {     
        public static Menu Menu { get; private set; }
        public static Menu ComboMenu { get; private set; }
        public static Menu HarassMenu { get; private set; }
        public static Menu LaneClear { get; private set; }
        public static Menu JungleClear { get; private set; }
        public static Menu LasthitMenu { get; private set; }
        public static Menu MiscMenu { get; private set; }
        public static Menu DrawMenu { get; private set; }     

        public static void Dattenosa()
        {
            // Menu
            Menu = MainMenu.AddMenu("UB Kennen", "UBKennen");          
            Menu.AddGroupLabel("Made by Uzumaki Boruto");

            //ComboMenu
            ComboMenu = Menu.AddSubMenu("Combo");
            {
                ComboMenu.AddGroupLabel("Combo Settings");
                ComboMenu.Add("useQCombo", new CheckBox("Dùng Q"));
                ComboMenu.Add("sep1", new Separator());
                ComboMenu.Add("useWCombo", new CheckBox("Dùng W"));
                ComboMenu.Add("WHitCombo", new Slider("Chỉ dùng W nếu dính {0} tướng địch", 1, 1, 5));
                ComboMenu.Add("sep2", new Separator());
                ComboMenu.Add("useECombo", new CheckBox("Dùng E", false));
                ComboMenu.Add("sep3", new Separator());
                ComboMenu.Add("useRCombo", new CheckBox("Dùng R"));
                ComboMenu.Add("RHitCombo", new Slider("Số champ địch tối thiểu để kích hoạt R", 2, 1, 5));
                ComboMenu.Add("sep", new Separator());
                ComboMenu.Add("useIg", new CheckBox("Dùng Thiêu Đốt nếu đủ kết liễu"));
            }

            //HarassMenu
            HarassMenu = Menu.AddSubMenu("Harass");
            {
                HarassMenu.AddGroupLabel("Harass Settings");
                HarassMenu.Add("useQ", new CheckBox("Dùng Q"));
                HarassMenu.Add("useW", new CheckBox("Dùng W"));
                HarassMenu.Add("HrEnergyManager", new Slider("Nếu nội năng dưới {0} ngưng cấu máu", 0, 0, 200));
            }

            //LaneJungleClear Menu
            LaneClear = Menu.AddSubMenu("LaneClear");
            {
                LaneClear.AddGroupLabel("Laneclear Settings");
                LaneClear.Add("useQLc", new CheckBox("Dùng Q để xóa sổ lính", false));
                LaneClear.Add("useWLc", new CheckBox("Dùng W để xóa sổ lính", false));
                LaneClear.Add("WHitLc", new Slider("Chỉ dùng W nếu dính {0} lính", 5, 1, 30));
                LaneClear.Add("useELc", new CheckBox("Dùng E để xóa sổ lính", false));
                LaneClear.Add("sep4", new Separator());
                LaneClear.Add("EnergyManager", new Slider("Nếu nội năng dưới {0} ngưng dùng skill để xóa sổ lính", 0, 0, 200));
                LaneClear.Add("sep5", new Separator(40));
            }
            //JungleClear Menu
            JungleClear =Menu.AddSubMenu("JungleClear");
            {
                JungleClear.AddGroupLabel("Jungleclear Settings");
                JungleClear.Add("useQJc", new CheckBox("Dùng Q để xóa sổ quái"));
                JungleClear.Add("useWJc", new CheckBox("Dùng W để xóa sổ quái"));
                JungleClear.Add("WHitJc", new Slider("Dùng W nếu dính {0} quái", 2, 1, 4));
                JungleClear.Add("useEJc", new CheckBox("Dùng E để xóa sổ quái"));
                JungleClear.Add("JcEnergyManager", new Slider("Nếu nội năng dưới {0} ngưng dùng skill để xóa sổ quái", 0, 0, 200));
            }

            //LasthitMenu
            LasthitMenu = Menu.AddSubMenu("Lasthit");
            {
                LasthitMenu.Add("useQLh", new CheckBox("Dùng Q để lasthit"));
                LasthitMenu.Add("useWLh", new CheckBox("Dùng W để lasthit"));
            }

            //DrawMenu
            DrawMenu = Menu.AddSubMenu("Drawings");
            {
                DrawMenu.Add("drawQ", new CheckBox("Vẽ Draw Q", false));
                DrawMenu.Add("drawW", new CheckBox("Vẽ Draw W", false));
                DrawMenu.Add("drawR", new CheckBox("Vẽ Draw R", false));
            }

            //MiscMenu          
            MiscMenu = Menu.AddSubMenu("MiscMenu");
            {
                MiscMenu.AddGroupLabel("Misc Settings");
                MiscMenu.AddLabel("Anti Gapcloser");
                MiscMenu.Add("useQAG", new CheckBox("Dùng Q để chống GapCloser"));
                MiscMenu.Add("useWAG", new CheckBox("Dùng W để chống Gapcloser"));
                MiscMenu.Add("useEAG", new CheckBox("Dùng E để chống Gapcloser"));

                MiscMenu.AddLabel("Killsteal Settings");
                MiscMenu.Add("useQKS", new CheckBox("Dùng Q để KS"));
                MiscMenu.Add("useWKS", new CheckBox("Dùng W để KS"));

                MiscMenu.AddLabel("Activator Item");
                MiscMenu.Add("item.1", new CheckBox("Tự dùng Gươm hải tặc"));
                MiscMenu.Add("item.1MyHp", new Slider("Lượng máu bản thân thấp hơn {0}%", 95));
                MiscMenu.Add("item.1EnemyHp", new Slider("Lượng máu kẻ địch thấp hơn {0}%", 70));
                MiscMenu.Add("item.sep", new Separator());

                MiscMenu.Add("item.2" , new CheckBox("Tự dùng Gươm vô danh"));
                MiscMenu.Add("item.2MyHp", new Slider("Lượng máu bản thân thấp hơn {0}%", 80));
                MiscMenu.Add("item.2EnemyHp", new Slider("Lượng máu kẻ địch thấp hơn {0}%", 70));
                MiscMenu.Add("sep7", new Separator());

                MiscMenu.Add("item.3" , new CheckBox("Tự dùng Dồng Hồ cát"));
                MiscMenu.Add("item.3MyHp", new Slider("Your HP lower than {0}%", 50));
            }         
        }                     
    }
}     
