using UnityEngine;
using UnityEngine.UI;
using NCalc;

public class ScreenSettingController : MonoBehaviour {

	public InputField playerName;

	void Start(){
		MyConst.Init ();
		playerName.text = PlayerPrefs.GetString ("PlayerInputName", "");
		MyConst.SetPlayerName (playerName.text);
	}


	public void StartButton(){
		MyConst.SetPlayerName (playerName.text);
		PlayerPrefs.SetString ("PlayerInputName", playerName.text);
		SystemScreenController.Instance.ShowScreen ("ScreenMainMenu");

	}


}
