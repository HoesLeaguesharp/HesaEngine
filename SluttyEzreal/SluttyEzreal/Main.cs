using System.Linq;
using HesaEngine.SDK;
using HesaEngine.SDK.GameObjects;

namespace SluttyEzreal
{
    public static class Main
    {
        public static AIHeroClient Player = ObjectManager.Me;


        /// <summary>
        /// Performs Combo
        /// </summary>
        public static void DoCombo()
        {
            var targetSelect = TargetSelector.GetTarget(SpellManager.Q.Range, TargetSelector.DamageType.Physical);

            if (!targetSelect.IsValidTarget() || Player.IsWindingUp)
                return;

            if (Config.comboMenu.GetCheckbox("useQ") && SpellManager.Q.IsReady())
            {
                if (Player.Distance(targetSelect) > Player.AttackRange)
                    SpellManager.Q.Cast(targetSelect);
            }

            if (Config.comboMenu.GetCheckbox("useWA"))
            {
                foreach (var hero in Player.GetAlliesInRange(SpellManager.W.Range))
                {
                    SpellManager.W.Cast(hero);
                }
            }

            if (Config.comboMenu.GetCheckbox("useW") && SpellManager.W.IsReady())
            {
                SpellManager.W.Cast(targetSelect);
            }
        }



        /// <summary>
        /// Performs Lane clear
        /// </summary>
        public static void DoLane()
        {
            if (!SpellManager.Q.IsReady()) return;
            var minion = ObjectManager.MinionsAndMonsters.Enemy.Where(x => x.IsValidTarget(SpellManager.Q.Range));

            foreach (var min in minion)
            {
                if (min.Health <= SpellManager.Q.GetDamage(min) && Config.laneMenu.GetCheckbox("usQllh"))
                    SpellManager.Q.Cast(min);

                if (Config.laneMenu.GetCheckbox("useQl") && !Config.laneMenu.GetCheckbox("usQllh"))
                    SpellManager.Q.Cast(min);
            }
        }

        /// <summary>
        /// Perform Harass
        /// </summary>
        public static void DoHarass()
        {
            if (Player.ManaPercent < Config.harassMenu.GetSlider("harassMinMana")) return;
            var targetSelect = TargetSelector.GetTarget(SpellManager.Q.Range, TargetSelector.DamageType.Physical);
            if (!targetSelect.IsValidTarget()) return;

            if (Config.harassMenu.GetCheckbox("useQh") && SpellManager.Q.IsReady())
            {
                SpellManager.Q.Cast(targetSelect);
            }

            if (Config.harassMenu.GetCheckbox("useWh") && SpellManager.W.IsReady() && targetSelect.IsValidTarget(SpellManager.W.Range))
            {
                SpellManager.W.Cast(targetSelect);
            }
        }


        /// <summary>
        /// Perform Kill Steal
        /// </summary>
        public static void KillSteal()
        {
            if (Config.KillSteal.GetCheckbox("QKS") && SpellManager.Q.IsReady())
            {
                foreach (var heros in ObjectManager.Heroes.Enemies.Where(
                    x => x.Distance(ObjectManager.Me.Position) <= SpellManager.Q.Range &&
                         x.Health <= SpellManager.Q.GetDamage(x)))
                {
                    SpellManager.Q.Cast(heros);
                }
            }

            if (Config.KillSteal.GetCheckbox("RKS") && SpellManager.R.IsReady())
            {
                foreach (var heros in ObjectManager.Heroes.Enemies.Where(
                    x => x.Distance(ObjectManager.Me.Position) <= SpellManager.Q.Range + 100 &&
                         x.Health <= SpellManager.R.GetDamage(x)))
                {
                    SpellManager.Q.CastIfHitchanceEquals(heros, HitChance.High);
                }
            }
        }
    }
}
