using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFireController : RotateFireController {

	protected override void Rotate() {
		base.Rotate();
		if (primeAdversary != null) {
			circleBody.RotateTargetPosition(primeAdversary.transform.position);
		} else {
			Spin();
		}
	}

}
