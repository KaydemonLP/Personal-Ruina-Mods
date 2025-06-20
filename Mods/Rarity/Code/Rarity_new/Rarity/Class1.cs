using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOR_DiceSystem;
using UnityEngine;

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