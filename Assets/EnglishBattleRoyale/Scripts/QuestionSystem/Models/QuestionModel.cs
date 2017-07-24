using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionModel{
	public string question;
	public string[] answers;

	public QuestionModel(string question, string[] answers){
		this.answers = answers;
		this.question = question;
	
	}

}
