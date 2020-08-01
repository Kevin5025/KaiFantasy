
using UnityEngine;

public enum EquipableClass { AccessoryItem, HandItem, PocketItem, HeadItem, BodyItem, Ability, LargeVassal, SmallVassal, Idea };

public interface IEquipable {

	EquipableClass GetEquipableClass();
	void SetEquipableClass(EquipableClass equipableClass);
	SpriteRenderer GetComponentSpriteRenderer();

}
