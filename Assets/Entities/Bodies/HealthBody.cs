using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBody : HealthStateBody, IHealthBody {

	protected bool respawns;  // TODO

	public float[] healthStateUpperThresholdList;  // set beforehand
	public float healthRateBase;  // set beforehand
	public float health;  // set starting value beforehand

	public bool ever_ascended;  // temporary coding variable

	void Awake()
	{
		healthStateUpperThresholdList[(int)HealthState.Ascended] = float.MaxValue;
	}

	void Start() {
		ever_ascended = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (healthState == HealthState.Disintegrated)
		{
			return;
		}

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
		if (this.healthState == HealthState.Ascended) {
			ever_ascended = true;
		}
	}

	protected virtual void HealthUpdate() {
		float healthRateCurrent = healthRateBase;
		if ((int)healthState >= (int)HealthState.Overflowing) {
			healthRateCurrent = 0f;
        }
		health += healthRateCurrent * Time.deltaTime;
	}

	public virtual float TakeDamage(CompleteBody casterAgent, float damage) {
		float trueDamage = damage;
		health -= trueDamage;
		return trueDamage;
	}

	public float[] GetHealthStateUpperThresholdList() {
		return healthStateUpperThresholdList;
	}

	public float GetHealth() {
		return health;
	}
}
