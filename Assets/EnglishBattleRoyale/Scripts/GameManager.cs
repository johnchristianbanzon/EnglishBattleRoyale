using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/* Sets GameSettings*/
public static class GameManager
{
	private static PlayerModel player;
	private static string playerName;
	private static int answerQuestionTime;
	private static List<Dictionary<string,System.Object>> gameSettingList;

	public static void SetPLayerName (string name)
	{
		playerName = name;
	}

	public static void SetSettings ()
	{
		
		gameSettingList = CSVParser.ParseCSV ("GameSettings");
		player = new PlayerModel (playerName, GetFloatList ());
		answerQuestionTime = int.Parse (gameSettingList [6] ["Value"].ToString ());

		SystemGlobalDataController.Instance.player = player;
		SystemGlobalDataController.Instance.answerQuestionTime = answerQuestionTime;
	}

	private static float[] GetFloatList ()
	{

		float[] floatList = new float[6];

		for (int i = 0; i < 5; i++) {
			floatList [i] = float.Parse (gameSettingList [i] ["Value"].ToString ());
		}

		return floatList;
	}

	public static PlayerModel GetPlayer ()
	{
		return player;
	}

}


