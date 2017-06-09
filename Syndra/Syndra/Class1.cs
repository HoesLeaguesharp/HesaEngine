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

namespace Syndra
{
    public class Class1 : IScript
    {

        public string ChampName = "Syndra";

        public string Name => "sSyndra";
        public string Version => "1.0.0";
        public string Author => "Hoes";
       

        public void OnInitialize()
        {
            Game.OnGameLoaded += Game_OnGameLoaded;
        }

        private void Game_OnGameLoaded()
        {
            if (ObjectManager.Me.ChampionName != ChampName)
                return;

            SpellManager spellManager = new SpellManager();
            spellManager.LoadSpells();

            MenuL loadMenu = new MenuL();
            loadMenu.LoadMenu();

            OrbManager loadOrb = new OrbManager();
            loadOrb.LoadEvents();

            Game.OnTick += OnTick;
            // Drawing.OnEndScene += EndScene;

        }

        private int _lasT = Environment.TickCount;
        private int _count;
        private const int Timer = 10000;
        public void AntiAfk()
        {
            if (Environment.TickCount - _lasT < Timer) return;

            ObjectManager.Me.IssueOrder(GameObjectOrder.MoveTo,
                new Vector3(ObjectManager.Me.Position.X + (_count % 2 == 0 ? 100 : -100),
                    ObjectManager.Me.Position.Y,
                    ObjectManager.Me.Position.Z));
            _count++;
            _lasT = Environment.TickCount;
        }
        
        
        private void OnTick()
        {     
            OrbManager orbsManager = new OrbManager();
            orbsManager.LoadOrbs();
            //  orbsManager.Display();
            // AntiAfk();

            foreach (var enemy in ObjectManager.Heroes.Enemies.Where(x=> x.IsValidTarget(SpellManager.Eq.Range)))
            {
                UseE(enemy);
            }
        }
       

        private void UseE(Obj_AI_Base enemy)
        {
            OrbManager orbsManager = new OrbManager();
            foreach (var orb in orbsManager.GetDict().)
            {
                if (ObjectManager.Player.Distance(orb) < SpellManager.E.Range + 100)
                {
                    Logger.Log("Hi");
                    var startPoint = orb.Value.Pos.To2D().Extend(ObjectManager.Player.ServerPosition.To2D(), 100);
                    var endPoint = ObjectManager.Player.ServerPosition.To2D()
                        .Extend(orb.Value.Pos.To2D(), ObjectManager.Player.Distance(orb.Value.Pos) > 200 ? 1300 : 1000);
                    SpellManager.Eq.Delay = SpellManager.E.Delay +
                                            ObjectManager.Player.Distance(orb.Value.Pos) / SpellManager.E.Speed;
                    SpellManager.Eq.From = orb.Value.Pos;
                    var enemyPred = SpellManager.Eq.GetPrediction(enemy);
                    if (enemyPred.Hitchance >= HitChance.High
                        && enemyPred.UnitPosition.To2D().Distance(startPoint, endPoint, false)
                        < SpellManager.Eq.Width + enemy.BoundingRadius)
                    {
                        Logger.Log("Hi");
                        SpellManager.E.Cast(orb.Value.Pos, true);
                        SpellManager.W.LastCastAttemptT = Utils.TickCount;
                        return;
                    }
                }
            }
        }
    }
}
