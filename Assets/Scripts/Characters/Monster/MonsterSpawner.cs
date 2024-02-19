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

        for(int i = 0; i < 10; i++)
        {
            Monster monsterSpawn = Instantiate(monsterPrefab, RandomPosition(), Quaternion.identity, transform);
            
            monsterSpawnQueue.Enqueue(monsterSpawn);
        }

        //StartCoroutine(SpawnTime());
    }

    private Monster CreateMonster()
    {
        Monster monsterSpawn = Instantiate(monsterPrefab, RandomPosition(),transform.rotation, transform);
        monsterSpawnQueue.Enqueue(monsterSpawn);
        monsterSpawn.gameObject.SetActive(false);
        return monsterSpawn;
    }

    IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(3f);
        GetQueue();
    }

    public void ReturnQueue(Monster _gameObject)
    {
        monsterSpawnQueue.Enqueue(_gameObject);
        _gameObject.gameObject.SetActive(false);
        Invoke("GetQueue", 2f);
    }

    public Monster GetQueue()
    {
        if(monsterSpawnQueue.Count > 0)
        {
            Monster getGameObject = monsterSpawnQueue.Dequeue();
            getGameObject.transform.position = RandomPosition();
            getGameObject.gameObject.SetActive(true);
            return getGameObject;
        }
        else //큐에 남아있는 오브젝트가 없을 때 새로 만들어서 사용
        {
            Monster objectInPool = CreateMonster();
            objectInPool.gameObject.SetActive(true);
            objectInPool.gameObject.transform.SetParent(null);
            return objectInPool;
        }
    }

    private Vector3 RandomPosition()
    {
        Vector3 originPosition = transform.position;
        Vector3 size = rangeCollider.size;

        float spawnX = originPosition.x + Random.Range(-(size.x/2f), (size.x/2f));
        float spawnY = originPosition.y + Random.Range(-(size.y/2f), (size.y/2f));
        float spawnZ = originPosition.z + Random.Range(-(size.z/2f), (size.z/2f));

        Vector3 respawnPosition = new Vector3(spawnX, spawnY, spawnZ);

        return respawnPosition;
    }
}
