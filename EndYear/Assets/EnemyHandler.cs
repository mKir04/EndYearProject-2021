using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    //Patrolling
    private bool patrolling;

    //Attacking
    public ParticleSystem muzzleFlash;
    private float damage = 10f;
    private float timeBetweenAttacks = 0.4f;
    private bool alreadyAttacked;

    // States
    private float sightRange = 40f;
    private float attackRange = 15f;
    public bool playerInSightRange, playerInAttack;
    
    private void Awake()
    {
        player = GameObject.Find("Main Character").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        playerInSightRange = Vector3.Distance(player.position, transform.position) <= sightRange;
        playerInAttack = Vector3.Distance(player.position, transform.position) <= attackRange;;

        if(playerInSightRange && !playerInAttack) ChasePlayer();
        if(playerInAttack) AttackPlayer();
        if(!playerInAttack && !playerInSightRange) Patrol();
    }

    private void ChasePlayer() {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer(){
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        
        if(!alreadyAttacked) {
            Shoot();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void Patrol() {
        float randomX = transform.position.x + Random.Range(-20, 20);
        float randomZ = transform.position.z + Random.Range(-20, 20);
        if(!patrolling) {
            agent.SetDestination(new Vector3(randomX, transform.position.y, randomZ));
            patrolling = true;
        }
        else {
            Invoke(nameof(bePatrolling), 6f);
        }
    }

    void Shoot() {
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange)){
            Target target = hit.transform.GetComponent<Target>();
            if(Random.Range(0, 50) <= 39) target.TakeDamage(damage);
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    private void bePatrolling() {
        patrolling = false;
    }
}
