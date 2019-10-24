using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : AgentController {

	protected override void FixedUpdate() {
		base.FixedUpdate();
		Rotate();
		Move();
		Fire();
	}
}
