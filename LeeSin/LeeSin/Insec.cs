using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.Args;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;
using SharpDX;

namespace LeeSin
{
    public class Insec
    {
        public Obj_AI_Base Player = ObjectManager.Player;
        private Vector3 _pPos = new Vector3();
        public Vector3 WardPos(Obj_AI_Base target)
        {
            Prediction getPrediction = new Prediction();
            var playerPos = Player.Position;
            var targetPosPred = getPrediction.GetPrediction(target, 0.3f).UnitPosition;

            _pPos = Environment.TickCount - Program.LastQCastT < 5000 ? Program.PlayerPosAfterQ : playerPos;

            return _pPos.Extend(target.Position, _pPos.Distance(target.Position) + Program.WardRange(target));
        }

        public void DoInsec(Obj_AI_Base target)
        {
            var mins = ObjectManager.MinionsAndMonsters.Ally.Where(
                x => x.IsValid() && x.Name.ToLower().Contains("ward") && x.Distance(target) <= 500 &&
                     x.Distance(WardPos(target)) <= 150);

            if (Program.R.IsReady())
            {
                if (!mins.Any() && Environment.TickCount - Program.LastWardT > 3000 && Environment.TickCount - Program.LastWCastT > 3000)
                    Shop.UseItem(ItemId.Warding_Totem_Trinket, WardPos(target));

                if (Program.W.IsReady())
                {
                    Program.W.Cast(mins.FirstOrDefault());
                }
                if (Program.WardPos != new Vector3()  && Program.R.IsReady() && Player.Distance(Program.WardPos) < 200)
                {
                    Program.R.Cast(target);   
                         
                    if (Environment.TickCount - Program.LastRCastT < 2000)  
                    Program.WardPos = new Vector3();
                }
            }

            if (Environment.TickCount - Program.LastRCastT < 1000 && Program.Q.IsReady())
                Program.Q.Cast(target.Position);
        }



        public  void OnEndScene(EventArgs args)
        {
            var insec = new Insec();
            var target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Physical);
            if (target.IsValidTarget())
                Drawing.DrawCircle(insec.WardPos(target), 70, Color.Red, 3);

        }

        public static void OnProc(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args) 
        {
            var insec = new Insec();
            if (!sender.IsMe) return;
            if (Program.KeyActiveF)
            {
                Logger.Log(args.SData.Name);
                if (args.SData.Name == "BlindMonkRKick") //check args.Target
                {
                    var target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Physical);
                    if (!target.IsValidTarget()) return; 

                        ObjectManager.Player.Spellbook.CastSpell(
                            ObjectManager.Player.GetSpellSlot(SummonerSpells.Flash), insec.WardPos(target));
                }
            }
            switch (args.SData.Name)
            {
                case "BlinkMonkRKick":
                    Program.LastRCastT = Environment.TickCount;
                    break;
                case "BlindMonkWOne":
                    Program.LastWCastT = Environment.TickCount;
                    Program.WardPos = args.End;
                    break;
                case "BlindMonkQOne":
                    Program.PlayerPosAfterQ = ObjectManager.Player.Position;
                    Program.LastQCastT = Environment.TickCount;
                    break;
            }
        }

        public static void OnCreate(Obj_AI_Base sender, EventArgs args)
        {
            if (sender.Name.ToLower().Contains("ward") && sender.IsAlly)
            {
                Program.WardPos = sender.Position;            
                Program.LastWardT = Environment.TickCount;
            }
        }
    }
}
