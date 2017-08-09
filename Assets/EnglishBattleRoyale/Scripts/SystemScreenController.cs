using UnityEngine;
using System.Collections.Generic;

public class SystemScreenController : SingletonMonoBehaviour<SystemScreenController>
{
	public GameObject screen;

	public void ShowScreen (string screenName)
	{
		ClearAllScreen ();
		SystemResourceController.Instance.LoadPrefab (screenName, screen);
	}

	public void ClearScreen (string screenName)
	{
		Destroy (screen.transform.Find (screenName).gameObject);
	}

	public void ClearAllScreen ()
	{
		for (int i = 0; i < screen.transform.childCount; i++) {
			Destroy (screen.transform.GetChild (i).gameObject);
		}
	}



}
