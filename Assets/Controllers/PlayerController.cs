using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Allows humans to interface their character. 
 */
public class PlayerController : AgentController {

	public static PlayerController playerController;
	protected static int selectedEei;

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
		selectedEei = 6;
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

		if (Input.GetKeyDown(KeyCode.G)) {
			agent.UnpocketItem(selectedEei);
			HudManager.hudManager.UpdateEquipmentImage(selectedEei);
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
			if (agent.equipmentEquipableArray[2] != null) {
				agent.equipmentEquipableArray[2].Activate(agent);
			}
		}
		if (Input.GetMouseButton(1)) {
			if (agent.equipmentEquipableArray[3] != null) {
				agent.equipmentEquipableArray[3].Activate(agent);
			}
		}
	}

	protected override void HandleItem() {
		base.HandleItem();
		//if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F)) {
		//	agent.HandleItem(-1);
		//}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Q)) {
			int eei = agent.HandleItem(0);
			HudManager.hudManager.UpdateEquipmentImage(eei);
		}
		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) {
			int eei = agent.HandleItem(1);
			HudManager.hudManager.UpdateEquipmentImage(eei);
		}
	}

	public CircleAgent GetAgent() {
		return agent;
	}
}
