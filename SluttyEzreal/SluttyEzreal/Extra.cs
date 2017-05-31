using HesaEngine.SDK;
using HesaEngine.SDK.Enums;
using HesaEngine.SDK.GameObjects;

namespace SluttyEzreal
{
    internal class Extra
    {
        public static AIHeroClient Player = ObjectManager.Me;


        /// <summary>
        /// Used to stack tears.
        /// </summary>
        public static void StackTear()
        {

            var minions = MinionManager.GetMinions(
                Player.Position, SpellManager.Q.Range + 600, MinionTypes.All, MinionTeam.Enemy,
                MinionOrderTypes.MaxHealth);

            if (!Player.InFountain() && Config.extraMenu.GetCheckbox("stackTearF")) return;

            if (Player.IsRecalling() || minions.Count >= 1) return;

            if (!Config.extraMenu.GetCheckbox("stackTear")) return;

            if (!CanStack()) return;

            if (Player.ManaPercent < Config.extraMenu.GetSlider("tearMinMana")) return;

            SpellManager.Q.Cast(Game.CursorPosition);
        }


        public static bool CanStack()
        {
            return Player.HasItem((int)ItemId.Tear_of_the_Goddess) || Player.HasItem((int)ItemId.Archangels_Staff) || Player.HasItem((int)ItemId.Manamune);
        }

    }
}
