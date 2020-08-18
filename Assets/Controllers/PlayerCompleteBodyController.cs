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

	public static PlayerCompleteBodyController playerCompleteBodyController;  // singleton

	protected override void Awake() {
		base.Awake();
		if (playerCompleteBodyController == null) {
			playerCompleteBodyController = this;
		} else {
			Destroy(gameObject);
		}
	}
	
	protected override void Start() {
		base.Start();
		MainCamera.mainCamera.playerTransform = transform;
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
		completeBody.RotateTargetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	/**
     * WASD controls. 
     */
	protected override void Move() {
		bool D = Input.GetKey(KeyCode.D);
		bool A = Input.GetKey(KeyCode.A);
		bool W = Input.GetKey(KeyCode.W);
		bool S = Input.GetKey(KeyCode.S);

		completeBody.MoveWASD(D, A, W, S);
	}

	protected override void Dash() {
		bool space = Input.GetKey(KeyCode.Space);
		if (space) {
			bool D = Input.GetKey(KeyCode.D);
			bool A = Input.GetKey(KeyCode.A);
			bool W = Input.GetKey(KeyCode.W);
			bool S = Input.GetKey(KeyCode.S);

			completeBody.DashWASD(D, A, W, S);
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
		int eeiHand0 = completeBody.GetEquipableClassEei(EquipableClass.HandItem, 0);
		SafeFire(eeiHand0, Input.GetMouseButton(0), Input.GetMouseButtonDown(0));

		int eeiHand1 = completeBody.GetEquipableClassEei(EquipableClass.HandItem, 1);
		SafeFire(eeiHand1, Input.GetMouseButton(1), Input.GetMouseButtonDown(1));
	}

	protected override void Reload() {
		int eeiHand0 = completeBody.GetEquipableClassEei(EquipableClass.HandItem, 0);
		if (Input.GetKey(KeyCode.R) && completeBody.GetEquipmentEquipableArray()[eeiHand0] is Gun) {
			Gun gun = (Gun)completeBody.GetEquipmentEquipableArray()[eeiHand0];
			gun.Reload(completeBody);
			HudCanvasManager.hudCanvasManager.UpdateFinanceText(gun.ammunitionType);
		}
		int eeiHand1 = completeBody.GetEquipableClassEei(EquipableClass.HandItem, 1);
		if (Input.GetKey(KeyCode.R) && completeBody.GetEquipmentEquipableArray()[eeiHand1] is Gun) {
			Gun gun = (Gun)completeBody.GetEquipmentEquipableArray()[eeiHand1];
			gun.Reload(completeBody);
			HudCanvasManager.hudCanvasManager.UpdateFinanceText(gun.ammunitionType);
		}
	}

	protected override void HandleItem() {
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			HandleItem(0);
		}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			HandleItem(1);
		}
	}

	private void HandleItem(int eeiHand) {
		Item minDistanceItem = completeBody.HandleItem(eeiHand);
		if (minDistanceItem == null) {
			return;
		}

		EquipableItem minDistanceEquipableItem = minDistanceItem.GetComponent<EquipableItem>();
		FinancialItem minDistanceFinancialItem = minDistanceItem.GetComponent<FinancialItem>();
		if (minDistanceEquipableItem != null) {
			HudCanvasManager.hudCanvasManager.UpdateHandPocketEquipmentImage(minDistanceEquipableItem.eei);
		} else if (minDistanceFinancialItem != null) {
			HudCanvasManager.hudCanvasManager.UpdateFinanceText((int)minDistanceFinancialItem.financialClass);  // TODO: test
		}
	}

	protected override void PocketHandItem() {
		int eeiHand0 = completeBody.GetEquipableClassEei(EquipableClass.HandItem, 0);
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			completeBody.PocketItem(eeiHand0);
			HudCanvasManager.hudCanvasManager.UpdateHandPocketEquipmentImage(eeiHand0);
		}
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			int eeiHand1 = completeBody.GetEquipableClassEei(EquipableClass.HandItem, 1);
			completeBody.PocketItem(eeiHand1);
			HudCanvasManager.hudCanvasManager.UpdateHandPocketEquipmentImage(eeiHand1);
		}
	}

	public void OnEquipmentImageClick(EquipmentImage equipmentImage, PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Middle) {
			completeBody.UnequipItem(equipmentImage.eei);
			HudCanvasManager.hudCanvasManager.UpdateEquipmentImage(equipmentImage.eei);
		}
	}

	public void OnFinanceImageClick(FinanceImage financeImage, PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Middle) {
			completeBody.DebitItem(financeImage.ffi);
			HudCanvasManager.hudCanvasManager.UpdateFinanceText(financeImage.ffi);
		}
	}

	public CompleteBody GetBody() {
		return completeBody;
	}
}
