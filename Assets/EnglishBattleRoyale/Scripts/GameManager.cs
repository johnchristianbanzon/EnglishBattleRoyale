using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/* Sets GameSettings*/
public static class GameManager
{
	private static PlayerModel player;
	private static string playerName;
	public static List<List<string>> gameSettingList;
	public static void SetPLayerName (string name)
	{
		playerName = name;
	}

	public static void SetSettings ()
	{
		player = new PlayerModel (playerName, GetFloatList ());
		SystemGlobalDataController.Instance.player = player;
	}

	//GET ALL VALUES FROM KEY VALUE CSV
	private static float[] GetFloatList ()
	{
		TextAsset csvData = SystemResourceController.Instance.LoadCSV ("GameSettings");
		gameSettingList = CSVParser.Parse (csvData.ToString ());

		float[] floatList = new float[6];

		for (int i = 1; i < gameSettingList.Count; i++) {
			floatList [i] = float.Parse (gameSettingList [i] [1].ToString ());
		}

		return floatList;
	}

	public static PlayerModel GetPlayer ()
	{
		return player;
	}

}