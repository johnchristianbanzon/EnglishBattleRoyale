using UnityEngine;
using UnityEngine.UI;

public class GameSettingsController : MonoBehaviour {

	public InputField correctGPBonus;
	public InputField correctDamageBonus;
	public InputField speedyAwesomeGPBonus;
	public InputField speedyAwesomeDamageBonus;
	public InputField speedyGoodGPBonus;
	public InputField speedyGoodDamageBonus;

	// Use this for initialization
	void OnEnable () {
		GetGameSettings ();
	}

	public void Close(){
		SetGameSettings ();
		this.gameObject.SetActive (false);
	}

	private void GetGameSettings(){
		correctGPBonus.text = MyConst.gameSettings.correctGPBonus.ToString();
		correctDamageBonus.text = MyConst.gameSettings.correctDamageBonus.ToString();
		speedyAwesomeGPBonus.text = MyConst.gameSettings.speedyAwesomeGPBonus.ToString();
		speedyAwesomeDamageBonus.text = MyConst.gameSettings.speedyAwesomeDamageBonus.ToString();
		speedyGoodGPBonus.text = MyConst.gameSettings.speedyGoodGPBonus.ToString();
		speedyGoodDamageBonus.text = MyConst.gameSettings.speedyGoodDamageBonus.ToString();
	}

	private void SetGameSettings(){
		MyConst.gameSettings.correctGPBonus = float.Parse(correctGPBonus.text);
		MyConst.gameSettings.correctDamageBonus = float.Parse(correctDamageBonus.text);
		MyConst.gameSettings.speedyAwesomeGPBonus = float.Parse(speedyAwesomeGPBonus.text);
		MyConst.gameSettings.speedyAwesomeDamageBonus = float.Parse(speedyAwesomeDamageBonus.text);
		MyConst.gameSettings.speedyGoodGPBonus = float.Parse(speedyGoodGPBonus.text);
		MyConst.gameSettings.speedyGoodDamageBonus = float.Parse(speedyGoodDamageBonus.text);
	}
}
