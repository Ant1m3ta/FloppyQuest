using UnityEngine;
using System.Collections;

public class CustomPhysics : MonoBehaviour {

	float acceleration = 40f;
	float maxSpeed = 200f;
	float gravity = 28f;
	float maxFall = 300f;
	float jump = 500f;

	public LayerMask layerMask;
	Rect box;
	[HideInInspector]
	public Vector2 velocity;

	[HideInInspector]
	public bool grounded = false;
	bool falling = false;

	int horizontalRays = 6;
	int verticalRays = 8;
	int margin = 8;

	float jumpPressTimestamp = 0f;
	bool jumpPressedPreviously = false;

	void Start () {
		layerMask = Raylayers.onlyCollisions;
	}

	void FixedUpdate () {
		Collider2D collider = GetComponent<Collider2D> ();
		box = new Rect (
			collider.bounds.min.x,
			collider.bounds.min.y,
			collider.bounds.size.x,
			collider.bounds.size.y
		);
		
		DropDown ();
		CheckForCeiling ();
		CalculateJump ();
		CalculateGravity ();
		CalculateMovement ();

	}

	void LateUpdate () {
		transform.Translate (velocity * Time.deltaTime);
	}

	void CalculateJump () {
		bool jumpPressed = Input.GetKey (KeyCode.Space);

		if (jumpPressed && !jumpPressedPreviously)
			jumpPressTimestamp = Time.time;
		else if (!jumpPressed)
			jumpPressTimestamp = 0f;

		if (grounded && Time.time - jumpPressTimestamp < 0.2f) {
			velocity = new Vector2 (velocity.x, jump);
			jumpPressTimestamp = 0f;
			grounded = false;
		}

		jumpPressedPreviously = jumpPressed;
	}

	void CheckForCeiling () {
		bool canJump = true;

		if (grounded || velocity.y > 0) {
			float upRayLength = grounded ? margin : velocity.y * Time.deltaTime;

			bool connection = false;
			int lastConnection = 0;
			Vector2 min = new Vector2 (box.xMin + margin, box.center.y);
			Vector2 max = new Vector2 (box.xMax - margin, box.center.y);
			RaycastHit2D[] upRays = new RaycastHit2D[verticalRays];

			for (int i = 0; i < verticalRays; i++) {
				Vector2 start = Vector2.Lerp (min, max, (float)i / (float)verticalRays);
				Vector2 end = start + Vector2.up * (upRayLength + box.height / 2f);
				upRays [i] = Physics2D.Linecast (start, end, Raylayers.upRay);

				if (upRays [i].fraction > 0) {
					connection = true;
					lastConnection = i;
				}
			}

			if (connection) {
				velocity = new Vector2 (velocity.x, 0f);
				transform.position += Vector3.up * (upRays [lastConnection].point.y - box.yMax);
				SendMessage ("OnHeadHit", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	void CalculateMovement () {
		float horizontalAxis = Input.GetAxisRaw ("Horizontal");
		float newVelocityX = velocity.x;

		if (horizontalAxis != 0f) {
			newVelocityX += acceleration * horizontalAxis;
			newVelocityX = Mathf.Clamp (newVelocityX, -maxSpeed, maxSpeed);
		} else if (velocity.x != 0) {
			int modifier  = velocity.x > 0? -1: 1;
			newVelocityX += acceleration * modifier;
		}

		velocity = new Vector2 (newVelocityX, velocity.y);

		if (velocity.x != 0) {
			Vector3 startPoint = new Vector3 (box.center.x, box.yMin + margin, transform.position.z);
			Vector3 endPoint = new Vector3 (box.center.x, box.yMax - margin, transform.position.z);

			float sideRayLength = box.width / 2 + Mathf.Abs (newVelocityX * Time.deltaTime);
			Vector3 direction = newVelocityX > 0 ? Vector3.right : Vector3.left;
			bool connected = false;

			for (int i = 0; i < horizontalRays; i++) {
				float lerpAmmount = (float)i / (float)(horizontalRays - 1);
				Vector3 origin = Vector3.Lerp (startPoint, endPoint, lerpAmmount);

				Debug.DrawRay (origin, direction * sideRayLength, Color.green);
				Debug.Log (layerMask.value);
				RaycastHit2D hitInfo = Physics2D.Raycast (origin, direction, sideRayLength, layerMask);

				connected = hitInfo.collider != null;

				if (connected) {
					transform.Translate (direction * (hitInfo.distance - box.width / 2));
					velocity = new Vector2 (0, velocity.y);
					break;
				}
			}
		}
	}

	void CalculateGravity () {
		if (!grounded)
			velocity = new Vector2 (velocity.x, Mathf.Max (velocity.y - gravity, -maxFall));

		if (velocity.y < 0)
			falling = true;

		if (grounded || falling) {
			Vector3 startingPoint = new Vector3 (box.xMin + margin, box.center.y, transform.position.z);
			Vector3 endPoint = new Vector3 (box.xMax - margin, box.center.y, transform.position.z);

			float distance = box.height / 2f + (grounded ? margin : Mathf.Abs (velocity.y * Time.deltaTime));

			bool connected = false;

			for (int i = 0; i < verticalRays; i++) {
				float lerpAmmount = (float)i / (float)(verticalRays - 1);
				Vector3 origin = Vector3.Lerp (startingPoint, endPoint, lerpAmmount);

				Debug.DrawRay (origin, Vector3.down * distance, Color.green);

				RaycastHit2D hitInfo = Physics2D.Raycast(origin, Vector3.down, distance, layerMask);
				connected = hitInfo.collider != null;

				if (connected) {
					layerMask = layerMask ==  Raylayers.upRay ? Raylayers.onlyCollisions : layerMask.value;
					grounded = true;
					falling = false;
					transform.Translate (Vector3.down * (hitInfo.distance - box.height / 2));
					velocity = new Vector2 (velocity.x, 0f);
					break;
				}
			}

			if (!connected) {
				grounded = false;
			}
		}
	}

	void DropDown () {
		if (grounded && layerMask.value != Raylayers.upRay) {
			float upRayLength = grounded ? margin : velocity.y * Time.deltaTime;

			bool connection = false;
			int lastConnection = 0;
			Vector2 min = new Vector2 (box.xMin + margin, box.center.y);
			Vector2 max = new Vector2 (box.xMax - margin, box.center.y);
			RaycastHit2D[] upRays = new RaycastHit2D[verticalRays];

			for (int i = 0; i < verticalRays; i++) {
				Vector2 start = Vector2.Lerp (min, max, (float)i / (float)verticalRays);
				Vector2 end = start + Vector2.down * (upRayLength + box.height / 2f);
				upRays [i] = Physics2D.Linecast (start, end, Raylayers.dropDownRay);

				if (upRays [i].fraction > 0) {
					connection = true;
					lastConnection = i;
				}
			}

			if (connection && Input.GetKey (KeyCode.S)) {
				Debug.Log ("Drop down");
				layerMask = Raylayers.upRay;
			}
		}
	}
}