using UnityEngine.UI;

public class GameTimerView : SingletonMonoBehaviour<GameTimerView> {

	public Text gameTimerText;
	public Image gameTimerImage;


	public void ToggleTimer(bool toggleFlag){
		gameTimerText.enabled = toggleFlag;
	//	gameTimerImage.enabled = toggleFlag;
	}
		

}
