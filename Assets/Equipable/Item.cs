using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ActivatableEquipable {
	
	public void BecomePocketed(CircleAgent agent) {
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
	}

	public void BecomeUnpocketed(CircleAgent agent) {
		transform.position = agent.transform.position;
		transform.rotation = agent.transform.rotation;
		GetComponent<Rigidbody2D>().velocity = agent.GetComponent<Rigidbody2D>().velocity;

		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<Collider2D>().enabled = true;
	}

}
