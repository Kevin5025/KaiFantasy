using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Abstracted for easy access later for abstract behaviors. 
 */
public class AgentController : MonoBehaviour {

	public CircleAgent agent;

	protected virtual void Awake() { }

	protected virtual void Start() {
		agent = GetComponent<CircleAgent>();
	}

	protected virtual void Update() { }

	protected virtual void FixedUpdate() { }
}
