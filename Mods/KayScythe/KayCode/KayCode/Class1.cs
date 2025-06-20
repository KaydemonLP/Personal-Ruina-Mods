using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootoutCode
{
    public class PassiveAbility_kay_absolute_defense : PassiveAbilityBase
    {

		public override void OnWinParrying(BattleDiceBehavior behavior)
		{
			if( behavior.Type != LOR_DiceSystem.BehaviourType.Def )
				return;

			if( behavior.Detail != LOR_DiceSystem.BehaviourDetail.Guard )
				return;

			owner.battleCardResultLog?.SetPassiveAbility(this);
			BattleDiceBehavior newbehavior = behavior.card.CopyDiceBehaviour(behavior);
			behavior.card.AddDiceFront(newbehavior);
		}
	}

	public class PassiveAbility_kay_fear_of_hurt : PassiveAbilityBase
	{
		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if( !IsDefenseDice(behavior.Detail) )
				return;

			owner.battleCardResultLog?.SetPassiveAbility(this);
			behavior.ApplyDiceStatBonus(new DiceStatBonus()
			{
				power = 2
			});
		}
	}


	public class PassiveAbility_kay_on_the_fly : PassiveAbilityBase
	{
		public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			if (behavior.Type != LOR_DiceSystem.BehaviourType.Def)
				return;

			double chance = 0.75;

			int increase = 0;

			while ( (double)RandomUtil.valueForProb <= chance && increase < 3)
			{ 
				owner.battleCardResultLog?.SetPassiveAbility(this);
				behavior.ApplyDiceStatBonus(new DiceStatBonus()
				{
					max = 2
				});

				chance /= 2;

				increase++;
			}
		}
	}
}
