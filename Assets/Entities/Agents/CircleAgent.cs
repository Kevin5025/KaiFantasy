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
	public List<Item> inventoryItemArray;
	public Equipable[] equipmentEquipableArray;

	protected override void Start() {
		base.Start();
		//gameObject.layer = LayersManager.layersManager.getTeamAgentLayer(team);

		agentController = GetComponent<AgentController>();

		fadeTime = 6f;
		fadeTimeConstant = 0.25f / fadeTime;

		inventoryItemArray = new List<Item>();
		equipmentEquipableArray = new Equipable[20];
		// equipmentEquipableArray[0] = Instantiate M9

		InitializeStats();
	}

	private void InitializeStats() {

	}

	/*
	 * Hand
	 */
	public virtual void HandleItem() {
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
		if (minDistanceItemCollider != null) {
			Item minDistanceItem = minDistanceItemCollider.GetComponent<Item>();
			PocketItem(minDistanceItem);
		}
	}
	
	/*
	 * Sometimes, people like to put their hands in their pockets. 
	 */
	public virtual void PocketItem(Item item) {
		item.BecomePocketed(this);
		inventoryItemArray.Add(item);
	}

	public virtual void UnpocketItem(Item item) {
		inventoryItemArray.Remove(item);
		item.BecomeUnpocketed(this);
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
