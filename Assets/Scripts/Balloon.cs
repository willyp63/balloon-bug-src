using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
  protected Bug owner;
  protected Vector2 stageDimensions;

  public Bug GetOwner() {
    return owner;
  }

  void Start()
  {
    owner = transform.parent.parent.GetComponent<Bug>();
    stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if ((collision.gameObject.tag == "Player Bug" && collision.gameObject != owner.gameObject) || (collision.gameObject.tag == "Enemy Bug" && collision.gameObject != owner.gameObject) || collision.gameObject.tag == "Hazard" || collision.gameObject.tag == "Balloon Hazard")
    {
      Pop();

      var rb = collision.gameObject.GetComponent<Rigidbody2D>();
      var bug = collision.gameObject.GetComponent<Bug>();
      if (bug && rb)
      {
        // push away popper
        var deltaPos = collision.transform.position - transform.position;
        deltaPos.Normalize();
        rb.AddForce(deltaPos * 250);
      }
    }
  }

  private void Pop()
  {
    owner.PopBalloon();
    Destroy(transform.parent.gameObject);
  }
}
