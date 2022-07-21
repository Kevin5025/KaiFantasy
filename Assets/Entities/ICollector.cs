using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollector {
	ICollectable CollectCollectable(int numNextEei);
	void CreditAccountable(Accountable accountable);
	Accountable DebitAccountable(int fci);
	float[] GetAccountQuantityArray();
}
