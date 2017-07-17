using UnityEngine;
using UnityEngine.UI;

/* Sets initial game preferences*/
public class GameController : SingletonMonoBehaviour<GameController>
{
	[SerializeField] private int playerLife = 45;
	[SerializeField] private int answerQuestionTime = 25;
	[SerializeField] private int playerGP = 0;
	[SerializeField] private int playerMaxGP = 9;
	[SerializeField] private float playerDamage = 4;
	public InputField gameName;

	void Start ()
	{
		gameName.text = PlayerPrefs.GetString ("GameName", "");
		GameData.Instance.modePrototype = ModeEnum.Mode1;
	}

	public void UpdateGame ()
	{
		PlayerPrefs.SetString ("GameName", gameName.text);

		PlayerModel player = new PlayerModel (gameName.text, playerLife, playerGP, playerMaxGP, playerDamage);
		GameData.Instance.player = player;

		GameData.Instance.answerQuestionTime = answerQuestionTime;
	}

	public void ResetPlayerDamage ()
	{
		GameData.Instance.player.playerDamage = playerDamage;
	}


}


