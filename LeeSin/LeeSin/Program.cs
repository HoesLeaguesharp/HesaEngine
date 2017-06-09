using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using SharpDX;

namespace LeeSin
{
    public class Program : IScript
    {
        public string Name => "Lee Sin";
        public string Version => "1.0.0";
        public string Author => "Hoes";

        public static int LastWardT;
        public static int LastWCastT;
        public static Vector3 WardPos = new Vector3();
        public static bool KeyActive;
        public static int LastRCastT;
        public static Spell Q, W, E, R, Rnormal;
        public SpellSlot FlashSlot, Smite;
        public static Vector3 PlayerPosAfterQ= new Vector3();
        public static int LastQCastT;
        public static bool UseQ1C;
        public static bool UseQ2C;
        public static int ComboQ2Delay;
        public static bool UseEc;
        public static bool UseRc;
        public static bool KeyActiveF;
        public static Orbwalker.OrbwalkerInstance Orb;

        public static int WardRange(Obj_AI_Base target)
        {
            return target.IsFacing(ObjectManager.Player) ? 200 : 350;
        }

        public void OnInitialize()
        {
            Game.OnGameLoaded += OnGameLoad;
        }



        private void OnGameLoad()
        {
            Q = new Spell(SpellSlot.Q, 1050);
            W = new Spell(SpellSlot.W, 700);
            E = new Spell(SpellSlot.E, 350);
            R = new Spell(SpellSlot.R, 400);
            Rnormal = new Spell(SpellSlot.R, 700);
            FlashSlot = ObjectManager.Player.GetSpellSlot("summonerflash");
            Rnormal.SetSkillshot(0f, 70f, 1500f, false, SkillshotType.SkillshotLine);
            Q.SetSkillshot(0.5f, 40, 2000, true, SkillshotType.SkillshotLine);

            foreach (var spell in ObjectManager.Player.Spellbook.Spells.Where(spell => spell.SpellData.Name.ToLower()
                .Contains("smite")))
            {
                Smite = spell.Slot;
            }
            var menuLoad = new MenuL();
            menuLoad.OnLoad();
            var insec = new Insec();

            Game.OnTick += OnTickGame;
            Drawing.OnEndScene += insec.OnEndScene;
            Obj_AI_Base.OnProcessSpellCast += Insec.OnProc;
            Obj_AI_Base.OnCreate += Insec.OnCreate;

        }

        public int Angle(Obj_AI_Base firstPoint, Obj_AI_Base secondPoint, Obj_AI_Base thirdPoint)
        {
            //var x1 = firstPoint.Position.X;
            //var y1 = firstPoint.Position.Y;
            //var z1 = firstPoint.Position.Z;

            //var x2 = secondPoint.Position.X;
            //var y2 = secondPoint.Position.Y;
            //var z2 = secondPoint.Position.Z;

            //var x3 = thirdPoint.Position.X;
            //var y3 = thirdPoint.Position.Y;
            //var z3 = thirdPoint.Position.Z;

            var a = firstPoint.Distance(secondPoint); //target-minion
            var c = firstPoint.Distance(thirdPoint); // target-player
            var b = secondPoint.Distance(thirdPoint); // minion-player
            var angle = ((-(a * a) + (b * b) + (c * c)) / (2 * b * c));
            var cosangle = Math.Acos(angle);
            var returnthis = (cosangle * 180) / Math.PI;
            return (int) returnthis;
        }
        public static bool Q1()
        {
            return ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).SpellData.Name == "BlindMonkQOne";
        }

        public static bool Q2()
        {
            return ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).SpellData.Name == "blindmonkqtwo";
        }

        public static float GetQDamage(Obj_AI_Base unit)
        {
            var firstq = Q.GetDamage(unit);
            var secondq = Q.GetDamage(unit) + (unit.MaxHealth - unit.Health - Q.GetDamage(unit)) * 0.08;
            return (float)(firstq + secondq);
        }

        private void OnTickGame()
        {
            //switch (Orb.ActiveMode)
            //{
            //    case Orbwalker.OrbwalkingMode.LastHit:
            //        break;
            //    case Orbwalker.OrbwalkingMode.LaneClear:
            //        break;
            //    case Orbwalker.OrbwalkingMode.JungleClear:
            //        break;
            //    case Orbwalker.OrbwalkingMode.Combo:
            //       // Mode.DoCombo();
            //        break;
            //    case Orbwalker.OrbwalkingMode.Freeze:
            //        break;
            //    case Orbwalker.OrbwalkingMode.CustomMode:
            //        break;
            //    case Orbwalker.OrbwalkingMode.Harass:
            //        break;
            //    case Orbwalker.OrbwalkingMode.Flee:
            //        break;
            //    case Orbwalker.OrbwalkingMode.None:
            //        break;
            //}
            if (Environment.TickCount - LastQCastT > 5000)
                PlayerPosAfterQ = new Vector3();

            Insec insec = new Insec();
            var target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget()) return;
            var qpred = Q.GetPrediction(target);
            var col = qpred.CollisionObjects;

            if (KeyActiveF && R.IsReady())
            {
                R.Cast(target);
            }

            if (KeyActive)
            {
                if (Q2())
                    Q.Cast();


                if (target.Distance(ObjectManager.Player) > 700 - WardRange(target) && Q.IsReady() && col.Count == 0)
                    Q.Cast(qpred.CastPosition);


                if (target.Distance(ObjectManager.Player) > 600 - WardRange(target) && Q.IsReady() && col.Count != 0)
                {
                    
                    foreach (var min in ObjectManager.MinionsAndMonsters.Enemy.Where(
                            x => x.IsValid() && x.Distance(ObjectManager.Player) <= Q.Range && Q.GetDamage(x) < x.Health &&
                                 x.Distance(insec.WardPos(target)) <= 600 && Angle(target, x, ObjectManager.Player) <= 60)
                        .OrderByDescending(y => y.Distance(target)))
                    {
                        Q.Cast(min.Position);
                    }
                }
                insec.DoInsec(target);
            }

        }
    }
}
