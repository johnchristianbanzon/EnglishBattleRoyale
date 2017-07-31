using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SlotMachineEvent : MonoBehaviour
{
	public GameObject slotContent;
	public GameObject correctLetterAnswer;
	private bool isDragging = false;

	private Color[] slotColor = new Color[3] {
		new Color (143 / 255, 255f / 255f, 36 / 255f),
		new Color (170 / 255, 167 / 255f, 255 / 255f),
		new Color (81 / 255, 255f / 255f, 241 / 255f)
	};

	private float dragStartingPosition;
	public void OnBeginDrag(){
		dragStartingPosition = Input.mousePosition.y;
		Debug.Log (dragStartingPosition);
		isDragging = true;
	}

	public void OnDrag ()
	{
		if (isDragging && Input.mousePosition.y>dragStartingPosition) {
			OnClickDownButton ();
			isDragging = false;
		}
		if (isDragging && Input.mousePosition.y<dragStartingPosition) {
			OnClickUpButton ();
			isDragging = false;
		}

	}

	public GameObject GetSelectedSlot ()
	{
		return slotContent.transform.GetChild (1).GetChild (0).gameObject;
	}

	public void Init (char answerLetter)
	{
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int randomAnswerPosition = Random.Range (0, 3);
		int slotIndex = 0;
		gameObject.SetActive (true);
		foreach (Transform slot in slotContent.transform) {
			if (slotIndex == randomAnswerPosition) {
				slot.GetComponentInChildren<Text> ().text = answerLetter.ToString ();
				correctLetterAnswer = slot.gameObject;
			} else {
				slot.GetComponentInChildren<Text> ().text = alphabet [Random.Range (0, alphabet.Length)].ToString ();
			}
			slot.GetComponent<Image> ().color = slotColor [slotIndex];
			slotIndex++;
		}

	}

	public void OnClickDownButton ()
	{
		slotContent.transform.GetChild (0).SetAsLastSibling ();
		QuestionSystemController.Instance.partSelection.slotMachine.CheckAnswer ();
	}

	public void OnClickUpButton ()
	{
		slotContent.transform.GetChild (2).SetAsFirstSibling ();
		QuestionSystemController.Instance.partSelection.slotMachine.CheckAnswer ();
	}
}
