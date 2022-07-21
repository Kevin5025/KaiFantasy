using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBody : HealthStateBody, IHealthBody {

	protected bool respawns;  // TODO

	public float[] healthStateUpperThresholdList_;
	public float healthRateBase_;
	public float health_;

	public bool ever_ascended;  // temporary coding variable

	protected virtual void Awake()
	{
		healthStateUpperThresholdList_[(int)HealthState.Ascended] = float.MaxValue;
	}

	protected virtual void Start() {
		ever_ascended = false;
		HealthStatusUpdate();
	}

	// Update is called once per frame
	protected virtual void Update()
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
			if (health_ < healthStateUpperThresholdList_[(int)healthState]) {
				this.healthState = healthState;
				break;
			}
		}
		if (this.healthState == HealthState.Ascended) {
			ever_ascended = true;
		}
	}

	protected virtual void HealthUpdate() {
		float healthRateCurrent = healthRateBase_;
		if ((int)healthState >= (int)HealthState.Overflowing) {
			healthRateCurrent = 0f;
        }
		health_ += healthRateCurrent * Time.deltaTime;
	}

	public virtual float TakeDamage(CompositeBody casterAgent, float damage) {
		float trueDamage = damage;
		health_ -= trueDamage;
		return trueDamage;
	}

	public float[] GetHealthStateUpperThresholdList() {
		return healthStateUpperThresholdList_;
	}

	public float GetHealth() {
		return health_;
	}
}
