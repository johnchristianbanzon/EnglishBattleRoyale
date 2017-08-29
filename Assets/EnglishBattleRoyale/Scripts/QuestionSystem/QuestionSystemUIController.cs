using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSystemUIController{
	public void ShowPopUP(string name){
		string popUpName = "";
		switch (name) {
		case "WordChoice":
			popUpName = "PopUpWordChoice";
			break;
		case "SelectLetter":
			popUpName = "PopUpSelectLetter";
			break;
		case "ChangeOrderController":
			popUpName = "PopUpChangeOrder";
			break;
		case "Typing":
			popUpName = "PopUpTyping";
			break;
		case "SlotMachine":
			popUpName = "PopUpSlotMachine";
			break;
		case "LetterLink":
			popUpName = "PopUpLetterLink";
			break;
		}
	}

	public void ShowQuestionSystemUI(){
		
	}
}
