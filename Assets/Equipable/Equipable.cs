using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Abilities, Items, Vassals, and Ideas
 */
public abstract class Equipable : MonoBehaviour {
	
	public enum EquipableClass { AccessoryItem, HandItem, PocketItem, HeadItem, BodyItem, Ability, LargeVassal, SmallVassal, Idea, Intrinsic };
	public EquipableClass equipableClass;  // set in inspector

	protected virtual void Start() {

	}

	/*
	 * This empty function just makes it easier to do polymorphic calls to Activate
	 * returns whether or not Actuation occurred
	 */
	public virtual bool Activate(Body casterAgent, Dictionary<object, object> argumentDictionary = null) {
		// do nothing
		return false;
	}

	public static bool IsHandPocketEquipableClass (EquipableClass equipableClass) {
		return equipableClass == EquipableClass.HandItem || equipableClass == EquipableClass.PocketItem;
	}

}
