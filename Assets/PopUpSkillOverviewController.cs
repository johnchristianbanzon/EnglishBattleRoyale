using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSkillOverviewController : MonoBehaviour {

	public Text charName;
	public Text charDescription;
	public Image charImage;

	public void SetCharCard(CharacterModel charCard){
		charName.text = charCard.characterName;
		charDescription.text = charCard.characterDescription;
		charImage.sprite = SystemResourceController.Instance.LoadCharacterCardSprite (charCard.characterName);
	}

	public void Close(){
		Destroy (this.gameObject);
		SystemPopupController.Instance.BackUnclickable (false);
	}
}
