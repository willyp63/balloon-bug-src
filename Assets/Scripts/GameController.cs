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

  bool isPaused = false;

  void Start()
  {
    Debug.Log(MenuSelection.Player1Character);
    Debug.Log(MenuSelection.Player2Character);
    PlayerBug player1 = Instantiate<PlayerBug>(characterPrefabs[MenuSelection.Player1Character], player1Spawn.position, characterPrefabs[MenuSelection.Player1Character].transform.rotation);
    player1.SetIsPlayer2(false);
    PlayerBug player2 = Instantiate<PlayerBug>(characterPrefabs[MenuSelection.Player2Character], player2Spawn.position, characterPrefabs[MenuSelection.Player2Character].transform.rotation);
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
    if (GameObject.FindGameObjectsWithTag("Player").Length == 0 || (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && GameObject.FindGameObjectsWithTag("Player").Length <= 1))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
      SceneManager.LoadScene("Scenes/MenuScene");
    }
  }

  public bool IsPaused()
  {
    return isPaused;
  }

  private void Pause()
  {
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
    {
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
    {
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    isPaused = true;
  }

  private void Resume()
  {
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Enemy"))
    {
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
    {
      gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    isPaused = false;
  }
}
