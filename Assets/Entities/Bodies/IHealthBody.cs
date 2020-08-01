using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthState { Disintegrated, Bricked, Fibrillating, Capable, Overflowing, Ascended, };

public interface IHealthBody {
	float TakeDamage(CompleteBody casterAgent, float damage);
	HealthState GetHealthState();
	float[] GetHealthStateUpperThresholdList();
	float GetHealth();
}
