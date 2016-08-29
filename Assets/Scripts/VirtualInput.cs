using UnityEngine;
using System.Collections;

public sealed class VirtualInput : MonoBehaviour {
	public float axisX = 0f;
	public float axisY = 0;

	static VirtualInput _instance;

	public static VirtualInput Instance {
		get{
			if (_instance == null)
				CreateInstance ();

			return _instance;
		}
	}

	static void CreateInstance () {
		_instance = new GameObject ("Script - VirtualInput").AddComponent<VirtualInput> ();
	}

	void FixedUpdate () {
		GetAxisInput ();
	}

	void GetAxisInput () {
		axisX = Input.GetAxisRaw ("Horizontal");
		axisY = Input.GetAxisRaw ("Vertical");
	}
}