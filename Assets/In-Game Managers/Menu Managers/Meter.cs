using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Meter : MonoBehaviour {

	public float capacity;
	public Image[] maskArray;  // set in inspector
	public float[] capacityArray;  // set in child class
	public float[] sizeArray;  // set in child class

	protected virtual void Start() {
		
	}


	protected virtual void Update() {
		
	}
}
