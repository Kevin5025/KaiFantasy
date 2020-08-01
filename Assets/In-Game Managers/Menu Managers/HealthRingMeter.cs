using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRingMeter : Meter {

	public CompleteBody body;
	public IList<GameObject> healthRingTickList;

	protected override void Start() {
		base.Start();
		capacityArray = new float[maskArray.Length];
		sizeArray = new float[maskArray.Length];
		healthRingTickList = new List<GameObject>();
	}

	protected override void Update() {
		capacity = body.GetHealthStateUpperThresholdList()[(int)HealthState.Capable];
		float cumulativeCapacity = 0;
		for (int m=0; m<maskArray.Length; m++) {
			maskArray[m].transform.localEulerAngles = new Vector3(0, 0, 360f * cumulativeCapacity / capacity);

			capacityArray[m] = body.GetHealthStateUpperThresholdList()[m + 1] - body.GetHealthStateUpperThresholdList()[m];
			cumulativeCapacity += capacityArray[m];

			sizeArray[m] = Mathf.Min(body.GetHealth(), body.GetHealthStateUpperThresholdList()[m + 1]) - body.GetHealthStateUpperThresholdList()[m];
			maskArray[m].fillAmount = sizeArray[m] / capacity;  // conveniently, cannot be negative
		}
		int t = 0;
		float numTicks = 0.01f * capacity;
		for (; t<numTicks; t++) {  // one tick per 100 health
			if (healthRingTickList.Count <= t) {
				healthRingTickList.Add(Instantiate(WorldCanvasManager.worldCanvasManager.ringThinTickPrefab));
				healthRingTickList[t].transform.SetParent(transform, false);
			}
			healthRingTickList[t].transform.localEulerAngles = new Vector3(0, 0, 360f * t / numTicks);
		}
		for (int t2=healthRingTickList.Count-1; t2>=t; t2--) {  // because Unity's Csharp doesn't seem to have RemoveRange
			Destroy(healthRingTickList[t2]);
			healthRingTickList.RemoveAt(t2);
		}
	}
}
