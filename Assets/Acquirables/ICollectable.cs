
using UnityEngine;

public interface ICollectable : IComponent {
	void BecomeCollected(Transform originTransform);
	void BecomeUncollected(Transform originTransform);
}
