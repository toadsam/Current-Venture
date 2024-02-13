using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    
    private Rigidbody rigid;
    private BoxCollider boxCollider;
    private Transform enemyTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;

    enum EnemyState { IDLE, CHASE, ATTACK, KILLED };
   
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        enemyTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        nvAgent.destination = playerTr.position;
    }
}
