using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : Item {

	protected override void Start() {
		equipableClass = EquipableClass.HeadItem;
	}

}
