using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothBug : PlayerBug
{
  public Rigidbody2D shootProjectile;
  public Transform shootPos;
  public float shootForce = 600f;

  protected override void UseAbility()
  {
    var projectile = Instantiate(shootProjectile, shootPos.position, Quaternion.identity);
    projectile.AddForce(new Vector2(isFacingRight ? 1f : -1f, 0.2f) * shootForce);
    Destroy(projectile.gameObject, 3f);
  }
}
