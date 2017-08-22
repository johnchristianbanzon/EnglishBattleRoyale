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
	private List<GameObject> wrongContainer = new List<GameObject> ();
	private Color[] slotColor = new Color[3] {
		new Color (143 / 255, 255f / 255f, 36 / 255f),
		new Color (170 / 255, 167 / 255f, 255 / 255f),
		new Color (81 / 255, 255f / 255f, 241 / 255f)
	};
	private float dragStartingPosition;
	public bool isDraggable = true;
	public static int overAllHintContainersLeft = 0;

	public int getOverAllHintLeft(){
		return overAllHintContainersLeft;
	}

	private void GetSlots(){
		topSlotPosition = slotContent.transform.GetChild (0).localPosition;
		middleSlotPosition = slotContent.transform.GetChild (1).localPosition;
		bottomSlotPosition = slotContent.transform.GetChild (2).localPosition;
		topSlot = slotContent.transform.GetChild (0).gameObject;
		middleSlot = slotContent.transform.GetChild (1).gameObject;
		bottomSlot = slotContent.transform.GetChild (2).gameObject;
	}
	public void OnBeginDrag(){
		if (isDraggable) {
			

			dragStartingPosition = Input.mousePosition.y;
			isDragging = true;
		}
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
		wrongContainer.Clear ();
		string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int randomAnswerPosition = Random.Range (0, 3);
		int slotIndex = 0;
		gameObject.SetActive (true);
		alphabet = alphabet.Replace (answerLetter.ToString(),"");
		foreach (Transform slot in slotContent.transform) {
			if (slotIndex == randomAnswerPosition) {
				slot.GetComponentInChildren<Text> ().text = answerLetter.ToString ();
				correctLetterAnswer = slot.gameObject;
			} else {
				slot.GetComponentInChildren<Text> ().text = alphabet [Random.Range (0, alphabet.Length)].ToString ();
				wrongContainer.Add (slot.gameObject);
				overAllHintContainersLeft++;
			}
			slot.GetComponent<Image> ().color = slotColor [slotIndex];
			slotIndex++;
		}
		hintContainersLeft = wrongContainer.Count;
		isDraggable = true;
	}

	public int hintContainersLeft = 0;
	public void HideHintContainer(){
		int randomizeContainer = Random.Range (0, wrongContainer.Count);
		hintContainersLeft--;
		overAllHintContainersLeft--;
		wrongContainer [randomizeContainer].GetComponentInChildren<Text> ().text = "";
		wrongContainer [randomizeContainer].GetComponent<Image> ().color = Color.black;
		wrongContainer.RemoveAt (randomizeContainer);
	}

	private Vector3 topSlotPosition;
	private Vector3 middleSlotPosition;
	private Vector3 bottomSlotPosition;
	private GameObject topSlot;
	private GameObject middleSlot;
	private GameObject bottomSlot;
	private static float scrollDelay = 0.5f;

	public void OnClickDownButton ()
	{
		GetSlots ();
		TweenFacade.RotateObject (topSlot,new Vector3(-50,0,0),scrollDelay);
		topSlot.transform.localPosition = new Vector2(topSlot.transform.localPosition.x,bottomSlot.transform.localPosition.y - 80f);
		TweenFacade.TweenMoveTo (topSlot.transform, bottomSlotPosition,scrollDelay);
		topSlot.transform.SetAsLastSibling ();
		TweenFacade.TweenMoveTo (middleSlot.transform, topSlotPosition,scrollDelay);
		TweenFacade.RotateObject (middleSlot,new Vector3(-50,0,0),scrollDelay);
		TweenFacade.TweenMoveTo (bottomSlot.transform, middleSlotPosition,scrollDelay);
		TweenFacade.RotateObject (bottomSlot,new Vector3(0,0,0),scrollDelay);
		isDraggable = false;
		if (QuestionSystemController.Instance.questionRoundHasStarted) {
			QuestionSystemController.Instance.partSelection.slotMachine.CheckAnswer ();
		}
		Invoke ("DraggingDone", scrollDelay);
	}

	public void DraggingDone(){
		isDraggable = true;
	}

	public void OnClickUpButton ()
	{
		GetSlots ();
		TweenFacade.RotateObject (topSlot,new Vector3(0,0,0),scrollDelay);
		TweenFacade.TweenMoveTo (topSlot.transform, middleSlotPosition,scrollDelay);

		TweenFacade.RotateObject (middleSlot,new Vector3(50,0,0),scrollDelay);
		TweenFacade.TweenMoveTo (middleSlot.transform, bottomSlotPosition,scrollDelay);

		bottomSlot.transform.localPosition = new Vector2(bottomSlot.transform.localPosition.x,topSlot.transform.localPosition.y + 80f);
		TweenFacade.TweenMoveTo (bottomSlot.transform, topSlotPosition,scrollDelay);
		TweenFacade.RotateObject (bottomSlot,new Vector3(-50,0,0),scrollDelay);
		bottomSlot.transform.SetAsFirstSibling ();
		isDraggable = false;
		if (QuestionSystemController.Instance.questionRoundHasStarted) {
			QuestionSystemController.Instance.partSelection.slotMachine.CheckAnswer ();
		}
		Invoke ("DraggingDone", scrollDelay);
	}
}
