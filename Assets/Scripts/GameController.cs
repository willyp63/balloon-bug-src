using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  public Text centerGameText;
  public List<PlayerBug> characterPrefabs;
  public Transform player1Spawn;
  public Transform player2Spawn;
  public EnemyBug enemyBugPrefab;
  public List<Transform> enemySpawns;
  public Slider player1EnergyBar;
  public Slider player2EnergyBar;

  bool isPaused = false;
  PlayerBug player1;
  PlayerBug player2;

  void Start()
  {
    player1 = Instantiate<PlayerBug>(characterPrefabs[MenuSelection.Player1Character], player1Spawn.position, characterPrefabs[MenuSelection.Player1Character].transform.rotation);
    player1.SetIsPlayer2(false);
    player2 = Instantiate<PlayerBug>(characterPrefabs[MenuSelection.Player2Character], player2Spawn.position, characterPrefabs[MenuSelection.Player2Character].transform.rotation);
    player2.SetIsPlayer2(true);

    List<Transform> availableEnemySpawns = new List<Transform>(enemySpawns);
    for (var i = 0; i < MenuSelection.NumEnemies; i++)
    {
      var randomIndx = Random.Range(0, availableEnemySpawns.Count);
      Instantiate<EnemyBug>(enemyBugPrefab, availableEnemySpawns[randomIndx].position, Quaternion.identity);
      availableEnemySpawns.RemoveAt(randomIndx);
    }

    StartCoroutine(CountDownAndStart());
  }

  private IEnumerator CountDownAndStart()
  {
    Pause();

    var count = 3;
    while (count > 0)
    {
      centerGameText.text = count.ToString();
      yield return new WaitForSeconds(1);
      count--;
    }

    centerGameText.text = "GO";
    yield return new WaitForSeconds(1);

    centerGameText.text = "";
    Resume();
  }

  void Update()
  {
    if (GameObject.FindGameObjectsWithTag("Player Bug").Length == 0 || (GameObject.FindGameObjectsWithTag("Enemy Bug").Length == 0 && GameObject.FindGameObjectsWithTag("Player Bug").Length <= 1))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      SceneManager.LoadScene("Scenes/MenuScene");
    }

    player1EnergyBar.value = player1.GetEnergy();
    player2EnergyBar.value = player2.GetEnergy();
  }

  public bool IsPaused()
  {
    return isPaused;
  }

  private void Pause()
  {
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy Bug"))
    {
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player Bug"))
    {
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    isPaused = true;
  }

  private void Resume()
  {
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy Bug"))
    {
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player Bug"))
    {
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    isPaused = false;
  }
}
