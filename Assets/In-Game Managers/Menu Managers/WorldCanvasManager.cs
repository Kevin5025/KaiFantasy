using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasManager : MonoBehaviour {

	public static WorldCanvasManager worldCanvasManager;

	public GameObject healthRingFillPrefab;  // set in Unity
	public GameObject ringThinTickPrefab;  // set in Unity

	public Body playerAgent;
	public Body debugAgent;  // set in inspector
	public IList<Body> bodyAgentList;
	public IList<GameObject> healthRingFillList;

	void Awake() {
		if (worldCanvasManager == null) {//like a singleton
			worldCanvasManager = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {
		playerAgent = PlayerCircleBodyController.playerCircleBodyController.GetBody();
		bodyAgentList = new List<Body>() { playerAgent, debugAgent };
		healthRingFillList = new List<GameObject>();
		for (int b = 0; b < bodyAgentList.Count; b++) {
			GameObject healthPieFill = Instantiate(healthRingFillPrefab);
			healthRingFillList.Add(healthPieFill);
			healthPieFill.transform.SetParent(transform, false);
			healthPieFill.GetComponent<HealthRingMeter>().body = bodyAgentList[b];
		}
	}
	
	void Update() {
		
	}

	private void FixedUpdate() {
		for (int b = 0; b < bodyAgentList.Count; b++) {
			healthRingFillList[b].transform.position = bodyAgentList[b].transform.position;  // having this is fixedUpdate somehow gives better results
		}
	}
}
