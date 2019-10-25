using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Abstracted behaviors for easy access later. 
 */
public abstract class AgentController : MonoBehaviour {

	protected CircleAgent agent;
	public GameObject primeAdversary;
	protected int alertCount;

	public float[] personalityUniform;
	public float[] personalityGaussian;

	protected virtual void Awake() { }

	protected virtual void Start() {
		agent = GetComponent<CircleAgent>();
		primeAdversary = null;
		alertCount = 0;

		personalityUniform = MyStaticLibrary.NextRandomUniformArray(5, -1f, 1f);
		personalityGaussian = MyStaticLibrary.NextRandomGaussianArray(5);
	}

	protected virtual void Update() { }

	protected virtual void FixedUpdate() {
		// alertCooldownTime -= Time.deltaTime;  // float.MinValue ~ -3.4E38 seconds
	}

	public virtual void AlertEnter() {
		Debug.Log("AlertEnter");
		alertCount++;
	}

	public virtual void AlertExit() {
		Debug.Log("AlertExit");
		alertCount--;
	}

	protected virtual void Rotate() { }

	protected virtual void Move() { }

	protected virtual void Fire() { }

	/**
     * Updates targetPosition to be the closestHostileAgent position
     */
	protected IEnumerator KeepFindClosestHostileAgent() {
		while (alertCount > 0) {
			primeAdversary = FindPrimeAdversary();
			yield return new WaitForSeconds(1f);
		}
	}

	/**
     * Returns closest circle agent who is on a different team or null if none. 
     */
	protected GameObject FindPrimeAdversary() {
		CircleAgent[] circleAgentArray = FindObjectsOfType<CircleAgent>();
		GameObject primeAdversaryGameObject = null;
		float primeAdversaryTransformDistance = float.MaxValue;
		for (int ca = 0; ca < circleAgentArray.Length; ca++) {
			if (circleAgentArray[ca].affinity != agent.affinity && !circleAgentArray[ca].defunct) {
				float distance = MyStaticLibrary.GetDistance(gameObject, circleAgentArray[ca].gameObject);
				if (distance < primeAdversaryTransformDistance) {
					primeAdversaryGameObject = circleAgentArray[ca].gameObject;
					primeAdversaryTransformDistance = distance;
				}
			}
		}
		return primeAdversaryGameObject;
	}
}
