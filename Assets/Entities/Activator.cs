using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour, IActivator {

	protected IHealthStateBody healthStateBody_;

	protected virtual void Awake() {
		healthStateBody_ = GetComponent<HealthStateBody>();
	}

	protected virtual void Start() {
		
	}

	public static bool Activate(IActivator activator, IActivatable activatable, Dictionary<object, object> argumentDictionary = null) {
		bool is_at_least_capable = (int)activator.GetHealthState() >= (int)HealthState.Capable;
		bool does_activate = is_at_least_capable && activatable != null;
		if (does_activate) {
			activatable.BecomeActivated(activator, argumentDictionary);
		}
		return does_activate;
	}

    public HealthState GetHealthState() {
        return healthStateBody_.GetHealthState();
    }
}
