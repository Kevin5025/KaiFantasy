using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBody : HealthStateBody, IHealthBody {

	protected bool disintegrates; // set beforehand; Disintegrates: turns into resources; TODO: Set of resources instead of bool
	protected bool respawns;  //set beforehand;  // TODO

	public float[] healthStateUpperThresholdList;  // set beforehand
	public float[] healthStateRateList;  // set beforehand
										 // public float capableCapacity;  // set beforehand
	public float capableCapacityRate;  // set beforehand
	public float capableCapacityComplexity;  // set beforehand
	public float healthRateGeneralBonus;  // set beforehand
	public float health;  // set starting value beforehand

	public bool ascended;  // temporary coding variable
						   // Start is called before the first frame update

	public bool defunct;  // aka "dead", "destroyed", etc. 

	void Start() {
		ascended = false;
		defunct = false;
	}

	// Update is called once per frame
	void Update() {
		HealthStatusUpdate();
		HealthUpdate();
	}

	protected virtual void HealthStatusUpdate() {
		foreach (HealthState healthState in System.Enum.GetValues(typeof(HealthState))) {
			if (health < healthStateUpperThresholdList[(int)healthState]) {
				this.healthState = healthState;
				break;
			}
		}
		if (healthState == HealthState.Disintegrated) {
			defunct = true;
		}
		if (health > healthStateUpperThresholdList[(int)HealthState.Overflowing]) {
			this.healthState = HealthState.Ascended;
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

	protected virtual void HealthUpdate() {  // TODO: TEST
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

	public virtual float TakeDamage(CompleteBody casterAgent, float damage) {
		float trueDamage = damage;
		health -= trueDamage;
		return trueDamage;
	}

	public HealthState GetHealthState() {
		return healthState;
	}

	public float[] GetHealthStateUpperThresholdList() {
		return healthStateUpperThresholdList;
	}

	public float GetHealth() {
		return health;
	}
}
