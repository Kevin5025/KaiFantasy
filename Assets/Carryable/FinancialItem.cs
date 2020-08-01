using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FinancialClass {
	redAmmo, yellowAmmo, greenAmmo, cyanAmmo, blueAmmo, magentaAmmo,
	redMana, yellowMana, greenMana, cyanMana, blueMana, magentaMana,
	wood, ore, gems, 
};

public class FinancialItem : Item {

	public static int numAmmunitionTypes;
	public static int numManaTypes;
	public static int numResourceTypes;
	static FinancialItem() {
		numAmmunitionTypes = 6;
		numManaTypes = 6;
		numResourceTypes = 3;
	}

	public FinancialClass financialClass;
	public int quantity;

	public override void BecomeObtained(ItemHandlerBody agent) {
		Destroy(gameObject);
	}

	public override void BecomeUnobtained(ItemHandlerBody agent) {
		throw new System.NotImplementedException();  // TODO
	}
}
