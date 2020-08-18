using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IItem {

	public void BecomeHandled() {
		BecomeObtained();
	}

	public abstract void BecomeObtained();

	public virtual void BecomeUnobtained(Transform originTransform) {
		transform.position = originTransform.position;
		transform.rotation = originTransform.rotation;
		GetComponent<Rigidbody2D>().velocity = originTransform.GetComponent<Rigidbody2D>().velocity;
	}
}
