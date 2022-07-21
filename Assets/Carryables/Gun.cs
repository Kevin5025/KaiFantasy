using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FiringMode { Manual, Automatic, Burst };

public class Gun : MonoBehaviour, IActivatable, ICollectable, IEquipable {

	protected IActivatable activatable_;
	protected ICollectable collectable_;
	protected IEquipable equipable_;

	public FiringMode firingMode_;
	
	public int ammunitionType_;  // {0...5}, one per color, for now
	public int magazineCapacity_;
	public int magazineCount;
	public float reloadTime_;

	protected virtual void Awake() {
		activatable_ = GetComponent<GunActivatable>();
		collectable_ = GetComponent<Collectable>();
		equipable_ = GetComponent<Equipable>();
	}

	protected virtual void Start() {
		magazineCount = 0;
	}

	public virtual void Reload(CompleteBody completeBodyActivator) {
		if ((int)completeBodyActivator.GetHealthState() >= (int)HealthState.Capable && completeBodyActivator.GetAccountQuantityArray()[ammunitionType_] > 0) {
			int reloadAmmunitionCount = Math.Min((int)completeBodyActivator.GetAccountQuantityArray()[ammunitionType_], magazineCapacity_ - magazineCount);
			magazineCount += reloadAmmunitionCount;
			completeBodyActivator.GetAccountQuantityArray()[ammunitionType_] -= reloadAmmunitionCount;
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



	public void BecomeCollected(Transform originTransform) {
		collectable_.BecomeCollected(originTransform);
	}

	public void BecomeUncollected(Transform originTransform) {
		collectable_.BecomeUncollected(originTransform);
	}

	public EquipableClass GetEquipableClass() {
		return equipable_.GetEquipableClass();
	}

	public void SetEquipableClass(EquipableClass equipableClass) {
		equipable_.SetEquipableClass(equipableClass);
	}

	public int getEei() {
		return equipable_.getEei();
	}

	public void setEei(int eei) {
		equipable_.setEei(eei);
	}

	public SpriteRenderer GetComponentSpriteRenderer() {
		return equipable_.GetComponentSpriteRenderer();
	}
}
