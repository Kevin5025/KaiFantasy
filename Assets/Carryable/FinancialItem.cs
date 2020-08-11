using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FinancialClass {
	RedAmmo, YellowAmmo, GreenAmmo, CyanAmmo, BlueAmmo, MagentaAmmo,
	RedMana, YellowMana, GreenMana, CyanMana, BlueMana, MagentaMana,
	Wood, Ore, Water, 
};

public class FinancialItem : Item {

	public static int numAmmunitionTypes;
	public static int numManaTypes;
	public static int numResourceTypes;
	public static int numFinanceTypes;
	public static Dictionary<FinancialClass, Color> financialItemColorDictionary;
	static FinancialItem() {
		numAmmunitionTypes = 6;
		numManaTypes = 6;
		numResourceTypes = 3;
		numFinanceTypes = numAmmunitionTypes + numManaTypes + numResourceTypes;
		financialItemColorDictionary = new Dictionary<FinancialClass, Color> {
			{ FinancialClass.RedAmmo, new Color(1, 0, 0) },
			{ FinancialClass.YellowAmmo, new Color(1, 1, 0) },
			{ FinancialClass.GreenAmmo, new Color(0, 1, 0) },
			{ FinancialClass.CyanAmmo, new Color(0, 1, 1) },
			{ FinancialClass.BlueAmmo, new Color(0, 0, 1) },
			{ FinancialClass.MagentaAmmo, new Color(1, 0, 1) },

			{ FinancialClass.RedMana, new Color(1, 0, 0) },
			{ FinancialClass.YellowMana, new Color(1, 1, 0) },
			{ FinancialClass.GreenMana, new Color(0, 1, 0) },
			{ FinancialClass.CyanMana, new Color(0, 1, 1) },
			{ FinancialClass.BlueMana, new Color(0, 0, 1) },
			{ FinancialClass.MagentaMana, new Color(1, 0, 1) },

			{ FinancialClass.Wood, new Color(139f / 255f, 69f / 255f, 19f / 255f) },
			{ FinancialClass.Ore, new Color(0.5f, 0.5f, 0.5f) },
			{ FinancialClass.Water, new Color(64f / 255f, 164f / 255f, 223f / 255f) }, // https://rgb.to/color/6058/clear-water-blue
		};
	}

	public FinancialClass financialClass;  // set beforehand
	public int quantity;  // set beforehand

	protected virtual void Start() {
		float scale = Mathf.Pow(2, (quantity/30) - 1);
		transform.localScale = new Vector2(scale, scale);
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = financialItemColorDictionary[financialClass];
	}

	public override void BecomeObtained(ItemHandlerBody agent) {
		Destroy(gameObject);
	}

	public override void BecomeUnobtained(ItemHandlerBody agent) {
		Debug.Log("testing");
		throw new System.NotImplementedException();  // TODO
	}
}
