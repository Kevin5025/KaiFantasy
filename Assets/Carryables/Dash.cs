using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour, IActivatable {

	IActivatable activatable_;

	protected virtual void Awake() {
		activatable_ = GetComponent<DashActivatable>();
		// activatable_.cooldownTimeout_ = 4f;
	}

	protected virtual void Start() {
		
	}

	public bool BecomeActivated(IActivator activator, Dictionary<object, object> argumentDictionary = null) {
		return activatable_.BecomeActivated(activator, argumentDictionary);
	}

}
