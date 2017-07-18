using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/* Sets initial game preferences*/
public static class GameManager
{
	private static PlayerModel player;
	private static int answerQuestionTime;
	private static List<List<string>> gameSettingList;

	public static void SetSettings(){
		gameSettingList = CSVParser.ParseCSV ("GameSettings");
		player.playerName = ScreenLobbyController.Instance.GetPlayerName ();
	}

	public static PlayerModel GetPlayer(){
		return player;
	}


		
}


