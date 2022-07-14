using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRingMeter : Meter {

	public CompleteBody body;
	public IList<GameObject> healthRingTickList;

	protected override void Start() {
		base.Start();
		healthRingTickList = new List<GameObject>();
	}

	protected override void Update() {
		float totalCapacity = body.GetHealthStateUpperThresholdList()[(int)HealthState.Overflowing];
		updateHealthRingMasks(totalCapacity);
		updateHealthRingTicks(totalCapacity);
	}

	private void updateHealthRingMasks(float totalCapacity) {
		float cumulativeCapacity = 0;  // cumulative for consecutive ring start position
		for (int m = 0; m < maskArray.Length; m++) {
			float healthStateCapacity = body.GetHealthStateUpperThresholdList()[m + 1] - body.GetHealthStateUpperThresholdList()[m];
			float healthStateFill = Mathf.Min(body.GetHealth(), body.GetHealthStateUpperThresholdList()[m + 1]) - body.GetHealthStateUpperThresholdList()[m];

			maskArray[m].transform.localEulerAngles = new Vector3(0, 0, 360f * cumulativeCapacity / totalCapacity);
			maskArray[m].fillAmount = healthStateFill / totalCapacity;

			cumulativeCapacity += healthStateCapacity;
		}
	}

	private void updateHealthRingTicks(float totalCapacity) {
		float numTicks = 0.01f * totalCapacity;  // one tick per 100 health
		for (int t = 0; t < numTicks; t++) {
			if (healthRingTickList.Count <= t) {
				healthRingTickList.Add(Instantiate(WorldCanvasManager.worldCanvasManager.ringThinTickPrefab));
				healthRingTickList[t].transform.SetParent(transform, false);
			}
			healthRingTickList[t].transform.localEulerAngles = new Vector3(0, 0, 360f * t / numTicks);
		}
		for (int t2 = healthRingTickList.Count - 1; t2 >= numTicks; t2--) {  // because Unity's Csharp doesn't seem to have RemoveRange
			Destroy(healthRingTickList[t2]);
			healthRingTickList.RemoveAt(t2);  // O(1) as per https://stackoverflow.com/questions/5396254/c-sharp-list-remove-from-end-really-on
		}
	}
}
