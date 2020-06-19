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

	private int eeiHand0;
	private int eeiHand1;

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
		eeiHand0 = itemHandlerBody.GetEquipableClassEei(Equipable.EquipableClass.HandItem, 0);
		eeiHand1 = itemHandlerBody.GetEquipableClassEei(Equipable.EquipableClass.HandItem, 1);
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
		circleBody.MoveWASD(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.A));
	}

	/**
     * Activation of abilities. 
     */
	protected override void Fire() {
		if (Input.GetMouseButton(0)) {
			if (itemHandlerBody.GetEquipmentEquipableArray()[eeiHand0] != null) {
				itemHandlerBody.GetEquipmentEquipableArray()[eeiHand0].Activate(circleBody);
			}
		}
		if (Input.GetMouseButton(1)) {
			if (itemHandlerBody.GetEquipmentEquipableArray()[eeiHand1] != null) {
				itemHandlerBody.GetEquipmentEquipableArray()[eeiHand1].Activate(circleBody);
			}
		}
	}

	protected override void HandleItem() {
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			int eei = itemHandlerBody.HandleItem(0);
			UpdateHandPocketEquipmentImage(eei);
		}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			int eei = itemHandlerBody.HandleItem(1);
			UpdateHandPocketEquipmentImage(eei);
		}
	}

	protected override void PocketHandItem() {
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			itemHandlerBody.PocketItem(eeiHand0);
			UpdateHandPocketEquipmentImage(eeiHand0);
		}
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			itemHandlerBody.PocketItem(eeiHand1);
			UpdateHandPocketEquipmentImage(eeiHand1);
		}
	}

	private void UpdateHandPocketEquipmentImage(int eei) {
		HudManager.hudManager.UpdateEquipmentImage(eei);
		int eeiPocket = eei + 1;
		HudManager.hudManager.UpdateEquipmentImage(eeiPocket);
	}

	public void OnEquipmentImageClick(EquipmentImage equipmentImage, PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Middle) {
			DiscardItem(equipmentImage.eei);
			HudManager.hudManager.UpdateEquipmentImage(equipmentImage.eei);
		}
	}

	protected void DiscardItem(int eei) {
		itemHandlerBody.UnequipItem(eei);
	}

	public Body GetBody() {
		return circleBody;
	}
}
