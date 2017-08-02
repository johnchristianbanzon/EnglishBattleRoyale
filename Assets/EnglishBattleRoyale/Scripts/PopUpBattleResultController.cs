using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopUpBattleResultController : MonoBehaviour {

	public Text battleResultText;

	public void SetBattleResultText (string battleResult){
		battleResultText.text = battleResult;
	}

	public void ReturnToLobby ()
	{
		SceneManager.LoadScene ("scene1");
	}

}
