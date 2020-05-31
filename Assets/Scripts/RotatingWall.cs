using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour
{
  public float speed = 1f;

  // Update is called once per frame
  void Update()
  {
    transform.Rotate(0, 0, speed * Time.deltaTime);
  }
}
