using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: handleables that aren't equipable: e.g. draggables
public abstract class Handleable : MonoBehaviour, IHandleable {

	public virtual void BecomeHandled(Transform originTransform) {
		transform.SetParent(originTransform);
	}

	public virtual void BecomeUnhandled(Transform originTransform) {
		transform.position = originTransform.position;
		transform.rotation = originTransform.rotation;

		Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
		Rigidbody2D originRb2d = originTransform.GetComponent<Rigidbody2D>();
		rb2d.velocity = originRb2d.velocity;
		rb2d.angularVelocity = originRb2d.angularVelocity;

		transform.SetParent(SceneReferences.sceneReferences_.itemsGameObject_.transform);
	}
}
