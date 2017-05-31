using HesaEngine.SDK;
using HesaEngine.SDK.DatabaseData;
using HesaEngine.SDK.Enums;

namespace SluttyEzreal
{
    public static class SpellManager
    {
        public static Spell Q, W, E, R;
        public static void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W, 950);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R);
            Q.SetSkillshot(0.25f, 80, 1400, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.25f, 80, 1200, false, SkillshotType.SkillshotLine);
        }



        /// <summary>
        /// Get Spell data {Empty}
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="spellName"></param>
        /// <returns></returns>
        private static SpellData GetData(SpellSlot slot, string spellName)
        {
            return SpellDatabase.GetBySpellSlot(slot, spellName);
        }
    }
}
