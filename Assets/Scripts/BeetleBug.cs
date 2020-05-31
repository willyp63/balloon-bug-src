using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleBug : PlayerBug
{
  public SpriteRenderer shoutSprite;

  public float shoutForce = 500f;
  public float shoutRadius = 5f;
  public float shoutSpriteMaxScale = 40f;
  public float shoutSpriteAnimationDurationn = 0.25f;

  bool isShowingShoutSprite = false;

  protected override void UseAbility(int abilityIndex)
  {
    StartCoroutine(ShowShoutSprite());

    var halfShoutRadius = shoutRadius / 2f;

    foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, shoutRadius))
    {
      var rb = collider.GetComponent<Rigidbody2D>();

      if (rb)
      {
        var adjustedDist = Vector3.Distance(transform.position, rb.transform.position);
        adjustedDist -= halfShoutRadius;
        if (adjustedDist < 0)
        {
          adjustedDist = 0;
        }

        var normalizedDeltaPos = rb.transform.position - transform.position;
        normalizedDeltaPos.Normalize();

        rb.AddForce(normalizedDeltaPos * ((halfShoutRadius - adjustedDist) / halfShoutRadius) * shoutForce);
      }
    }
  }

  public override void Update()
  {
    base.Update();

    if (isShowingShoutSprite)
    {
      var newScale = shoutSprite.transform.localScale;
      var frameGrowRate = (Time.deltaTime / shoutSpriteAnimationDurationn) * (shoutSpriteMaxScale - 1);
      newScale += new Vector3(frameGrowRate, frameGrowRate, 0);
      shoutSprite.transform.localScale = newScale;

      var frameFadeRate = (Time.deltaTime / shoutSpriteAnimationDurationn) * 0.75f;
      shoutSprite.color = new Color(shoutSprite.color.r, shoutSprite.color.g, shoutSprite.color.b, shoutSprite.color.a - frameFadeRate);
    }
  }

  private IEnumerator ShowShoutSprite()
  {
    isShowingShoutSprite = true;
    shoutSprite.enabled = true;

    yield return new WaitForSeconds(shoutSpriteAnimationDurationn);

    isShowingShoutSprite = false;
    shoutSprite.enabled = false;
    shoutSprite.transform.localScale = new Vector3(1f, 1f, 1f);
    shoutSprite.color = new Color(shoutSprite.color.r, shoutSprite.color.g, shoutSprite.color.b, 1f);
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, shoutRadius);
  }
}
