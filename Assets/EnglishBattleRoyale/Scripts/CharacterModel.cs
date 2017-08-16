using System.Collections.Generic;
using System;
[Serializable]
public class CharacterModel {
	public int characterID;
	public string characterName;
	public string characterDescription;
	public int characterGPCost;
	public int characterSkillID;
	public int characterCalculationType;
	public string characterAmount;
	public int characterConditionType;
	public string characterConditionRef;
	public string characterConditionAmount;
	public int characterSacrificeType;
	public int characterSacrificeAmount;
	public int characterTurn;
	public int characterType;

	public CharacterModel(
		int characterID,
		string characterName,
		string characterDescription,
		int characterGPCost,
		int characterSkillID,
		int characterCalculationType,
		string characterAmount,
		int characterConditionType,
		string characterConditionRef,
		string characterConditionAmount,
		int characterSacrificeType,
		int characterSacrificeAmount,
		int characterTurn,
		int characterType){

		this.characterID = characterID;
		this.characterName = characterName;
		this.characterDescription = characterDescription;
		this.characterGPCost = characterGPCost;
		this.characterSkillID = characterSkillID;
		this.characterCalculationType = characterCalculationType;
		this.characterAmount = characterAmount;
		this.characterConditionType = characterConditionType;
		this.characterConditionRef = characterConditionRef;
		this.characterConditionAmount = characterConditionAmount;
		this.characterSacrificeType = characterSacrificeType;
		this.characterSacrificeAmount = characterSacrificeAmount;
		this.characterTurn = characterTurn;
		this.characterType = characterType;
	
	}

}
//make list class for charactermodel and serializable so that it can be wrapped and send to firebase
[Serializable]
public class CharacterModelList
{
	public List<CharacterModel> list;
}