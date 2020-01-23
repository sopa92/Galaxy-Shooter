using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _EnemyPrefab;
    [SerializeField]
    private GameObject[] powerups;
    private GameManager _gameManager; 
	// Use this for initialization
	void Start () {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        StartSpawning();
    }
    public void StartSpawning() {
        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerupSpawn());
    }
    IEnumerator EnemySpawn()
    {
        while (!_gameManager.GameOver)
        {
            GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (existingEnemies.Length < 3)
            {
                Instantiate(_EnemyPrefab, new Vector3(Random.Range(-9.10f, 9.10f), Random.Range(9.0f, 17.0f), 0), Quaternion.identity);                
            }
            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator PowerupSpawn()
    {
        while (!_gameManager.GameOver)
        {
            int randomPowerup = Random.Range(0, 3);
            Instantiate(powerups[randomPowerup], new Vector3(Random.Range(-9.10f, 9.10f), Random.Range(9.0f, 17.0f), 0), Quaternion.identity);
            yield return new WaitForSeconds(7.0f);
        }
    }
}
