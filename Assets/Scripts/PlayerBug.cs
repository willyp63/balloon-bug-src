using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBug : Bug
{
  public List<SpriteRenderer> balloonSprites;

  bool isPlayer2 = false;

  new void FixedUpdate()
  {
    base.FixedUpdate();

    Move(Input.GetAxis(isPlayer2 ? "Horizontal2" : "Horizontal"));
  }

  new void Update()
  {
    base.Update();

    if (Input.GetKeyDown(isPlayer2 ? KeyCode.UpArrow : KeyCode.W))
    {
      Jump();
    }
  }

  public void SetIsPlayer2(bool isPlayer2) {
    this.isPlayer2 = isPlayer2;
    
    foreach (SpriteRenderer balloonSprite in balloonSprites) {
      balloonSprite.color = isPlayer2 ? Color.blue: Color.green;
    }
  }
}
