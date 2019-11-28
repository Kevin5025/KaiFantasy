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
	public Item[] inventoryItemArray;
	public Equipable[] equipmentEquipableArray;

	protected override void Start() {
		base.Start();
		//gameObject.layer = LayersManager.layersManager.getTeamAgentLayer(team);

		agentController = GetComponent<AgentController>();

		fadeTime = 6f;
		fadeTimeConstant = 0.25f / fadeTime;

		inventoryItemArray = new Item[64];
		equipmentEquipableArray = new Equipable[20];
		// equipmentEquipableArray[0] = Instantiate M9

		InitializeStats();
	}

	private void InitializeStats() {

	}

	//public virtual void Hand() {

	//}

	public virtual void AcquireItem() {
		int itemLayerMask = LayersManager.layersManager.allLayerMaskArray[LayersManager.layersManager.itemLayer];
		Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, radius, itemLayerMask);
		if (colliderArray.Length > 0) {
			for (int rh=0; rh<colliderArray.Length; rh++) {
				Debug.Log(colliderArray[rh].name);
				Debug.Log(MyStaticLibrary.GetDistance(transform.position, colliderArray[rh].ClosestPoint(transform.position)));
			}
		} else {
			Debug.Log("nothing");
		}
	}

	public virtual void DiscardItem() {

	}

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
