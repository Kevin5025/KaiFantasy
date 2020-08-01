using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : MonoBehaviour, IActivatable {

	public float cooldownTimeout; // set in inspector
	protected float nextReadyTime;

	public virtual void Start() {
		nextReadyTime = 0f;
	}

	/**
     * Checks whether the user is still functional and whether the cooldown period has ended. 
     */
	public virtual bool BecomeActivated(IActivator activator, Dictionary<object, object> argumentDictionary = null) {
		bool isCapable = (int)activator.GetHealthState() >= (int)HealthState.Capable;
		bool isReady = nextReadyTime <= Time.time;

		bool didActivate = false;
		if (isCapable && isReady) {
			Actuate(activator, argumentDictionary);
			nextReadyTime = Time.time + cooldownTimeout;
			didActivate = true;
		}
		return didActivate;
	}

	public abstract void Actuate(IActivator activator, Dictionary<object, object> argumentDictionary = null);

}
