using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FinanceImage : MonoBehaviour, IPointerClickHandler {

	public int fi;

	public void OnPointerClick(PointerEventData eventData) {
		PlayerCompleteBodyController.playerCompleteBodyController.OnFinanceImageClick(this, eventData);
	}
	
}
