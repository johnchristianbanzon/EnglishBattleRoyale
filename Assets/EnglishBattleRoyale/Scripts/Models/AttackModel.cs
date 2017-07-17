using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Battle Status Model */
public class AttackModel
{
	public string param;

	public AttackModel ()
	{
	}

	public AttackModel (string param)
	{
		this.param = param;
	}

	public Dictionary<string, System.Object> ToDictionary ()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["Attack"] = param;

		return result;
	}

}
