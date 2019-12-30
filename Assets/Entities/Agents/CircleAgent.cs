﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is anything that can be controlled and fight. 
 */
public class CircleAgent : CircleEntity {

	protected AgentController agentController;

	protected float fadeTime;
	protected float fadeTimeConstant;

	// TODO: repositorySkillArray;  // repository / library / studio
	// public List<Item> inventoryItemArray;
	public Equipable.EquipableClass[] equipmentEquipableClassArray; // TODO: different agent classes with different equipable class arrays
	public Equipable[] equipmentEquipableArray;

	protected override void Start() {
		base.Start();
		//gameObject.layer = LayersManager.layersManager.getTeamAgentLayer(team);

		agentController = GetComponent<AgentController>();

		fadeTime = 6f;
		fadeTimeConstant = 0.25f / fadeTime;

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
		equipmentEquipableArray = new Equipable[equipmentEquipableClassArray.Length];
		// equipmentEquipableArray[0] = Instantiate M9

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
		Collider2D[] itemColliderArray = Physics2D.OverlapCircleAll(transform.position, radius, itemLayerMask);

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
	 * If pocket exists and full, put pocketItem into hand
	 * 
	 * If pocketItem:
	 * Drop pocketItem onto ground
	 * Pocket of pocket will not exist
	 */
	public void UnequipItem(int eei) {
		bool eeiInArrayBounds = eei > -1 && eei < equipmentEquipableArray.Length;
		if (eeiInArrayBounds && equipmentEquipableArray[eei] != null) {
			Item unequipItem = (Item)equipmentEquipableArray[eei];
			equipmentEquipableArray[eei] = null;
			unequipItem.BecomeUnequiped(this);

			int eeiPocketHypothetical = eei + 1;  // hypothetical because pocket may or may not exist
			if (equipmentEquipableClassArray[eeiPocketHypothetical] == Equipable.EquipableClass.PocketItem && equipmentEquipableArray[eeiPocketHypothetical] != null) {
				PocketItem(eei);
			}
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

	//public virtual void AcquireItem() {
	//}

	//public virtual void DiscardItem() {

	//}

	//public override float takeDamage(CircleAgent casterAgent, float damage) {
	//	float trueDamage = damage;
	//	health -= trueDamage;
	//	return trueDamage;
	//}

	/**
     * Overrides entity fade for a gradual disappearance, since these agents are more important than any entity. 
     */
	protected override IEnumerator Fade() {
		for (float f = 0.25f; f > 0; f -= Time.deltaTime * fadeTimeConstant) {
			spriteRenderer.color = new Color(r, g, b, f);
			//yield return new WaitForSeconds(1f);//3f? //is this consistent? 
			yield return null;  // https://answers.unity.com/questions/755196/yield-return-null-vs-yield-return-waitforendoffram.html
		}
		EliminateSelf();
	}

}
