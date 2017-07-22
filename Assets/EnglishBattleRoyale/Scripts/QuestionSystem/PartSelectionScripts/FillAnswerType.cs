using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FillAnswerType : MonoBehaviour {
	
	private List<GameObject> answerContainers = new List<GameObject>();
	public GameObject answerContainerPrefab;
	public GameObject outviewContent;
	public List<GameObject> FillAnswerActivate(string questionAnswer){
		return populateContainer (questionAnswer);
	}

	public List<GameObject>populateContainer(string questionAnswer){
		
		foreach (Transform child in outviewContent.transform) {
				GameObject.Destroy(child.gameObject);
		}
		answerContainers.Clear ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject answerPrefab = Instantiate (answerContainerPrefab) as GameObject; 
			answerPrefab.transform.SetParent (outviewContent.transform, false);
			answerPrefab.name = "output" + (i+1);
			answerContainers.Add (answerPrefab);
			answerPrefab.GetComponent<Button> ().onClick.AddListener (() => {
				OnAnswerClick (answerPrefab.GetComponent<Button> ());
			});
		}

		return answerContainers;
	}
		
	public void OnAnswerClick (Button answerButton)
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		string answerclicked = "";
		if (string.IsNullOrEmpty (answerButton.transform.GetChild (0).GetComponent<Text> ().text)) {
			TweenFacade.TweenShakePosition (answerButton.transform, 0.5f, 15.0f, 50, 90f);
		} else {
			Debug.Log (answerButton.name);
			for (int i = 0; i < answerContainers.Count; i++) {
				if (answerButton.name.Equals ("output" + (i+1))) {
					answerclicked = answerContainers [i].transform.GetChild (0).GetComponent<Text> ().text;
					answerContainers [i].transform.GetChild (0).GetComponent<Text> ().text = "";
					QuestionSystemController.Instance.answerController.GetSelectionIdentifier (i)
						.GetComponentInChildren<Text>().text = answerclicked;
//					answerIdentifier [i]

				}
			}
			//CheckAnswerHolder ();
		}
	}
}
