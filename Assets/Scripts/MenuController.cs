using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
  public Slider numEnemiesSlider;
  public Text numEnemiesText;
  public List<Sprite> characterSprites;
  public Image player1CharacterImage;
  public Image player2CharacterImage;

  public void Start()
  {
    MenuSelection.Player1Character = 0;
    MenuSelection.Player2Character = 0;
    MenuSelection.NumEnemies = 0;
  }

  public void StartGame()
  {
    SceneManager.LoadScene("Scenes/GameScene");
  }

  public void SelectPlayer1Character(int character)
  {
    MenuSelection.Player1Character = character;
    player1CharacterImage.sprite = characterSprites[character];
  }

  public void SelectPlayer2Character(int character)
  {
    MenuSelection.Player2Character = character;
    player2CharacterImage.sprite = characterSprites[character];
  }

  void Update()
  {
    MenuSelection.NumEnemies = (int)numEnemiesSlider.value;
    numEnemiesText.text = numEnemiesSlider.value.ToString();
  }
}
