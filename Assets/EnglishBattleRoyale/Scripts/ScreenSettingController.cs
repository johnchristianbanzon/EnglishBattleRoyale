using UnityEngine;
using UnityEngine.UI;

public class ScreenSettingController : MonoBehaviour {

	public InputField playerName;
	public ToggleGroup toggleGroup;

	void Start(){
		playerName.text = PlayerPrefs.GetString ("PlayerInputName", "");
	}

	public void StartButton(){
		GameManager.SetPLayerName (playerName.text);
		PlayerPrefs.SetString ("PlayerInputName", playerName.text);
		SystemScreenController.Instance.ShowScreen ("ScreenLobby");
	}

	public void OnModeChange ()
	{
		foreach (Toggle tg in toggleGroup.ActiveToggles()) {

			ModeEnum modeChosen = ModeEnum.Mode1;
			switch (tg.name) {

			case "Mode1":
				modeChosen = ModeEnum.Mode1;
				break;
			case "Mode2":
				modeChosen = ModeEnum.Mode2;
				break;
			}
			SystemGlobalDataController.Instance.modePrototype = modeChosen;
		}
	}
}
