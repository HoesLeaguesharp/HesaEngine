using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK;
using HesaEngine.SDK.GameObjects;
using static LeeSin.Program;

namespace LeeSin
{

    public static class Mode
    {

        public static void CastSkill(Spell spell, Obj_AI_Base target, HitChance hChance = HitChance.High)
        {
            var pred = spell.GetPrediction(target);
            if (pred.Hitchance >= hChance)
                spell.Cast(pred.CastPosition);
        }
        public static void DoCombo()
        {
            var target = TargetSelector.GetTarget(1500, TargetSelector.DamageType.Physical);
            if (!target.IsValidTarget()) return;
            if (UseQ1C && Q.IsReady() && target.IsValidTarget(Q.Range)) /*check for toggle state*/
                CastSkill(Q, target);
            if (UseQ2C && Q.IsReady())
                Q.Cast();

            if (UseEc && E.IsReady() && target.IsValidTarget(E.Range))
                E.Cast();

            if (UseRc && target.Health < R.GetDamage(target) + (Q.IsReady() ? Q.GetDamage(target) * 1.8 : 0) +
                ObjectManager.Player.GetAutoAttackDamage(target, true) && target.IsValidTarget(R.Range))
            {
                R.Cast(target);
            }
        }
    }
}
