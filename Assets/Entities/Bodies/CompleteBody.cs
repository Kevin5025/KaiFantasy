using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * This is anything that can be destroyed or killed by taking damage, unless invincible. 
 */
public class CompleteBody : SpriteBody, IHealthBody, IPhysicsBody, IItemHandlerBody, IActivator {

	protected IHealthBody healthBody_;
	protected IPhysicsBody physicsBody_;
	protected IItemHandlerBody itemHandlerBody_;

	public HealthState healthState;

	public Vector2 headPosition;
	public float viscosity;
	
	protected float brickedColorAlpha;
	protected float fibrillatingColorAlpha;
	protected float capableColorAlpha;
	protected Color brickedColor;
	protected Color fibrillatingColor;
	protected Color capableColor;

	protected override void Awake() {
		base.Awake();
		healthBody_ = GetComponent<HealthBody>();
		physicsBody_ = GetComponent<CirclePhysicsBody>();
		itemHandlerBody_ = GetComponent<ItemHandlerBody>();
	}

	protected override void Start () {
		base.Start();

		headPosition = new Vector2(0, 0.6f * GetRadius());  // 0.297
		viscosity = 5f;

		brickedColorAlpha = 0.25f;
		fibrillatingColorAlpha = 0.5f;
		capableColorAlpha = 1f;
		brickedColor = new Color(spriteRenderer_.color.r, spriteRenderer_.color.g, spriteRenderer_.color.b, brickedColorAlpha);
		fibrillatingColor = new Color(spriteRenderer_.color.r, spriteRenderer_.color.g, spriteRenderer_.color.b, fibrillatingColorAlpha);
		capableColor = new Color(spriteRenderer_.color.r, spriteRenderer_.color.g, spriteRenderer_.color.b, capableColorAlpha);
	}

	protected override void Update() {
		base.Update();

		if (GetHealthState() != healthState) {
			HealthEffectUpdate(GetHealthState());
			healthState = GetHealthState();
		}
	}
	
	protected override void FixedUpdate () {
		base.FixedUpdate();
	}

	protected virtual void HealthEffectUpdate(HealthState healthState) {
		if (healthState == HealthState.Disintegrated) {
			Disintegrate();
		} else if (healthState == HealthState.Bricked) {
			// float fadeBrickedColorAlpha = LinearRangeAnalogy(health_, healthStateUpperThresholdList_[(int)HealthState.Disintegrated], healthStateUpperThresholdList_[(int)HealthState.Bricked], fadeDisintegratedUpperColorAlpha, fadeBrickedUpperColorAlpha);
			spriteRenderer_.color = brickedColor;
		} else if (healthState == HealthState.Fibrillating) {
			spriteRenderer_.color = fibrillatingColor;
		} else if ((int)healthState >= (int)HealthState.Capable) {
			spriteRenderer_.color = capableColor;
		}
	}

	protected override int GetTeamLayer() {
		return LayersManager.layersManager.GetTeamEntityLayer(GetAffinity());
	}

    public float TakeDamage(CompleteBody casterAgent, float damage) {
        return healthBody_.TakeDamage(casterAgent, damage);
    }

    public float[] GetHealthStateUpperThresholdList() {
        return healthBody_.GetHealthStateUpperThresholdList();
    }

    public float GetHealth() {
        return healthBody_.GetHealth();
    }

    public HealthState GetHealthState() {
        return healthBody_.GetHealthState();
    }

    public void RotateTargetPosition(Vector2 targetPosition) {
        physicsBody_.RotateTargetPosition(targetPosition);
    }

    public void RotateOffsetRotation(float offsetRotation) {
        physicsBody_.RotateOffsetRotation(offsetRotation);
    }

    public void MoveTargetPosition(Vector2 targetPosition, bool crawl = false) {
        physicsBody_.MoveTargetPosition(targetPosition, crawl);
    }

    public void MoveWASD(bool D, bool A, bool W, bool S, bool crawl = false) {
        physicsBody_.MoveWASD(D, A, W, S, crawl);
    }

    public void DashWASD(bool D, bool A, bool W, bool S) {
        physicsBody_.DashWASD(D, A, W, S);
    }

    public float GetRadius() {
        return physicsBody_.GetRadius();
    }

    public Handleable HandleItem(int numNextEei) {
        return itemHandlerBody_.HandleItem(numNextEei);
    }

    public void EquipItem(EquipableItem equipableItem, int numNextEei = 0) {
        itemHandlerBody_.EquipItem(equipableItem, numNextEei);
    }

    public void UnequipItem(int eei) {
        itemHandlerBody_.UnequipItem(eei);
    }

    public void PocketItem(int eeiHand) {
        itemHandlerBody_.PocketItem(eeiHand);
    }

    public int GetEquipableClassEei(EquipableClass equipableClass, int numNextEei) {
        return itemHandlerBody_.GetEquipableClassEei(equipableClass, numNextEei);
    }

    public void CreditAccountable(Accountable accountable) {
        itemHandlerBody_.CreditAccountable(accountable);
    }

    public void DebitAccountable(int fci) {
        itemHandlerBody_.DebitAccountable(fci);
    }

    public EquipableClass[] GetEquipmentEquipableClassArray() {
        return itemHandlerBody_.GetEquipmentEquipableClassArray();
    }

    public IEquipable[] GetEquipmentEquipableArray() {
        return itemHandlerBody_.GetEquipmentEquipableArray();
    }

    public float[] GetFinanceQuantityArray() {
        return itemHandlerBody_.GetFinanceQuantityArray();
    }
}
