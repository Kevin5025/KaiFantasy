using System.Collections.Generic;
using UnityEngine;

/**
 * This is anything that has a team affiliation. 
 */
public class Spirit : MonoBehaviour, ISpirit {

	public static Dictionary<Affinity, Color> teamColorDictionary;

	public Affinity affinity;
	public Color color;

	static Spirit() {
		teamColorDictionary = new Dictionary<Affinity, Color> {
			{ Affinity.RED, new Color(1, 0, 0) },
			{ Affinity.GREEN, new Color(0, 1, 0) },
			{ Affinity.BLUE, new Color(0, 0, 1) },
			{ Affinity.YELLOW, new Color(1, 1, 0) },
			{ Affinity.MAGENTA, new Color(1, 0, 1) },
			{ Affinity.CYAN, new Color(0, 1, 1) },
			{ Affinity.BROWN, new Color(139f / 255f, 69f / 255f, 19f / 255f) }
		};
	}

	protected virtual void Awake () {

	}

	/**
     * Assign sprite color based on team. 
     */
	protected virtual void Start () {
		color = teamColorDictionary[affinity];
	}
	
	protected virtual void Update () {

	}

	protected virtual void FixedUpdate () {

	}

	public Affinity GetAffinity() {
		return affinity;
	}

	public void SetAffinity(Affinity affinity) {
		this.affinity = affinity;
	}

	public Color GetColor() {
		return color;
	}

	public Transform GetTransform() {
		return transform;
	}
}
