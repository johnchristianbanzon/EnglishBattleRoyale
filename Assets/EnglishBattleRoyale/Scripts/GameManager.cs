using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/* Sets initial game preferences*/
public static class GameManager
{
	private static PlayerModel player;
	private static int answerQuestionTime;
	private static List<Dictionary<string,System.Object>> gameSettingList;

	public static void SetSettings ()
	{
		gameSettingList = CSVParser.ParseCSV ("GameSettings");
		for (int i = 0; i < gameSettingList.Count; i++) {
			player.playerHP = int.Parse (gameSettingList [i] ["PlayerHP"].ToString ());
			player.playerGP = int.Parse (gameSettingList [i] ["PlayerGP"].ToString ());
			player.playerMaxGP = int.Parse (gameSettingList [i] ["PlayerMaxGP"].ToString ());
			player.playerBaseDamage = int.Parse (gameSettingList [i] ["PlayerBaseDamage"].ToString ());
			player.playerGuardDamage = int.Parse (gameSettingList [i] ["PlayerGuardDamage"].ToString ());
			player.playerCriticalDamageRate = int.Parse (gameSettingList [i] ["PlayerCriticalDamageRate"].ToString ());
			answerQuestionTime = int.Parse (gameSettingList [i] ["AnswerQuestionTime"].ToString ());
		}

		player.playerName = ScreenLobbyController.Instance.GetPlayerName ();
		SystemGlobalDataController.Instance.player = player;
		SystemGlobalDataController.Instance.answerQuestionTime = answerQuestionTime;
	}

	public static PlayerModel GetPlayer ()
	{
		return player;
	}


		
}


