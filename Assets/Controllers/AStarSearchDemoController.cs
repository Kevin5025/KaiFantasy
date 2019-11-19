﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearchDemoController : AimFireController {

	protected override void Update() {
		base.Update();
		UpdateNextNodePosition();
	}

	protected override void Move() {
		base.Move();
		if (!IsAtNextNodePosition()) {
			agent.MoveTargetPosition(nextNodePosition);
		}
	}

	protected override void ManualDebug() {
		base.ManualDebug();
		if (Input.GetKeyDown(KeyCode.Alpha8)) {
			StartCoroutine(FindPathAStarSearch(PlayerController.playerController.transform.position));
		}
	}

}
