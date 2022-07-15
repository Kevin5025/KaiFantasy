
using UnityEngine;

public interface IHandleable
{
	void BecomeHandled(Transform originTransform);
	void BecomeUnhandled(Transform originTransform);
}
