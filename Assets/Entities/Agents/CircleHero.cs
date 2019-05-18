using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is anything that is important enough to perpetually respawn. 
 */
public class CircleHero : CircleAgent {

	protected bool eliminated;
	protected float respawnTime;

	protected override void Start() {
		base.Start();

		eliminated = false;
		respawnTime = 10f;
	}

	protected override void EliminateSelf() {
		eliminated = true;
		StartCoroutine(Respawn());
	}

	/**
     * Respawning logic to just reinitialize any necessary values to be good as new. 
     * Respawns at spawn point. 
     */
	protected virtual IEnumerator Respawn() {
		yield return new WaitForSeconds(respawnTime - fadeTime);
		health = maxHealth;
		defunct = false; eliminated = false;
		gameObject.GetComponent<Collider2D>().enabled = true;
		spriteRenderer.color = new Color(r, g, b, 1f);

		//GameObject spawnPoint = SpawnManager.spawnManager.GetTeamHeroSpawnPointGameObject(team);
		//if (spawnPoint != null) {
		//	transform.position = spawnPoint.transform.position;
		//	transform.rotation = spawnPoint.transform.rotation;
		//}
	}
}
