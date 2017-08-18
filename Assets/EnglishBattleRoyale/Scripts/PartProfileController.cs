using UnityEngine;
using UnityEngine.UI;

public class PartProfileController : MonoBehaviour {

	public Text playerDamageText;
	public Text playerHPText;

	// Use this for initialization
	void Start () {
		playerDamageText.text = GameManager.player.playerBD.ToString();
		playerHPText.text = GameManager.player.playerHP.ToString();
	}
}
