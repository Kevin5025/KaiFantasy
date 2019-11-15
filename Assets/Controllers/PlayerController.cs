using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Allows humans to interface their character. 
 */
public class PlayerController : AgentController {

	public static PlayerController playerController;

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
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
		ManualDebug();
		Rotate();
		Move();
		Fire();
	}

	/**
	 * For Debugging In Game
	 */
	protected virtual void ManualDebug() {
		if (Input.GetKeyDown(KeyCode.BackQuote)) {
			Debug.Log("ManualDebug");
			Collider2D collider = GetComponent<Collider2D>();
			collider.enabled = !collider.enabled;
		}
		if (Input.GetKeyDown(KeyCode.O) && primeAdversary != null) {
			StartCoroutine(FindPathAStarSearch(primeAdversary.transform.position));
		}
		if (Input.GetKeyDown(KeyCode.I)) {
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
		agent.Move(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.D), Input.GetKey(KeyCode.A));
	}

	/**
     * Activation of abilities. 
     */
	protected override void Fire() {
		if (Input.GetMouseButton(0)) {
			if (agent.equipmentEquipableArray[0] != null) {
				agent.equipmentEquipableArray[0].Activate(agent);
			}
		}
		if (Input.GetMouseButton(1)) {
			if (agent.equipmentEquipableArray[1] != null) {
				agent.equipmentEquipableArray[1].Activate(agent);
			}
		}
	}
}
