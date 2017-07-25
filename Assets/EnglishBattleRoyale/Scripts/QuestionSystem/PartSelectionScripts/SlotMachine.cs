using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SlotMachine : MonoBehaviour,ISelection
{

	public GameObject[] slots = new GameObject[6];
	private List<Color> previousSlotColor = new List<Color> ();
	private string questionAnswer = "";

	public List<GameObject> GetSlots ()
	{
		List<GameObject> slotsItems = new List<GameObject> ();
		for (int i = 0; i < slots.Length; i++) {
			if (i > questionAnswer.Length) {
				Debug.Log (i + "/" + questionAnswer.Length);
				slots [i].SetActive (false);
			} else {
				for (int j = 0; j < slots [i].transform.childCount; j++) {
					slots [j].transform.GetChild (j).name = "slot" + j;
					slotsItems.Add (slots [i].transform.GetChild (j).gameObject);

				}
			}
		}
		return slotsItems;
	}

	public void DeploySelectionType (string questionAnswer)
	{
		gameObject.SetActive (true);
		this.questionAnswer = questionAnswer;
		ShuffleAlgo (questionAnswer);
	}

	public void RemoveSelection (int hintIndex)
	{
		
	}

	public void OnDrag(GameObject scrollContent){
		
	}

	private int scrollIndex = 120;
	public void OnClickDownButton(GameObject scrollContent){
		TweenFacade.TweenMoveTo (scrollContent.transform,new Vector2(scrollContent.transform.position.x,scrollContent
			.transform.position.y - scrollIndex),0.3f);
		scrollIndex += 120;
	}

	public void OnClickUpButton(GameObject scrollContent){
		Debug.Log (scrollIndex);
		TweenFacade.TweenMoveTo (scrollContent.transform,new Vector2(scrollContent.transform.position.x,scrollContent
			.transform.position.y + scrollIndex),0.3f);
		scrollIndex += 120;
	}

	public void ShuffleAlgo (string questionAnswer)
	{
		List<GameObject> roulleteItem = GetSlots ();
		List<GameObject> correctItems = new List<GameObject> ();
		string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int letterIndex = 0;
		int letterStartIndex = 0;
		int letterEndIndex = 3;
		int randomnum = UnityEngine.Random.Range (letterStartIndex+1, letterEndIndex);
		for (int i = 0; i < roulleteItem.Count; i++) {
			roulleteItem [i].GetComponentInChildren<Text>().text = (i%randomnum)==0 ?
				questionAnswer [letterIndex].ToString ().ToUpper ():
				Letters [UnityEngine.Random.Range (0, Letters.Length)].ToString ().ToUpper ();
			if ((i % randomnum) == 0) {
				letterIndex += 1;
				letterStartIndex = letterEndIndex;
				letterEndIndex = letterEndIndex + 3;
				randomnum = UnityEngine.Random.Range (letterStartIndex, letterEndIndex);
				correctItems.Add (roulleteItem [i]);
				previousSlotColor.Add (roulleteItem [i].GetComponent<Image> ().color);
			}
		}
		QuestionSystemController.Instance.correctAnswerButtons = correctItems;

	}

}
