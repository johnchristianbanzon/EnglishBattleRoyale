using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCharacterOverviewController : MonoBehaviour {

	public Text charName;
	public Text charDescription;
	public Image charImage;

	public void SetCharCard(CharacterModel charCard){
		charName.text = charCard.characterName;
		charDescription.text = charCard.characterDescription;
		charImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.characterID);
	}

	public void Close(){
		Destroy (this.gameObject);
		SystemPopupController.Instance.BackUnclickable (false);
	}
}
