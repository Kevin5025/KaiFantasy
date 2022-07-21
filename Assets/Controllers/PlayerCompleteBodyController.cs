using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Allows humans to interface their character. 
 * TODO: extend into Keyboard Controller and XBox Controller
 */
public class PlayerCompleteBodyController : CompleteBodyController {

	public static PlayerCompleteBodyController playerCompleteBodyController_;  // singleton

	protected override void Awake() {
		base.Awake();
		if (playerCompleteBodyController_ == null) {
			playerCompleteBodyController_ = this;
		} else {
			Destroy(gameObject);
		}
	}
	
	protected override void Start() {
		base.Start();
		MainCamera.mainCamera_.playerTransform_ = transform;
	}

	/**
	 * For Debugging In Game
	 */
	protected override void ManualDebug() {
		if (Input.GetKeyDown(KeyCode.BackQuote)) {
			Debug.Log("ManualDebug");
			Collider2D collider = GetComponent<Collider2D>();
			collider.enabled = !collider.enabled;
		}
		if (Input.GetKeyDown(KeyCode.Alpha9) && primeAdversary != null) {
			StartCoroutine(FindPathAStarSearch(primeAdversary.GetTransform().position));
		}
		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			StartCoroutine(ErasePathNodes());
		}
	}

	/**
     * Character rotates to face wherever the mouse is. 
     */
	protected override void Rotate() {
		completeBody_.RotateTargetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	/**
     * WASD controls. 
     */
	protected override void Move() {
		bool D = Input.GetKey(KeyCode.D);
		bool A = Input.GetKey(KeyCode.A);
		bool W = Input.GetKey(KeyCode.W);
		bool S = Input.GetKey(KeyCode.S);

		completeBody_.MoveWASD(D, A, W, S);
	}

	protected override void Dash() {
		bool space = Input.GetKey(KeyCode.Space);
		if (space) {
			bool D = Input.GetKey(KeyCode.D);
			bool A = Input.GetKey(KeyCode.A);
			bool W = Input.GetKey(KeyCode.W);
			bool S = Input.GetKey(KeyCode.S);

			completeBody_.DashWASD(D, A, W, S);
		}

	}

	public static Vector2 GetUnitVector(bool D, bool A, bool W, bool S) {
		float horizontalDirection = 0;
		horizontalDirection += D ? 1 : 0;
		horizontalDirection += A ? -1 : 0;

		float verticalDirection = 0;
		verticalDirection += W ? 1 : 0;
		verticalDirection += S ? -1 : 0;

		Vector2 unitDirectionVector = new Vector2(horizontalDirection, verticalDirection);
		if (unitDirectionVector.magnitude > 1) {
			unitDirectionVector /= unitDirectionVector.magnitude;
			//double direction = Math.Atan2(verticalDirection, horizontalDirection);
			//float verticalMagnitude = (float)Math.Sin(direction);
			//float horizontalMagnitude = (float)Math.Cos(direction);
		}

		return unitDirectionVector;
	}

	/**
     * Activation of abilities. 
     */
	protected override void Fire() {
		int eeiHand0 = completeBody_.GetEquipableClassEei(EquipableClass.HandItem, 0);
		SafeFire(eeiHand0, Input.GetMouseButton(0), Input.GetMouseButtonDown(0));

		int eeiHand1 = completeBody_.GetEquipableClassEei(EquipableClass.HandItem, 1);
		SafeFire(eeiHand1, Input.GetMouseButton(1), Input.GetMouseButtonDown(1));
	}

	protected override void Reload() {
		for (int numNextEei=0; numNextEei<2; numNextEei++) {
			int eeiHand = completeBody_.GetEquipableClassEei(EquipableClass.HandItem, numNextEei);
			if (Input.GetKey(KeyCode.R) && completeBody_.GetEquipmentEquipableArray()[eeiHand] != null) {
				Gun gun = completeBody_.GetEquipmentEquipableArray()[eeiHand].GetComponent<Gun>();
				if (gun != null) {
					gun.Reload(completeBody_);
					HudCanvasManager.hudCanvasManager_.UpdateFinanceText(gun.ammunitionType_);
				}
			}
		}
	}

	protected override void HandleItem() {
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			HandleItem(0);
		}
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			HandleItem(1);
		}
	}

	private void HandleItem(int eeiHand) {
		ICollectable minDistanceItem = completeBody_.CollectCollectable(eeiHand);
		if (minDistanceItem == null) {
			return;
		}

		Equipable minDistanceEquipableItem = minDistanceItem.GetComponent<Equipable>();
		Accountable minDistanceAccountable = minDistanceItem.GetComponent<Accountable>();
		if (minDistanceEquipableItem != null) {
			HudCanvasManager.hudCanvasManager_.UpdateHandPocketEquipmentImage(minDistanceEquipableItem.eei);
		} else if (minDistanceAccountable != null) {
			HudCanvasManager.hudCanvasManager_.UpdateFinanceText((int)minDistanceAccountable.accountableClass_);
		}
	}

	protected override void PocketHandItem() {
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			int eeiHand0 = completeBody_.GetEquipableClassEei(EquipableClass.HandItem, 0);
			completeBody_.PocketEquipable(eeiHand0);
			HudCanvasManager.hudCanvasManager_.UpdateHandPocketEquipmentImage(eeiHand0);
		}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			int eeiHand1 = completeBody_.GetEquipableClassEei(EquipableClass.HandItem, 1);
			completeBody_.PocketEquipable(eeiHand1);
			HudCanvasManager.hudCanvasManager_.UpdateHandPocketEquipmentImage(eeiHand1);
		}
	}

	public void OnEquipmentImageClick(EquipmentImage equipmentImage, PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Middle) {
			completeBody_.UnequipEquipable(equipmentImage.eei);
			HudCanvasManager.hudCanvasManager_.UpdateEquipmentImage(equipmentImage.eei);
		}
	}

	public void OnFinanceImageClick(FinanceImage financeImage, PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Middle) {
			completeBody_.DebitAccountable(financeImage.ffi);
			HudCanvasManager.hudCanvasManager_.UpdateFinanceText(financeImage.ffi);
		}
	}

	public CompleteBody GetBody() {
		return completeBody_;
	}
}
