using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCharacterOverviewController : MonoBehaviour {

	public Text charName;
	public Text charEffectDescription;
	public Text charConditionDescription;
	public Text charTurnDescription;
	public Image charImage;

	public void SetCharCard(CharacterModel charCard){
		charName.text = charCard.name;
		charEffectDescription.text = charCard.effectDescription;
		charConditionDescription.text = charCard.conditionDescription;
		charTurnDescription.text = charCard.turnDescription;
		charImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.iD);
	}

	public void Close(){
		Destroy (this.gameObject);
		SystemPopupController.Instance.BackUnclickable (false);
	}
}
