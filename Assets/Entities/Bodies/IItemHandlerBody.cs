
public interface IItemHandlerBody {
	
	Item HandleItem(int numNextEei);
	void EquipItem(EquipableItem equipableItem, int numNextEei = 0);
	void UnequipItem(int eei);
	void PocketItem(int eeiHand);
	int GetEquipableClassEei(EquipableClass equipableClass, int numNextEei);
	void CreditItem(FinancialItem financialItem);
	void DebitItem(int fci);
	EquipableClass[] GetEquipmentEquipableClassArray();
	IEquipable[] GetEquipmentEquipableArray();
	int[] GetFinanceCountArray();

}
