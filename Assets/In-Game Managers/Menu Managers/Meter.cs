using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Meter : MonoBehaviour {  // TODO: extend with bar and not just ring

	public Image[] maskArray_;  // E.G.: red: 0-100 bricked, yellow: 100-200 fibrillating, green: 200-400 capable, blue: 400-1000 overflowing

	protected virtual void Start() {
		
	}


	protected virtual void Update() {
		
	}
}
