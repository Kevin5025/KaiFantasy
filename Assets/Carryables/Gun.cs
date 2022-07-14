using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FiringMode { Manual, Automatic, Burst };

public class Gun : EquipableItem, IActivatable {

	protected IActivatable activatable_;
	
	public FiringMode firingMode_;
	
	public int ammunitionType_;  // {0...5}, one per color, for now
	public int magazineCapacity_;
	public int magazineCount;
	public float reloadTime_;
	public int bulletCount_;  // TODO

	protected override void Awake() {
		base.Awake();
		activatable_ = GetComponent<GunActivatable>();
		// equipable_.SetEquipableClass(EquipableClass.HandItem);
		magazineCount = 0;
	}

	protected override void Start() {
		base.Start();
	}

	public virtual void Reload(CompleteBody completeBodyActivator) {
		if ((int)completeBodyActivator.GetHealthState() >= (int)HealthState.Capable && completeBodyActivator.GetFinanceQuantityArray()[ammunitionType_] > 0) {
			int reloadAmmunitionCount = Math.Min((int)completeBodyActivator.GetFinanceQuantityArray()[ammunitionType_], magazineCapacity_ - magazineCount);
			magazineCount += reloadAmmunitionCount;
			completeBodyActivator.GetFinanceQuantityArray()[ammunitionType_] -= reloadAmmunitionCount;
		}
	}

	public virtual bool BecomeActivated(IActivator activator, Dictionary<object, object> argumentDictionary = null) {
		bool clickActivate = (bool)argumentDictionary["MBD"] && firingMode_ == FiringMode.Manual;
		bool holdActivate = (bool)argumentDictionary["MB"] && (firingMode_ == FiringMode.Automatic || firingMode_ == FiringMode.Burst);
		bool controlActivate = clickActivate || holdActivate;

		bool didActivate = false;
		if (controlActivate && magazineCount > 0) {
			didActivate = activatable_.BecomeActivated(activator, argumentDictionary);
			if (didActivate) {
				magazineCount--;
			}
		}
		return didActivate;
	}
}
