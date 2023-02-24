using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyInMonster : MonoBehaviour,Enemy
{
    SpriteRenderer renderer;
    Rigidbody2D rigidbody;
    [SerializeField] Transform[] positionList;
    [SerializeField] int life = 100;
    [SerializeField] float maxLife = 100f;
    [SerializeField] Transform healthTransform;
    [SerializeField] GameObject healthBarObj;
    float healthBarShowTime = 0f;
    float healthBarShowTimeDuration = 1f;
    [SerializeField] float disapperSpeed = 1f;
    [SerializeField] float alpha = 1f;
    Color hitColor =new Color(244f / 255f, 85f / 255f, 85f / 255f);
    [SerializeField] bool isAlive = true;
    [SerializeField] int damage = 30;
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] float moveSpeed = 1f;
    Transform playerTransform;
    Vector2 initialPosition;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
        initialPosition = transform.position;
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive)
        {
            Disappear();
        }
        MoveTowardsPlayer();
        updataHealthUI();
        AutoHideHealthBar();


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="bullet") 
        {
            renderer.color = hitColor;
            Invoke("ResetColor",0.5f);
            int damage = collision.gameObject.GetComponent<BulletSprite>().GetDamage();
            life -= damage;
            healthBarShowTime = 0f;
            if (life<=0)
            {
                isAlive = false;
            }

        }
    }
    void Disappear()
    {
        Color bodyColor = renderer.color;
        alpha -= disapperSpeed * Time.deltaTime;
        renderer.color = new Color(bodyColor.r, bodyColor.g, bodyColor.b, alpha);
        if (alpha <=0)
        {
            //healthBarObj.SetActive(false);
            Destroy(gameObject);
        }
    }
    void AutoHideHealthBar()
    {
        if (healthBarShowTime<healthBarShowTimeDuration) 
        {
            healthBarObj.SetActive(true);
        }
        else
        {
            healthBarObj.SetActive(false);
        }
        healthBarShowTime += Time.deltaTime;
    }
    void ResetColor()
    {
        renderer.color = new Color(1f, 1f, 1f);
    }
    void DestorySelf()
    {
        Destroy(gameObject, 2f);
    }

    public void DeductLife(int damage)
    {
        renderer.color = hitColor;
        Invoke("ResetColor", 0.5f);
        life -= damage;
        healthBarShowTime = 0f;
        if (life <= 0)
        {
            isAlive = false;
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    void MoveTowardsPlayer()
    {
        if (!playerController.isAlive)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            FlipFace();
            return;
        }
        if (Vector2.Distance(transform.position, playerTransform.position)<chaseDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            FlipFace();
        }
    }
    void FlipFace()//控制飞行怪物转向面朝主角
    {
        if (playerTransform.position.x - transform.position.x >= 0)
        {
            transform.localScale = new Vector3(-1,1,1);
            healthBarObj.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            healthBarObj.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    public void RecoverSpeedInSecounds(float secounds)
    {
        Invoke("RecoverSpeed", secounds);
    }
    void RecoverSpeed()
    {
        rigidbody.velocity = Vector2.zero;
    }
    void updataHealthUI()
    {
        float healthPercentage = life / maxLife;
        healthTransform.localScale = new Vector3(healthPercentage, 1f, 1f);
    }
}
