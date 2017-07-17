using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Battle Status Model */
public class GestureModel
{
	public string param;

	public GestureModel ()
	{
	}

	public GestureModel (string param)
	{
		this.param = param;
	}

	public Dictionary<string, System.Object> ToDictionary ()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["Gesture"] = param;

		return result;
	}

}
