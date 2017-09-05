using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/* Sets GameSettings*/
public static class GameManager
{
	public static bool isHost{ get; set; }

	public static Dictionary<Firebase.Database.DataSnapshot, bool> initialState{ get; set; }


	public static void ResetGame(){
		SystemSoundController.Instance.ResetAudio ();
	}
}