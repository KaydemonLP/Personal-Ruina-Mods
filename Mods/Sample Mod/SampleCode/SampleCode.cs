using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOR_Sample_dll
{
    public class Initializer : ModInitializer
    {
        /// <summary>
        /// Called when the mod is loaded.
        /// </summary>
        public override void OnInitializeMod()
        {

        }
    }
    #region card ability
    public class DiceCardSelfAbility_projmoon_custom_buf: DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "bstart_Keyword", "Strength_Keyword" };

        public static string Desc = "[Combat Start] Give 1 Strength to 2 other allies";

        public override void OnStartBattle()
        {
            List<BattleUnitModel> targets = BattleObjectManager.instance.GetAliveList_random(owner.faction,
                2);

            foreach (var unit in targets)
            {
                unit.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Strength, 1, owner);
            }
        }
    }
    public class DiceCardSelfAbility_projmoon_custom_smoke : DiceCardSelfAbilityBase
    {
        public override string[] Keywords => new string[] { "Smoke_Keyword", "DrawCard_Keyword" };

        public static string Desc = "[On Use] Gain 4 Smoke and draw 1 page";

        public override void OnUseCard()
        {
            owner.bufListDetail.AddKeywordBufThisRoundByCard(KeywordBuf.Smoke, 4);
            owner.allyCardDetail.DrawCards(1);
        }
    }

    #endregion

    #region dice ability
    public class DiceCardAbility_projmoon_custom_debuf : DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Weak_Keyword", "Disarm_Keyword", "Binding_Keyword" };

        public static string Desc = "[On Hit] Inflict 1 Feeble, Disarm, and Bind next Scene";

        public override void OnSucceedAttack()
        {
            card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Weak, 1, owner);
            card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Disarm, 1, owner);
            card.target?.bufListDetail.AddKeywordBufByCard(KeywordBuf.Binding, 1, owner);
        }
    }

    public class DiceCardAbility_projmoon_custom_energy1pw: DiceCardAbilityBase
    {
        public override string[] Keywords => new string[] { "Energy_Keyword" };

        public static string Desc = "[On Clash Win] Restore 1 Light";
        public override void OnWinParrying()
        {
            owner.cardSlotDetail.RecoverPlayPointByCard(1);
        }
    }

    public class DiceCardAbility_projmoon_custom_oneside : DiceCardAbilityBase
    {
        public static string Desc = "Add +10 Power if the attack is one-sided";

        public override void BeforeRollDice()
        {
            if (!behavior.IsParrying())
            {
                behavior.ApplyDiceStatBonus(new DiceStatBonus() { power = 10 });
            }
        }
    }
    #endregion


    /// <summary>
    /// "Shimmering" passive
    /// </summary>
    public class PassiveAbility_projmoon_custom_shimmering : PassiveAbilityBase
    {
        private string _packageId = "projmoon.sample";
        private int _patternCount = 0;

        public override int SpeedDiceNumAdder()
        {
            return 2;
        }

        public override void OnWaveStart()
        {
            _patternCount = 0;
        }

        public override void OnRoundStartAfter()
        {
            SetCards();

            _patternCount++;
            _patternCount = _patternCount % 3;
        }

        private void SetCards()
        {
            owner.allyCardDetail.ExhaustAllCards();

            if (_patternCount == 0)
            {
                AddNewCard(new LorId(_packageId, 1));
                AddNewCard(new LorId(_packageId, 2));
                AddNewCard(new LorId(_packageId, 3));
            }
            else if (_patternCount == 1)
            {
                AddNewCard(601001);
                AddNewCard(new LorId(_packageId, 2));
                AddNewCard(new LorId(_packageId, 2));
            }
            else if (_patternCount == 2)
            {
                AddNewCard(601001);
                AddNewCard(new LorId(_packageId, 3));
                AddNewCard(new LorId(_packageId, 3));
            }
        }

        private void AddNewCard(int id)
        {
            var card = owner.allyCardDetail.AddTempCard(id);
            if (card != null)
                card.SetCostToZero();
        }
        private void AddNewCard(LorId id)
        {
            var card = owner.allyCardDetail.AddTempCard(id);
            if (card != null)
                card.SetCostToZero();
        }
    }

}
