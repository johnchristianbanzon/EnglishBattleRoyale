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

	public void ShowTargetType(string targetString){
		clues = ClueArrayToList (targetString);
		gameObject.SetActive (true);
		InvokeRepeating ("ShowClue",0,2f);
	}

	public void ShowClue(){
		GameObject clueObject = SystemResourceController.Instance.LoadPrefab ("CluePrefab",clueList);
		TweenFacade.TweenScaleToLarge (clueObject.transform, Vector3.one, 0.3f);
		clueObject.GetComponentInChildren<Text>().text = clues[clueNumber-1];
		clueNumber ++;
		if (clueNumber > clueNumberLimit) {
			CancelInvoke ();
		}
	}

	public void HideTargetType(){
		gameObject.SetActive (false);
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

}
