using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question{
	public string question;
	public string[] answers;

	public Question(string question, string[] answers){
		this.answers = answers;
		this.question = question;
	
	}

}
