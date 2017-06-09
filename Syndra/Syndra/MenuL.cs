using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;

namespace Syndra
{
    class MenuL
    {
        public Menu Home, comboMenu;
        public bool UseQ, UseW, UseE,UseR;
        public void LoadMenu()
        {
            Home = Menu.AddMenu("Syndra");

            comboMenu = AddSMenu(Home, "Combo");
            var useQ = AddMenuCheckBox("useQ", "Use Q", comboMenu);
            useQ.OnValueChanged += (checkbox, newValue) => { UseQ = newValue; };
            var useW = AddMenuCheckBox("useW", "Use W", comboMenu);
            useW.OnValueChanged += (checkbox, newValue) => { UseW = newValue; };
            var useE = AddMenuCheckBox("useE", "Use E", comboMenu);
            useE.OnValueChanged += (checkbox, newValue) => { UseE = newValue; };
            var useR = AddMenuCheckBox("useR", "Use R", comboMenu);
            useR.OnValueChanged += (checkbox, newValue) => { UseR = newValue; };

        }


        public Menu AddSMenu(Menu MenuName, string Name)
        {
            return MenuName.AddSubMenu(Name);
        }

        public MenuCheckbox AddMenuCheckBox(string menuName, string displayName, Menu MenuName, bool State = true)
        {
            return MenuName.Add(new MenuCheckbox(menuName, displayName, State));
        }

        public MenuSlider AddMenuSlider(string menuName, string displayName, Menu MenuName, int setValue,
            int minValue = 0, int maxValue = 100)
        {
            return MenuName.Add(new MenuSlider(menuName, displayName, minValue, maxValue, setValue));
        }
    }
}
