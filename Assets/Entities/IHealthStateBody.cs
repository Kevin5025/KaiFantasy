using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthStateBody {
	// bool Activate(IActivatable activatable_, Dictionary<object, object> argumentDictionary = null);
	HealthState GetHealthState();
}
