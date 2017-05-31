using HesaEngine.SDK;

namespace SluttyEzreal
{
    internal static class Config
    {
        public static Menu Home, comboMenu, laneMenu, extraMenu, KillSteal, harassMenu, drawMenu;

        /// <summary>
        /// Menu
        /// </summary>
        public static void MenuAdd()
        {

            Home = Menu.AddMenu("Slutty Ezreal");

            Program.Orb = new Orbwalker.OrbwalkerInstance(Home.AddSubMenu("Orbwalker"));

            comboMenu = AddSMenu("Combo", Home);
            AddMenuCheckBox("useQ", "Use Q", comboMenu);
            AddMenuCheckBox("useW", "Use W", comboMenu);
            AddMenuCheckBox("useWA", "Use W On Allies", comboMenu);
            AddMenuCheckBox("useR", "Use R", comboMenu);

            laneMenu = AddSMenu("Lane", Home);
            AddMenuSlider("minManaL", "Min Mana %", laneMenu, 30);
            AddMenuCheckBox("useQl", "Use Q", laneMenu);
            AddMenuCheckBox("usQllh", "Use Q LH only", laneMenu);

            harassMenu = AddSMenu("Harass", Home);
            AddMenuSlider("harassMinMana", "Min Mana", harassMenu, 90);
            AddMenuCheckBox("useQh", "Use Q", harassMenu);
            AddMenuCheckBox("useWh", "Use W", harassMenu);

            extraMenu = AddSMenu("Extra", Home);
            AddMenuSlider("tearMinMana", "Min Mana", extraMenu, 90);
            AddMenuCheckBox("stackTear", "Stack Tear", extraMenu);
            AddMenuCheckBox("stackTearF", "Stack Tear Fountain Only", extraMenu);

            KillSteal = AddSMenu("Kill Steal", Home);
            AddMenuCheckBox("QKS", "Use Q", KillSteal);
            AddMenuCheckBox("RKS", "Use R", KillSteal);

            drawMenu = AddSMenu("Draw Menu", Home);
            AddMenuCheckBox("enableD", "Enable Drawing", drawMenu);
            AddMenuCheckBox("drawQ", "Draw Q Range", drawMenu);
            AddMenuCheckBox("drawE", "Draw E Range", drawMenu);

        }




        public static Menu AddSMenu(string Name, Menu MenuName)
        {
            return MenuName.AddSubMenu(Name);
        }

        public static MenuCheckbox AddMenuCheckBox(string menuName, string displayName, Menu MenuName, bool State = true)
        {
            return MenuName.Add(new MenuCheckbox(menuName, displayName, State));
        }

        public static MenuSlider AddMenuSlider(string menuName, string displayName, Menu MenuName, int setValue,
            int minValue = 0, int maxValue = 100)
        {
            return MenuName.Add(new MenuSlider(menuName, displayName, minValue, maxValue, setValue));
        }

        /// <summary>
        /// Thanks @BoliBerries for these.
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetCheckbox(this Menu menu, string value)
        {
            return menu.Get<MenuCheckbox>(value).Checked;
        }

        public static bool GetKeybind(this Menu menu, string value)
        {
            return menu.Get<MenuKeybind>(value).Active;
        }

        public static int GetSlider(this Menu menu, string value)
        {
            return menu.Get<MenuSlider>(value).CurrentValue;
        }

        public static int GetCombobox(this Menu menu, string value)
        {
            return menu.Get<MenuCombo>(value).CurrentValue;
        }
    }
}
