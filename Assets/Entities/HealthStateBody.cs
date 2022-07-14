using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStateBody : MonoBehaviour, IHealthStateBody {
	public HealthState healthState;

    public HealthState GetHealthState() {
		return healthState;
	}
}
