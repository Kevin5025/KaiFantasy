using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item {

	// public float cooldownTimeout; // set in inspector  // from base
	public float projectileTimeout;  // set in inspector
	public float initialVelocity;  // set in inspector
	public float baseDamage;  // set in inspector

	public enum FiringMode { Manual, Automatic, Burst };
	public FiringMode firingMode;
	
	public static int numAmmunitionTypes = 4;
	public int ammunitionType;
	public int magazineCapacity;  // inspector
	public int magazineCount;
	public float reloadTime;  // inspector
	public int bulletCount;  // inspector

	protected override void Start() {
		base.Start();
		equipableClass = EquipableClass.HandItem;
		magazineCount = 0;
	}

	public override bool Activate(Body casterAgent, Dictionary<object, object> argumentDictionary = null) {
		bool clickActivate = (bool)argumentDictionary["MBD"] && firingMode == FiringMode.Manual;
		bool holdActivate = (bool)argumentDictionary["MB"] && (firingMode == FiringMode.Automatic || firingMode == FiringMode.Burst);
		bool controlActivate = clickActivate || holdActivate;

		bool didActivate = false;
		if (controlActivate && magazineCount > 0) {
			didActivate = base.Activate(casterAgent, argumentDictionary);
			if (didActivate) {
				magazineCount--;
			}
		}
		return didActivate;
	}

	public override void Actuate(Body casterAgent, Dictionary<object, object> argumentDictionary = null) {
		base.Actuate(casterAgent);
		Vector2 headPosition = casterAgent.transform.TransformPoint(casterAgent.headPosition);
		GameObject projectileGameObject = GameObject.Instantiate(PrefabReferences.prefabReferences.bullet, headPosition, casterAgent.transform.rotation);

		Projectile projectile = projectileGameObject.AddComponent<Projectile>();
		projectile.affinity = casterAgent.affinity;
		projectile.casterAgent = casterAgent;
		projectile.timeout = projectileTimeout;
		projectile.initialVelocity = initialVelocity;
		projectile.baseDamage = baseDamage;
		projectile.LateStart();

		projectileGameObject.GetComponent<Rigidbody2D>().velocity = projectileGameObject.transform.TransformDirection(new Vector2(0, projectile.initialVelocity));
		projectileGameObject.GetComponent<Collider2D>().enabled = true;
	}

	public virtual void Reload(Body casterAgent) {
		if ((int)casterAgent.healthState >= (int)CircleBody.HealthState.Capable && casterAgent.GetAmmunitionCountArray()[ammunitionType] > 0) {
			int reloadAmmunitionCount = Math.Min(casterAgent.GetAmmunitionCountArray()[ammunitionType], magazineCapacity - magazineCount);
			magazineCount += reloadAmmunitionCount;
			casterAgent.GetAmmunitionCountArray()[ammunitionType] -= reloadAmmunitionCount;
		}
	}
}
