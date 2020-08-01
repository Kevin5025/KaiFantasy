using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Abilities, Items, Vassals, and Ideas
 */
public class Equipable : MonoBehaviour, IEquipable {
	
	public EquipableClass equipableClass;  // set in inspector

	public EquipableClass GetEquipableClass() {
		return equipableClass;
	}

	public void SetEquipableClass(EquipableClass equipableClass) {
		this.equipableClass = equipableClass;
	}

	public SpriteRenderer GetComponentSpriteRenderer() {
		return GetComponent<SpriteRenderer>();
	}
}
