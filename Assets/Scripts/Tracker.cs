using UnityEngine;
using System.Collections;

public class Tracker : MonoBehaviour {
	public Transform target;

	void Update () {

		transform.position = Vector3.MoveTowards (transform.position, target.position - Vector3.forward * 10f, Time.deltaTime * 70f);

	}
}
