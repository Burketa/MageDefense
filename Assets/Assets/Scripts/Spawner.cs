using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool canSpawn = true;
    public GameObject enemyPrefab;

    public float spawnTime = 1f;

    [Space]

    [SerializeField]
    private int multiplier = 1;

    [SerializeField]
    public int spawned = 0;

    private float currentSpawnTime;

    private void Start()
    {
        currentSpawnTime = spawnTime;
    }

    void Update()
    {
        if (canSpawn)
        {
            if (currentSpawnTime >= spawnTime)
            {
                var enemyInst = Instantiate(enemyPrefab, transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-3, 3), 0), transform.rotation, transform);
                var enemy = enemyInst.GetComponent<Enemy>();

                //Adiciona a barra de vida
                FindObjectOfType<UI>().AddHealthbar(enemy);

                spawned++;

                UpgradeEnemy(enemy);

                if (Random.value > 0.3f)
                    spawnTime *= 1f - 0.005f;
                spawnTime = Mathf.Clamp(spawnTime, 0.3f, 3f);
                currentSpawnTime = 0;
            }
            else
                currentSpawnTime += Time.deltaTime;
        }
    }

    public void UpgradeEnemy(Enemy enemy)
    {
        if (spawned > Random.Range(8, 12) * multiplier)
            multiplier++;
        if (Random.value < 0.3f)
        {
            enemy.atk *= Mathf.FloorToInt(multiplier * 0.5f);
            enemy.def *= Mathf.FloorToInt(multiplier * 0.5f);
            enemy.maxHealth *= Mathf.FloorToInt(multiplier * 0.5f);
            enemy.maxHealth = Mathf.Clamp(enemy.maxHealth, 1, enemy.maxHealth);
            enemy.currentHealth = enemy.maxHealth;
        }
        else
        {
            enemy.atk *= Mathf.FloorToInt(multiplier * 0.3f);
            enemy.def *= Mathf.FloorToInt(multiplier * 0.3f);
            enemy.maxHealth *= Mathf.FloorToInt(multiplier * 0.1f);
            enemy.maxHealth = Mathf.Clamp(enemy.maxHealth, 1, enemy.maxHealth);
            enemy.currentHealth = enemy.maxHealth;
        }
    }
}
