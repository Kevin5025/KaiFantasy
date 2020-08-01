using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentImage : MonoBehaviour, IPointerClickHandler {

	public int eei;

	public void OnPointerClick(PointerEventData eventData) {
		PlayerCompleteBodyController.playerCompleteBodyController.OnEquipmentImageClick(this, eventData);
	}

}
