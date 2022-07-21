
using UnityEngine;

public enum EquipableClass { HandItem, PocketItem, HeadItem, BodyItem, Ability, LargeVassal, SmallVassal, AccessoryItem, Idea };

public interface IEquipable : IComponent {

	EquipableClass GetEquipableClass();
	void SetEquipableClass(EquipableClass equipableClass);
	int getEei();
	void setEei(int eei);
	SpriteRenderer GetComponentSpriteRenderer();

}
