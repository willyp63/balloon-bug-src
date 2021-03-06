﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBug : PlayerBug
{
  public float blinkForce = 1f;

  protected override void UseAbility()
  {
    rb.AddForce(new Vector2(isFacingRight ? 1 : -1, 0) * blinkForce);
  }
}
