using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/* Sets GameSettings*/
public static class GameManager
{
	public static PlayerModel player{ get; set; }

	public static bool isHost{ get; set; }

	public static QuestionResultCountModel playerAnswerParam{ get; set; }

	public static QuestionResultCountModel enemyAnswerParam{ get; set; }

	public static Dictionary<Firebase.Database.DataSnapshot, bool> initialState{ get; set; }

	private static string playerName;
	public static List<List<string>> gameSettingList;


	public static void SetPLayerName (string name)
	{
		playerName = name;
	}

	public static void SetSettings ()
	{
		player = new PlayerModel (playerName, GetFloatList ());
	}

	//GET ALL VALUES FROM KEY VALUE CSV
	private static float[] GetFloatList ()
	{
		TextAsset csvData = SystemResourceController.Instance.LoadCSV ("GameSettings");
		gameSettingList = CSVParserUtility.Parse (csvData.ToString ());

		float[] floatList = new float[6];

		for (int i = 1; i < gameSettingList.Count - 1; i++) {
			floatList [i - 1] = float.Parse (gameSettingList [i] [1].ToString ());
		}

		return floatList;
	}

	public static PlayerModel GetPlayer ()
	{
		return player;
	}

}