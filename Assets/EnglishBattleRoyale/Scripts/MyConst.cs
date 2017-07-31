using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/* Handles Constants */
public static class MyConst
{
	public static List<List<string>> questionConst;
	public static List<List<string>> characterConst;


	public static void Init(){
		InitQuestionConst ();
		InitCharacterConst ();
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

	public const string GAMEROOM_NAME = "GameRoom";
	public const string GAMEROOM_STATUS = "RoomStatus";
	public const string GAMEROOM_BATTLE_STATUS = "BattleStatus";
	public const string GAMEROOM_INITITAL_STATE = "InitialState";
	public const string GAMEROOM_RPC = "RPC";
	public const string GAMEROOM_HOME = "Home";
	public const string GAMEROOM_VISITOR = "Visitor";
	public const string GAMEROOM_OPEN = "Open";
	public const string GAMEROOM_FULL = "Full";
	public  const string GAMEROOM_PROTOTYPE_MODE = "Mode";

	public  const string BATTLE_STATUS_ANSWER = "answer";
	public  const string BATTLE_STATUS_CHARACTER = "character";
	public  const string BATTLE_STATUS_ATTACK = "attack";
	public  const string BATTLE_STATUS_STATE = "State";
	public  const string BATTLE_STATUS_COUNT = "Count";


	public const string BATTLE_STATUS_HANSWER = "HAnswer";
	public const string BATTLE_STATUS_HTIME = "HTime";
	public const string BATTLE_STATUS_VANSWER = "VAnswer";
	public const string BATTLE_STATUS_VTIME = "VTime";

}

