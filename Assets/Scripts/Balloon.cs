using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
  protected Bug owner;
  protected Vector2 stageDimensions;

  void Start()
  {
    owner = transform.parent.parent.GetComponent<Bug>();
    stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
  }

  public void FixedUpdate()
  {
    // pop balloon if too high
    if (transform.position.y > stageDimensions.y * 1.15f)
    {
      Pop();
    }
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.layer == LayerMask.NameToLayer("Bug") && collision.gameObject != owner.gameObject)
    {
      // push away popper
      var deltaPos = collision.transform.position - transform.position;
      deltaPos.Normalize();
      collision.gameObject.GetComponent<Rigidbody2D>().AddForce(deltaPos * 250);

      Pop();
    }
  }

  private void Pop()
  {
    owner.PopBalloon();
    Destroy(transform.parent.gameObject);
  }
}
