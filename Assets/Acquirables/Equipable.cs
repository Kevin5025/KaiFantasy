﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Abilities, Items, Vassals, and Ideas
 */
// TODO: equipables that aren't collectables: e.g. learnables
public class Equipable : MonoBehaviour, IEquipable {

	public static Dictionary<EquipableClass, Color> equipableColorDictionary;  // returns the correct background color for the given equipable_ class

	static Equipable() {
		equipableColorDictionary = new Dictionary<EquipableClass, Color>() {
			{ EquipableClass.HandItem, new Color(1f, 0f, 0f) },
			{ EquipableClass.PocketItem, new Color(0.75f, 0f, 0f) },
			{ EquipableClass.HeadItem, new Color(1f, 0.5f, 0f) },
			{ EquipableClass.BodyItem, new Color(1f, 1f, 0f) },
			{ EquipableClass.Ability, new Color(0f, 1f, 0f) },
			{ EquipableClass.LargeVassal, new Color(0f, 1f, 1f) },
			{ EquipableClass.SmallVassal, new Color(0f, 0f, 1f) },
			{ EquipableClass.AccessoryItem, new Color(1f, 0f, 1f) },
			{ EquipableClass.Idea, new Color(0.5f, 0.5f, 0.5f) },
		};
	}

	public EquipableClass equipableClass_;  // set in inspector
	public int eei;  // set elsewhere

	public EquipableClass GetEquipableClass() {
		return equipableClass_;
	}

	public void SetEquipableClass(EquipableClass equipableClass) {
		this.equipableClass_ = equipableClass;
	}

	public int getEei() {
		return eei;
	}

	public void setEei(int eei) {
		this.eei = eei;
	}

	public SpriteRenderer GetComponentSpriteRenderer() {
		return GetComponent<SpriteRenderer>();
	}
}
