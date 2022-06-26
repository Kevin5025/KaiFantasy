using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FiringMode { Manual, Automatic, Burst };

public class Gun : EquipableItem, IActivatable {

	protected IActivatable activatable;  // set in inspector as GunActivatable
	
	public FiringMode firingMode;
	
	public int ammunitionType;  // {0...5}, one per color, for now
	public int magazineCapacity;  // inspector
	public int magazineCount;
	public float reloadTime;  // inspector
	public int bulletCount;  // inspector

	protected override void Awake() {
		base.Awake();
		activatable = GetComponent<GunActivatable>();
		// equipable.SetEquipableClass(EquipableClass.HandItem);
		magazineCount = 0;
	}

	protected override void Start() {
		base.Start();
	}

	public virtual void Reload(CompleteBody completeBodyActivator) {
		if ((int)completeBodyActivator.GetHealthState() >= (int)HealthState.Capable && completeBodyActivator.GetFinanceQuantityArray()[ammunitionType] > 0) {
			int reloadAmmunitionCount = Math.Min((int)completeBodyActivator.GetFinanceQuantityArray()[ammunitionType], magazineCapacity - magazineCount);
			magazineCount += reloadAmmunitionCount;
			completeBodyActivator.GetFinanceQuantityArray()[ammunitionType] -= reloadAmmunitionCount;
		}
	}

	public virtual bool BecomeActivated(IActivator activator, Dictionary<object, object> argumentDictionary = null) {
		bool clickActivate = (bool)argumentDictionary["MBD"] && firingMode == FiringMode.Manual;
		bool holdActivate = (bool)argumentDictionary["MB"] && (firingMode == FiringMode.Automatic || firingMode == FiringMode.Burst);
		bool controlActivate = clickActivate || holdActivate;

		bool didActivate = false;
		if (controlActivate && magazineCount > 0) {
			didActivate = activatable.BecomeActivated(activator, argumentDictionary);
			if (didActivate) {
				magazineCount--;
			}
		}
		return didActivate;
	}
}
