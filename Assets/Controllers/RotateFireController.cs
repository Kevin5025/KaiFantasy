using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFireController : CompleteBodyController {

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
		completeBody.RotateOffsetRotation(rotateFactor * 60f * Time.fixedDeltaTime);
	}

	protected override void Fire() {
		base.Fire();
		int eeiHand0 = completeBody.GetEquipableClassEei(EquipableClass.HandItem, 0);
		SafeFire(eeiHand0, true, true);
	}
}
