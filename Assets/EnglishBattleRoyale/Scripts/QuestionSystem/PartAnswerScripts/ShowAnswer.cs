using UnityEngine;
using UnityEngine.UI;

public class ShowAnswer : MonoBehaviour,IAnswer {

	public GameObject showLetterView;
	public void DeployAnswerType(){
		this.gameObject.SetActive (true);
	}

	public void ShowLetterInView(GameObject selectedLetter){
		GameObject letterPrefab = SystemResourceController.Instance.LoadPrefab ("Input-UI",showLetterView);
		letterPrefab.GetComponentInChildren<Text> ().text = selectedLetter.GetComponentInChildren<Text> ().text;
		TweenFacade.TweenScaleToLarge (letterPrefab.transform,Vector3.one,0.3f);
	}

	public void ClearLettersInView(){
		foreach (Transform letter in showLetterView.transform) {
			GameObject.Destroy(letter.gameObject);
		}
	}

}
