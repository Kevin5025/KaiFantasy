using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Allows humans to interface their character. 
 * TODO: extend into Keyboard Controller and XBox Controller
 */
public class PlayerCircleBodyController : CircleBodyController {

	public static PlayerCircleBodyController playerCircleBodyController;  // singleton

	protected virtual void Awake() {
		if (playerCircleBodyController == null) {
			playerCircleBodyController = this;
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
			StartCoroutine(FindPathAStarSearch(primeAdversary.transform.position));
		}
		if (Input.GetKeyDown(KeyCode.Alpha0)) {
			StartCoroutine(ErasePathNodes());
		}
	}

	/**
     * Character rotates to face wherever the mouse is. 
     */
	protected override void Rotate() {
		circleBody.RotateTargetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	/**
     * WASD controls. 
     */
	protected override void Move() {
		bool D = Input.GetKey(KeyCode.D);
		bool A = Input.GetKey(KeyCode.A);
		bool W = Input.GetKey(KeyCode.W);
		bool S = Input.GetKey(KeyCode.S);

		circleBody.MoveWASD(D, A, W, S);
	}

	protected override void Dash() {
		bool space = Input.GetKey(KeyCode.Space);
		if (space) {
			bool D = Input.GetKey(KeyCode.D);
			bool A = Input.GetKey(KeyCode.A);
			bool W = Input.GetKey(KeyCode.W);
			bool S = Input.GetKey(KeyCode.S);

			circleBody.DashWASD(D, A, W, S);
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
		if (Input.GetMouseButton(0)) {
			int eeiHand0 = itemHandlerBody.GetEquipableClassEei(Equipable.EquipableClass.HandItem, 0);
			if (itemHandlerBody.GetEquipmentEquipableArray()[eeiHand0] != null) {
				itemHandlerBody.GetEquipmentEquipableArray()[eeiHand0].Activate(circleBody);
			}
		}
		if (Input.GetMouseButton(1)) {
			int eeiHand1 = itemHandlerBody.GetEquipableClassEei(Equipable.EquipableClass.HandItem, 1);
			if (itemHandlerBody.GetEquipmentEquipableArray()[eeiHand1] != null) {
				itemHandlerBody.GetEquipmentEquipableArray()[eeiHand1].Activate(circleBody);
			}
		}
	}

	protected override void HandleItem() {
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			int eei = itemHandlerBody.HandleItem(0);
			HudManager.hudManager.UpdateHandPocketEquipmentImage(eei);
		}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			int eei = itemHandlerBody.HandleItem(1);
			HudManager.hudManager.UpdateHandPocketEquipmentImage(eei);
		}
	}

	protected override void PocketHandItem() {
		int eeiHand0 = itemHandlerBody.GetEquipableClassEei(Equipable.EquipableClass.HandItem, 0);
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			itemHandlerBody.PocketItem(eeiHand0);
			HudManager.hudManager.UpdateHandPocketEquipmentImage(eeiHand0);
		}
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			int eeiHand1 = itemHandlerBody.GetEquipableClassEei(Equipable.EquipableClass.HandItem, 1);
			itemHandlerBody.PocketItem(eeiHand1);
			HudManager.hudManager.UpdateHandPocketEquipmentImage(eeiHand1);
		}
	}

	public void OnEquipmentImageClick(EquipmentImage equipmentImage, PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Middle) {
			itemHandlerBody.UnequipItem(equipmentImage.eei);
			HudManager.hudManager.UpdateEquipmentImage(equipmentImage.eei);
		}
	}

	public Body GetBody() {
		return circleBody;
	}
}
