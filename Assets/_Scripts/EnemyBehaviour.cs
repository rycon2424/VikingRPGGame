using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyBehaviour : EnemyPawn
{
    [Space]
    public bool friendly;
    [Space]
    public float runDistance = 5;
    public float attackDistance = 2f;
    public float lookDistance = 5;
    public float hearDistance = 2;
    [Header("Animations")]
    public int deadAnimations = 1;
    public int attackAnimations = 5;
    [Space]
    [Range(0, 180)] public int viewAngle = 70;
    [Range(0, 1)] public float stunTime;
    [Range(0, 2)] public float minThinkTime;
    [Range(0, 5)] public float maxThinkTime;
    [Range(0, 20)] public float chaseTime;
    [Space]
    public EnemyStates currentState;
    public enum EnemyStates { normal, chasing, inCombat, dead }
    public Transform[] dropWeapon;

    [Header("Patrol settings")]
    public bool oldPost;
    private Vector3 oldPostPos;
    public Transform[] patrolPoints;

    [Header("PatrolDebug")]
    [SerializeField] private Mesh debugPos;
    [SerializeField] private Color32 colorDebug;

    [Header("Private Values")]
    [SerializeField] bool showPrivate;

    [Header("Private/Dont Assign")]
    [ShowIf("showPrivate")] [SerializeField] [ReadOnly] int notWalkedStraight;
    [ShowIf("showPrivate")] [ReadOnly]                  public Transform player;
    [ShowIf("showPrivate")] [SerializeField] [ReadOnly] bool targetOfPlayer;
    [ShowIf("showPrivate")] [SerializeField] [ReadOnly] bool attackCooldown;
    [ShowIf("showPrivate")] [SerializeField] [ReadOnly] bool lostPlayer;
    [ShowIf("showPrivate")] [SerializeField] [ReadOnly] bool walkBehaviour;
    [ShowIf("showPrivate")] [SerializeField] [ReadOnly] bool rotateCooldown;
    [ShowIf("showPrivate")] [SerializeField] [ReadOnly] float randomInt;
    [ShowIf("showPrivate")] [SerializeField] [ReadOnly] float distancePlayer;

    private NavMeshAgent agent;
    private PlayerBehaviour pb;

    void Start()
    {
        pb = FindObjectOfType<PlayerBehaviour>();
        player = pb.transform;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        oldPostPos = transform.position;

        SwitchState(EnemyStates.normal);
    }

    void Update()
    {
        if (dead)
        {
            return;
        }
        distancePlayer = Vector3.Distance(transform.position, player.position);
        switch (currentState)
        {
            case EnemyStates.normal:
                Patrol();
                break;
            case EnemyStates.chasing:
                InChase();
                break;
            case EnemyStates.inCombat:
                InCombat();
                break;
            default:
                break;
        }
    }

    public void GotParried()
    {
        TakeDamage(0);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (dead == false)
        {
            health -= damage;

            AttackStunned();
            anim.Play("Damaged");
            if (health < 1)
            {
                currentState = EnemyStates.dead;
                dead = true;
                health = 0;
                Death();
            }
        }
    }

    public override void Death()
    {
        OrbitCamera oc = FindObjectOfType<OrbitCamera>();

        anim.SetBool("Dead", true);

        anim.SetInteger("RollType", Random.Range(1, deadAnimations + 1));
        anim.SetTrigger("Death");

        StopCoroutine("WalkDirection");

        GetComponent<CharacterController>().enabled = false;

        foreach (var weapon in dropWeapon)
        {
            weapon.parent = null;
            weapon.GetComponent<Rigidbody>().isKinematic = false;
            weapon.GetComponent<Collider>().enabled = true;
        }
    }

    void SwitchState(EnemyStates es)
    {
        currentState = es;
        anim.SetInteger("WalkingDir", 0);
        switch (currentState)
        {
            case EnemyStates.normal:
                lostPlayer = false;
                anim.applyRootMotion = false;
                agent.enabled = true;
                agent.speed = 1;

                if (patrolPoints.Length > 0)
                {
                    StartCoroutine("Patrolling");
                }
                else
                {
                    if (oldPost)
                    {
                        StartCoroutine("GoToPost");
                    }
                    else
                    {
                        anim.SetBool("Walking", false);
                        agent.SetDestination(transform.position);
                    }
                }

                anim.SetBool("inCombat", false);

                StopCoroutine("WalkDirection");
                break;

            case EnemyStates.chasing:
                lostPlayer = false;
                anim.applyRootMotion = false;
                agent.enabled = true;

                anim.SetBool("inCombat", false);

                StopCoroutine("Patrolling");
                StopCoroutine("WalkDirection");
                StopCoroutine("GoToPost");
                break;

            case EnemyStates.inCombat:
                lostPlayer = false;
                AttackStunned();

                anim.applyRootMotion = true;
                agent.enabled = false;

                anim.SetBool("inCombat", true);
                anim.SetBool("Walking", true);
                anim.SetInteger("WalkingDir", 1);

                walkBehaviour = false;

                StopCoroutine("Patrolling");
                StopCoroutine("GoToPost");

                StartCoroutine("WalkDirection");
                break;

            default:
                break;
        }
    }

    public bool PlayerInSight()
    {
        float angel = Vector3.Angle(transform.forward, player.position - transform.position);
        if (angel <= viewAngle)
        {
            RaycastHit hit;
            Vector3 dir = (player.transform.position - transform.position).normalized;
            Ray ray = new Ray(transform.position + Vector3.up, dir);
            Debug.DrawRay(transform.position + Vector3.up, dir * lookDistance, Color.black);
            if (Physics.Raycast(ray, out hit, lookDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }

    #region patrolling

    IEnumerator Patrolling()
    {
        Vector3 randomWayPoint = patrolPoints[Random.Range(0, patrolPoints.Length)].position;
        agent.SetDestination(randomWayPoint);
        anim.SetBool("Walking", true);
        while (Vector3.Distance(randomWayPoint, transform.position) > 1)
        {
            yield return new WaitForSeconds(0.25f);
        }
        anim.SetBool("Walking", false);
        yield return new WaitForSeconds(Random.Range(minThinkTime, maxThinkTime));
        StartCoroutine("Patrolling");
    }

    IEnumerator GoToPost()
    {
        agent.SetDestination(oldPostPos);
        anim.SetBool("Walking", true);
        while (Vector3.Distance(oldPostPos, transform.position) > 1)
        {
            yield return new WaitForSeconds(0.25f);
        }
        anim.SetBool("Walking", false);
    }

    void Patrol()
    {
        if (distancePlayer <= hearDistance)
        {
            SwitchState(EnemyStates.inCombat);
        }
        if (friendly == false)
        {
            if (PlayerInSight())
            {
                SwitchState(EnemyStates.chasing);
            }
        }
    }

    #endregion

    #region Chase

    void InChase()
    {
        agent.SetDestination(player.position);
        if (distancePlayer < runDistance)
        {
            if (PlayerInSight())
            {
                SwitchState(EnemyStates.inCombat);
            }
        }
        else if (distancePlayer > runDistance + 2)
        {
            agent.speed = 3;
            anim.SetBool("Walking", false);
            anim.SetBool("Running", true);
        }
        else if (distancePlayer > runDistance - 1)
        {
            agent.speed = 1;
            anim.SetBool("Running", false);
            anim.SetBool("Walking", true);
        }
        if (lostPlayer == false)
        {
            if (PlayerInSight() == false)
            {
                lostPlayer = true;
                StartCoroutine("LosingPlayer");
            }
        }
        if (lostPlayer)
        {
            if (PlayerInSight())
            {
                lostPlayer = false;
                StopCoroutine("LosingPlayer");
            }
        }
    }

    IEnumerator LosingPlayer()
    {
        yield return new WaitForSeconds(chaseTime);
        anim.SetBool("Running", false);
        anim.SetBool("Walking", true);
        SwitchState(EnemyStates.normal);
    }

    #endregion

    #region Combat

    void InCombat()
    {
        if (PlayerInSight() == false)
        {
            anim.SetBool("Walking", true);
            SwitchState(EnemyStates.chasing);
            return;
        }
        if (rotateCooldown == false)
        {
            RotateTowardsPlayer();
        }
        if (distancePlayer > (runDistance + 1f))
        {
            if (attackCooldown == false)
            {
                SwitchState(EnemyStates.chasing);
            }
        }
        else if (distancePlayer > attackDistance)
        {
            StartWalking();
        }
        else
        {
            if (attackCooldown == false)
            {
                Attack();
            }
            StopWalking();
        }
    }

    void Attack()
    {
        anim.SetInteger("AttackType", Random.Range(1, attackAnimations + 1));
        anim.SetTrigger("Attack");
        StartCoroutine(AttackCld());
    }

    IEnumerator AttackCld()
    {
        attackCooldown = true;
        rotateCooldown = true;
        yield return new WaitForSeconds(1.2f);
        rotateCooldown = false;
        yield return new WaitForSeconds(Random.Range(minThinkTime, maxThinkTime));
        attackCooldown = false;
    }

    void AttackStunned()
    {
        if (attackCooldown == false)
        {
            attackCooldown = true;
            StopCoroutine("Stunned");
            StartCoroutine("Stunned");
        }
    }

    IEnumerator Stunned()
    {
        yield return new WaitForSeconds(stunTime);
        attackCooldown = false;
    }

    void StopWalking()
    {
        if (walkBehaviour == true)
        {
            StopCoroutine("WalkDirection");
            anim.SetBool("Walking", false);
            walkBehaviour = false;
        }
    }

    void StartWalking()
    {
        if (walkBehaviour == false)
        {
            StartCoroutine("WalkDirection");
        }
    }

    IEnumerator WalkDirection()
    {
        walkBehaviour = true;

        randomInt = Random.Range(1.5f, 3.0f);
        yield return new WaitForSeconds(randomInt);

        anim.SetBool("Walking", false);

        randomInt = Random.Range(0.5f, 2.0f);
        yield return new WaitForSeconds(randomInt);

        int randomWalkDir = Random.Range(1, 4);
        if (randomWalkDir != 1)
        {
            notWalkedStraight++;
            if (notWalkedStraight > 2)
            {
                randomWalkDir = 1;
                notWalkedStraight = 0;
            }
        }
        else
        {
            notWalkedStraight = 0;
        }
        anim.SetBool("Walking", true);
        anim.SetInteger("WalkingDir", randomWalkDir);

        StartCoroutine("WalkDirection");
    }

    public void RotateTowardsPlayer()
    {
        Quaternion newLookAt = Quaternion.LookRotation(player.position - transform.position);
        newLookAt.x = 0;
        newLookAt.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, newLookAt, Time.deltaTime * 5);
    }

    public void _DealDamage(float range)
    {
        if (Vector3.Distance(transform.position, player.position) <= (1 + range))
        {
            pb.TakeDamage(10);
        }
    }

    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, hearDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * 1.2f, transform.forward * lookDistance);

        Gizmos.DrawRay(transform.position + Vector3.up * 1.2f, (transform.forward + ((transform.right * ((float)viewAngle / 100)) * 2)).normalized * lookDistance);
        Gizmos.DrawRay(transform.position + Vector3.up * 1.2f, (transform.forward + ((-transform.right * ((float)viewAngle / 100)) * 2)).normalized * lookDistance);

        Gizmos.color = Color.cyan;

        Vector3 pointRight = transform.position + Vector3.up * 1.2f + (transform.forward + ((transform.right * ((float)viewAngle / 100)) * 2)).normalized * lookDistance;
        Gizmos.DrawSphere(pointRight, 0.1f);

        Vector3 pointLeft = transform.position + Vector3.up * 1.2f + (transform.forward + ((-transform.right * ((float)viewAngle / 100)) * 2)).normalized * lookDistance;
        Gizmos.DrawSphere(pointLeft, 0.1f);

        Vector3 middlePoint = transform.position + Vector3.up * 1.2f + transform.forward * lookDistance;
        Gizmos.DrawSphere(middlePoint, 0.1f);

        Gizmos.DrawLine(middlePoint, pointLeft);
        Gizmos.DrawLine(middlePoint, pointRight);

        Gizmos.color = colorDebug;

        if (patrolPoints.Length > 0)
        {
            foreach (var t in patrolPoints)
            {
                Gizmos.DrawWireMesh(debugPos, t.position - Vector3.up);
            }
        }
    }
}
