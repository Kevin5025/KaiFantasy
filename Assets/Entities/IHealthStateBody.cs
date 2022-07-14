using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthStateBody {
	// bool Activate(IActivatable activatable, Dictionary<object, object> argumentDictionary = null);
	HealthState GetHealthState();
}
