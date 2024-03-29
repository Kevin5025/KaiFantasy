﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpiritController : MonoBehaviour {

	protected ISpirit spirit_;

	// TODO: nest into map that takes "ally" and "adversary" as keys
	protected IList<ISpirit> presentAllyList;
	protected IList<ISpirit> presentAdversaryList;
	public ISpirit primeAlly;  // TODO: antagonist / protagonist? 
	public ISpirit primeAdversary;

	protected virtual void Awake() {
		spirit_ = GetComponent<Spirit>();
	}

	protected virtual void Start() {
		presentAllyList = new List<ISpirit>();
		presentAdversaryList = new List<ISpirit>();
		primeAlly = null;
		primeAdversary = null;

	}

	/*
	 * Alert
	 */
	protected virtual void OnTriggerEnter2D(Collider2D collider) {
		ISpirit colliderAgent = collider.GetComponent<Spirit>();
		if (colliderAgent != null) {
			if (colliderAgent.GetAffinity() == spirit_.GetAffinity()) {
				presentAllyList.Add(colliderAgent);
				// Debug.Log(presentAllyList.Count);
				if (primeAlly == null) {
					FindPrimeAlly();
				}
			} else {
				presentAdversaryList.Add(colliderAgent);
				// Debug.Log(presentAdversaryList.Count);
				if (primeAdversary == null) {
					FindPrimeAdversary();
				}
			}
		}
	}

	/*
	 * Alert
	 */
	protected virtual void OnTriggerExit2D(Collider2D collider) {
		ISpirit colliderAgent = collider.GetComponent<Spirit>();
		if (colliderAgent != null) {
			if (colliderAgent.GetAffinity() == spirit_.GetAffinity()) {
				presentAllyList.Remove(colliderAgent);
				// Debug.Log(presentAllyList.Count);
				if (colliderAgent == primeAlly) {
					FindPrimeAlly();
				}
			} else {
				presentAdversaryList.Remove(colliderAgent);
				// Debug.Log(presentAdversaryList.Count);
				if (colliderAgent == primeAdversary) {
					FindPrimeAdversary();
				}
			}
		}
	}

	/**
     * Returns prime present ally circle agent or null if none. 
     */
	protected virtual void FindPrimeAlly() {
		if (presentAllyList.Count > 0) {
			primeAlly = presentAllyList[0];
		} else {
			primeAlly = null;
		}
	}

	/**
     * Returns prime present adversary circle agent or null if none. 
     */
	protected virtual void FindPrimeAdversary() {
		if (presentAdversaryList.Count > 0) {
			primeAdversary = presentAdversaryList[0];
		} else {
			primeAdversary = null;
		}
	}
}
