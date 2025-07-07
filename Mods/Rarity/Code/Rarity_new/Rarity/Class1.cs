using LOR_DiceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static CharacterSound;

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
public class DiceCardSelfAbility_hand_endurance : DiceCardSelfAbilityBase
{
	public override string[] Keywords => new string[]
	{
		"bstart_Keyword",
		"Protection_Keyword",
	};

	public static string Desc = "[Combat Start] Give Endurance equal to half the number of cards in hand to all allies next scene.";

	public override void OnStartBattle()
	{
		int count = this.owner.allyCardDetail.GetHand().Count;

		count = count / 2;

		if (count < 1)
			count = 1;

		foreach (BattleUnitModel alive in BattleObjectManager.instance.GetAliveList(this.card.owner.faction))
		{
			alive.bufListDetail.AddKeywordBufByCard(KeywordBuf.Protection, count, this.owner);
		}
	}
}

#endregion

#region passive ability
public class PassiveAbility_discord_discard : PassiveAbilityBase
{
	public override void OnDiscardByAbility(List<BattleDiceCardModel> cards)
	{
		DiceCardSelfAbilityBase oldAbility = owner.currentDiceAction.cardAbility;

		foreach ( BattleDiceCardModel card in cards )
		{
			DiceCardSelfAbilityBase ability = card.CreateDiceCardSelfAbilityScript();
			ability.card = owner.currentDiceAction;
			owner.currentDiceAction.cardAbility = ability;
			owner.currentDiceAction.OnUseCard();
			owner.currentDiceAction.card.OnUseCard(owner, owner.currentDiceAction);
		}

		owner.currentDiceAction.cardAbility = oldAbility;
	}
}
#endregion