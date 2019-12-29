using System.Collections;
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
			Equipable.EquipableClass.HandItem,
			Equipable.EquipableClass.HandItem,
			Equipable.EquipableClass.HandItem,
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
	public virtual int HandleItem(int numEeiAlternative) {
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

		int eei = -1;
		if (minDistanceItemCollider != null) {
			Item minDistanceItem = minDistanceItemCollider.GetComponent<Item>();
			eei = GetPocketItemClassEei(minDistanceItem.equipableClass, numEeiAlternative);
			PocketItem(minDistanceItem, eei);
		}
		return eei;
	}

	/*
	 * Sometimes, people like to put their hands into their pockets. 
	 */
	public virtual void PocketItem(Item pocketItem, int eei) {
		if (eei > -1) {
			UnpocketItem(eei);
			pocketItem.BecomePocketed(this);
			equipmentEquipableArray[eei] = pocketItem;
		}
	}

	public virtual void UnpocketItem(int eei) {
		if (eei > -1 && equipmentEquipableArray[eei] != null) {
			Item oldItem = (Item)equipmentEquipableArray[eei];
			equipmentEquipableArray[eei] = null;
			oldItem.BecomeUnpocketed(this);
		}
	}

	public virtual int GetPocketItemClassEei(Equipable.EquipableClass pocketItemEquipableClass, int numEeiAlternative) {
		int eei = GetEquipableClassEei(pocketItemEquipableClass);
		int eeiAlternative = eei + numEeiAlternative;
		int eeiFinal = pocketItemEquipableClass == equipmentEquipableClassArray[eeiAlternative] ? eeiAlternative : eei;
		return eeiFinal;
	}

	private int GetEquipableClassEei(Equipable.EquipableClass equipableClass) {
		int equipableClassEei = -1;
		for (int eei = 0; eei < equipmentEquipableClassArray.Length; eei++) {
			if (equipableClass == equipmentEquipableClassArray[eei]) {
				equipableClassEei = eei;
				break;
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

	//protected override void EliminateSelf () {
	//	base.EliminateSelf();
	//}

}
