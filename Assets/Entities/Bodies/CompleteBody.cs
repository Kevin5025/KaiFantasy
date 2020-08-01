using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

/**
 * This is anything that can be destroyed or killed by taking damage, unless invincible. 
 */
public class CompleteBody : SpriteBody, IHealthBody, IPhysicsBody, IItemHandlerBody, IActivator {

	protected IPhysicsBody physicsBody;  // set in inspector
	protected IHealthBody healthBody;  // set in inspector
	protected IItemHandlerBody itemHandlerBody;  // set in inspector
	protected IActivator activator;  // set in inspector

	public HealthState spriteHealthState;

	public Vector2 headPosition;
	public float viscosity;
	
	protected float brickedColorAlpha;
	protected float fibrillatingColorAlpha;
	protected float capableColorAlpha;
	protected Color brickedColor;
	protected Color fibrillatingColor;
	protected Color capableColor;

	protected override void Start () {
		base.Start();
		healthBody = GetComponent<HealthBody>();
		physicsBody = GetComponent<CirclePhysicsBody>();
		itemHandlerBody = GetComponent<ItemHandlerBody>();
		activator = GetComponent<Activator>();

		headPosition = new Vector2(0, 0.6f * GetRadius());  // 0.297
		viscosity = 5f;

		brickedColorAlpha = 0.25f;
		fibrillatingColorAlpha = 0.5f;
		capableColorAlpha = 1f;
		brickedColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, brickedColorAlpha);
		fibrillatingColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fibrillatingColorAlpha);
		capableColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, capableColorAlpha);
	}

	protected override void Update() {
		base.Update();

		if (GetHealthState() != spriteHealthState) {
			HealthEffectUpdate(GetHealthState());
			spriteHealthState = GetHealthState();
		}
	}
	
	protected override void FixedUpdate () {
		base.FixedUpdate();
	}

	protected virtual void HealthEffectUpdate(HealthState healthState) {
		if (healthState == HealthState.Disintegrated) {
			Disintegrate();
		} else if (healthState == HealthState.Bricked) {
			// float fadeBrickedColorAlpha = LinearRangeAnalogy(health, healthStateUpperThresholdList[(int)HealthState.Disintegrated], healthStateUpperThresholdList[(int)HealthState.Bricked], fadeDisintegratedUpperColorAlpha, fadeBrickedUpperColorAlpha);
			spriteRenderer.color = brickedColor;
		} else if (healthState == HealthState.Fibrillating) {
			spriteRenderer.color = fibrillatingColor;
		} else if ((int)healthState >= (int)HealthState.Capable) {
			spriteRenderer.color = capableColor;
		}
	}

	protected override int GetTeamLayer() {
		return LayersManager.layersManager.GetTeamEntityLayer(GetAffinity());
	}





	public void RotateTargetPosition(Vector2 targetPosition) {
		physicsBody.RotateTargetPosition(targetPosition);
	}

	public void RotateOffsetRotation(float offsetRotation) {
		physicsBody.RotateOffsetRotation(offsetRotation);
	}

	public void MoveTargetPosition(Vector2 targetPosition, bool crawl = false) {
		physicsBody.MoveTargetPosition(targetPosition, crawl);
	}

	public void MoveWASD(bool D, bool A, bool W, bool S, bool crawl = false) {
		physicsBody.MoveWASD(D, A, W, S, crawl);
	}

	public void DashWASD(bool D, bool A, bool W, bool S) {
		physicsBody.DashWASD(D, A, W, S);
	}

	public float GetRadius() {
		return physicsBody.GetRadius();
	}

	public float TakeDamage(CompleteBody casterAgent, float damage) {
		return healthBody.TakeDamage(casterAgent, damage);
	}

	public HealthState GetHealthState() {
		return healthBody.GetHealthState();
	}

	public Item HandleItem(int numNextEei) {
		return itemHandlerBody.HandleItem(numNextEei);
	}

	public void EquipItem(EquipableItem equipableItem) {
		itemHandlerBody.EquipItem(equipableItem);
	}

	public void UnequipItem(int eei) {
		itemHandlerBody.UnequipItem(eei);
	}

	public void PocketItem(int eeiHand) {
		itemHandlerBody.PocketItem(eeiHand);
	}

	public int GetEquipableClassEei(EquipableClass equipableClass, int numNextEei) {
		return itemHandlerBody.GetEquipableClassEei(equipableClass, numNextEei);
	}

	public void CreditItem(FinancialItem financialItem) {
		itemHandlerBody.CreditItem(financialItem);
	}

	public void DebitItem(int fci) {
		itemHandlerBody.DebitItem(fci);
	}

	public EquipableClass[] GetEquipmentEquipableClassArray() {
		return itemHandlerBody.GetEquipmentEquipableClassArray();
	}

	public IEquipable[] GetEquipmentEquipableArray() {
		return itemHandlerBody.GetEquipmentEquipableArray();
	}

	public int[] GetFinanceCountArray() {
		return itemHandlerBody.GetFinanceCountArray();
	}

	public void SetHealthState(HealthState healthState) {
		activator.SetHealthState(healthState);
	}

	public float[] GetHealthStateUpperThresholdList() {
		return healthBody.GetHealthStateUpperThresholdList();
	}

	public float GetHealth() {
		return healthBody.GetHealth();
	}
}
