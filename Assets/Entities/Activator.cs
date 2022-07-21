using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : HealthStateBody, IActivator {

	protected virtual void Awake() {
		
	}

	protected virtual void Start() {

	}

	//public bool Activate(IActivatable activatable, Dictionary<object, object> argumentDictionary = null) {
	//	return Activator.Activate(this, activatable, argumentDictionary);
	//}

	public static bool Activate(IActivator activator, IActivatable activatable, Dictionary<object, object> argumentDictionary = null) {
		bool is_at_least_capable = (int)activator.GetHealthState() >= (int)HealthState.Capable;
		bool does_activate = is_at_least_capable && activatable != null;
		if (does_activate) {
			activatable.BecomeActivated(activator, argumentDictionary);
		}
		return does_activate;
	}
}
