﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An equipable that has an activatable ability
 */
public abstract class ActivatableEquipable : Equipable {

	protected float cooldownTimeout;
	protected float nextReadyTime;

	public ActivatableEquipable(EquipableClass equipableClass, float cooldownTimeout) : base(equipableClass) {
		this.cooldownTimeout = cooldownTimeout;
	}

	/**
     * Checks whether the user is still functional and whether the cooldown period has ended. 
     */
	public override void Activate(CircleAgent casterAgent) {
		base.Activate(casterAgent);
		if (!casterAgent.defunct && nextReadyTime <= Time.time) {
			Actuate(casterAgent);
			nextReadyTime = Time.time + cooldownTimeout;
		}
	}

	/**
	 * To be override with the details of the ability
	 */
	public virtual void Actuate(CircleAgent casterAgent) {
		
	}

}