using UnityEngine;

public class PartSelectionController : MonoBehaviour
{
	public GameObject[] inputSelections = new GameObject[4];
	public SelectLetter selectLetter;
	public Typing typing;
	public ChangeOrder changeOrder;
	public WordChoice wordChoice;
	public SlotMachine slotMachine;
	public LetterLink letterLink;

	public void DeploySelectionType(ISelection selectionType, string questionAnswer){
		selectionType.DeploySelectionType (questionAnswer);
	}


}
