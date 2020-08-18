using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem : IHandleable {
	void BecomeObtained();
	void BecomeUnobtained(Transform originTransform);
}
