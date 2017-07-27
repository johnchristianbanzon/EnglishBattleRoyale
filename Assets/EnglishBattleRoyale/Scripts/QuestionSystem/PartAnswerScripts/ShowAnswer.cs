using UnityEngine;
using UnityEngine.UI;

public class ShowAnswer : MonoBehaviour,IAnswer {

	public GameObject showLetterView;

	public void DeployAnswerType(){
		this.gameObject.SetActive (true);
	}

	public void OnClickHint (int hintCounter){
	
	}

	public void ShowLetterInView(GameObject selectedLetter){
		GameObject letterPrefab = SystemResourceController.Instance.LoadPrefab ("Input-UI",showLetterView);
		letterPrefab.GetComponentInChildren<Text> ().text = selectedLetter.GetComponentInChildren<Text> ().text;
		TweenFacade.TweenScaleToLarge (letterPrefab.transform,Vector3.one,0.3f);
	}

	public void ClearLettersInView(int hintCounter){
		
		foreach (Transform letter in showLetterView.transform) {
			GameObject.Destroy(letter.gameObject);
		}
		/*
		for (int i = hintCounter; i < showLetterView.transform.childCount; i++) {
			//GameObject.Destroy (showLetterView.transform.GetChild (i));
			Debug.Log(showLetterView.transform.GetChild (i).GetComponentInChildren<Text>().text);
		}*/
	}

}
