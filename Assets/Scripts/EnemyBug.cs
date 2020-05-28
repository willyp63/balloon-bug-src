using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBug : Bug
{
  public float randomJumpChance = 0.05f;

  float moveInput = 0;

  new void Start()
  {
    base.Start();

    StartCoroutine(RandomMoves());
  }

  new void FixedUpdate()
  {
    base.FixedUpdate();

    Move(moveInput);
  }

  new void Update()
  {
    base.Update();

    // dont jump if in the top quarter of the screen
    if (transform.position.y < stageDimensions.y / 2f)
    {
      // always jump if in the bottom quarter of the screen
      if (transform.position.y < -stageDimensions.y / 2f)
      {
        Jump();
      }
      else if (Random.Range(0f, 1f) < randomJumpChance * ((3 - numBalloons) * 1 / 3 + 1))
      {
        Jump();
      }
    }
  }

  private IEnumerator RandomMoves()
  {
    // random alternate between moving left and right
    while (true)
    {
      if (Random.Range(0, 2) == 0)
      {
        moveInput = 0.5f;
      }
      else
      {
        moveInput = -0.5f;
      }
      yield return new WaitForSeconds(Random.Range(0.25f, 0.75f));
    }
  }
}
