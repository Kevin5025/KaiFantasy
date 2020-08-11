
using UnityEngine;

public enum EquipableClass { HandItem, PocketItem, HeadItem, BodyItem, Ability, LargeVassal, SmallVassal, AccessoryItem, Idea };

public interface IEquipable {

	EquipableClass GetEquipableClass();
	void SetEquipableClass(EquipableClass equipableClass);
	SpriteRenderer GetComponentSpriteRenderer();

}
