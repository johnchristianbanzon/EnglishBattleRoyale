using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionList{
	public string definition;
	public string answer;
	public string synonym;
	public string antonym;
	public bool hasDefinition;
	public bool hasSynonym;
	public bool hasAntonym;

	public QuestionList(string definition, string answer ,string synonym ,string antonym,bool hasDefinition,
		bool hasSynonym, bool hasAntonym){
		this.definition = definition;
		this.synonym = synonym;
		this.antonym = antonym;
		this.answer = answer;
		this.hasDefinition = hasDefinition;
		this.hasSynonym = hasSynonym;
		this.hasAntonym = hasAntonym;
	
	}

}
