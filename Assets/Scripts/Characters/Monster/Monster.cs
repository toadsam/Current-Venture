using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public enum MonsterState { IDLE, TRACE, ATTACK, DIE };
    public MonsterState monsterState = MonsterState.IDLE;

    public string monsterName;

    public int maxHP;
    public int curHP;
    public float traceDist;
    public float attackDist;
    public float speed;
    private Vector3 pos;

    //원거리 공격
    public GameObject firePrefab;
    public GameObject mouthTransform;
    public float bulletSpawnRateMin = 0.5f;
    public float bulletSpawnRateMax = 3f;
    private float bulletSpawnRate = 1f;
    private float timeAfterSpawn;

    private Rigidbody rigid;
    private BoxCollider boxCollider;
    private Animator anim;
    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
   
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();

        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = GetComponent<NavMeshAgent>();

        timeAfterSpawn = 0f;
        // bulletSpawnRate = Random.Range(bulletSpawnRateMin, bulletSpawnRateMax);

        monsterState = MonsterState.IDLE;
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    void OnEnable()
    {
        nvAgent = GetComponent<NavMeshAgent>();
        curHP = maxHP;
        timeAfterSpawn = 0f;
        // bulletSpawnRate = Random.Range(bulletSpawnRateMin, bulletSpawnRateMax);
    }

    IEnumerator CheckMonsterState()
    {
        while (curHP > 0)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if(dist <= attackDist)  monsterState = MonsterState.ATTACK;
            else if(dist <= traceDist)  monsterState = MonsterState.TRACE;
            else    monsterState = MonsterState.IDLE;
        }
    }

    IEnumerator MonsterAction()
    {
        while (curHP > 0)
        {
            yield return StartCoroutine(monsterState.ToString());    
        }
        if(curHP <= 0){
            MonsterSpawner.instance.ReturnQueue(this);
        }
    }

#region UNITY_EVENTS
    IEnumerator IDLE()
    {
        nvAgent.isStopped = true;
        anim.SetBool("isTrace", false);
        anim.SetBool("isAttack", false);

        // yield return new WaitForSeconds(10.0f);

        // pos = new Vector3();
        // pos.x = Random.Range(-3f, 3f);
        // pos.z = Random.Range(-3f, 3f);

        // anim.SetBool("isWalk", true);

        // while(true)
        // {
        //     var dir = (pos - monsterTr.position).normalized;
        //     monsterTr.LookAt(pos);
        //     monsterTr.position += dir * speed * Time.deltaTime;

        //     float distance = Vector3.Distance(monsterTr.position, pos);
        //     if(distance <= 0.1f)
        //     {
        //         anim.SetBool("isWalk", false);
        //         break;
        //         // yield return new WaitForSeconds(Random.Range(1f, 3f));
        //         // anim.SetBool("isWalk", true);
        //         // pos.x = Random.Range(-3f, 3f);
        //         // pos.z = Random.Range(-3f, 3f);
        //     }
        // }

        yield return null;  
    }

    IEnumerator TRACE()
    {
        nvAgent.SetDestination(playerTr.position);
        nvAgent.isStopped = false;
        // anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", false);
        anim.SetBool("isTrace", true);

        yield return null;  
    }

    IEnumerator ATTACK()
    {
        nvAgent.isStopped = true; 
        anim.SetBool("isAttack", true);
        float e_RotSpeed = 6.0f; //초당 회전 속도
        Vector3 a_CacDir = playerTr.position - monsterTr.position;
        a_CacDir.y = 0.0f;
        //a_CacDir의 벡터 길이가 0.0f보다 크면 해당 벡터를 바라보도록 함
        if(0.0f < a_CacDir.magnitude)
        {
            Quaternion a_TargetRot = Quaternion.LookRotation(a_CacDir.normalized);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, a_TargetRot, Time.deltaTime * e_RotSpeed);
        }

        if(monsterName == "Shoot")
        {
            // Vector3 mouthPos = fire.transform.parent.transform.position;
            // fire.transform.position = mouthPos;
            // fire.SetActive(true);
            // Quaternion a_TargetRot = Quaternion.LookRotation(a_CacDir.normalized);
            // fire.transform.rotation = Quaternion.Slerp(
            //     transform.rotation, a_TargetRot, Time.deltaTime * e_RotSpeed);
            timeAfterSpawn += Time.deltaTime;
            if(timeAfterSpawn >= bulletSpawnRate)
            {
                timeAfterSpawn = 0;
                GameObject fireObj = Instantiate(firePrefab, mouthTransform.transform.position, mouthTransform.transform.rotation);
                fireObj.transform.LookAt(playerTr);
            }

        }
        yield return null;  
    }
#endregion
}
