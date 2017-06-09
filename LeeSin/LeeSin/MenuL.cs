using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using SharpDX;
using HesaEngine.SDK.Enums;
using SharpDX.DirectInput;

namespace LeeSin
{
   

    public class MenuL
    {
        public Menu Home, insecMenu, comboMenu;

        public void OnLoad()
        {
            Home = Menu.AddMenu("Lee Sin");

            insecMenu = AddSMenu(Home, "Insec");
            var useinsec = insecMenu.Add(new MenuKeybind("useInsec", "Enable Insec Ward", new KeyBind(SharpDX.DirectInput.Key.K, MenuKeybindType.Hold)));
            useinsec.OnValueChanged += (keybind, b) => { Program.KeyActive = b; };
            var useinsecF = insecMenu.Add(new MenuKeybind("useInsecF", "Enable Insec Flash", new KeyBind(SharpDX.DirectInput.Key.G, MenuKeybindType.Hold)));
            useinsecF.OnValueChanged += (keybind, b) => { Program.KeyActiveF = b; };

            comboMenu = AddSMenu(Home, "Combo");
            var useQ1C = AddMenuCheckBox("useQc", "Use [Q]", comboMenu);
            useQ1C.OnValueChanged += (checkbox, b) => { Program.UseQ1C = b; };
            var useQ2C = AddMenuCheckBox("useQ2c", "Use Second [Q]", comboMenu);
            useQ2C.OnValueChanged += (checkbox, b) => { Program.UseQ2C = b; };
            var useQ2Cd = comboMenu.Add(new MenuSlider("useq2D", "Use Second [Q] Delay", new Slider(0, 2000, 500)));
            useQ2Cd.OnValueChanged += (slider, i) => { Program.ComboQ2Delay = i; };
            var useEc = AddMenuCheckBox("useEc", "Use [E]", comboMenu);
            useEc.OnValueChanged += (checkbox, b) => { Program.UseEc = b; };
            var useRc = AddMenuCheckBox("useRc", "Use [R]", comboMenu);
            useRc.OnValueChanged += (checkbox, b) => { Program.UseRc = b; };

        }

        public MenuCheckbox AddMenuCheckBox(string menuName, string displayName, Menu MenuName, bool State = true)
        {
            return MenuName.Add(new MenuCheckbox(menuName, displayName, State));
        }

        public Menu AddSMenu(Menu menuName, string name)
        {
            return menuName.AddSubMenu(name);
        }
    }

}
