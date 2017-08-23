using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/* Handles CSV Constants */
public static partial class MyConst {

	public static List<List<string>> questionConst;
	public static List<List<string>> characterConst;
	private static List<QuestionRowModel> questionList = new List<QuestionRowModel> ();
	private static List<string> wrongChoices = new List<string> ();
	public static PlayerModel player{ get; set; }
	public static GameSettingsModel gameSettings{ get; set; }


	public static void Init(){
		InitQuestionConst ();
		InitCharacterConst ();
		InitQuestionList ();
		InitPlayerSettings ();
		InitGameSettings ();
	}

	private static void InitPlayerSettings(){
		player = new PlayerModel ("", GetPlayerSettingsFloatList ("PlayerSettings",6), false);
	}
		
	public static void SetPlayerName(string playerName){
		player.playerName = playerName;
	}

	private static void InitGameSettings(){
		gameSettings = new GameSettingsModel (GetPlayerSettingsFloatList ("GameSettings",6));
	}

	public static PlayerModel GetPlayer ()
	{
		return player;
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

	public static List<QuestionRowModel>  GetQuestionList(){
		return questionList;
	}

	public static  List<string> GetWrongChoices(){
		return wrongChoices;
	}

	public static void InitQuestionList ()
	{
		questionList.Clear ();
		TextAsset csvData = SystemResourceController.Instance.LoadCSV ("QuestionSystemCsv");
		List<List<string>> csvQuestionList = CSVParserUtility.Parse (csvData.ToString());
		for (int i = 1; i < csvQuestionList.Count; i++) {
			//0 FOR LEVELID, QUESTIONID FOR TESTING : LACKING VALUES
			questionList.Add (new QuestionRowModel (
				0,
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "answer")[i].ToString (),
				0,
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "definition")[i].ToString (),
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "synonym1")[i].ToString (),
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "synonym2")[i].ToString (),
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "antonym1")[i].ToString (),
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "antonym2")[i].ToString (),
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "clue1")[i].ToString (),
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "clue2")[i].ToString (),
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "clue3")[i].ToString (),
				CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "clue4")[i].ToString (),
				int.Parse(CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "de")[i].ToString()),
				int.Parse(CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "sy")[i].ToString()),
				int.Parse(CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "an")[i].ToString()),
				int.Parse(CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "cl")[i].ToString())
			));
			wrongChoices.Add (CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "choice1")[i].ToString ()
				+"/"+ CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "choice2")[i].ToString ()
			);
		}
	}

	private static void InitQuestionConst ()
	{
		TextAsset csvData = SystemResourceController.Instance.LoadCSV ("QuestionConst");
		questionConst = CSVParserUtility.Parse (csvData.ToString ());
	}

	private static void InitCharacterConst ()
	{
		TextAsset csvData = SystemResourceController.Instance.LoadCSV ("Character");
		characterConst = CSVParserUtility.Parse (csvData.ToString ());
	}

	public static object GetQuestionConst (string constName)
	{
		object questionConstValue = CSVParserUtility.GetValueFromKey(questionConst,constName);
		return questionConstValue;
	}

	public static List<CharacterModel> GetCharacterList(){

		List<CharacterModel> characterList = new List<CharacterModel>();
		for (int i = 1; i < characterConst.Count; i++) {
			CharacterModel character = new CharacterModel (
				int.Parse(characterConst[i][0].ToString()), //Character ID
				characterConst[i][1].ToString(), // Character Name
				characterConst[i][2].ToString(), //Character Description
				characterConst[i][3].ToString(), //Character Alternate name
				int.Parse(characterConst[i][4].ToString()), //Character GP COST
				int.Parse(characterConst[i][5].ToString()),  //Character SKillType
				int.Parse(characterConst[i][6].ToString()),  //Character Skill Operator
				characterConst[i][7].ToString(),  //Character Calculation
				int.Parse(characterConst[i][8].ToString()), //Character Target
				characterConst[i][9].ToString(),  //Character Sacrifice Calculation
				int.Parse(characterConst[i][10].ToString()),  //Character Sacrifice type
				int.Parse(characterConst[i][11].ToString()), //Character Type
				int.Parse(characterConst[i][12].ToString())  //Character Turn

			);
			characterList.Add(character);
		}
		return characterList;
	}

	public static CharacterModel GetCharacterBySkillType (int skillID)
	{
		List<CharacterModel> characterList = GetCharacterList ();
		CharacterModel character = characterList.Where(p => p.characterSkillType == skillID).FirstOrDefault();
		return character;
	}

	public static CharacterModel GetCharacterByCharID (int charID)
	{
		List<CharacterModel> characterList = GetCharacterList ();
		CharacterModel character = characterList.Where(p => p.characterID == charID).FirstOrDefault();
		return character;
	}
}

