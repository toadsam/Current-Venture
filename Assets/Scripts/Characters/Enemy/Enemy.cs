using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { IDLE, TRACE, WALK, ATTACK, DIE };
    public EnemyState enemyState = EnemyState.IDLE;

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
    private Transform enemyTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
   
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        anim = GetComponent<Animator>();

        enemyTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = GetComponent<NavMeshAgent>();

        enemyState = EnemyState.IDLE;
        StartCoroutine(CheckEnemyState());
        StartCoroutine(EnemyAction());
    }

    IEnumerator CheckEnemyState()
    {
        while (curHP > 0)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if(dist <= attackDist)  enemyState = EnemyState.ATTACK;
            else if(dist <= traceDist)  enemyState = EnemyState.TRACE;
            else    enemyState = EnemyState.IDLE;
        }
    }

    IEnumerator EnemyAction()
    {
        while (curHP > 0)
        {
            switch (enemyState)
            {
                case EnemyState.IDLE:
                    nvAgent.isStopped = true;
                    anim.SetBool("isTrace", false);
                    break;
                case EnemyState.TRACE:
                    nvAgent.SetDestination(playerTr.position);
                    nvAgent.isStopped = false;
                    //anim.SetBool("isWalk", false);
                    anim.SetBool("isAttack", false);
                    anim.SetBool("isTrace", true);     
                    break;
                case EnemyState.ATTACK:
                    nvAgent.isStopped = true; 
                    anim.SetBool("isAttack", true);
                    float e_RotSpeed = 6.0f; //초당 회전 속도
                    Vector3 a_CacDir = playerTr.position - enemyTr.position;
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
    }
}
