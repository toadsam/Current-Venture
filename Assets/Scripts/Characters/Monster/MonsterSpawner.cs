using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner instance;
    public Monster monsterPrefab;
    public Player player;
    
    public Queue<Monster> monsterSpawnQueue = new Queue<Monster>();

    BoxCollider rangeCollider;

    private void Awake()
    {
        instance = this;

        rangeCollider = GetComponent<BoxCollider>();

        StartCoroutine(SpawnTime());
    }
    private void CreateMonster(int _count)
    {
        for(int i = 0; i < _count; i++)
        {
            Monster monsterSpawn = Instantiate(monsterPrefab, RandomPosition(),transform.rotation, transform);
            
            monsterSpawnQueue.Enqueue(monsterSpawn);
        }
    }
    IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(3f);
        CreateMonster(10);
    }
    public void ReturnQueue(Monster _gameObject)
    {
        _gameObject.gameObject.SetActive(false);
        monsterSpawnQueue.Enqueue(_gameObject);
        Invoke("GetQueue", 7f);
    }

    public Monster GetQueue()
    {
        Monster getGameObject = monsterSpawnQueue.Dequeue();
        gameObject.transform.position = RandomPosition();
        getGameObject.gameObject.SetActive(true);
        return getGameObject;
    }

    private Vector3 RandomPosition()
    {
        Vector3 originPosition = transform.position;

        float spawnX = Random.Range((rangeCollider.bounds.size.x /2)*-1, (rangeCollider.bounds.size.x/2));
        float spawnZ = Random.Range((rangeCollider.bounds.size.z /2)*-1, (rangeCollider.bounds.size.z/2));

        Vector3 newPosition = new Vector3(spawnX, 0f, spawnZ);
        Vector3 respawnPosition = originPosition + newPosition;

        return respawnPosition;
    }
}
