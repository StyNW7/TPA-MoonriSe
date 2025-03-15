using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Health System
    public float maxHealth = 100f;
    private float currentHealth;
    public ZombieBar zombieBar;
    public GameObject xpLootPrefab;

    // Level System
    public int zombieLevel;
    public TMP_Text levelText;
    public Color greenColor = Color.green;
    public Color orangeColor = new Color(1f, 0.5f, 0f);
    public Color redColor = Color.red;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 15f;

    // Attacking
    public float attackCooldown = 7f;
    bool alreadyAttacked;
    public int attackDamage = 10;

    // Zombie States
    public float sightRange = 15f, attackRange = 1.5f;
    public bool playerInSightRange, playerInAttackRange;

    // Movement Speeds
    public float walkingSpeed = 1f;
    public float chasingSpeed = 3f;
    public float walkingAngularSpeed = 60f;
    public float chasingAngularSpeed = 85f;

    // Screaming
    public float screamingCooldown = 15f;
    private float lastScreamTime;

    // Animations
    public Animator animator;
    public bool isZombieIdling;

    // Player
    public PlayerManager playerManager;

    public LootSpawner lootSpawner;

    public GameObject damageTextPrefab; // Prefab DamageText
    public Transform damageTextSpawnPoint; // Posisi spawn damage text

    public MusicManager playerMusic;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //zombieBar = Get<ZombieBar>();
        player = FindObjectOfType<ThirdPersonController>().transform;
        playerManager = FindObjectOfType<PlayerManager>();
        lootSpawner = FindObjectOfType<LootSpawner>();
        animator = GetComponent<Animator>();

        SetZombieLevel();

        // maxHealth = CalculateHealth();
        currentHealth = maxHealth;
        
        UpdateZombieColor();

        if (zombieBar != null)
        {
            zombieBar.InitializeBars(maxHealth);
        }

        // Set default walk animation
        animator.SetBool("isWalking", true);
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        if (Time.time - lastScreamTime > screamingCooldown)
        {
            StartCoroutine(Scream());
            lastScreamTime = Time.time;
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < 2f)
            walkPointSet = false;

        agent.speed = walkingSpeed;
        agent.angularSpeed = walkingAngularSpeed;

        ResetAllAnimation();
        animator.SetBool("isWalking", true);
        
        isZombieIdling = true;
    }

    float CalculateHealth()
    {
        return Mathf.FloorToInt(150 * Mathf.Pow(zombieLevel, 1.5f) * zombieLevel);
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = chasingSpeed;
        agent.angularSpeed = chasingAngularSpeed;

        // ResetAllAnimation();
        ResetAnimationExcept("isChasing");
        animator.SetBool("isChasing", true);
        isZombieIdling = false;
    }

    private void AttackPlayer()
    {

        ResetAllAnimation();
        animator.SetBool("isAttacking", true);
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            int attackType = Random.Range(0, 2);
            if (attackType == 0)
                animator.SetTrigger("attackArm");
            else
                animator.SetTrigger("attackLeg");

            playerManager.TakeDamage(attackDamage);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }

        isZombieIdling = false;

    }

    public bool GetZombieIdle()
    {
        return isZombieIdling;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        //animator.SetBool("isAttacking", false);
        // ResetAnimationExcept("isAttacking");
        ResetAllAnimation();
    }

    private IEnumerator Scream()
    {
        Debug.Log("Zombie Screaming di AI");
        animator.SetTrigger("isScream");
        yield return new WaitForSeconds(2f);
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        zombieBar.UpdateHealth(currentHealth);

        ShowDamageText(damage);

        if (currentHealth <= 0) StartCoroutine(Die());
        else animator.SetTrigger("getHit");

    }

    IEnumerator Die()
    {

        ResetAnimationExcept("isDead");
        animator.SetBool("isDead", true);

        yield return new WaitForSeconds(0.2f);

        agent.isStopped = true;

        yield return new WaitForSeconds(1.75f);
        animator.enabled = false;

        lootSpawner.SpawnXP(transform.position, zombieLevel);
        Destroy(gameObject, 1f);

    }

    private void SetZombieLevel()
    {
        int worldLevel = playerManager.GetLevel();

        if (worldLevel == 1)
            zombieLevel = Random.Range(1, 20);
        else if (worldLevel == 2)
            zombieLevel = Random.Range(21, 45);
        else if (worldLevel == 3)
            zombieLevel = Random.Range(46, 70);
        else if (worldLevel == 4)
            zombieLevel = Random.Range(71, 90);

        levelText.text = "Lv. " + zombieLevel;
    }

    private void UpdateZombieColor()
    {
        int playerLevel = playerManager.GetLevel();
        if (zombieLevel <= playerLevel)
            levelText.color = greenColor;
        else if (zombieLevel > playerLevel && zombieLevel <= playerLevel + 5)
            levelText.color = orangeColor;
        else
            levelText.color = redColor;
    }

    public float GetHealth() => currentHealth;

    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab)
        {
            GameObject dmgText = Instantiate(damageTextPrefab, damageTextSpawnPoint.position, Quaternion.identity);
            dmgText.GetComponent<DamageText>().SetDamage(damage);
            dmgText.GetComponent<DamageText>().SetColor(Color.white);
        }
    }

    public void ResetAllAnimation()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isChasing", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdleSpecial", false);
    }

    public void ResetAnimationExcept(string animationToKeep)
    {
        string[] animations = { "isWalking", "isChasing", "isAttacking", "isDead", "isIdleSpecial" };
        foreach (string anim in animations)
        {
            if (anim != animationToKeep)
                animator.SetBool(anim, false);
        }
    }


}
