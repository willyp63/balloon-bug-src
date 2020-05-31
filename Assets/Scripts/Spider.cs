using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
  public float walkForce = 2f;
  public float maxVelocity = 2f;
  public Transform groundCheck;

  protected Rigidbody2D rb;
  bool isFacingRight = false;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!IsWallBelow())
    {
      isFacingRight = !isFacingRight;
      var newScale = transform.localScale;
      newScale.x *= -1;
      transform.localScale = newScale;
    }

    rb.AddForce(new Vector2(isFacingRight ? walkForce : -walkForce, 0));

    var newVelocity = rb.velocity;
    newVelocity.x = Mathf.Max(newVelocity.x, -maxVelocity);
    newVelocity.x = Mathf.Min(newVelocity.x, maxVelocity);
    rb.velocity = newVelocity;
  }

  private bool IsWallBelow()
  {
    foreach (Collider2D collider in Physics2D.OverlapPointAll(groundCheck.position))
    {
      if (collider.tag == "Wall")
      {
        return true;
      }
    }
    return false;
  }
}
