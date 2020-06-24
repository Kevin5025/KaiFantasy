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
		int eeiHand0 = itemHandlerBody.GetEquipableClassEei(Equipable.EquipableClass.HandItem, 0);
		if (itemHandlerBody.GetEquipmentEquipableArray()[eeiHand0] != null) {
			itemHandlerBody.GetEquipmentEquipableArray()[eeiHand0].Activate(circleBody);
		}
	}
}
