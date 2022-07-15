using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudCanvasManager : MonoBehaviour {

	public static HudCanvasManager hudCanvasManager_;

	public CompleteBody playerAgent;

	public GameObject equipmentPanel_;
	public GameObject[] equipmentSlotArray;  // filled in UpdateEquipmentMenu
	public GameObject[] equipmentImageArray;
	public GameObject equipmentSlotPrefab_;

	public GameObject financePanel_;
	public GameObject[] financeSlotArray;
	public GameObject[] financeImageArray;
	public GameObject[] financeTextArray;
	public GameObject financeSlotPrefab_;
	public Sprite[] financeSpriteArray;

	public Color pocketColor;

	void Awake() {
		if (hudCanvasManager_ == null) {//like a singleton
			//DontDestroyOnLoad (gameObject);
			hudCanvasManager_ = this;
		} else {
			Destroy(gameObject);
		}

		financeSpriteArray = new Sprite[] {
			PrefabReferences.prefabReferences_.squareMediumX_,
			PrefabReferences.prefabReferences_.squareMediumX_,
			PrefabReferences.prefabReferences_.squareMediumX_,
			PrefabReferences.prefabReferences_.squareMediumX_,
			PrefabReferences.prefabReferences_.squareMediumX_,
			PrefabReferences.prefabReferences_.squareMediumX_,

			PrefabReferences.prefabReferences_.circleMediumY_,
			PrefabReferences.prefabReferences_.circleMediumY_,
			PrefabReferences.prefabReferences_.circleMediumY_,
			PrefabReferences.prefabReferences_.circleMediumY_,
			PrefabReferences.prefabReferences_.circleMediumY_,
			PrefabReferences.prefabReferences_.circleMediumY_,

			PrefabReferences.prefabReferences_.squareMediumYL_,  // square = discrete
			PrefabReferences.prefabReferences_.squareMediumYL_,
			PrefabReferences.prefabReferences_.circleMediumYL_,  // circle = continuous
		};
	}

	// Start is called before the first frame update
	void Start() {
		playerAgent = PlayerCompleteBodyController.playerCompleteBodyController_.GetBody();

		InitializeEquipmentPanel();
		InitializeFinancePanel();
	}

	// Update is called once per frame
	void Update() {
		
	}

	void FixedUpdate() {
		
	}

	protected void InitializeFinancePanel() {
		financeSlotArray = new GameObject[Accountable.numFinanceTypes];
		financeImageArray = new GameObject[Accountable.numFinanceTypes];
		financeTextArray = new GameObject[Accountable.numFinanceTypes];
		for (int fi = 0; fi < Accountable.numFinanceTypes; fi++) {
			int fMod3 = fi % 3;
			int fDiv3 = fi / 3;
			int fDiv6 = fi / 6;

			financeSlotArray[fi] = Instantiate(financeSlotPrefab_);
			financeSlotArray[fi].transform.SetParent(financePanel_.transform);
			financeSlotArray[fi].transform.localScale = Vector3.one;
			financeSlotArray[fi].GetComponent<RectTransform>().anchoredPosition = new Vector2(-32f + 32f * fMod3, 47 - 18f * fDiv3 - 2f * fDiv6);

			financeImageArray[fi] = financeSlotArray[fi].transform.GetChild(0).gameObject;
			financeImageArray[fi].GetComponent<Image>().sprite = financeSpriteArray[fi];
			financeImageArray[fi].GetComponent<Image>().color = Accountable.accountableClassColorDictionary[(AccountableClass)fi];
			financeImageArray[fi].GetComponent<FinanceImage>().ffi = fi;

			financeTextArray[fi] = financeSlotArray[fi].transform.GetChild(1).gameObject;
		}
		UpdateAllFinanceText();
	}

	public void UpdateAllFinanceText() {
		for (int ft = 0; ft < financeTextArray.Length; ft++) {
			UpdateFinanceText(ft);
		}
	}

	public void UpdateFinanceText(int ffi) {
		bool ftInArrayBounds = ffi > -1 && ffi < playerAgent.GetEquipmentEquipableArray().Length;
		if (ftInArrayBounds) {
			financeTextArray[ffi].GetComponent<Text>().text = playerAgent.GetFinanceQuantityArray()[ffi].ToString();  // TODO: floating point, etc.
		}
	}

	protected void InitializeEquipmentPanel() {
		int numEquipmentSlots = playerAgent.GetEquipmentEquipableClassArray().Length;
		equipmentSlotArray = new GameObject[numEquipmentSlots];
		equipmentImageArray = new GameObject[numEquipmentSlots];
		for (int eei = 0; eei < numEquipmentSlots; eei++) {
			int eMod2 = eei % 2;
			int eDiv2 = eei / 2;

			equipmentSlotArray[eei] = Instantiate(equipmentSlotPrefab_);
			equipmentSlotArray[eei].transform.SetParent(equipmentPanel_.transform);
			equipmentSlotArray[eei].transform.localScale = Vector3.one;
			equipmentSlotArray[eei].GetComponent<RectTransform>().anchoredPosition = new Vector2(-24f + 48f * eMod2, 216f - 48f * eDiv2);

			equipmentImageArray[eei] = equipmentSlotArray[eei].transform.GetChild(0).gameObject;
			equipmentImageArray[eei].GetComponent<Image>().color = Equipable.equipableColorDictionary[playerAgent.GetEquipmentEquipableClassArray()[eei]];
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
			equipmentImageArray[eei].GetComponent<Image>().color = Equipable.equipableColorDictionary[playerAgent.GetEquipmentEquipableClassArray()[eei]];
		}
	}

}
