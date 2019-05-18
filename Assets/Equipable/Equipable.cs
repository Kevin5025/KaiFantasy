using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Abilities, Items, and Vassals
 */
public abstract class Equipable {
	
	public enum EquipableClass { AccessoryItem, HandItem, HeadItem, BodyItem, Ability, LargeVassal, SmallVassal };
	public EquipableClass equipableClass;

	public Equipable(EquipableClass equipableClass) {
		this.equipableClass = equipableClass;
	}

	public virtual void Activate(CircleAgent casterAgent) {

	}

}
