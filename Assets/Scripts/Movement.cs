using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 2000000f, ForceMode2D.Impulse);

        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0f)
            GetComponent<Rigidbody2D>().AddForce(new Vector2 (horizontal * 2000f, 0f));
        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, GetComponent<Rigidbody2D>().velocity.y);

        GetComponent<Rigidbody2D>().velocity = new Vector2(
            Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.x, -80f, 80f),
            Mathf.Clamp(GetComponent<Rigidbody2D>().velocity.y, -260f, 260f)
            );
    }

}
