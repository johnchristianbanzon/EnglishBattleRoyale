using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOrderEvent : MonoBehaviour
{
	private GameObject selectedButton;
	public GameObject selectionContent;
	public static int selectedIndex;
	public static bool isDragging;
	public Text letterText;

	public void Init(string letter){
		letterText.text = letter;
		gameObject.SetActive (true);
		gameObject.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
	}

	/// <summary>
	/// Sets the parent of this container withiout a gridlayout for it to freely move on the canvas
	/// </summary>
	public void OnSelectionBeginDrag ()
	{
		this.transform.SetParent (selectionContent.transform.parent);
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
	}

	/// <summary>
	/// After the drag ends, sets the parent of this container 
	/// and rearrange its position with its siblings depending on the last child detected on OnDetectDraggedSelection
	/// Enables the raycast again for clickability
	/// Activates check answer afterwards
	/// </summary>
	public void OnSelectionEndDrag ()
	{
		this.transform.SetParent (selectionContent.transform);
		this.GetComponent<Image> ().raycastTarget = true;
		transform.SetSiblingIndex (selectedIndex);
		QuestionSystemController.Instance.partSelection.changeOrder.OnChangeOrder ();
		isDragging = false;
	}

	/// <summary>
	/// Gets the sibling index on the container detected and sets it
	/// </summary>
	public void OnDetectDraggedSelection ()
	{
		if (isDragging) {
			selectedIndex = this.transform.GetSiblingIndex ();
		}	
	}
}
