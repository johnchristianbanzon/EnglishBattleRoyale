using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionListModel{
	public string definition;
	public string answer;
	public string synonym;
	public string antonym;
	public string clues;
	public object hasDefinition;
	public object hasSynonym;
	public object hasAntonym;
	public object hasClues;
	public QuestionListModel(string definition, string answer ,string synonym ,string antonym,string clues,
		object hasDefinition, object hasSynonym, object hasAntonym, object hasClues){
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
