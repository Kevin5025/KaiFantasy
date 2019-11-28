using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ActivatableEquipable {
	
	public void BecomeAcquired() {
		Debug.Log(gameObject.name);
	}

	public void BecomeDiscarded() {

	}

}
