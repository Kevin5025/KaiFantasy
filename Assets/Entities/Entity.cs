using System.Collections;
using UnityEngine;

/**
 * This is anything that can be destroyed or killed by taking damage. 
 */
public abstract class Entity : Spirit {

	public float maxHealth;
	public float health;
	public float healthRegenerationRate;
	public float viscosity;

	public bool defunct;//aka dead, destroyed, etc. 
	protected float fadeDuration = 1f;

	protected override void Start () {
		base.Start();
		defunct = false;
	}

	/**
     * Handles death (expiration) and regeneration. 
     */
	protected override void FixedUpdate () {
		base.FixedUpdate();
		if (!defunct) {
			if (health < maxHealth) {
				health += healthRegenerationRate * maxHealth * Time.fixedDeltaTime;
			} else if (health > maxHealth) {
				health = maxHealth;
			}
			if (health <= 0) {
				health = 0;
				Expire();
			}
		}
	}

	protected override int GetTeamLayer() {
		return LayersManager.layersManager.GetTeamEntityLayer(affinity);
	}

	public virtual float takeDamage(CircleAgent casterAgent, float damage) {
		float trueDamage = damage;
		health -= trueDamage;
		return trueDamage;
	}

	/**
     * Occurs on death / destruction
     */
	protected override void Expire () {
		defunct = true;
		Collider2D collider2D = GetComponent<Collider2D>();
		if (collider2D != null) {
			collider2D.enabled = false;
		}
		StartCoroutine(Fade());
	}

	/**
     * Visually lets users know that the entity is defunct. 
     */
	protected virtual IEnumerator Fade () {
		spriteRenderer.color = new Color(r, g, b, 0.25f);//instant fade
		yield return new WaitForSeconds(fadeDuration);
		EliminateSelf();
	}

	protected virtual void EliminateSelf () {
		transform.position = new Vector3(transform.position.x, transform.position.y, 10f);  // for OnTriggerExit2D  // TODO: test
		Destroy(gameObject);
	}
}
