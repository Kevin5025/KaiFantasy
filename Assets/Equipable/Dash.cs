using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : ActivatableEquipable {
	
	protected override void Start() {
		base.Start();
		equipableClass = EquipableClass.Intrinsic;
		cooldownTimeout = 4f;
	}

	public override void Actuate(Body casterAgent, Dictionary<object, object> argumentDictionary = null) {
		base.Actuate(casterAgent);
		bool D = (bool)argumentDictionary['D'];
		bool A = (bool)argumentDictionary['A'];
		bool W = (bool)argumentDictionary['W'];
		bool S = (bool)argumentDictionary['S'];
		Vector2 impulseVector = ((CircleBody)casterAgent).dashImpulse * PlayerCircleBodyController.GetUnitVector(D, A, W, S);
		casterAgent.GetComponent<Rigidbody2D>().AddForce(impulseVector, ForceMode2D.Impulse);
	}

}
