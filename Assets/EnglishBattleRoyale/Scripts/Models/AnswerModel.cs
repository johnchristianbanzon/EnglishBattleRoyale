using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Battle Status Model */
public class AnswerModel
{
	public string param;

	public AnswerModel ()
	{
	}

	public AnswerModel (string param)
	{
		this.param = param;
	}

	public Dictionary<string, System.Object> ToDictionary ()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["AnswerIndicator"] = param;

		return result;
	}

}
