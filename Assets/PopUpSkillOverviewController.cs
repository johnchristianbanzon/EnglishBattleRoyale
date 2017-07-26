using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSkillOverviewController : MonoBehaviour {

	public Text charName;
	public Text charDescription;
	public Image charImage;

	public void SetCharCard(SkillModel charCard){
		charName.text = charCard.skillName;
		charDescription.text = charCard.skillDescription;
		charImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.skillName);
	}

	public void Close(){
		Destroy (this.gameObject);
		SystemPopupController.Instance.BackUnclickable (false);
	}
}
