using UnityEngine;
using UnityEngine.UI;

public class PartProfileController : MonoBehaviour {

	public Text playerDamageText;
	public Text playerHPText;

	// Use this for initialization
	void Start () {
		playerDamageText.text = MyConst.player.bd.ToString();
		playerHPText.text = MyConst.player.hp.ToString();
	}
}
