using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudCanvasManager : MonoBehaviour {

	public static HudCanvasManager hudCanvasManager;

	public Body playerAgent;

	public GameObject equipmentPanel;  //set in Unity
	public GameObject[] equipmentSlotArray;//filled in UpdateEquipmentMenu
	public GameObject[] equipmentImageArray;
	public GameObject equipmentSlotPrefab;  //set in Unity

	public Color pocketColor;

	void Awake() {
		if (hudCanvasManager == null) {//like a singleton
			//DontDestroyOnLoad (gameObject);
			hudCanvasManager = this;
		} else {
			Destroy(gameObject);
		}
	}

	// Start is called before the first frame update
	void Start() {
		playerAgent = PlayerCircleBodyController.playerCircleBodyController.GetBody();

		int numEquipmentSlots = playerAgent.GetEquipmentEquipableClassArray().Length;
		equipmentSlotArray = new GameObject[numEquipmentSlots];
		equipmentImageArray = new GameObject[numEquipmentSlots];
		for (int eei = 0; eei < numEquipmentSlots; eei++) {
			int eMod2 = eei % 2;
			int eDiv2 = eei / 2;

			equipmentSlotArray[eei] = Instantiate(equipmentSlotPrefab);
			equipmentSlotArray[eei].transform.SetParent(equipmentPanel.transform);
			// equipmentSlotArray[e].transform.parent = equipmentPanel.transform;
			equipmentSlotArray[eei].transform.localScale = Vector3.one;
			equipmentSlotArray[eei].GetComponent<RectTransform>().anchoredPosition = new Vector2(-24f + 48f * eMod2, 216f - 48f * eDiv2);
			equipmentImageArray[eei] = equipmentSlotArray[eei].transform.GetChild(0).gameObject;
			equipmentImageArray[eei].GetComponent<Image>().color = GetEquipmentSlotArrayColor(playerAgent.GetEquipmentEquipableClassArray()[eei]);
			equipmentImageArray[eei].GetComponent<EquipmentImage>().eei = eei;
		}
		pocketColor = new Color(0.75f, 0.75f, 0.75f);
		HudCanvasManager.hudCanvasManager.UpdateAllEquipmentImage();  // TODO: test if this line is needed
	}

	// Update is called once per frame
	void Update() {
		
	}

	void FixedUpdate() {
		
	}

	public void UpdateAllEquipmentImage() {
		for (int e = 0; e < equipmentImageArray.Length; e++) {
			UpdateEquipmentImage(e);
		}
	}
	
	public void UpdateHandPocketEquipmentImage(int eei) {
		HudCanvasManager.hudCanvasManager.UpdateEquipmentImage(eei);
		int eeiPocket = eei + 1;
		HudCanvasManager.hudCanvasManager.UpdateEquipmentImage(eeiPocket);
	}

	public void UpdateEquipmentImage(int eei) {
		bool eeiInArrayBounds = eei > -1 && eei < playerAgent.GetEquipmentEquipableArray().Length;
		if (eeiInArrayBounds && playerAgent.GetEquipmentEquipableArray()[eei] != null) {
			equipmentImageArray[eei].GetComponent<Image>().sprite = playerAgent.GetEquipmentEquipableArray()[eei].GetComponent<SpriteRenderer>().sprite;
			if (playerAgent.GetEquipmentEquipableClassArray()[eei] != Equipable.EquipableClass.PocketItem) {
				equipmentImageArray[eei].GetComponent<Image>().color = Color.white;
			} else {
				equipmentImageArray[eei].GetComponent<Image>().color = pocketColor;
			}
		} else if (eeiInArrayBounds && playerAgent.GetEquipmentEquipableArray()[eei] == null) {
			equipmentImageArray[eei].GetComponent<Image>().sprite = null;
			equipmentImageArray[eei].GetComponent<Image>().color = GetEquipmentSlotArrayColor(playerAgent.GetEquipmentEquipableClassArray()[eei]);
		}
	}

	/**
     * returns the correct background color for the given equipable class
     */
	public Color GetEquipmentSlotArrayColor(Equipable.EquipableClass equipableClass) {
		if (equipableClass == Equipable.EquipableClass.AccessoryItem) {
			return new Color(1f, 0f, 1f);
		} else if (equipableClass == Equipable.EquipableClass.HandItem) {
			return new Color(1f, 0f, 0f);
		} else if (equipableClass == Equipable.EquipableClass.PocketItem) {
			return new Color(0.75f, 0f, 0f);
		} else if (equipableClass == Equipable.EquipableClass.HeadItem) {
			return new Color(1f, 0.5f, 0f);
		} else if (equipableClass == Equipable.EquipableClass.BodyItem) {
			return new Color(1f, 1f, 0f);
		} else if (equipableClass == Equipable.EquipableClass.Ability) {
			return new Color(0f, 1f, 0f);
		} else if (equipableClass == Equipable.EquipableClass.LargeVassal) {
			return new Color(0f, 1f, 1f);
		} else if (equipableClass == Equipable.EquipableClass.SmallVassal) {
			return new Color(0f, 0f, 1f);
		} else if (equipableClass == Equipable.EquipableClass.Idea) {
			return new Color(0.5f, 0.5f, 0.5f);
		} else {
			return new Color(1, 1, 1);
		}
	}
}
