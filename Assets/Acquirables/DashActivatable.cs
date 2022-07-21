using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashActivatable : Activatable {
	public override void Actuate(IActivator activator, Dictionary<object, object> argumentDictionary = null) {
		CirclePhysicsBody circlePhysicsBody = (CirclePhysicsBody)activator;
		bool D = (bool)argumentDictionary['D'];
		bool A = (bool)argumentDictionary['A'];
		bool W = (bool)argumentDictionary['W'];
		bool S = (bool)argumentDictionary['S'];
		Vector2 impulseVector = circlePhysicsBody.dashImpulse * PlayerCompositeBodyController.GetUnitVector(D, A, W, S);
		circlePhysicsBody.GetComponent<Rigidbody2D>().AddForce(impulseVector, ForceMode2D.Impulse);
	}
}
