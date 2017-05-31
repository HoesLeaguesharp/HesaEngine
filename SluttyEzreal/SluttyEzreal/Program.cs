using System;
using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using SharpDX;

namespace SluttyEzreal
{
    public class Program : IScript
    {
        public void OnInitialize()
        {
            Logger.Log("Hello World");

            Game.OnGameLoaded += Game_OnGameLoaded;

        }

        /// <summary>
        /// Will be called upon Game load
        /// </summary>
        private void Game_OnGameLoaded()
        {
            if (ObjectManager.Me.Hero != Champion)
                return;

            SpellManager.LoadSpells();
            Config.MenuAdd();


            Drawing.OnEndScene += Drawing_OnDraw;

            Orbwalker.AfterAttack += orb_AfterATT;
            Game.OnTick += Game_OnTick;
        }



        /// <summary>
        /// Drawing
        /// </summary>
        /// <param name="args"></param>
        private void Drawing_OnDraw(EventArgs args)
        {
            if (!Config.drawMenu.GetCheckbox("enableD")) return;

            Vector2 textPosE = new Vector2(ObjectManager.Me.Position.X + 20, ObjectManager.Me.Position.Y + 20);
            Vector2 textPosQ = new Vector2(ObjectManager.Me.Position.X + SpellManager.Q.Range + 20, ObjectManager.Me.Position.Y + SpellManager.Q.Range + 20);

            if (Config.drawMenu.GetCheckbox("drawQ"))
            {
                Drawing.DrawText(textPosQ, Color.Red, "Q");
                Drawing.DrawCircle(ObjectManager.Me.Position, SpellManager.Q.Range, SpellManager.Q.IsReady() ? Color.Green : Color.Red);
            }

            if (Config.drawMenu.GetCheckbox("drawE"))
            {
                Drawing.DrawText(textPosE, Color.Red, "E");
                Drawing.DrawCircle(ObjectManager.Me.Position, SpellManager.E.Range, SpellManager.E.IsReady() ? Color.Green : Color.Red);
            }


        }


        /// <summary>
        /// After Attack event, gets called after an attack.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="target"></param>
        private void orb_AfterATT(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe) return;
            if (!target.IsValidTarget())
                return;

            if (!target.IsEnemy) return;
            if (Orb.ActiveMode != Orbwalker.OrbwalkingMode.Combo)
                return;

            if (Config.comboMenu.GetCheckbox("useQ") && SpellManager.Q.IsReady())
            {
                SpellManager.Q.Cast((Obj_AI_Base)target);
            }
        }



        /// <summary>
        /// Updates each tick.
        /// </summary>
        private static void Game_OnTick()
        {
            Main.KillSteal();
            switch (Orb.ActiveMode)
            {

                case Orbwalker.OrbwalkingMode.LastHit:
                    break;
                case Orbwalker.OrbwalkingMode.LaneClear:
                    Main.DoLane();
                    break;
                case Orbwalker.OrbwalkingMode.JungleClear:
                    break;
                case Orbwalker.OrbwalkingMode.Combo:
                    Main.DoCombo();
                    break;
                case Orbwalker.OrbwalkingMode.Freeze:
                    break;
                case Orbwalker.OrbwalkingMode.CustomMode:
                    break;
                case Orbwalker.OrbwalkingMode.Harass:
                    Main.DoHarass();
                    break;
                case Orbwalker.OrbwalkingMode.Flee:
                    break;
                case Orbwalker.OrbwalkingMode.None:
                    Extra.StackTear();
                    break;
            }
        }

        public Champion Champion = Champion.Ezreal;
        public string Name => "Slutty Ezreal";
        public string Version => "1.0.0";
        public string Author => "Hoes";
        public static Orbwalker.OrbwalkerInstance Orb;
    }
}
