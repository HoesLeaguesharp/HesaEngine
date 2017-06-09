using HesaEngine.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HesaEngine.SDK.Enums;

namespace Syndra
{   
    public class SpellManager
    {
        public static Spell Q, W, E, R, Eq;

        public void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R, 5000);
            Eq = new Spell(SpellSlot.Q, Q.Range + 500);

            Q.SetSkillshot(0.6f, 125f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            W.SetSkillshot(0.25f, 140f, 1600f, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, (float)(45 * 0.5), 2500f, false, SkillshotType.SkillshotCircle);
            Eq.SetSkillshot(float.MaxValue, 55f, 2000f, false, SkillshotType.SkillshotCircle);
        }
    }
}
