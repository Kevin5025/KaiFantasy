using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Abilities, Items, Vassals, and Ideas
 */
public abstract class Equipable : MonoBehaviour {
	
	public enum EquipableClass { AccessoryItem, HandItem, PocketItem, HeadItem, BodyItem, Ability, LargeVassal, SmallVassal, Idea, };
	public EquipableClass equipableClass;  // set in inspector

	protected virtual void Start() {

	}

	public virtual void Activate(Body casterAgent) {

	}

	public static bool IsHandPocketEquipableClass (EquipableClass equipableClass) {
		return equipableClass == EquipableClass.HandItem || equipableClass == EquipableClass.PocketItem;
	}

}
