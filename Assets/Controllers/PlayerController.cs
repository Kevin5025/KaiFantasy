using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Allows humans to interface their character. 
 * TODO: extend into Keyboard Controller and XBox Controller
 */
public class PlayerController : AgentController {

	public static PlayerController playerController;
	private int eeiHand0;
	private int eeiHand1;

	protected override void Awake() {
		base.Awake();
		if (playerController == null) {
			playerController = this;
		} else {
			Destroy(gameObject);
		}
	}
	
	protected override void Start() {
		base.Start();
		MainCamera.mainCamera.playerTransform = transform;
		eeiHand0 = agent.GetEquipableClassEei(Equipable.EquipableClass.HandItem, 0);
		eeiHand1 = agent.GetEquipableClassEei(Equipable.EquipableClass.HandItem, 1);
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
		agent.RotateTargetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	/**
     * WASD controls. 
     */
	protected override void Move() {
		agent.MoveWASD(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.A));
	}

	/**
     * Activation of abilities. 
     */
	protected override void Fire() {
		if (Input.GetMouseButton(0)) {
			if (agent.equipmentEquipableArray[eeiHand0] != null) {
				agent.equipmentEquipableArray[eeiHand0].Activate(agent);
			}
		}
		if (Input.GetMouseButton(1)) {
			if (agent.equipmentEquipableArray[eeiHand1] != null) {
				agent.equipmentEquipableArray[eeiHand1].Activate(agent);
			}
		}
	}

	protected override void HandleItem() {
		base.HandleItem();
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			int eei = agent.HandleItem(0);
			UpdateHandPocketEquipmentImage(eei);
		}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			int eei = agent.HandleItem(1);
			UpdateHandPocketEquipmentImage(eei);
		}
	}

	protected override void PocketHandItem() {
		base.PocketHandItem();
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			agent.PocketItem(eeiHand0);
			UpdateHandPocketEquipmentImage(eeiHand0);
		}
		if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			agent.PocketItem(eeiHand1);
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

	public CircleAgent GetAgent() {
		return agent;
	}
}
