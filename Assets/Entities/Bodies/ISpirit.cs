
using UnityEngine;

public enum Affinity { RED, GREEN, BLUE, YELLOW, MAGENTA, CYAN, BROWN };//TODO: GREY for zombies?

public interface ISpirit {
	Affinity GetAffinity();
	void SetAffinity(Affinity affinity);
	Transform GetTransform();
}
