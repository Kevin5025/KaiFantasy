using UnityEngine;

/**
 * This is anything that has a team affiliation. 
 */
public abstract class Spirit : MonoBehaviour {

	public enum Affinity { RED, GREEN, BLUE, YELLOW, MAGENTA, CYAN, BROWN, NONE };//TODO: GREY for zombies
	public Affinity affinity;

	protected SpriteRenderer spriteRenderer;
	protected float r; protected float g; protected float b;

	protected virtual void Awake () {

	}

	/**
     * Assign sprite color based on team. 
     */
	protected virtual void Start () {
		gameObject.layer = GetTeamLayer();

		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = GetTeamColor(spriteRenderer.color, affinity);
		r = spriteRenderer.color.r; g = spriteRenderer.color.g; b = spriteRenderer.color.b;
	}
	
	protected virtual void Update () {

	}

	protected virtual void FixedUpdate () {

	}

	protected abstract int GetTeamLayer(); 

	/**
	 * Occurs on death / destruction
	 */
	protected virtual void Expire() {
		Destroy(gameObject);
	}

	public Color GetTeamColor (Color color, Affinity affinity) {
		if (affinity == Affinity.RED) {
			return new Color(color.r, 0, 0);
		} else if (affinity == Affinity.GREEN) {
			return new Color(0, color.g, 0);
		} else if (affinity == Affinity.BLUE) {
			return new Color(0, 0, color.b);
		} else if (affinity == Affinity.YELLOW) {
			return new Color(color.r, color.g, 0);
		} else if (affinity == Affinity.MAGENTA) {
			return new Color(color.r, 0, color.b);
		} else if (affinity == Affinity.CYAN) {
			return new Color(0, color.g, color.b);
		} else if (affinity == Affinity.BROWN) {//saddle brown = (139,69,19)/256
			return new Color(139f / 255f * color.r, 69f / 255f * color.g, 19f / 255f * color.b);
		} else {
			return color;
		}
	}
}
