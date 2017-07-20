﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnlockedSkillController : MonoBehaviour
{
	private List<SkillModel> charCardList = new List<SkillModel> ();
	public Text charCardCount;

	// Show unlocked cards, if cards already in equip, do not show
	public void ShowCharacterCards (List<SkillModel> equipCardList)
	{
		charCardList = SkillManager.GetCSVSkillList ();
		charCardCount.text = charCardList.Count.ToString () + "/20";


		for (int i = 0; i < charCardList.Count; i++) {
			bool hasCardInEquip = false;

			for (int j = 0; j < equipCardList.Count; j++) {
				if (equipCardList [j].skillName.Equals (charCardList [i].skillName)) {
					hasCardInEquip = true;
					break;
				} 
			}

			if (!hasCardInEquip) {
				for (int k = 0; k < this.transform.childCount; k++) {
					if (this.transform.GetChild (k).childCount == 0) {
						GameObject charCard = SystemResourceController.Instance.LoadPrefab ("CharCard", this.transform.GetChild (k).gameObject);
						charCard.GetComponent<CharCardController> ().SetCardParameter (charCardList[i]);
						break;
					}
				}
			}
			hasCardInEquip = false;
		}
			
	}
}