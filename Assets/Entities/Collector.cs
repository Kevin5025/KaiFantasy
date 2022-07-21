using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: add inventory so that collector no longer extends/implements Equipper
public class Collector : MonoBehaviour, ICollector {

    protected float collectRange;

    public float[] accountQuantityArray;

    protected virtual void Awake() {
        
    }

    protected virtual void Start() {
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
        float circle_radius = Mathf.Sqrt(2 * rb2D.inertia / rb2D.mass);  // I = mr^2
        collectRange = circle_radius;

        accountQuantityArray = new float[Accountable.numFinanceTypes];
        accountQuantityArray[0] = 15;
    }

    protected virtual void Update() {

    }

    public ICollectable CollectCollectable(int numNextIi) {
        ICollectable minDistanceCollectable = GetMinDistanceCollectable();
        if (minDistanceCollectable != null) {
            InventoryCollectable(minDistanceCollectable, numNextIi);
        }
        return minDistanceCollectable;
    }

    // Inventory (verb)
    public void InventoryCollectable(ICollectable collectable, int numNextIi = 0) {
        IEquipable equipable = collectable.GetComponent<Equipable>();
        Accountable accountable = collectable.GetComponent<Accountable>();

        if (equipable != null) {
            IEquipper equipper = GetComponent<Equipper>();  // TODO: consider possibility overridden
            equipper.EquipEquipable(equipable, numNextIi);
        } else if (accountable != null) {
            CreditAccountable(accountable);
        }
    }

    public ICollectable GetMinDistanceCollectable() {
        ICollectable minDistanceCollectable = null;
        Collider2D minDistanceItemCollider = GetMinDistanceItemCollider();
        if (minDistanceItemCollider != null) {
            minDistanceCollectable = minDistanceItemCollider.GetComponent<Collectable>();
        }
        return minDistanceCollectable;
    }

    private Collider2D GetMinDistanceItemCollider() {
        int itemLayerMask = LayersManager.layersManager.allLayerMaskArray[LayersManager.layersManager.itemLayer];
        Collider2D[] itemColliderArray = Physics2D.OverlapCircleAll(transform.position, collectRange, itemLayerMask);

        Collider2D minDistanceItemCollider = null;
        float minDistanceItemColliderDistance = float.MaxValue;
        for (int rh = 0; rh < itemColliderArray.Length; rh++) {
            float itemColliderDistance = MyStaticLibrary.GetDistance(transform.position, itemColliderArray[rh].ClosestPoint(transform.position));
            if (itemColliderDistance < minDistanceItemColliderDistance) {
                minDistanceItemCollider = itemColliderArray[rh];
                minDistanceItemColliderDistance = itemColliderDistance;
            }
        }
        return minDistanceItemCollider;
    }

    public void CreditAccountable(Accountable accountable) {
        int ffi = (int)accountable.accountableClass_;
        accountQuantityArray[ffi] += accountable.quantity_;
        accountable.BecomeCollected(transform);
    }

    public Accountable DebitAccountable(int ffi) {
        Accountable accountable = null;
        if (accountQuantityArray[ffi] >= 1) {
            float quantity = Mathf.Ceil(accountQuantityArray[ffi] / 3f);
            accountQuantityArray[ffi] -= quantity;

            AccountableClass accountableClass = (AccountableClass)ffi;
            accountable = Accountable.InstantiateAccountableGameObject(accountableClass, quantity, transform);
        }
        return accountable;
    }

    public float[] GetAccountQuantityArray() {
        return accountQuantityArray;
    }
}
