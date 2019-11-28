using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Abilities, Items, Vassals, and Ideas
 */
public abstract class Equipable : MonoBehaviour {
	
	public enum EquipableClass { AccessoryItem, HandItem, HeadItem, BodyItem, Ability, LargeVassal, SmallVassal };
	public EquipableClass equipableClass;  // set in inspector

	protected virtual void Start() {

	}

	public virtual void Activate(CircleAgent casterAgent) {

	}

}
