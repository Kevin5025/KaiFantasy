using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is anything that can use items. 
 */
public class ItemHandlerBody : MonoBehaviour, IItemHandlerBody {
	
	protected float itemHandleRadius;

	// TODO: repositorySkillArray;  // repository / library / studio
	// public List<Item> inventoryItemArray;
	public Equipable.EquipableClass[] equipmentEquipableClassArray;  // TODO: different agent classes with different equipable class arrays
	protected int eeiHand0;
	protected int eeiHand1;
	public Equipable[] equipmentEquipableArray;

	protected virtual void Start() {
		Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
		itemHandleRadius = Mathf.Sqrt(2 * rb2D.inertia / rb2D.mass);

		// inventoryItemArray = new List<Item>();
		// equipmentEquipableClassArray = new Equipable.EquipableClass[20];
		equipmentEquipableClassArray = new Equipable.EquipableClass[] {
			Equipable.EquipableClass.AccessoryItem,
			Equipable.EquipableClass.AccessoryItem,
			Equipable.EquipableClass.HandItem,
			Equipable.EquipableClass.PocketItem,
			Equipable.EquipableClass.HandItem,
			Equipable.EquipableClass.PocketItem,
			Equipable.EquipableClass.HeadItem,
			Equipable.EquipableClass.BodyItem,
			Equipable.EquipableClass.Ability,
			Equipable.EquipableClass.Ability,
			Equipable.EquipableClass.Ability,
			Equipable.EquipableClass.Ability,
			Equipable.EquipableClass.LargeVassal,
			Equipable.EquipableClass.SmallVassal,
			Equipable.EquipableClass.SmallVassal,
			Equipable.EquipableClass.SmallVassal,
			Equipable.EquipableClass.SmallVassal,
			Equipable.EquipableClass.SmallVassal,
			Equipable.EquipableClass.Idea,
			Equipable.EquipableClass.Idea,
		};
		eeiHand0 = GetEquipableClassEei(Equipable.EquipableClass.HandItem, 0);
		eeiHand1 = GetEquipableClassEei(Equipable.EquipableClass.HandItem, 1);

		equipmentEquipableArray = new Equipable[equipmentEquipableClassArray.Length];
		GameObject m9GameObject = Instantiate(PrefabReferences.prefabReferences.m9GameObject);
		Debug.Log(m9GameObject);
		EquipItem(m9GameObject.GetComponent<Item>(), eeiHand0);
		Debug.Log(equipmentEquipableArray[eeiHand0]);

		InitializeStats();
	}

	private void InitializeStats() {

	}

	/*
	 * Hand
	 */
	public int HandleItem(int numNextEei) {
		Item minDistanceItem = GetMinDistanceItem();
		int eeiHand;
		if (minDistanceItem != null) {
			eeiHand = GetEquipableClassEei(minDistanceItem.equipableClass, numNextEei);
			EquipItem(minDistanceItem, eeiHand);
		} else {
			eeiHand = -1;
		}
		return eeiHand;
	}

	private Item GetMinDistanceItem() {
		Item minDistanceItem = null;
		Collider2D minDistanceItemCollider = GetMinDistanceItemCollider();
		if (minDistanceItemCollider != null) {
			minDistanceItem = minDistanceItemCollider.GetComponent<Item>();
		}
		return minDistanceItem;
	}

	private Collider2D GetMinDistanceItemCollider() {
		int itemLayerMask = LayersManager.layersManager.allLayerMaskArray[LayersManager.layersManager.itemLayer];
		Collider2D[] itemColliderArray = Physics2D.OverlapCircleAll(transform.position, itemHandleRadius, itemLayerMask);

		Collider2D minDistanceItemCollider = null;
		float minDistanceItemColliderDistance = float.MaxValue;
		for (int rh = 0; rh < itemColliderArray.Length; rh++) {
			float itemColliderDistance = MyStaticLibrary.GetDistance(transform.position, itemColliderArray[rh].ClosestPoint(transform.position));
			if (itemColliderDistance < minDistanceItemColliderDistance) {
				minDistanceItemCollider = itemColliderArray[rh];
				minDistanceItemColliderDistance = itemColliderDistance;
			}
		}
		return minDistanceItemCollider;
	}

	/*
	 * If pocket exists and empty, put handItem into pocket
	 * If pocket full or nonexisting, drop handItem onto ground
	 * Put groundItem into hand
	 */
	public void EquipItem(Item equipItem, int eeiHand) {
		if (eeiHand > -1) {
			int eeiPocketHypothetical = eeiHand + 1;  // hypothetical because pocket may or may not exist
			if (equipmentEquipableClassArray[eeiPocketHypothetical] == Equipable.EquipableClass.PocketItem && equipmentEquipableArray[eeiPocketHypothetical] == null) {
				PocketItem(eeiHand);
			} else {
				UnequipItem(eeiHand);
			}

			equipItem.BecomeEquiped(this);
			equipmentEquipableArray[eeiHand] = equipItem;
		}
	}

	/*
	 * If handItem: 
	 * Drop handItem onto ground
	 * // If pocket exists and full, put pocketItem into hand
	 * 
	 * If pocketItem:
	 * Drop pocketItem onto ground
	 * // Pocket of pocket will not exist
	 */
	public void UnequipItem(int eei) {
		bool eeiInArrayBounds = eei > -1 && eei < equipmentEquipableArray.Length;
		if (eeiInArrayBounds && equipmentEquipableArray[eei] != null) {
			Item unequipItem = (Item)equipmentEquipableArray[eei];
			equipmentEquipableArray[eei] = null;
			unequipItem.BecomeUnequiped(this);

			//int eeiPocketHypothetical = eei + 1;  // hypothetical because pocket may or may not exist
			//if (equipmentEquipableClassArray[eeiPocketHypothetical] == Equipable.EquipableClass.PocketItem && equipmentEquipableArray[eeiPocketHypothetical] != null) {
			//	PocketItem(eei);
			//}
		}
	}

	/*
	 * Swaps handItem and pocketItem
	 * Sometimes, people like to put their hands into their pockets. 
	 * Assumes that pocket exists
	 */
	public void PocketItem(int eeiHand) {
		int eeiPocket = eeiHand + 1;
		Equipable equipable = equipmentEquipableArray[eeiPocket];
		equipmentEquipableArray[eeiPocket] = equipmentEquipableArray[eeiHand];
		equipmentEquipableArray[eeiHand] = equipable;
	}

	/*
	 * Gets the numNextEei-th index that matches the equipableClass
	 * If numNextEei too big, then gets the last index that matches the equipableClass
	 */
	public int GetEquipableClassEei(Equipable.EquipableClass equipableClass, int numNextEei) {
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

	public Equipable.EquipableClass[] GetEquipmentEquipableClassArray() {
		return equipmentEquipableClassArray;
	}

	public Equipable[] GetEquipmentEquipableArray() {
		return equipmentEquipableArray;
	}

	//public virtual void AcquireItem() {
	//}

	//public virtual void DiscardItem() {

	//}

}
