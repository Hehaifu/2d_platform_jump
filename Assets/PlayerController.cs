using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] bool isOnGround;
    Rigidbody2D rigidbody;
    Vector2 userInput;
    Animator animator;
    BoxCollider2D feetCollider;
    [SerializeField] int jumpCount;
    [SerializeField] Transform shootingPos;
    [SerializeField] GameObject arrow;
    [SerializeField] float dashingTime = 0f;
    [SerializeField] float dashDuration = 0.5f;
    [SerializeField] float rollingDistance = 1f;
    [SerializeField] Transform meleePointGround;
    [SerializeField] float meleeRadiusGround;
    [SerializeField] Transform meleePointAir;
    [SerializeField] float meleeRadiusAir = 1f;
    [SerializeField] float attackInterval = 0.5f;
    [SerializeField] float attackTime = 0f;
    [SerializeField] bool attackAllowed;
    [SerializeField] float shootInterval;
    [SerializeField] float shootTime = 0f;
    [SerializeField] bool shootAllowed;
    float faceDirection = 1f;
    bool isDashing = false;
    [SerializeField] bool isRolling = false;
    [SerializeField] int meleeDamage = 40;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int health = 100;
    [SerializeField] float hitBlowEffect = 2f;
    [SerializeField] float hitBlowEffectTime = 0.2f;
    [SerializeField] bool canControl = true;
    [SerializeField] float freezeTime = 0f;
    [SerializeField] float freezeDuration = 0.5f;
    [SerializeField] AudioClip swordOneShot;
    [SerializeField] AudioClip swordAir;
    [SerializeField] AudioClip swordComb;
    [SerializeField] AudioClip swordHit;
    public bool isAlive = true;
    UIController uIController;
    GameSession gameSession;
    GameObject[] rebornPos;
    //need to be unlock



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        feetCollider = GetComponent<BoxCollider2D>();
        shootInterval = arrow.GetComponent<BulletSprite>().GetBulletInterval();
        uIController = FindObjectOfType<UIController>();
        gameSession = FindObjectOfType<GameSession>();
        rebornPos = GameObject.FindGameObjectsWithTag("reborn");
        isRolling = false;
        jumpCount = 0;
        attackTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        Run();
        CheckIfOnGround();
        FlipFace();
        Dash();
        CheckIfRolling();
        updateAttackTime();
        updateShootTime();
        CheckIfAttackInTheAir();
        updateFreezeTime();
    }

    void updateAttackTime()
    {
        attackTime += Time.deltaTime;
        if (attackTime < attackInterval)
        {
            attackAllowed = false;
        }
        else
        {
            attackAllowed = true;

        }
    }

    void updateFreezeTime()
    {
        if (freezeTime<freezeDuration)
        {
            canControl = false;
        }
        else
        {
            canControl = true;
        }
        freezeTime += Time.deltaTime;
    }

    void updateShootTime()
    {
        shootTime += Time.deltaTime;
        if (shootTime<shootInterval)
        {
            shootAllowed = false;
        }
        else
        {
            shootAllowed = true;
        }
    }
    void OnMove(InputValue value)
    {
        userInput = value.Get<Vector2>();
    }

    void Run()
    {
        if (!canControl) return;
        Vector2 move = new Vector2(userInput.x * moveSpeed, rigidbody.velocity.y);
        rigidbody.velocity = move;
        if (Mathf.Abs(userInput.x)>0)
        {
            animator.SetBool("run",true);
        }
        else
        {
            animator.SetBool("run", false);
        }
    }
    void FlipFace()
    {
        if (Mathf.Abs(rigidbody.velocity.x) < Mathf.Epsilon) return;
        faceDirection = Mathf.Sign(rigidbody.velocity.x);
        transform.localScale = new Vector3(faceDirection, 1, 1);
    }

    void CheckIfOnGround()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("platform")))
        {
            jumpCount = 0;
            animator.SetBool("ground", true);
            isOnGround = true;
        }
        else
        {
            animator.SetBool("ground", false);
            isOnGround = false;
        }
    }
    void CheckIfAttackInTheAir()
    {
        AnimatorStateInfo aniInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (aniInfo.IsName("Attack-Air_CrossBow") || aniInfo.IsName("Attack-Air_Gun"))
        {
            float horizontalSpeed = rigidbody.velocity.x;
            rigidbody.velocity = new Vector2(horizontalSpeed, 0f);
        }
    }
    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("platform")))
        {
            if (jumpCount >=gameSession.maxJumpCount-1)
            {
                return;
            }
            else if (value.isPressed)
            {
                float xspeed = rigidbody.velocity.x;
                rigidbody.velocity = new Vector2(xspeed, jumpSpeed);
                animator.SetTrigger("jump");
                jumpCount++;
                return;
            }
        }
        else
        {
            jumpCount = 0;
        }
        if (value.isPressed)
        {
            rigidbody.velocity += new Vector2(0f, jumpSpeed);
            animator.SetTrigger("jump");
            jumpCount++;
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) return;
        if (!shootAllowed) return;
        shootTime = 0f;
        if (value.isPressed)
        {
            animator.SetTrigger("shoot");
            GameObject newArrow = Instantiate(arrow);
            newArrow.transform.position = shootingPos.transform.position;
            newArrow.transform.localScale = new Vector3(faceDirection, 1, 1);
            newArrow.transform.localRotation = Quaternion.Euler(Vector3.zero);
            newArrow.GetComponent<ArrowScript>().arrowDirection = faceDirection;
        }
    }
    void OnAttack(InputValue value)
    {
        if (!attackAllowed) return;
        attackTime = 0f;
        animator.SetTrigger("attack");
        if (isDashing)
        {
            animator.SetBool("dashAttack", true);
            //AudioSource.PlayClipAtPoint(swordOneShot, Camera.main.transform.position);
        }
        else
        {
            animator.SetBool("dashAttcak", false);
            //AudioSource.PlayClipAtPoint(swordAir, Camera.main.transform.position);
        }
        Attack();
    }
    void Attack()
    {
        Collider2D[] hitEnemies;
        if (isOnGround)
        {
            hitEnemies = Physics2D.OverlapCircleAll(meleePointGround.position,meleeRadiusGround, LayerMask.GetMask("enemy"));
        }
        else
        {
            hitEnemies = Physics2D.OverlapCircleAll(meleePointAir.position, meleeRadiusAir, LayerMask.GetMask("enemy"));
        }
        Vector2 playerPosition = transform.position;
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<Enemy>().DeductLife(meleeDamage);
            Vector2 enemyPosition = enemy.gameObject.transform.position;
            Vector2 direction = (enemyPosition - playerPosition).normalized;
            enemy.GetComponent<Rigidbody2D>().velocity = direction * hitBlowEffect;
            enemy.gameObject.GetComponent<Enemy>().RecoverSpeedInSecounds(hitBlowEffectTime);


        }
    }
    void OnSlide()
    {
        animator.SetTrigger("slide");
    }

    void OnDash(InputValue value)
    {
        if (isDashing) return;
        if (value.isPressed)
        {
            isDashing = true;
            animator.SetTrigger("dash");
            dashingTime = 0f;
        }
    }
    void Dash()
    {
        if (!gameSession.isDashEnabled) return;
        if (!canControl) return;
        if (dashingTime>dashDuration)
        {
            isDashing = false;
            moveSpeed = 5f;
        }
        else
        {
            dashingTime += Time.deltaTime;
            moveSpeed = 10f;
            if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("platform")))
            {
                Vector2 velocity = rigidbody.velocity;
                rigidbody.velocity = new Vector2(velocity.x, 0f);
            }
        }
    }
    void OnRoll(InputValue value)
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("platform"))) return;
        if (value.isPressed)
        {
            animator.SetTrigger("roll");
        }
        
    }

    void CheckIfRolling()
    {
        AnimatorStateInfo aniInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (aniInfo.IsName("Roll_CrossBow")|| aniInfo.IsName("Roll_Gun"))
        {
            isRolling = true;
            transform.Translate(new Vector3(rollingDistance * faceDirection * Time.deltaTime,0f,0f));
        }
        else
        {
            isRolling = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (isOnGround)
        {
            Gizmos.DrawWireSphere(meleePointGround.position, meleeRadiusGround);
        }
        else
        {
            Gizmos.DrawWireSphere(meleePointAir.position, meleeRadiusAir);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;
        if (collision.gameObject.tag == "enemy")
        {
            int damage = collision.gameObject.GetComponent<Enemy>().GetDamage();
            health -= damage;
            animator.SetTrigger("hurt");
            freezeTime = 0f;
            float healthPercentage = health *1f/ maxHealth;
            uIController.UpdatePlayerhealthy(healthPercentage);
            if (health <=0)
            {
                animator.SetTrigger("death");
                gameSession.DeductPlayerlife();
                isAlive = false;
            }
        }
    }

    void SetJumpCount(int possibleJumpCount) 
    {
        gameSession.maxJumpCount = possibleJumpCount;
    }

    public void IncreaseJumpCount()
    {
        if (gameSession.maxJumpCount >= 3) return;
        gameSession.maxJumpCount++;
    }

    public void EnableDash()
    {
        gameSession.isDashEnabled = true;
    }
    public void Reborn()
    {
        if (animator == null) {
            animator = GetComponent<Animator>();
        }
        animator.SetTrigger("reborn");
        isAlive = true;
        Vector3 rebornPosition = FindClosestRebornPlace();
        transform.position = rebornPosition;
        health = maxHealth;
        uIController.UpdatePlayerhealthy(1f);
    }

    Vector3 FindClosestRebornPlace()
    {
        float minDist = 10000000f;
        Vector3 targetPos = rebornPos[0].transform.position;
        foreach(GameObject pos in rebornPos)
        {
            if (pos.transform.position.x > transform.position.x) continue;
            float dist = Vector3.Distance(transform.position, pos.transform.position);
            if (dist<minDist)
            {
                minDist = dist;
                targetPos = pos.transform.position;
            }
        }
        return targetPos;
    }
}
