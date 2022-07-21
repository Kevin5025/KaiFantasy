using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearchDemoController : AimFireController {

	protected override void FixedUpdate() {
		base.FixedUpdate();
		UpdateNextNodePosition();
	}

	protected override void Move() {
		base.Move();
		if (!IsAtNextNodePosition()) {
			compositeBody_.MoveTargetPosition(nextNodePosition);
		}
	}

	protected override void ManualDebug() {
		base.ManualDebug();
		if (Input.GetKeyDown(KeyCode.Alpha8)) {
			StartCoroutine(FindPathAStarSearch(PlayerCompositeBodyController.playerCompositeBodyController_.transform.position));
		}
	}

}
