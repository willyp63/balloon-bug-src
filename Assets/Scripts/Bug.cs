using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bug : MonoBehaviour
{
  public float moveForce = 200f;
  public float jumpForce = 200f;
  public int jumpCooldownLength = 2;
  public float maxMassMultiplier = 1.5f;
  public Transform bugSprite;

  protected GameController gameController;
  protected Rigidbody2D rb;
  protected Vector2 stageDimensions;

  protected int jumpCooldown = 0;
  protected bool isFacingRight = true;
  protected int numBalloons = 3;
  protected bool isJumpQueued = false;
  protected float startingMass;

  // Start is called before the first frame update
  public virtual void Start()
  {
    gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    rb = GetComponent<Rigidbody2D>();
    stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

    jumpCooldown = jumpCooldownLength;

    startingMass = rb.mass;

    UpdateMass();
  }

  public void Move(float input)
  {
    if (input == 0) { return; }
    if (gameController.IsPaused()) { return; }

    var force = new Vector2(input * Time.deltaTime * moveForce, 0);

    rb.AddForce(force);

    if ((isFacingRight && force.x < 0) || (!isFacingRight && force.x > 0))
    {
      var newScale = bugSprite.localScale;
      newScale.x *= -1;
      bugSprite.localScale = newScale;
      isFacingRight = !isFacingRight;
    }
  }

  public void Jump()
  {
    if (gameController.IsPaused()) { return; }

    if (numBalloons > 0 && jumpCooldown >= jumpCooldownLength)
    {
      isJumpQueued = true;
    }
  }

  public void PopBalloon()
  {
    numBalloons--;
    UpdateMass();
    Debug.Log("Pop");
  }

  public virtual void Update()
  {
    if (jumpCooldown < jumpCooldownLength)
    {
      jumpCooldown++;
    }
  }

  public virtual void FixedUpdate()
  {
    if (isJumpQueued)
    {
      rb.AddForce(new Vector2(0, jumpForce));
      jumpCooldown = 0;
      isJumpQueued = false;
    }

    // teleport bug to oposite side if it hits the side of the screen
    var newPos = transform.position;
    newPos.x = WarpSides(newPos.x, stageDimensions.x);
    transform.position = newPos;
  }

  private void UpdateMass()
  {
    rb.mass = startingMass * (1 + (maxMassMultiplier - 1) * ((3 - numBalloons) / 3f));
  }

  private float WarpSides(float pos, float max)
  {
    var newPos = pos;

    if (newPos < -max)
    {
      newPos *= -1;
      newPos -= newPos - max;
    }
    else if (newPos >= max)
    {
      newPos *= -1;
      newPos -= newPos + max;
    }

    return newPos;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Hazard")
    {
      Destroy(gameObject);
    }
  }
}
