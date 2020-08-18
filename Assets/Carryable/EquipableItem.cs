using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableItem : Item, IEquipable {
	public IEquipable equipable;  // set in inspector
	public int eei;  // set elsewhere
	
	protected virtual void Awake() {
		equipable = GetComponent<Equipable>();
	}

	protected virtual void Start() {
		
	}

	public override void BecomeObtained() {
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
	}

	public override void BecomeUnobtained(Transform originTransform) {
		base.BecomeUnobtained(originTransform);
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
