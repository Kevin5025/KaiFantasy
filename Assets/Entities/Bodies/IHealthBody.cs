using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthState { Disintegrated, Bricked, Fibrillating, Capable, Overflowing, Ascended, };

public interface IHealthBody : IHealthStateBody {
	float TakeDamage(CompleteBody casterAgent, float damage);
	float[] GetHealthStateUpperThresholdList();
	float GetHealth();
}
