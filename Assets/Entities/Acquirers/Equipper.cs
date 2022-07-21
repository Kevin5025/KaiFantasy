using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipper : MonoBehaviour, IEquipper {

	// public List<Item> inventoryItemArray;
	public EquipableClass[] equipmentEquipableClassArray;  // TODO: different agent classes with different equipable_ class arrays
	protected int eeiHand0;
	protected int eeiHand1;
	public IEquipable[] equipmentEquipableArray;
	protected virtual void Awake() {
		
	}

	protected virtual void Start() {
		// inventoryItemArray = new List<Item>();
		// equipmentEquipableClassArray = new Equipable.EquipableClass[20];
		equipmentEquipableClassArray = new EquipableClass[] {
			EquipableClass.HandItem,
			EquipableClass.PocketItem,
			EquipableClass.HandItem,
			EquipableClass.PocketItem,
			EquipableClass.HeadItem,
			EquipableClass.BodyItem,
			EquipableClass.Ability,
			EquipableClass.Ability,
			EquipableClass.Ability,
			EquipableClass.Ability,
			EquipableClass.LargeVassal,
			EquipableClass.SmallVassal,
			EquipableClass.SmallVassal,
			EquipableClass.SmallVassal,
			EquipableClass.SmallVassal,
			EquipableClass.SmallVassal,
			EquipableClass.AccessoryItem,
			EquipableClass.AccessoryItem,
			EquipableClass.Idea,
			EquipableClass.Idea,
		};
		eeiHand0 = GetEquipableClassEei(EquipableClass.HandItem, 0);
		eeiHand1 = GetEquipableClassEei(EquipableClass.HandItem, 1);

		equipmentEquipableArray = new IEquipable[equipmentEquipableClassArray.Length];
		GameObject m9GameObject = Instantiate(PrefabReferences.prefabReferences_.m9Prefab_, transform);
		IEquipable m9 = m9GameObject.GetComponent<Gun>();
		EquipEquipable(m9);
	}

	protected virtual void Update() {

	}

	/*
	 * If pocket exists and empty, put handItem into pocket
	 * If pocket full or nonexisting, drop handItem onto ground
	 * Put groundItem into hand
	 */
	public void EquipEquipable(IEquipable equipable, int numNextEei = 0) {
		int eei = GetEquipableClassEei(equipable.GetEquipableClass(), numNextEei);
		equipable.setEei(eei);
		int eeiPocketHypothetical = eei + 1;  // hypothetical because pocket may or may not exist
		if (equipmentEquipableClassArray[eeiPocketHypothetical] == EquipableClass.PocketItem && equipmentEquipableArray[eeiPocketHypothetical] == null) {
			PocketEquipable(eei);
		} else {
			UnequipEquipable(eei);
		}

		equipmentEquipableArray[eei] = equipable;

		ICollectable collectable = equipable.GetComponent<Collectable>();  // TODO: consider possibility override
		collectable.BecomeCollected(transform);
	}

	public IEquipable UnequipEquipable(int eei) {
		IEquipable equipable = null;
		if (equipmentEquipableArray[eei] != null) {
			equipable = equipmentEquipableArray[eei];
			equipmentEquipableArray[eei] = null;

			ICollectable collectable = equipable.GetComponent<Collectable>();  // TODO set eei to -1  // TODO: consider possibility override
			collectable.BecomeUncollected(transform);  // TODO
		}
		return equipable;
	}

	/*
	 * Swaps handItem and pocketItem
	 * Sometimes, people like to put their hands into their pockets. 
	 * Assumes that pocket exists
	 */
	public void PocketEquipable(int eeiHand) {
		int eeiPocket = eeiHand + 1;
		IEquipable equipable = equipmentEquipableArray[eeiPocket];
		equipmentEquipableArray[eeiPocket] = equipmentEquipableArray[eeiHand];
		equipmentEquipableArray[eeiHand] = equipable;
	}

	/*
	 * Gets the numNextEei-th index that matches the equipableClass_
	 * If numNextEei too big, then gets the last index that matches the equipableClass_
	 */
	public int GetEquipableClassEei(EquipableClass equipableClass, int numNextEei) {
		int equipableClassEei = -1;
		int numEeiCurrent = 0;
		for (int eei = 0; eei < equipmentEquipableClassArray.Length; eei++) {
			if (equipableClass == equipmentEquipableClassArray[eei]) {
				equipableClassEei = eei;
				numEeiCurrent++;
				if (numEeiCurrent > numNextEei) {
					break;
				}
			}
		}
		return equipableClassEei;
	}

	public EquipableClass[] GetEquipmentEquipableClassArray() {
		return equipmentEquipableClassArray;
	}

	public IEquipable[] GetEquipmentEquipableArray() {
		return equipmentEquipableArray;
	}
}