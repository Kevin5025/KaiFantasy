using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour, IActivator {

	protected HealthStateBody healthStateBody;

	protected virtual void Awake() {
		healthStateBody = GetComponent<HealthStateBody>();
	}

	protected virtual void Start() {
		
	}

	public static bool Activate(IActivator activator, IActivatable activatable, Dictionary<object, object> argumentDictionary = null) {
		bool capable = (int)activator.GetHealthState() >= (int)HealthState.Capable;
		if (capable && activatable != null) {
			activatable.BecomeActivated(activator, argumentDictionary);
		}
		return capable;
	}

	public HealthState GetHealthState() {
		return healthStateBody.healthState;
	}

	public void SetHealthState(HealthState healthState) {
		healthStateBody.healthState = healthState;
	}
}
