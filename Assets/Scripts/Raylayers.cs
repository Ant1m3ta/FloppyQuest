using UnityEngine;
using System.Collections;

public class Raylayers : MonoBehaviour {

	/// <summary>
	/// Standable, SoftTop, SoftBottom
	/// </summary>
	public static readonly int onlyCollisions;
	/// <summary>
	/// Standable, SoftTop
	/// </summary>
	public static readonly int upRay;
	/// <summary>
	/// Standable, SoftTop
	/// </summary>
	public static readonly int downRay;
	/// <summary>
	/// SoftBottom
	/// </summary>
	public static readonly int dropDownRay;

	static Raylayers() {
		onlyCollisions = 1 << LayerMask.NameToLayer ("Standable")
		| 1 << LayerMask.NameToLayer ("SoftTop")
		| 1 << LayerMask.NameToLayer ("SoftBottom");

		upRay = 1 << LayerMask.NameToLayer ("Standable")
		| 1 << LayerMask.NameToLayer ("SoftTop");

		downRay = 1 << LayerMask.NameToLayer ("Standable")
		| 1 << LayerMask.NameToLayer ("SoftBottom");

		dropDownRay = 1 << LayerMask.NameToLayer ("SoftBottom");
	}
}
