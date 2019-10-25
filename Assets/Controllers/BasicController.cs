using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : AgentController {

	public float rotateFactor;

	protected override void Start() {
		base.Start();
		rotateFactor = MyStaticLibrary.maxMagnitudeFloat(personalityUniform);
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
		Rotate();
		Fire();
	}

	protected override void Rotate() {
		base.Rotate();
		agent.RotateOffsetRotation(rotateFactor * 60f * Time.deltaTime);
	}

	protected override void Fire() {
		base.Fire();
		if (agent.equipmentEquipableArray[0] != null) {
			agent.equipmentEquipableArray[0].Activate(agent);
		}
	}
}
