using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public enum MonsterState { IDLE, TRACE, WALK, ATTACK, DIE };
    public MonsterState monsterState = MonsterState.IDLE;

    public int maxHP;
    public int curHP;
    public float traceDist;
    public float attackDist;
    public float speed;
    private Vector3 pos;
    private bool check;

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

        monsterState = MonsterState.IDLE;
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    void OnEnable()
    {
        nvAgent = GetComponent<NavMeshAgent>();
        curHP = maxHP;
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
            switch (monsterState)
            {
                case MonsterState.IDLE:
                    nvAgent.isStopped = true;
                    anim.SetBool("isTrace", false);
                    break;
                case MonsterState.TRACE:
                    nvAgent.SetDestination(playerTr.position);
                    nvAgent.isStopped = false;
                    //anim.SetBool("isWalk", false);
                    anim.SetBool("isAttack", false);
                    anim.SetBool("isTrace", true);     
                    break;
                case MonsterState.ATTACK:
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
                    break;
            }
            yield return null;     
        }
        if(curHP <= 0){
            MonsterSpawner.instance.ReturnQueue(this);
        }
    }
}
