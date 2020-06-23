using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An equipable that has an activatable ability
 */
public abstract class ActivatableEquipable : Equipable {

	public float cooldownTimeout; // set in inspector
	protected float nextReadyTime;

	protected override void Start() {
		base.Start();
		nextReadyTime = 0f;
	}

	/**
     * Checks whether the user is still functional and whether the cooldown period has ended. 
     */
	public override void Activate(Body casterAgent, Dictionary<object, object> argumentDictionary = null) {
		if (((int)casterAgent.healthState) >= (int) CircleBody.HealthState.Capable && nextReadyTime <= Time.time) {
			Actuate(casterAgent, argumentDictionary);
			nextReadyTime = Time.time + cooldownTimeout;
		}
	}

	/**
	 * To be override with the details of the ability
	 */
	public virtual void Actuate(Body casterAgent, Dictionary<object, object> argumentDictionary = null) {
		
	}

}
