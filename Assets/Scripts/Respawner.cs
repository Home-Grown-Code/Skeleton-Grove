using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] RespawnPoints;
    [SerializeField] private float cooldown;
    private float timer;
    private float cooldownDecreseRate = 0.1f;
    private float cooldownCap = 0.25f;
    private Transform player;

    private void Awake()
    {
        player = FindFirstObjectByType<Entity>().transform;
    }   
 
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = cooldown;
            CreateNewEnemy();

            cooldown = Mathf.Max(cooldownCap, cooldown - cooldownDecreseRate);
        }
    }

    private void CreateNewEnemy()
    {
        int respawnPointIndex = Random.Range(0, RespawnPoints.Length);
        Vector3 spawnPoint = RespawnPoints[respawnPointIndex].position;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        bool createdOnTheRight = newEnemy.transform.position.x > player.transform.position.x;

        if (createdOnTheRight)
        {
            newEnemy.GetComponent<Enemy>().flip();
        }
    }
}