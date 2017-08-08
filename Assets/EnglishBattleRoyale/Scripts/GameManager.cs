using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/* Sets GameSettings*/
public static class GameManager
{
	public static PlayerModel player{ get; set; }

	public static GameSettingsModel gameSettings{ get; set; }

	public static bool isHost{ get; set; }

	public static QuestionResultCountModel playerAnswerParam{ get; set; }

	public static QuestionResultCountModel enemyAnswerParam{ get; set; }

	public static Dictionary<Firebase.Database.DataSnapshot, bool> initialState{ get; set; }

	private static string playerName;



	public static void SetPLayerName (string name)
	{
		playerName = name;
	}

	public static void SetSettings ()
	{
		SetPlayerSettings ();
		SetGameSettings ();
	}

	private static void SetPlayerSettings(){
		player = new PlayerModel (playerName, GetPlayerSettingsFloatList ("PlayerSettings",6));
	}

	private static void SetGameSettings(){
		gameSettings = new GameSettingsModel (GetPlayerSettingsFloatList ("GameSettings",6));
	}

	//GET ALL SETTING VALUES FROM KEY VALUE CSV
	private static float[] GetPlayerSettingsFloatList (string csvName, int entryCount)
	{
		List<List<string>> settingsList;
		TextAsset csvData = SystemResourceController.Instance.LoadCSV (csvName);
		settingsList = CSVParserUtility.Parse (csvData.ToString ());

		float[] floatList = new float[entryCount];

		for (int i = 1; i < settingsList.Count - 1; i++) {
			floatList [i - 1] = float.Parse (settingsList [i] [1].ToString ());
		}

		return floatList;
	}

	public static PlayerModel GetPlayer ()
	{
		return player;
	}

}