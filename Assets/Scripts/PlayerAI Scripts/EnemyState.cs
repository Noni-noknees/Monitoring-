using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour
{
    // public Transform player;
    public GameObject player;
    public  NavMeshAgent navMeshAgent;
    public List<GameObject> waypoints;

    public enum State { Patrol, Chase, Attack, Retreat, Aggressive }
    private State currentState;
    public float speed = 2f;
    public float chaseRange = 10f;
    public float detectionDistance = 2f;
    public float rotationSpeed = 2f;
    private int currentWaypoint = 0;
    private float distanceToPlayer;
    private Vector3 targetPosition;
    // private PlayerStates playerScript; //used to access treasure count
    //private HealthManager playerHealthScript; //used to check if player has lost
    private EnemyHealth enemyHealthScript; //used to checkon enemy's health

    public float attackRange = 5f;
    public GameObject projectilePrefab; // Drag the projectile prefab in the inspector
    public GameObject flashlight;
    public bool flashlightStatus = false;
    public float projectileSpeed = 10f;
    private int flashlightCount = 0; //num times player presses f

    private spawner waves;
    private int currentWave = 0;
    private float[,] transitionProb = {
        {0.8f, 0.2f},
        {0.2f, 0.8f}
    };



    
    void Start()
    {
        navMeshAgent.autoBraking = false;
        waypoints.AddRange(GameObject.FindGameObjectsWithTag("WayPoint"));
        Debug.Log("There are these many waypoints " + waypoints.Count);
        player = GameObject.FindGameObjectWithTag("Player");
        currentState = State.Patrol;
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (waypoints.Count > 0)
        {
            targetPosition = waypoints[currentWaypoint].transform.position;
        }
        // playerScript = player.GetComponent<PlayerStates>();
        //playerHealthScript = player.GetComponent<HealthManager>();
        enemyHealthScript = GetComponent<EnemyHealth>();

        navMeshAgent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 target = new Vector3(10f, 0f, 10f); // Set a fixed target position
        //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move the enemy towards the target position
        navMeshAgent.SetDestination(targetPosition);
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlightStatus = !flashlightStatus;

            if (flashlightStatus)
            {
                flashlightCount++;
                transitionProb[0, 1] = Mathf.Clamp(0.8f + (flashlightCount * 0.1f), 0f, 1f);
                transitionProb[0, 0] = 1f - transitionProb[0, 1];
            }
            else
            {
                transitionProb[0, 1] = 0.8f;
                transitionProb[0, 0] = 0.2f;
            }


            switch (currentState)
            {
                case State.Patrol:
                    Patrol();
                    //   Debug.Log("Patrol State is running");
                    if (PlayerDetected())
                    {
                        float randomValue = Random.value;
                        if (randomValue < transitionProb[0, 1])
                        {
                            currentState = State.Chase;
                            Debug.Log("Enemy has entered Chase State.");
                        }
                    }
                    break;

                case State.Chase:
                    Chase();
                    Debug.Log("Chase State is running");
                    if (InAttackRange())
                    {
                        currentState = State.Attack;
                        Debug.Log("Enemy has entered Attack State");
                    }
                    else if (LowHealth() && !IsAggressive())
                    {
                        currentState = State.Retreat;
                    }
                    break;

                case State.Attack:
                    Attack();
                    Debug.Log("Attack State is running");
                    if (PlayerDetected() == false)
                    {
                        currentState = State.Patrol;
                        Debug.Log("Enemy has entered Patrol State");
                    }
                    else if (LowHealth() && !IsAggressive())
                    {
                        currentState = State.Retreat;
                        Debug.Log("Enemy has entered Retreat State");
                    }
                    break;

                case State.Retreat:
                    Retreat();
                    Debug.Log("Retreat state is running");
                    if (distanceToPlayer > chaseRange) currentState = State.Patrol;
                    break;

                case State.Aggressive:
                    Aggressive();
                    break;
            }
            if (currentWave >= 2 && !IsAggressive())
            {
                currentState = State.Aggressive;
            }
        }

        //Patrol State

    }
    private void Patrol()
    {
        navMeshAgent.SetDestination(targetPosition);
       
        // Check if reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SelectNextWaypoint();
        }
    }

    

    private void SelectNextWaypoint()
    {

        if (waypoints.Count == 0)
            return;
        var Rnd = new System.Random();
        var w = Rnd.Next(0, 6);
        currentWaypoint = (currentWaypoint + w) % waypoints.Count;
        targetPosition = waypoints[currentWaypoint].transform.position;
    }


    // Optional: Visualize Raycast in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionDistance);
    }

    private bool PlayerDetected()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= chaseRange;
    }

    //Chase state
    private void Chase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < chaseRange)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    private bool InAttackRange()
    {
        return distanceToPlayer <= attackRange;
    }
    //Attack State
    private void Attack()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, attackRange))
            {
                if (hit.transform == player)
                {
                    AttackPlayer();
                }
            }
        }

    }

    private void AttackPlayer()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 1.0f; // Adjust the offset as needed
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = (player.transform.position - transform.position).normalized * projectileSpeed * 4;


        Destroy(projectile, 1.5f); // Destroy the projectile after 2 seconds
    }

    private bool LowHealth()
    {
        return enemyHealthScript.health <= 6;
    }

    private bool IsAggressive()
    {
        return currentState == State.Aggressive;
    }

    //Retreat State
    private void Retreat()
    {
        Vector3 directionAway = transform.position - player.transform.position;
        Vector3 retreatPosition = transform.position + directionAway.normalized * chaseRange;

        navMeshAgent.SetDestination(retreatPosition);

        if (distanceToPlayer > chaseRange * 1.5f)
        {
            currentState = State.Patrol;
        }
    }

    private void Aggressive()
    {
        // Increase stats based on current wave
        speed = 2f + (waves.wave * 0.5f);
        chaseRange = 10f + (waves.wave * 2f);

        // Aggressive behavior
        if (PlayerDetected())
        {
            Chase();
        }
        if (InAttackRange())
        {
            Attack();
        }
    }
}
