using System.Collections.Generic;

public class CharacterModel {
	public int characterID;
	public string characterName;
	public string characterDescription;
	public int characterGPCost;
	public int characterSkillID;
	public int characterCalculationType;
	public string characterAmount;
	public string characterAmountVariable;
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
		string characterAmountVariable,
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
		this.characterAmountVariable = characterAmountVariable;
		this.characterConditionType = characterConditionType;
		this.characterConditionRef = characterConditionRef;
		this.characterConditionAmount = characterConditionAmount;
		this.characterSacrificeType = characterSacrificeType;
		this.characterSacrificeAmount = characterSacrificeAmount;
		this.characterTurn = characterTurn;
		this.characterType = characterType;
	
	}

}
