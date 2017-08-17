using UnityEngine;
using UnityEngine.UI;

public class ScreenSettingController : MonoBehaviour {

	public InputField playerName;

	void Start(){
		MyConst.Init ();
		playerName.text = PlayerPrefs.GetString ("PlayerInputName", "");
	}

	public void StartButton(){
		GameManager.SetPLayerName (playerName.text);
		PlayerPrefs.SetString ("PlayerInputName", playerName.text);
		SystemScreenController.Instance.ShowScreen ("ScreenMainMenu");

		GameManager.SetSettings ();
	}


}
