using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipper : IComponent {
	void EquipEquipable(IEquipable equipable, int numNextEei = 0);
	IEquipable UnequipEquipable(int eei);
	void PocketEquipable(int eeiHand);
	int GetEquipableClassEei(EquipableClass equipableClass, int numNextEei);
	EquipableClass[] GetEquipmentEquipableClassArray();
	IEquipable[] GetEquipmentEquipableArray();
}
