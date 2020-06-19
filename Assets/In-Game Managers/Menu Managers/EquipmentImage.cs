using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentImage : MonoBehaviour, IPointerClickHandler {

	public int eei;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void OnPointerClick(PointerEventData eventData) {
		PlayerCircleBodyController.playerCircleBodyController.OnEquipmentImageClick(this, eventData);
	}

}
