using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFireController : CircleBodyController {

	public float rotateFactor;

	protected override void Start() {
		base.Start();
		rotateFactor = MyStaticLibrary.maxMagnitudeFloat(personalityUniform);
	}

	protected override void Rotate() {
		base.Rotate();
		Spin();
	}

	protected virtual void Spin() {
		circleBody.RotateOffsetRotation(rotateFactor * 60f * Time.fixedDeltaTime);
	}

	protected override void Fire() {
		base.Fire();
		if (itemHandlerBody.GetEquipmentEquipableArray()[0] != null) {
			itemHandlerBody.GetEquipmentEquipableArray()[0].Activate(circleBody);
		}
	}
}
