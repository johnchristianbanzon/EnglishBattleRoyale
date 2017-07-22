using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Association : MonoBehaviour,ITarget {
	public GameObject clueList;
	private GameObject currenctClueSelected;
	public GameObject cluePrefab;
	private List<string> clues = new List<string>();
	private int clueNumberLimit = 4;
	private int clueNumber = 1;

	public void DeployTargetType(string targetString){
		clueList.SetActive (true);
		clues = ClueArrayToList (targetString);
		GameObject clueObject = Instantiate (cluePrefab) as GameObject; 
		clueObject.GetComponentInChildren<Text>().text = clues[clueNumber-1];
		clueNumber += 1;
		clueObject.transform.SetParent (clueList.transform, false);
		clueObject.GetComponent<Button> ().onClick.AddListener (() => {
			OnClueClick ();
		});
		InstantiateCluePrefab ();
	}

	public void OnClueClick(){
		GameObject clueSelected = EventSystem.current.currentSelectedGameObject;
		if (clueSelected.GetComponentInChildren<Text> ().text == "?" && clueNumber <= clueNumberLimit) {
			currenctClueSelected = clueSelected;
			clueSelected.GetComponentInChildren<Text>().text = clues[clueNumber-1];
			clueNumber += 1;
			if (clueNumber <= clueNumberLimit) {
				TweenFacade.TweenTextScale (clueSelected.transform, new Vector3 (0.03f, clueSelected.transform.localScale.y, clueSelected.transform.localScale.z), 0.02f);
				Invoke ("AfterScalingTween", 0.02f);
			}
		}
	}

	public List<string> ClueArrayToList(string targetString){
		clueNumber = 1;
		ClearCluePrefabs ();

		List<string> listClues = new List<string>(targetString.Split ('/'));
		listClues.AddRange(targetString.Split ('/'));
		return listClues;
	}
	public void ClearCluePrefabs(){
		foreach (Transform letter in clueList.transform) {
			GameObject.Destroy(letter.gameObject);
		}
	}

	public void AfterScalingTween(){
		TweenFacade.TweenScaleToSmall (currenctClueSelected.transform,Vector3.one,0.5f);
		InstantiateCluePrefab ();
	}

	public void InstantiateCluePrefab(){
		GameObject clueObject = Instantiate (cluePrefab) as GameObject; 
		clueObject.transform.SetParent (clueList.transform, false);
		clueObject.GetComponent<Button> ().onClick.AddListener (() => {
			OnClueClick ();
		});
	}
}
