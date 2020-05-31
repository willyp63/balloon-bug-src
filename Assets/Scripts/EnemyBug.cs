using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBug : Bug
{
  public float randomJumpChance = 0.05f;
  public float playerBalloonCheckRadius = 16f;
  public float hazardCheckRadius = 8f;
  public float enemyBalloonCheckRadius = 8f;
  public float moveInputConstant = 0.5f;
  public float dangerYVelocity = 5f;
  public List<Collider2D> balloonColliders;

  float moveInput = 0;
  Coroutine randomJumpCoroutine, randomMovesCoroutine, rapidJumpsCoroutine;

  public override void Start()
  {
    base.Start();

    StartCoroutine(RandomMoves());
  }

  public override void FixedUpdate()
  {
    base.FixedUpdate();

    Move(moveInput);

    if (isJumpQueued)
    {
      Jump();
      isJumpQueued = false;
    }
  }

  public override void Update()
  {
    base.Update();

    // check for hazards and avoid them first
    var closestHazard = GetClosestObjectWithTag(new List<string> { "Hazard", "Balloon Hazard" }, hazardCheckRadius);
    if (closestHazard != null)
    {
      var dirToMove = transform.position - closestHazard.transform.position;
      dirToMove.Normalize();
      MoveInDirection(dirToMove);

      return;
    }

    // next check for player balloon and pursue them
    var closestPlayerBalloon = GetClosestObjectWithTag(new List<string> { "Player Balloon" }, playerBalloonCheckRadius);
    if (closestPlayerBalloon != null)
    {
      var player = closestPlayerBalloon.GetComponent<Balloon>().GetOwner();
      Vector3 dirToMove;
      if (player != null && transform.position.y < player.transform.position.y)
      {
        dirToMove = transform.position - closestPlayerBalloon.transform.position;
      }
      else
      {
        dirToMove = closestPlayerBalloon.transform.position - transform.position;
      }

      dirToMove.Normalize();
      MoveInDirection(dirToMove);

      return;
    }

    // next check for enemy balloons and avoid them
    var closestEnemyBalloon = GetClosestObjectWithTag(new List<string> { "Enemy Balloon" }, enemyBalloonCheckRadius, true);
    if (closestEnemyBalloon != null)
    {
      var dirToMove = transform.position - closestEnemyBalloon.transform.position;
      dirToMove.Normalize();
      MoveInDirection(dirToMove);

      return;
    }

    MoveRandomly();
  }

  private IEnumerator RandomMoves()
  {
    if (Random.Range(0, 2) == 0)
    {
      moveInput = moveInputConstant;
    }
    else
    {
      moveInput = -moveInputConstant;
    }

    while (true)
    {
      yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
      moveInput *= -1;
    }
  }

  private IEnumerator RandomJumps()
  {
    while (true)
    {
      yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
      isJumpQueued = true;
    }
  }

  private IEnumerator RapidJumps()
  {
    while (true)
    {
      yield return new WaitForSeconds(0.2f);
      isJumpQueued = true;
    }
  }

  private GameObject GetClosestObjectWithTag(List<string> tagsToMatch, float checkRadius, bool excludeOurBalloons = false)
  {
    GameObject closestObject = null;
    var closestObjectDist = float.MaxValue;

    foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, checkRadius))
    {
      if (tagsToMatch.Contains(collider.tag) && (!excludeOurBalloons || !balloonColliders.Contains(collider)))
      {
        var dist = Vector3.Distance(transform.position, collider.transform.position);
        if (dist < closestObjectDist)
        {
          closestObjectDist = dist;
          closestObject = collider.gameObject;
        }
      }
    }

    return closestObject;
  }

  private void MoveInDirection(Vector2 dirToMove)
  {
    if (randomJumpCoroutine != null)
    {
      StopCoroutine(randomJumpCoroutine);
      randomJumpCoroutine = null;
    }
    if (randomMovesCoroutine != null)
    {
      StopCoroutine(randomMovesCoroutine);
      randomMovesCoroutine = null;
    }

    if (numBalloons <= 0)
    {
      DontJump();
    }
    else
    {
      if (rb.velocity.y <= -dangerYVelocity || dirToMove.y > 0)
      {
        JumpRapidly();
      }
      else
      {
        DontJump();
      }
    }

    if (dirToMove.x > 0) {
      moveInput = moveInputConstant;
    } else {
      moveInput = -moveInputConstant;
    }
  }

  private void MoveRandomly()
  {
    if (randomMovesCoroutine == null)
    {
      randomMovesCoroutine = StartCoroutine(RandomMoves());
    }

    if (numBalloons <= 0)
    {
      DontJump();
    }
    else
    {
      if (rb.velocity.y <= -dangerYVelocity)
      {
        JumpRapidly();
      }
      else if (rb.velocity.y >= dangerYVelocity)
      {
        DontJump();
      }
      else
      {
        JumpRandomly();
      }
    }
  }

  private void DontJump()
  {
    if (rapidJumpsCoroutine != null)
    {
      StopCoroutine(rapidJumpsCoroutine);
      rapidJumpsCoroutine = null;
    }
    if (randomJumpCoroutine != null)
    {
      StopCoroutine(randomJumpCoroutine);
      randomJumpCoroutine = null;
    }
  }

  private void JumpRandomly()
  {
    if (rapidJumpsCoroutine != null)
    {
      StopCoroutine(rapidJumpsCoroutine);
      rapidJumpsCoroutine = null;
    }
    if (randomJumpCoroutine == null)
    {
      randomJumpCoroutine = StartCoroutine(RandomJumps());
    }
  }

  private void JumpRapidly()
  {
    if (randomJumpCoroutine != null)
    {
      StopCoroutine(randomJumpCoroutine);
      randomJumpCoroutine = null;
    }
    if (rapidJumpsCoroutine == null)
    {
      rapidJumpsCoroutine = StartCoroutine(RapidJumps());
    }
  }
}
