using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootoutCode
{
	using GLHF;
	using LOR_DiceSystem;
	using System;
	using static CharacterSound;

	public class DiceCardAbility_sebby_reuse : GLHF_DiceAbilityBase
	{
		public static string Desc = "[On Clash Win] Spend 1 charge to move this dice to the back of the dice list.";

		public override void BeforeRollDice()
		{
			behavior.SetBlocked(false);
		}

		public override void BeforeWinParrying()
		{
			if (
				!(owner.bufListDetail.GetActivatedBuf(KeywordBuf.WarpCharge) is BattleUnitBuf_warpCharge activatedBuf) 
				|| activatedBuf.stack < 1)
				return;

			if (this.card?.target?.currentDiceAction == null)
				return;

			activatedBuf.UseStack(1, true);

			behavior.SetBlocked(true);
			card.AddDice(behavior);
		}

	}
}
