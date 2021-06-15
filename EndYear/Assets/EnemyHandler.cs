using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    ParticleSystem muzzleFlash;
    public float damage = 10f;
    private System.Random ran;
    private float timeBetweenAttacks;
    private bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttack;
    
    void Start() {
        ran = new System.Random();
    }

    private void Awake()
    {
        player = GameObject.Find("Main Character").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSightRange && !playerInAttack) Patrolling();
        if(playerInSightRange && !playerInSightRange) ChasePlayer();
        if(playerInAttack && playerInSightRange) AttackPlayer();
    }

    private void Patrolling() {
        if(!walkPointSet) SearchWalkPoint();
    }

    private void SearchWalkPoint() {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }
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

    void Shoot() {
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, attackRange)){
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            target.TakeDamage(damage);
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }
}
