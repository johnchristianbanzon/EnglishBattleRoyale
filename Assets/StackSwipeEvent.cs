using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackSwipeEvent : MonoBehaviour {
	public StackSwipeController stackSwipeParent;
	public int selectedIndex;
	public bool isDragging = false;
	private GameObject duplicateContainer;
	private static float yMovement;

	public void OnSelectionBeginDrag ()
	{
		GetComponent<Image> ().raycastTarget = false;
		SystemSoundController.Instance.PlaySFX ("SFX_ClickButton");
		duplicateContainer = SystemResourceController.Instance.LoadPrefab ("WordSwipeContainer",stackSwipeParent.stackSwipeContent);
		duplicateContainer.transform.SetSiblingIndex (this.transform.GetSiblingIndex ());
		selectedIndex = transform.GetSiblingIndex ();
//		transform.parent.GetComponent<VerticalLayoutGroup> ().enabled = false;
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
		Debug.Log (duplicateContainer.transform.localPosition.y);
	}

	/// <summary>
	/// After the drag ends, sets the parent of this container 
	/// and rearrange its position with its siblings depending on the last child detected on OnDetectDraggedSelection
	/// Enables the raycast again for clickability
	/// Activates check answer afterwards
	/// </summary>
	public void OnSelectionEndDrag ()
	{
		Debug.Log (stackSwipeParent.isPointerInside);
		SystemSoundController.Instance.PlaySFX ("SFX_ClickButton");
		duplicateContainer.transform.parent.GetComponent<VerticalLayoutGroup> ().enabled = true;
		this.transform.SetParent (stackSwipeParent.stackSwipeContent.transform);
		this.GetComponent<Image> ().raycastTarget = true;
		transform.SetSiblingIndex (selectedIndex);
		QuestionSystemController.Instance.partSelection.changeOrder.OnChangeOrder ();
		Destroy (duplicateContainer);
		isDragging = false;
	}
}
