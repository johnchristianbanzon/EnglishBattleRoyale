using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionTypeModel
{
	public ITarget targetType;
	public IAnswer answerType;
	public ISelection selectionType;
	public QuestionSystemEnums.QuestionType questionCategory;

	public QuestionTypeModel (
		QuestionSystemEnums.QuestionType questionCategory,
		ITarget targetType,
		IAnswer answerType,
		ISelection selectionType)
	{
		this.questionCategory = questionCategory;
		this.targetType = targetType;
		this.answerType = answerType;
		this.selectionType = selectionType;
	}

}
