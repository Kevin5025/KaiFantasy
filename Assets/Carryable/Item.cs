using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IItem {

	public void BecomeHandled(ItemHandlerBody agent) {
		BecomeObtained(agent);
	}

	public abstract void BecomeObtained(ItemHandlerBody agent);

	public abstract void BecomeUnobtained(ItemHandlerBody agent);
}
