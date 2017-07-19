using UnityEngine.UI;

public class GameTimerController: SingletonMonoBehaviour<GameTimerController> {

	public Text gameTimerText;


	public void ToggleTimer(bool toggleFlag){
		gameTimerText.enabled = toggleFlag;
	}
		

}
