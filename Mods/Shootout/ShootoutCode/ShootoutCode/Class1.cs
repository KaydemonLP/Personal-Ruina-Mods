using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootoutCode
{
	using LOR_DiceSystem;
	using System;

	public class DiceCardSelfAbility_shootout_base : DiceCardSelfAbilityBase
	{
		public static string Desc = "[On Use] Replace all die of this card with a random 0-1 cost card, then, discard that card and return to hand.\nIf no valid card is avalible, use this card's die and don't return.";

		public virtual int GetLightRange()
		{
			return 1;
		}

		public override void OnUseCard()
		{
			BattleDiceCardModel card = GetBestCard();

			if( card == null )
				return;
			
			this.card.RemoveAllDice();
			if (this.owner.faction == Faction.Player)
				++this.owner.UnitData.unitData.history.removeDiceByDog;

			foreach (BattleDiceBehavior diceCardBehavior in card.CreateDiceCardBehaviorList())
			{
				if (diceCardBehavior.Type != BehaviourType.Standby)
					this.card.AddDice(diceCardBehavior);
			}

			List<BattleDiceCardModel> list2 = new List<BattleDiceCardModel>
			{
				card
			};

			this.card.owner.allyCardDetail.DiscardACardByAbility(list2);

			ReturnToHand();
			
		}

		private BattleDiceCardModel GetBestCard()
		{
			List<BattleDiceCardModel> hand = this.card.owner.allyCardDetail.GetHand();
			List<List<BattleDiceCardModel>> valid = new List<List<BattleDiceCardModel>>();
			for (int i = 0; i < GetLightRange() + 1; i++)
				valid.Add(new List<BattleDiceCardModel>());


			for( int i = 0; i < hand.Count; i++ )
			{
				BattleDiceCardModel card = hand[i];

				if( card.GetCost() > GetLightRange())
					continue;

				valid[card.GetCost()].Add( card );
			}

			while( valid.Count > 0 && valid[valid.Count-1].Count == 0 )
			{
				valid.RemoveAt( valid.Count-1 );
			}

			if (valid.Count == 0)
				return null;

			List<BattleDiceCardModel> highestLight = valid[valid.Count - 1];

			return RandomUtil.SelectOne(highestLight);
		}

		private void ReturnToHand()
		{
			this.card.card.exhaust = true;
			this.owner.allyCardDetail.AddNewCard(this.card.card.GetID());
		}
	}

	public class DiceCardSelfAbility_shootout_lv2 : DiceCardSelfAbility_shootout_base
	{
		public new static string Desc = "[On Use] Replace all die of this card with a random 0-2 cost card (prioritizes higher cost), then, discard that card and return to hand.\nIf no valid card is avalible, use this card's die and don't return.";

		public override int GetLightRange()
		{
			return 2;
		}
	}
}
