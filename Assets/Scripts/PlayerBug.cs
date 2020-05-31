using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerBug : Bug
{
  public List<SpriteRenderer> balloonSprites;
  public float energyRegenRate = 0.01f;
  public float abilityEnergyCost = 0.33f;

  bool isPlayer2 = false;

  float energy = 0f;

  protected abstract void UseAbility();

  public float GetEnergy()
  {
    return energy;
  }

  public override void FixedUpdate()
  {
    base.FixedUpdate();

    Move(Input.GetAxis(isPlayer2 ? "Horizontal2" : "Horizontal"));
  }

  public override void Update()
  {
    base.Update();


    if (energy < 1f)
    {
      energy += energyRegenRate * Time.deltaTime;
    }
    else
    {
      energy = 1f;
    }

    if (Input.GetKeyDown(isPlayer2 ? KeyCode.UpArrow : KeyCode.W))
    {
      Jump();
    }

    if (Input.GetKeyDown(isPlayer2 ? KeyCode.DownArrow : KeyCode.S))
    {
      if (energy >= abilityEnergyCost)
      {
        energy -= abilityEnergyCost;
        UseAbility();
      }
    }
  }

  public void SetIsPlayer2(bool isPlayer2)
  {
    this.isPlayer2 = isPlayer2;

    foreach (SpriteRenderer balloonSprite in balloonSprites)
    {
      balloonSprite.color = isPlayer2 ? Color.blue : Color.green;
    }
  }
}
