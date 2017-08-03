using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/* Handles Constants */
public static class MyConst
{
	public static List<List<string>> questionConst;
	public static List<List<string>> characterConst;
	private static List<QuestionRowModel> questionList = new List<QuestionRowModel> ();
	private static List<string> wrongChoices = new List<string> ();

	public static void Init(){
		InitQuestionConst ();
		InitCharacterConst ();
		InitQuestionList ();
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
			wrongChoices.Add (CSVParserUtility.GetValueArrayFromKey(csvQuestionList, "answer")[i].ToString ()
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
		TextAsset csvData = SystemResourceController.Instance.LoadCSV ("Characters");
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
				int.Parse(characterConst[i][3].ToString()), //Character GP COST
				int.Parse(characterConst[i][4].ToString()),  //Character SKill ID
				int.Parse(characterConst[i][5].ToString()),  //Character Calculation Type
				characterConst[i][6].ToString(),  //Character Amount
				characterConst[i][7].ToString(),  //Character Amount Variable
				int.Parse(characterConst[i][8].ToString()),  //Character Condition Type
				characterConst[i][9].ToString(), //Character Condition Ref
				characterConst[i][10].ToString(), //Character Condition Amount
				int.Parse(characterConst[i][11].ToString()),  //Character Sacrifice type
				int.Parse(characterConst[i][12].ToString()),  //Character Sacrifice amount
				int.Parse(characterConst[i][13].ToString()),  //Character Turn
				int.Parse(characterConst[i][14].ToString()) //Character Type
			);
			characterList.Add(character);
		}

		return characterList;
	}

	public static CharacterModel GetCharacterBySkillID (int skillID)
	{
		List<CharacterModel> characterList = GetCharacterList ();
		CharacterModel character = characterList.Where(p => p.characterSkillID == skillID).FirstOrDefault();
		return character;
	}

	public static CharacterModel GetCharacterByCharID (int charID)
	{
		List<CharacterModel> characterList = GetCharacterList ();
		CharacterModel character = characterList.Where(p => p.characterID == charID).FirstOrDefault();
		return character;
	}


	public const string URL_FIREBASE_DATABASE = "https://chatprototype-39807.firebaseio.com";
	public const string URL_FIREBASE_DATABASE_CONNECTION = "https://chatprototype-39807.firebaseio.com/.info/connected";

	public const string GAMEROOM_ROOM = "GAMEROOM_ROOM";
	public const string GAMEROOM_STATUS = "GAMEROOM_STATUS";
	public const string GAMEROOM_BATTLE_STATUS = "GAMEROOM_BATTLE_STATUS";
	public const string GAMEROOM_INITITAL_STATE = "GAMEROOM_INITITAL_STATE";
	public const string GAMEROOM_RPC = "GAMEROOM_RPC";
	public const string GAMEROOM_HOME = "GAMEROOM_HOME";
	public const string GAMEROOM_VISITOR = "GAMEROOM_VISITOR";
	public const string GAMEROOM_OPEN = "GAMEROOM_OPEN";
	public const string GAMEROOM_FULL = "GAMEROOM_FULL";
	public  const string GAMEROOM_PROTOTYPE_MODE = "GAMEROOM_PROTOTYPE_MODE";

	public const string RPC_DATA_USERHOME = "RPC_DATA_USERHOME";
	public const string RPC_DATA_PARAM = "RPC_DATA_PARAM";
	public const string RPC_DATA_ATTACK = "RPC_DATA_ATTACK";
	public const string RPC_DATA_GESTURE = "RPC_DATA_GESTURE";
	public const string RPC_DATA_CHARACTER = "RPC_DATA_CHARACTER";
	public const string RPC_DATA_PLAYER = "RPC_DATA_PLAYER";
	public const string RPC_DATA_PLAYER_ANSWER_PARAM = "RPC_DATA_PLAYER_ANSWER_PARAM";
	public const string RPC_DATA_ENEMY_ANSWER_PARAM = "RPC_DATA_ENEMY_ANSWER_PARAM";


	public  const string BATTLE_STATUS_ANSWER = "BATTLE_STATUS_ANSWER";
	public  const string BATTLE_STATUS_ATTACK = "BATTLE_STATUS_ATTACK";
	public  const string BATTLE_STATUS_STATE = "BATTLE_STATUS_STATE";
	public  const string BATTLE_STATUS_COUNT = "BATTLE_STATUS_COUNT";


}

