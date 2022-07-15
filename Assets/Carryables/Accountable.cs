using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccountableClass {
	RedAmmo, YellowAmmo, GreenAmmo, CyanAmmo, BlueAmmo, MagentaAmmo,
	RedMana, YellowMana, GreenMana, CyanMana, BlueMana, MagentaMana,
	Wood, Ore, Water, 
};

public class Accountable : Handleable {

	public static int numAmmunitionTypes;
	public static int numManaTypes;
	public static int numResourceTypes;
	public static int numFinanceTypes;
	public static Dictionary<AccountableClass, Color> accountableClassColorDictionary;
	static Accountable() {
		numAmmunitionTypes = 6;
		numManaTypes = 6;
		numResourceTypes = 3;
		numFinanceTypes = numAmmunitionTypes + numManaTypes + numResourceTypes;
		accountableClassColorDictionary = new Dictionary<AccountableClass, Color> {
			{ AccountableClass.RedAmmo, new Color(1, 0, 0) },
			{ AccountableClass.YellowAmmo, new Color(1, 1, 0) },
			{ AccountableClass.GreenAmmo, new Color(0, 1, 0) },
			{ AccountableClass.CyanAmmo, new Color(0, 1, 1) },
			{ AccountableClass.BlueAmmo, new Color(0, 0, 1) },
			{ AccountableClass.MagentaAmmo, new Color(1, 0, 1) },

			{ AccountableClass.RedMana, new Color(1, 0, 0) },
			{ AccountableClass.YellowMana, new Color(1, 1, 0) },
			{ AccountableClass.GreenMana, new Color(0, 1, 0) },
			{ AccountableClass.CyanMana, new Color(0, 1, 1) },
			{ AccountableClass.BlueMana, new Color(0, 0, 1) },
			{ AccountableClass.MagentaMana, new Color(1, 0, 1) },

			{ AccountableClass.Wood, new Color(139f / 255f, 69f / 255f, 19f / 255f) },
			{ AccountableClass.Ore, new Color(0.5f, 0.5f, 0.5f) },
			{ AccountableClass.Water, new Color(64f / 255f, 164f / 255f, 223f / 255f) }, // https://rgb.to/color/6058/clear-water-blue
		};
	}

	public AccountableClass accountableClass_;
	public float quantity_;

	protected virtual void Start() {
		while (quantity_ > 30) {
			float newQuantity = Mathf.Ceil(quantity_ / 3f);
			InstantiateAccountableGameObject(accountableClass_, newQuantity, transform);
			quantity_ -= newQuantity;
		}

		float scale = 2f * Mathf.Sqrt(quantity_ / 30f);
		transform.localScale = new Vector2(scale, scale);
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = HudCanvasManager.hudCanvasManager_.financeSpriteArray[(int)accountableClass_];
		spriteRenderer.color = accountableClassColorDictionary[accountableClass_];
	}

	public static Accountable InstantiateAccountableGameObject(AccountableClass accountableClass, float quantity, Transform originTransform) {
		GameObject accountableGameObject = Instantiate(PrefabReferences.prefabReferences_.accountablePrefab_, originTransform.position, originTransform.rotation);
		Accountable accountable = accountableGameObject.GetComponent<Accountable>();
		accountable.accountableClass_ = accountableClass;
		accountable.quantity_ = quantity;
		accountable.BecomeUnhandled(originTransform);
		return accountable;
	}

	public override void BecomeHandled(Transform originTransform) {
		base.BecomeHandled(originTransform);
		Destroy(gameObject);
	}

	public override void BecomeUnhandled(Transform originTransform) {
		base.BecomeUnhandled(originTransform);
		gameObject.SetActive(true);
	}
}
