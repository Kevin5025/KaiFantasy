﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FinanceImage : MonoBehaviour, IPointerClickHandler {

	public int ffi;

	public void OnPointerClick(PointerEventData eventData) {
		PlayerCompleteBodyController.playerCompleteBodyController.OnFinanceImageClick(this, eventData);
	}
	
}
