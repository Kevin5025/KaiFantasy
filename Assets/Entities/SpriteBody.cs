using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpriteBody : MonoBehaviour, ISpirit {

	protected ISpirit spirit_;
	
	protected SpriteRenderer spriteRenderer_;
	protected float disintegratedColorAlpha;
	protected float fadeDuration;

	protected virtual void Awake() {
		spirit_ = GetComponent<Spirit>();
	}

	protected virtual void Start() {
		spriteRenderer_ = GetComponent<SpriteRenderer>();
		spriteRenderer_.color = Spirit.teamColorDictionary[spirit_.GetAffinity()];

		disintegratedColorAlpha = 0.125f;  // disintegration start alpha
		fadeDuration = 4f;  // time it takes for alpha to disintegrate to 0

		gameObject.layer = GetTeamLayer();
	}

	protected virtual void Update() {

	}
	
	protected virtual void FixedUpdate() {
		
	}

	protected abstract int GetTeamLayer();

	/**
     * Occurs on death / destruction / expiration
     */
	protected virtual void Disintegrate() {
		Collider2D collider2D = GetComponent<Collider2D>();
		if (collider2D != null) {
			collider2D.enabled = false;  // TODO: test if OnTriggerExit2D
		}
		// TODO: become resources
		StartCoroutine(FadeDisintegrated());
	}

	/**
     * Overrides entity fade for a gradual disappearance, since these agents are more important than any entity. 
     */
	protected virtual IEnumerator FadeDisintegrated() {
		float fadeTimeConstant = disintegratedColorAlpha / fadeDuration;
		for (float f = disintegratedColorAlpha; f > 0; f -= Time.deltaTime * fadeTimeConstant) {
			spriteRenderer_.color = new Color(spriteRenderer_.color.r, spriteRenderer_.color.g, spriteRenderer_.color.b, f);
			//yield return new WaitForSeconds(1f);//3f? //is this consistent? 
			yield return null;  // https://answers.unity.com/questions/755196/yield-return-null-vs-yield-return-waitforendoffram.html
		}
		EliminateSelf();
	}

	protected virtual void EliminateSelf() {
		Destroy(gameObject);  // TODO: eliminate agent override in Body class
	}

	public Affinity GetAffinity() {
		return spirit_.GetAffinity();
	}

	public void SetAffinity(Affinity affinity) {
		spirit_.SetAffinity(affinity);
	}

	public Transform GetTransform() {
		return spirit_.GetTransform();
	}
}
