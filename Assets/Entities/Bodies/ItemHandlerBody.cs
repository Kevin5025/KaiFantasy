using System.Collections.Generic;
using UnityEngine;

/**
 * This is anything that can use items. 
 */
public class ItemHandlerBody : MonoBehaviour, IItemHandlerBody, IActivator {

	protected IActivator activator;  // set in inspector

	protected float itemHandleRadius;

	// TODO: repositorySkillArray;  // repository / library / studio
	// public List<Item> inventoryItemArray;
	public EquipableClass[] equipmentEquipableClassArray;  // TODO: different agent classes with different equipable class arrays
	protected int eeiHand0;
	protected int eeiHand1;
	public IEquipable[] equipmentEquipableItemArray;
	public float[] bankFinancialQuantityArray;

	protected virtual void Awake() {
		activator = GetComponent<Activator>();
	}

	protected virtual void Start() {
		Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
		itemHandleRadius = Mathf.Sqrt(2 * rb2D.inertia / rb2D.mass);

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

		equipmentEquipableItemArray = new IEquipable[equipmentEquipableClassArray.Length];
		GameObject m9GameObject = Instantiate(PrefabReferences.prefabReferences.m9GameObject);
		EquipableItem m9 = m9GameObject.GetComponent<EquipableItem>();
		m9.eei = GetEquipableClassEei(m9.GetEquipableClass(), 0);
		EquipItem(m9);

		bankFinancialQuantityArray = new float[FinancialItem.numFinanceTypes];
		bankFinancialQuantityArray[0] = 15;
	}

	/*
	 * Hand
	 */
	public Item HandleItem(int numNextIi) {
		Item minDistanceItem = GetMinDistanceItem();
		if (minDistanceItem != null) {
			ObtainItem(minDistanceItem, numNextIi);
		}
		return minDistanceItem;
	}

	public void ObtainItem(Item item, int numNextIi=0) {
		EquipableItem equipableItem = item.GetComponent<EquipableItem>();
		FinancialItem financialItem = item.GetComponent<FinancialItem>();

		if (equipableItem != null) {
			EquipItem(equipableItem, numNextIi);
		} else if (financialItem != null) {
			CreditItem(financialItem);
		}
	}

	public Item GetMinDistanceItem() {
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
	public void EquipItem(EquipableItem equipableItem, int numNextEei=0) {
		equipableItem.eei = GetEquipableClassEei(equipableItem.GetEquipableClass(), numNextEei);
		int eei = equipableItem.eei;
		int eeiPocketHypothetical = eei + 1;  // hypothetical because pocket may or may not exist
		if (equipmentEquipableClassArray[eeiPocketHypothetical] == EquipableClass.PocketItem && equipmentEquipableItemArray[eeiPocketHypothetical] == null) {
			PocketItem(eei);
		} else {
			UnequipItem(eei);
		}

		equipmentEquipableItemArray[eei] = equipableItem;
		equipableItem.BecomeObtained();
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
		if (equipmentEquipableItemArray[eei] != null) {
			EquipableItem unequipItem = (EquipableItem)equipmentEquipableItemArray[eei];  // TODO handle
			equipmentEquipableItemArray[eei] = null;
			unequipItem.BecomeUnobtained(transform);  // TODO set eei to -1

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
		IEquipable equipable = equipmentEquipableItemArray[eeiPocket];
		equipmentEquipableItemArray[eeiPocket] = equipmentEquipableItemArray[eeiHand];
		equipmentEquipableItemArray[eeiHand] = equipable;
	}

	/*
	 * Gets the numNextEei-th index that matches the equipableClass
	 * If numNextEei too big, then gets the last index that matches the equipableClass
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

	public void CreditItem(FinancialItem financialItem) {
		int ffi = (int)financialItem.financialClass;
		bankFinancialQuantityArray[ffi] += financialItem.quantity;
		financialItem.BecomeObtained();
	}

	public void DebitItem(int ffi) {
		if (bankFinancialQuantityArray[ffi] >= 1) {
			FinancialClass financialClass = (FinancialClass)ffi;
			float quantity = Mathf.Ceil(bankFinancialQuantityArray[ffi] / 3f);
			FinancialItem.InstantiateFinancialItemGameObject(financialClass, quantity, transform);
			bankFinancialQuantityArray[ffi] -= quantity;
		}
	}



	public EquipableClass[] GetEquipmentEquipableClassArray() {
		return equipmentEquipableClassArray;
	}

	public IEquipable[] GetEquipmentEquipableArray() {
		return equipmentEquipableItemArray;
	}

	public float[] GetFinanceQuantityArray() {
		return bankFinancialQuantityArray;
	}

	public HealthState GetHealthState() {
		return activator.GetHealthState();
	}

	public void SetHealthState(HealthState healthState) {
		activator.SetHealthState(healthState);
	}

	//public virtual void AcquireItem() {
	//}

	//public virtual void DiscardItem() {

	//}

}
