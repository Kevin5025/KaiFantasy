using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableItem : Item, IEquipable {
	public IEquipable equipable;  // set in inspector
	public int eei;  // set elsewhere
	
	protected virtual void Start() {
		equipable = GetComponent<Equipable>();
	}

	public override void BecomeObtained(ItemHandlerBody agent) {
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
	}

	public override void BecomeUnobtained(ItemHandlerBody agent) {
		transform.position = agent.transform.position;
		transform.rotation = agent.transform.rotation;
		GetComponent<Rigidbody2D>().velocity = agent.GetComponent<Rigidbody2D>().velocity;

		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<Collider2D>().enabled = true;
	}

	public SpriteRenderer GetComponentSpriteRenderer() {
		return equipable.GetComponentSpriteRenderer();
	}

	public EquipableClass GetEquipableClass() {
		return equipable.GetEquipableClass();
	}

	public void SetEquipableClass(EquipableClass equipableClass) {
		equipable.SetEquipableClass(equipableClass);
	}
}
