using UnityEngine;
public class PartCameraWorksController: SingletonMonoBehaviour<PartCameraWorksController> {
	public GameObject introCamera;
	public GameObject winLoseCamera;
	public GameObject gestureCamera;

	public void StartIntroCamera(){
//		introCamera.SetActive (true);
//		winLoseCamera.SetActive (false);
	}

	public void StartWinLoseCamera(){
//		introCamera.SetActive (false);
//		winLoseCamera.SetActive (true);
	}

	public void ShowGestureCamera(){
		gestureCamera.SetActive (true);
	}

	public void HideGestureCamera(){
		gestureCamera.SetActive (false);
	}
}
