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
		playerName.text = MyConst.player.name;
		playerHP.text = MyConst.player.hp.ToString();
		playerGP.text = MyConst.player.gp.ToString();
		playerMaxGP.text = MyConst.player.maxGP.ToString();
		playerBD.text = MyConst.player.bd.ToString();
		playerSD.text = MyConst.player.sdm.ToString();
		playerTD.text = MyConst.player.td.ToString();
	}

	private void SetPlayerSettings(){
		MyConst.player.name = playerName.text;
		MyConst.player.hp = float.Parse(playerHP.text);
		MyConst.player.gp = float.Parse(playerGP.text);
		MyConst.player.maxGP = float.Parse(playerMaxGP.text);
		MyConst.player.bd = float.Parse(playerBD.text);
		MyConst.player.sdm = float.Parse(playerSD.text);
		MyConst.player.td = float.Parse(playerTD.text);
	}
	

}
