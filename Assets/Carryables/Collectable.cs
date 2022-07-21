using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, ICollectable {

	public virtual void BecomeCollected(Transform originTransform) {
		transform.SetParent(originTransform);
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
	}

	public virtual void BecomeUncollected(Transform originTransform) {
		transform.position = originTransform.position;
		transform.rotation = originTransform.rotation;

		Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
		Rigidbody2D originRb2d = originTransform.GetComponent<Rigidbody2D>();
		rb2d.velocity = originRb2d.velocity;
		rb2d.angularVelocity = originRb2d.angularVelocity;

		transform.SetParent(SceneReferences.sceneReferences_.itemsGameObject_.transform);
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<Collider2D>().enabled = true;
	}
}
