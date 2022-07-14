using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasManager : MonoBehaviour {

	public static WorldCanvasManager worldCanvasManager;

	public GameObject healthRingFillPrefab;  // set in Unity
	public GameObject ringThinTickPrefab;  // set in Unity

	public CompleteBody playerAgent;
	public CompleteBody debugAgent;  // set in inspector
	public IList<CompleteBody> bodyAgentList;
	public IList<GameObject> healthRingFillList;

	void Awake() {
		if (worldCanvasManager == null) {//like a singleton
			worldCanvasManager = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {
		playerAgent = PlayerCompleteBodyController.playerCompleteBodyController.GetBody();
		bodyAgentList = new List<CompleteBody>() { playerAgent, debugAgent };
		healthRingFillList = new List<GameObject>();
		for (int b = 0; b < bodyAgentList.Count; b++) {
			GameObject healthRingFill = Instantiate(healthRingFillPrefab);
			healthRingFillList.Add(healthRingFill);
			healthRingFill.transform.SetParent(transform, false);
			healthRingFill.GetComponent<HealthRingMeter>().body = bodyAgentList[b];
		}
	}
	
	void Update() {
		
	}

	void FixedUpdate() {
		for (int b = 0; b < bodyAgentList.Count; b++) {
			healthRingFillList[b].transform.position = bodyAgentList[b].transform.position;  // having this is fixedUpdate somehow gives better results
		}
	}
}
