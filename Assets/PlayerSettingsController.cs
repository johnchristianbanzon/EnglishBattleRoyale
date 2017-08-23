using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingsController : MonoBehaviour {

	public InputField playerName;
	public InputField playerHP;
	public InputField playerGP;
	public InputField playerMaxGP;
	public InputField playerBD;
	public InputField playerSD;
	public InputField playerTD;


	// Use this for initialization
	void OnEnable () {
		GetPlayerSettings ();
	}

	public void Close(){
		SetPlayerSettings ();
		this.gameObject.SetActive (false);
	}

	private void GetPlayerSettings(){
		playerName.text = MyConst.player.playerName;
		playerHP.text = MyConst.player.playerHP.ToString();
		playerGP.text = MyConst.player.playerGP.ToString();
		playerMaxGP.text = MyConst.player.playerMaxGP.ToString();
		playerBD.text = MyConst.player.playerBD.ToString();
		playerSD.text = MyConst.player.playerSDM.ToString();
		playerTD.text = MyConst.player.playerTD.ToString();
	}

	private void SetPlayerSettings(){
		MyConst.player.playerName = playerName.text;
		MyConst.player.playerHP = float.Parse(playerHP.text);
		MyConst.player.playerGP = float.Parse(playerGP.text);
		MyConst.player.playerMaxGP = float.Parse(playerMaxGP.text);
		MyConst.player.playerBD = float.Parse(playerBD.text);
		MyConst.player.playerSDM = float.Parse(playerSD.text);
		MyConst.player.playerTD = float.Parse(playerTD.text);
	}
	

}
