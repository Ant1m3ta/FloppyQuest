using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	[SerializeField]
	Ammo ammoPrefab;

	public System.Action OnShoot;

	void Shoot () {
		if (OnShoot != null)
			OnShoot ();
	}

	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.Return))
			Shoot ();
	}

	void FireAmmo () {
		Ammo ammoInstance = (Ammo)Instantiate (ammoPrefab, transform.position, transform.rotation);
		Debug.Log (ammoInstance.transform.position);
		ammoInstance.Launch (transform.localScale);
		Shoot ();
	}
}
