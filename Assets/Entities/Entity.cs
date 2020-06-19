using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Spirit {
	
	protected SpriteRenderer spriteRenderer;
	protected float fadeDisintegratedUpperColorAlpha;
	protected float fadeDuration;

	protected override void Start() {
		base.Start();
		spriteRenderer = GetComponent<SpriteRenderer>();
		fadeDisintegratedUpperColorAlpha = 0.125f;
		fadeDuration = 4f;

		gameObject.layer = GetTeamLayer();
		spriteRenderer.color = color;
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
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
		float fadeTimeConstant = fadeDisintegratedUpperColorAlpha / fadeDuration;
		for (float f = fadeDisintegratedUpperColorAlpha; f > 0; f -= Time.deltaTime * fadeTimeConstant) {
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, f);
			//yield return new WaitForSeconds(1f);//3f? //is this consistent? 
			yield return null;  // https://answers.unity.com/questions/755196/yield-return-null-vs-yield-return-waitforendoffram.html
		}
		EliminateSelf();
	}

	protected virtual void EliminateSelf() {
		Destroy(gameObject);  // TODO: eliminate agent override in Body class
	}
}
