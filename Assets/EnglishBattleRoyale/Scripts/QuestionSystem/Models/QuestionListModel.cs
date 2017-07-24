using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionListModel{
	public string definition;
	public string answer;
	public string synonym;
	public string antonym;
	public string clues;
	public bool hasDefinition;
	public bool hasSynonym;
	public bool hasAntonym;
	public bool hasClues;
	public QuestionListModel(string definition, string answer ,string synonym ,string antonym,string clues,bool hasDefinition,
		bool hasSynonym, bool hasAntonym, bool hasClues){
		this.definition = definition;
		this.synonym = synonym;
		this.antonym = antonym;
		this.answer = answer;
		this.clues = clues;
		this.hasDefinition = hasDefinition;
		this.hasSynonym = hasSynonym;
		this.hasAntonym = hasAntonym;
		this.hasClues = hasClues;
	
	}

}
