using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemHandlerBody {
	
	int HandleItem(int numNextEei);
	void EquipItem(Item equipItem, int eeiHand);
	void UnequipItem(int eei);
	void PocketItem(int eeiHand);
	int GetEquipableClassEei(Equipable.EquipableClass equipableClass, int numNextEei);
	Equipable.EquipableClass[] GetEquipmentEquipableClassArray();
	Equipable[] GetEquipmentEquipableArray();

}
