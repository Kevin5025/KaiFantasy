
public interface IItemHandlerBody {
	
	Handleable HandleItem(int numNextEei);
	void EquipItem(EquipableItem equipableItem, int numNextEei = 0);
	void UnequipItem(int eei);
	void PocketItem(int eeiHand);
	int GetEquipableClassEei(EquipableClass equipableClass, int numNextEei);
	void CreditAccountable(Accountable accountable);
	void DebitAccountable(int fci);
	EquipableClass[] GetEquipmentEquipableClassArray();
	IEquipable[] GetEquipmentEquipableArray();
	float[] GetFinanceQuantityArray();

}
