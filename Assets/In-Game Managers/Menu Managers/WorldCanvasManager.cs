using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasManager : MonoBehaviour {

	public static WorldCanvasManager worldCanvasManager_;

	public GameObject healthRingFillPrefab_;
	public GameObject ringThinTickPrefab_;

	public CompositeBody playerAgent_;
	public CompositeBody debugAgent_;
	public IList<CompositeBody> bodyAgentList;
	public IList<GameObject> healthRingFillList;

	void Awake() {
		if (worldCanvasManager_ == null) {//like a singleton
			worldCanvasManager_ = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {
		playerAgent_ = PlayerCompositeBodyController.playerCompositeBodyController_.GetBody();
		bodyAgentList = new List<CompositeBody>() { playerAgent_, debugAgent_ };
		healthRingFillList = new List<GameObject>();
		for (int b = 0; b < bodyAgentList.Count; b++) {
			GameObject healthRingFill = Instantiate(healthRingFillPrefab_);
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
