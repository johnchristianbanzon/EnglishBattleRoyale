using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackSwipeEvent : MonoBehaviour {
	public StackSwipeController stackSwipeParent;
	public int selectedIndex;
	public bool isDragging = false;
	public bool isCorrect = false;
	private GameObject duplicateContainer;
	private static float yMovement;
	private static int numberOfRemovedContainers = 0;
	private static GameObject correctObject;

	public void Init(bool isCorrect){
		numberOfRemovedContainers = 0;
		string answer = "";
		if (isCorrect) {
			answer = stackSwipeParent.questionAnswer;
			correctObject = gameObject;
			this.isCorrect = true;
		} else {
			answer = QuestionBuilder.GetRandomChoices ().Split('/')[0];
		}
	
		GetComponentInChildren<Text> ().text = answer.ToUpper();
		GetComponentInChildren<Image> ().color = Color.white;
		GetComponentInChildren<Image> ().raycastTarget = true; 
		gameObject.SetActive (true);
		transform.SetSiblingIndex (UnityEngine.Random.Range (0,stackSwipeParent.stackSwipeContainers.Length));
	}

	public void OnSelectionBeginDrag ()
	{
		GetComponent<Image> ().raycastTarget = false;
		SystemSoundController.Instance.PlaySFX ("SFX_ClickButton");
		duplicateContainer = SystemResourceController.Instance.LoadPrefab ("WordSwipeContainer",stackSwipeParent.stackSwipeContent);
		duplicateContainer.transform.SetSiblingIndex (this.transform.GetSiblingIndex ());
		selectedIndex = transform.GetSiblingIndex ();
		duplicateContainer.GetComponent<Image> ().color = new Color (81f / 255, 134f / 255f, 221f / 255f);
		duplicateContainer.transform.position = transform.position;
		yMovement = this.transform.position.y;
		this.transform.SetParent (stackSwipeParent.transform);
	}

	/// <summary>
	/// While Dragging event,
	/// follows input movement anywhere on the canvas
	/// Removes the raycast momentarily to allow detection on the other container
	/// </summary>
	public void OnSelectionDrag ()
	{
		Vector2 pos = new Vector2 (0, 0);
		isDragging = true;
		Canvas myCanvas = SystemGlobalDataController.Instance.gameCanvas;
		this.GetComponent<Image> ().raycastTarget = false;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		this.transform.position = myCanvas.transform.TransformPoint (pos);
		this.transform.position = new Vector2(this.transform.position.x,yMovement);
	}

	/// <summary>
	/// After the drag ends, sets the parent of this container 
	/// and rearrange its position with its siblings depending on the last child detected on OnDetectDraggedSelection
	/// Enables the raycast again for clickability
	/// Activates check answer afterwards
	/// </summary>
	public void OnSelectionEndDrag ()
	{
		SystemSoundController.Instance.PlaySFX ("SFX_ClickButton");
		duplicateContainer.transform.parent.GetComponent<VerticalLayoutGroup> ().enabled = true;
		this.transform.SetParent (stackSwipeParent.stackSwipeContent.transform);
		this.GetComponent<Image> ().raycastTarget = true;
		transform.SetSiblingIndex (selectedIndex);
//		QuestionSystemController.Instance.partSelection.changeOrder.OnChangeOrder ();
		Destroy (duplicateContainer);
		RemoveContainer ();
		isDragging = false;
	}

	public void RemoveContainer(){
		if (!stackSwipeParent.isPointerInside) {
			if (numberOfRemovedContainers < 3) {
				gameObject.SetActive (false);
			}
			numberOfRemovedContainers++;
			if (numberOfRemovedContainers > 2) {
				if (correctObject.activeInHierarchy) {
					QuestionSystemController.Instance.CheckAnswer (true);
				} else {
					stackSwipeParent.ResetSelections ();
					QuestionSystemController.Instance.CheckAnswer (false);
				}

				numberOfRemovedContainers = 0;
			}
		}
	}
}
