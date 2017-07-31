using UnityEngine;
using System.Collections.Generic;

public static class SkillManager
{
	
	private static CharacterModel[] character = new CharacterModel[3];
	private static Queue<CharacterModel> characterQueue = new Queue<CharacterModel> (8);

	/// <summary>
	/// Load skill list from parsed CSV
	/// </summary>
	/// <returns>The skill list.</returns>
	public static List<CharacterModel>  GetCharacterList ()
	{
		List<CharacterModel> characterList = MyConst.GetCharacterList();

		return characterList;
	}

	//TEST ONLY FOR NOW!!!!!
	public static List<CharacterModel>  GetEquipSkillList ()
	{
		List<CharacterModel> equipCharacterList = new List<CharacterModel> (8);
		List<CharacterModel> characterList = GetCharacterList ();
		equipCharacterList.Add (characterList [0]);
		equipCharacterList.Add (characterList [1]);
		equipCharacterList.Add (characterList [2]);
		equipCharacterList.Add (characterList [3]);
		equipCharacterList.Add (characterList [4]);
		equipCharacterList.Add (characterList [5]);
		equipCharacterList.Add (characterList [6]);
		equipCharacterList.Add (characterList [7]);
		return equipCharacterList;
	}

	public static void ActivateCharacter (int characterNumber)
	{
		StartCharacter (character [characterNumber - 1]);
	}

	public static void StartCharacter (CharacterModel characterModel)
	{
		ScreenBattleController.Instance.partState.PlayerGP -= characterModel.characterGPCost;
		SystemFirebaseDBController.Instance.SetCharacterParam (characterModel);
		if (SystemGlobalDataController.Instance.modePrototype == ModeEnum.Mode1) {
			SystemFirebaseDBController.Instance.SkillPhase ();
		} 
	}
		
	//set the skill in the UI
	public static void SetCharacterUI (int characterIndex, CharacterModel characterModel)
	{
		character [characterIndex] = characterModel;
		ScreenBattleController.Instance.partSkill.SetCharacterUI (characterIndex, characterModel);
	}

	//receive skill list from prepare phase and shuffle for random skill in start and put in queue
	public static void SetCharacterEnqueue (List<CharacterModel> characterList)
	{
		characterQueue.Clear ();
		characterList.Shuffle ();

		for (int i = 0; i < characterList.Count; i++) {
			Debug.Log (characterList [i].characterName);
			characterQueue.Enqueue (characterList [i]);
		}
	}

	//Fisher-Yates shuffle
	private static System.Random rng = new System.Random();  
	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}


	//Default 3 starting skills when starting the game
	public static void SetStartCharacters ()
	{
		for (int i = 0; i < 3; i++) {
			SetCharacterUI (i, characterQueue.Dequeue ());
		}
	}

	//When skill is used, remove previous skill and enqueue replace with new skill in queue
	public static void UseCharacterUI (int characterIndex)
	{
		//remove this if you want skill will be gone after use
		characterQueue.Enqueue (character [characterIndex]);

		SetCharacterUI (characterIndex, characterQueue.Dequeue ());
	}

	public static CharacterModel GetCharacter (int characterNumber)
	{
		return character [characterNumber - 1];
	}
}
