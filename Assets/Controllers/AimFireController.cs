﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFireController : RotateFireController {

	protected override void Rotate() {
		base.Rotate();
		if (primeAdversary != null) {
			completeBody_.RotateTargetPosition(primeAdversary.GetTransform().position);
		} else {
			Spin();
		}
	}

}
