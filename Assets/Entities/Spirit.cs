using UnityEngine;

/**
 * This is anything that has a team affiliation. 
 */
public abstract class Spirit : MonoBehaviour {

	public enum Affinity { RED, GREEN, BLUE, YELLOW, MAGENTA, CYAN, BROWN, NONE };//TODO: GREY for zombies?
	public Affinity affinity;
	public Color color;

	protected virtual void Awake () {

	}

	/**
     * Assign sprite color based on team. 
     */
	protected virtual void Start () {
		color = GetTeamColor(affinity);
	}
	
	protected virtual void Update () {

	}

	protected virtual void FixedUpdate () {

	}

	public static Color GetTeamColor(Affinity affinity) {
		if (affinity == Affinity.RED) {
			return new Color(1, 0, 0);
		} else if (affinity == Affinity.GREEN) {
			return new Color(0, 1, 0);
		} else if (affinity == Affinity.BLUE) {
			return new Color(0, 0, 1);
		} else if (affinity == Affinity.YELLOW) {
			return new Color(1, 1, 0);
		} else if (affinity == Affinity.MAGENTA) {
			return new Color(1, 0, 1);
		} else if (affinity == Affinity.CYAN) {
			return new Color(0, 1, 1);
		} else if (affinity == Affinity.BROWN) {//saddle brown = (139,69,19)/256
			return new Color(139f / 255f, 69f / 255f, 19f / 255f);
		} else {
			return new Color(1, 1, 1);
		}
	}
}
