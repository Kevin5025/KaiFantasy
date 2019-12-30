﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {

	public static HudManager hudManager;
	public CircleAgent playerAgent;

	public GameObject equipmentPanel;//set in Unity
	public GameObject[] equipmentSlotArray;//filled in UpdateEquipmentMenu
	public GameObject[] equipmentImageArray;
	public GameObject equipmentSlotPrefab;//set in Unity

	public Color pocketColor;

	void Awake() {
		if (hudManager == null) {//like a singleton
			//DontDestroyOnLoad (gameObject);
			hudManager = this;
		} else {
			Destroy(gameObject);
		}
	}

	// Start is called before the first frame update
	void Start() {
		playerAgent = PlayerController.playerController.GetAgent();
		int numEquipmentSlots = playerAgent.equipmentEquipableClassArray.Length;
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
			equipmentImageArray[eei].GetComponent<Image>().color = GetEquipmentSlotArrayColor(playerAgent.equipmentEquipableClassArray[eei]);
			equipmentImageArray[eei].GetComponent<EquipmentImage>().eei = eei;
		}

		pocketColor = new Color(0.75f, 0.75f, 0.75f);
	}

	// Update is called once per frame
	void Update() {

	}

	public void UpdateEquipmentImage(int eei) {
		bool eeiInArrayBounds = eei > -1 && eei < playerAgent.equipmentEquipableArray.Length;
		if (eeiInArrayBounds && playerAgent.equipmentEquipableArray[eei] != null) {
			equipmentImageArray[eei].GetComponent<Image>().sprite = playerAgent.equipmentEquipableArray[eei].GetComponent<SpriteRenderer>().sprite;
			if (playerAgent.equipmentEquipableClassArray[eei] != Equipable.EquipableClass.PocketItem) {
				equipmentImageArray[eei].GetComponent<Image>().color = Color.white;
			} else {
				equipmentImageArray[eei].GetComponent<Image>().color = pocketColor;
			}
		} else if (eeiInArrayBounds && playerAgent.equipmentEquipableArray[eei] == null) {
			equipmentImageArray[eei].GetComponent<Image>().sprite = null;
			equipmentImageArray[eei].GetComponent<Image>().color = GetEquipmentSlotArrayColor(playerAgent.equipmentEquipableClassArray[eei]);
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