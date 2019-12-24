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
		selectedEei = 4;
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
		if (Input.GetMouseButton(4)) {
			if (agent.equipmentEquipableArray[0] != null) {
				agent.equipmentEquipableArray[0].Activate(agent);
			}
		}
		if (Input.GetMouseButton(6)) {
			if (agent.equipmentEquipableArray[1] != null) {
				agent.equipmentEquipableArray[1].Activate(agent);
			}
		}
	}

	protected override void HandleItem() {
		base.HandleItem();
		if (Input.GetKeyDown(KeyCode.F)) {
			agent.HandleItem(selectedEei);
		}
		//if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F)) {
		//	Debug.Log("F");
		//} else if (Input.GetKeyDown(KeyCode.F)) {
		//	Debug.Log("f");
		//}
	}

	public CircleAgent GetAgent() {
		return agent;
	}
}
