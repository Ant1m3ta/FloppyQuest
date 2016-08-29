using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	public void Launch (Vector2 direction) {
		GetComponent<Rigidbody2D> ().AddForce (direction* 300f, ForceMode2D.Impulse);
	}

	void Start () {
		StartCoroutine (RemoveTimer ());
	}

	IEnumerator RemoveTimer () {
		yield return new WaitForSeconds (0.5f);
		RemoveFromGame ();
	}

	void RemoveFromGame () {
		Destroy (gameObject);
	}

	void OnCollisionEnter2D (Collision2D collisionObject) {
		if (collisionObject.gameObject.tag != "Player")
			RemoveFromGame ();
	}
}
