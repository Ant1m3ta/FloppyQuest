using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	public UnityEvent OnJump;
	public UnityEvent OnStep;
	public UnityEvent OnAttack;

	Attack localAttack;
	CustomPhysics cstmPhysics;
	Animator localAnimator;

	void Awake () {
		CacheComponents ();
	}

	void CacheComponents () {
		cstmPhysics = GetComponentInParent<CustomPhysics> ();
		localAnimator = GetComponent<Animator> ();
		localAttack = GetComponentInParent<Attack> ();

		localAttack.OnShoot += InvokeOnAttack;
	}

	void LateUpdate () {
		FaceDirection ();
		UpdateAnimatorValues ();
	}

	void FaceDirection () {
		if (cstmPhysics.velocity.x > 1f)
			transform.localScale = new Vector3 (1f, 1f, 1f);
		else if (cstmPhysics.velocity.x < -1f)
			transform.localScale = new Vector3 (-1f, 1f, 1f);
	}

	void UpdateAnimatorValues () {
		localAnimator.SetFloat ("velocityX", cstmPhysics.velocity.x);
		localAnimator.SetFloat ("velocityY", cstmPhysics.velocity.y);
		localAnimator.SetBool ("Jumping", !cstmPhysics.grounded);
	}

	public void InvokeOnStep () {
		OnStep.Invoke ();
	}

	public void InvokeOnJump () {
		OnJump.Invoke ();
	}

	public void InvokeOnAttack () {
		localAnimator.SetTrigger ("Shoot");

		OnAttack.Invoke ();
	}

	void OnDestroy () {
		localAttack.OnShoot -= InvokeOnAttack;
	}
}
