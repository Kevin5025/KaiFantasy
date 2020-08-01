using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudCanvasManager : MonoBehaviour {

	public static HudCanvasManager hudCanvasManager;

	public CompleteBody playerAgent;

	public GameObject equipmentPanel;  // set in Unity
	public GameObject[] equipmentSlotArray;  // filled in UpdateEquipmentMenu
	public GameObject[] equipmentImageArray;
	public GameObject equipmentSlotPrefab;  // set in Unity

	public GameObject financePanel;  // set in inspector
	public GameObject[] financeSlotArray;
	public GameObject[] financeImageArray;
	public GameObject[] financeTextArray;
	public GameObject financeSlotPrefab;  // set in inspector

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
		playerAgent = PlayerCompleteBodyController.playerCompleteBodyController.GetBody();

		InitializeEquipmentPanel();
		InitializeFinancePanel();
	}

	// Update is called once per frame
	void Update() {
		
	}

	void FixedUpdate() {
		
	}

	protected void InitializeFinancePanel() {
		int numFinanceSlots = playerAgent.GetFinanceCountArray().Length;
		financeSlotArray = new GameObject[numFinanceSlots];
		financeImageArray = new GameObject[numFinanceSlots];
		financeTextArray = new GameObject[numFinanceSlots];
		for (int fi = 0; fi < numFinanceSlots; fi++) {
			int fMod3 = fi % 3;
			int fDiv3 = fi / 3;
			int fDiv6 = fi / 6;

			financeSlotArray[fi] = Instantiate(financeSlotPrefab);
			financeSlotArray[fi].transform.SetParent(financePanel.transform);
			financeSlotArray[fi].transform.localScale = Vector3.one;
			financeSlotArray[fi].GetComponent<RectTransform>().anchoredPosition = new Vector2(-32f + 32f * fMod3, 47 - 18f * fDiv3 - 2f * fDiv6);

			financeImageArray[fi] = financeSlotArray[fi].transform.GetChild(0).gameObject;
			// financeImageArray[fi].GetComponent<Image>().color;
			financeImageArray[fi].GetComponent<FinanceImage>().fi = fi;

			financeTextArray[fi] = financeSlotArray[fi].transform.GetChild(1).gameObject;
		}
		UpdateAllFinanceText();
	}

	public void UpdateAllFinanceText() {
		for (int ft = 0; ft < financeTextArray.Length; ft++) {
			UpdateFinanceText(ft);
		}
	}

	public void UpdateFinanceText(int fti) {
		bool ftInArrayBounds = fti > -1 && fti < playerAgent.GetEquipmentEquipableArray().Length;
		if (ftInArrayBounds) {
			financeTextArray[fti].GetComponent<Text>().text = playerAgent.GetFinanceCountArray()[fti].ToString();  // TODO
		}
	}

	protected void InitializeEquipmentPanel() {
		int numEquipmentSlots = playerAgent.GetEquipmentEquipableClassArray().Length;
		equipmentSlotArray = new GameObject[numEquipmentSlots];
		equipmentImageArray = new GameObject[numEquipmentSlots];
		for (int eei = 0; eei < numEquipmentSlots; eei++) {
			int eMod2 = eei % 2;
			int eDiv2 = eei / 2;

			equipmentSlotArray[eei] = Instantiate(equipmentSlotPrefab);
			equipmentSlotArray[eei].transform.SetParent(equipmentPanel.transform);
			equipmentSlotArray[eei].transform.localScale = Vector3.one;
			equipmentSlotArray[eei].GetComponent<RectTransform>().anchoredPosition = new Vector2(-24f + 48f * eMod2, 216f - 48f * eDiv2);

			equipmentImageArray[eei] = equipmentSlotArray[eei].transform.GetChild(0).gameObject;
			equipmentImageArray[eei].GetComponent<Image>().color = GetEquipmentSlotArrayColor(playerAgent.GetEquipmentEquipableClassArray()[eei]);
			equipmentImageArray[eei].GetComponent<EquipmentImage>().eei = eei;
		}
		pocketColor = new Color(0.75f, 0.75f, 0.75f);
		UpdateAllEquipmentImage();  // TODO: test if this line is needed
	}

	public void UpdateAllEquipmentImage() {
		for (int e = 0; e < equipmentImageArray.Length; e++) {
			UpdateEquipmentImage(e);
		}
	}
	
	public void UpdateHandPocketEquipmentImage(int eei) {
		UpdateEquipmentImage(eei);
		int eeiPocket = eei + 1;
		UpdateEquipmentImage(eeiPocket);
	}

	public void UpdateEquipmentImage(int eei) {
		bool eeiInArrayBounds = eei > -1 && eei < playerAgent.GetEquipmentEquipableArray().Length;
		if (eeiInArrayBounds && playerAgent.GetEquipmentEquipableArray()[eei] != null) {
			equipmentImageArray[eei].GetComponent<Image>().sprite = playerAgent.GetEquipmentEquipableArray()[eei].GetComponentSpriteRenderer().sprite;
			if (playerAgent.GetEquipmentEquipableClassArray()[eei] != EquipableClass.PocketItem) {
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
	public Color GetEquipmentSlotArrayColor(EquipableClass equipableClass) {
		if (equipableClass == EquipableClass.AccessoryItem) {
			return new Color(1f, 0f, 1f);
		} else if (equipableClass == EquipableClass.HandItem) {
			return new Color(1f, 0f, 0f);
		} else if (equipableClass == EquipableClass.PocketItem) {
			return new Color(0.75f, 0f, 0f);
		} else if (equipableClass == EquipableClass.HeadItem) {
			return new Color(1f, 0.5f, 0f);
		} else if (equipableClass == EquipableClass.BodyItem) {
			return new Color(1f, 1f, 0f);
		} else if (equipableClass == EquipableClass.Ability) {
			return new Color(0f, 1f, 0f);
		} else if (equipableClass == EquipableClass.LargeVassal) {
			return new Color(0f, 1f, 1f);
		} else if (equipableClass == EquipableClass.SmallVassal) {
			return new Color(0f, 0f, 1f);
		} else if (equipableClass == EquipableClass.Idea) {
			return new Color(0.5f, 0.5f, 0.5f);
		} else {
			return new Color(1, 1, 1);
		}
	}
}
