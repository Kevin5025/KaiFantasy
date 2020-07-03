using System.Collections;
using UnityEngine;
using System;

/**
 * This is anything that can be destroyed or killed by taking damage, unless invincible. 
 */
public abstract class Body : Entity, IItemHandlerBody {
	
	public ItemHandlerBody itemHandlerBody;  // set beforehand

	public Vector2 headPosition;
	public float viscosity;

	public enum HealthState { Disintegrated, Bricked, Fibrillating, Capable, Overflowing, Ascended, };
	public HealthState healthState;
	protected bool disintegrates; // set beforehand; Disintegrates: turns into resources; TODO: Set of resources instead of bool
	protected bool respawns;  //set beforehand;
	
	public float[] healthStateUpperThresholdList;  // set beforehand
	public float[] healthStateRateList;  // set beforehand
	// public float capableCapacity;  // set beforehand
	public float capableCapacityRate;  // set beforehand
	public float capableCapacityComplexity;  // set beforehand
	public float healthRateGeneralBonus;  // set beforehand
	public float health;  // set starting value beforehand

	public bool ascended;  // temporary coding variable
	
	protected float fadeBrickedUpperColorAlpha;
	protected float fadeFibrillatingUpperColorAlpha;
	protected float fadeCapableUpperColorAlpha;

	protected override void Start () {
		base.Start();
		viscosity = 5f;

		ascended = false;

		fadeBrickedUpperColorAlpha = 0.25f;
		fadeFibrillatingUpperColorAlpha = 0.5f;
		fadeCapableUpperColorAlpha = 1f;
	}

	/**
     * Handles death (expiration) and regeneration. 
     */
	protected override void FixedUpdate () {
		base.FixedUpdate();
		HealthStatusUpdate();
		HealthUpdate();
	}

	protected virtual void HealthStatusUpdate() {
		foreach (HealthState healthState in System.Enum.GetValues(typeof(HealthState))) {
			if (health < healthStateUpperThresholdList[(int)healthState]) {
				if (healthState != this.healthState) {
					HealthEffectUpdate(healthState);
					this.healthState = healthState;
				}
				break;
			}
		}
		if (health > healthStateUpperThresholdList[(int)HealthState.Overflowing]) {
			this.healthState = HealthState.Ascended;
		}
	}

	protected virtual void HealthEffectUpdate(HealthState healthState) {
		if (healthState == HealthState.Disintegrated) {
			Disintegrate();
		} else if (healthState == HealthState.Bricked) {
			// float fadeBrickedColorAlpha = LinearRangeAnalogy(health, healthStateUpperThresholdList[(int)HealthState.Disintegrated], healthStateUpperThresholdList[(int)HealthState.Bricked], fadeDisintegratedUpperColorAlpha, fadeBrickedUpperColorAlpha);
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadeBrickedUpperColorAlpha);
		} else if (healthState == HealthState.Fibrillating) {
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadeFibrillatingUpperColorAlpha);
		} else if (healthState == HealthState.Capable && this.healthState != HealthState.Overflowing) {
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadeCapableUpperColorAlpha);
		} else if (healthState == HealthState.Overflowing && this.healthState != HealthState.Capable) {
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadeCapableUpperColorAlpha);
		} else if (healthState == HealthState.Ascended) {
			ascended = true;
		}
	}

	public static float LinearRangeAnalogy(float sourceValue, float sourceRangeLowerBound, float sourceRangeUpperBound, float targetRangeLowerBound, float targetRangeUpperBound) {
		float sourceRange = sourceRangeUpperBound - sourceRangeLowerBound;
		float targetRange = targetRangeUpperBound - targetRangeLowerBound;
		float sharedPercentile = (sourceValue - sourceRangeLowerBound) / sourceRange;
		float targetValue = sharedPercentile * targetRange + targetRangeLowerBound;
		return targetValue;
	}

	///**
	//    * Visually lets users know that the entity is defunct. 
	//    */
	//protected virtual IEnumerator FadeBricked() {
	//	spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadeInitialColorAlpha);//instant fade
	//	yield return new WaitForSeconds(fadeDuration);
	//	EliminateSelf();
	//}

	protected virtual void HealthUpdate () {  // TODO: TEST
		if (defunct) {
			return;
		}

		float healthRateCurrent = healthRateGeneralBonus;
		if (healthState == HealthState.Bricked) {
			healthRateCurrent += healthStateRateList[(int)HealthState.Bricked];
		} else if (healthState == HealthState.Fibrillating) {
			float healthToCapable = healthStateUpperThresholdList[(int)HealthState.Fibrillating] - health;
			healthRateCurrent -= healthStateRateList[(int)HealthState.Fibrillating] * healthToCapable;
		} else if ((int)healthState >= (int)HealthState.Capable) {
			float healthFromFibrillating = health - healthStateUpperThresholdList[(int)HealthState.Fibrillating];
			float capableCapacity = healthStateUpperThresholdList[(int)HealthState.Capable] - healthStateUpperThresholdList[(int)HealthState.Fibrillating];
			healthRateCurrent += healthStateRateList[(int)HealthState.Capable] * healthFromFibrillating * (1 - Mathf.Pow(healthFromFibrillating / capableCapacity, capableCapacityComplexity));
		}
		if ((int)healthState >= (int)HealthState.Overflowing) {
			float healthFromCapable = health - healthStateUpperThresholdList[(int)HealthState.Capable];
			healthRateCurrent += healthStateRateList[(int)HealthState.Overflowing] * healthFromCapable;
			healthStateUpperThresholdList[(int)HealthState.Capable] += capableCapacityRate * healthFromCapable * Time.deltaTime;
		}
		health += healthRateCurrent * Time.fixedDeltaTime;
	}

	public virtual float takeDamage(Body casterAgent, float damage) {
		float trueDamage = damage;
		health -= trueDamage;
		return trueDamage;
	}

	protected override int GetTeamLayer() {
		return LayersManager.layersManager.GetTeamEntityLayer(affinity);
	}

	public int HandleItem(int numNextEei) {
		return itemHandlerBody.HandleItem(numNextEei);
	}

	public void EquipItem(Item equipItem, int eeiHand) {
		itemHandlerBody.EquipItem(equipItem, eeiHand);
	}

	public void UnequipItem(int eei) {
		itemHandlerBody.UnequipItem(eei);
	}

	public void PocketItem(int eeiHand) {
		itemHandlerBody.PocketItem(eeiHand);
	}

	public int GetEquipableClassEei(Equipable.EquipableClass equipableClass, int numNextEei) {
		return itemHandlerBody.GetEquipableClassEei(equipableClass, numNextEei);
	}

	public Equipable.EquipableClass[] GetEquipmentEquipableClassArray() {
		return itemHandlerBody.GetEquipmentEquipableClassArray();
	}

	public Equipable[] GetEquipmentEquipableArray() {
		return itemHandlerBody.GetEquipmentEquipableArray();
	}

	public int[] GetAmmunitionCountArray() {
		return (itemHandlerBody).GetAmmunitionCountArray();
	}
}
