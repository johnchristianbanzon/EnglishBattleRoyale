using System.Collections.Generic;
using System;

[Serializable]
public class CharacterModel
{
	public int characterID;
	public string characterName;
	public string characterDescription;
	public string characterAlternateName;
	public int characterGPCost;
	public int characterSkillType;
	public string characterCalculation;
	public string characterSacrificeCalculation;
	public int characterSacrificeType;
	public int characterType;
	public int characterTurn;

	public CharacterModel (
		int characterID,
		string characterName,
		string characterDescription,
		string characterAlternateName,
		int characterGPCost,
		int characterSkillType,
		string characterCalculation,
		string characterSacrificeCalculation,
		int characterSacrificeType, 
		int characterType,
		int characterTurn
		)
	{

		this.characterID = characterID;
		this.characterName = characterName;
		this.characterDescription = characterDescription;
		this.characterAlternateName = characterAlternateName;
		this.characterGPCost = characterGPCost;
		this.characterSkillType = characterSkillType;
		this.characterCalculation = characterCalculation;
		this.characterSacrificeType = characterSacrificeType;
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