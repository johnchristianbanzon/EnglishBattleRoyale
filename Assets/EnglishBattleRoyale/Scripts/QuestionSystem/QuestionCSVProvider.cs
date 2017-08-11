using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestionCSVProvider: IQuestionProvider {
	public List<QuestionRowModel> GetQuestionList (){
		return MyConst.GetQuestionList ();
	}
}