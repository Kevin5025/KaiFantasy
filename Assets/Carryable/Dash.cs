using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour, IActivatable {

	IActivatable activatable;  // set in inspector as DashActivatable

	protected virtual void Awake() {
		activatable = GetComponent<DashActivatable>();
		// activatable.cooldownTimeout = 4f;
	}

	protected virtual void Start() {
		
	}

	public bool BecomeActivated(IActivator activator, Dictionary<object, object> argumentDictionary = null) {
		return activatable.BecomeActivated(activator, argumentDictionary);
	}

}
