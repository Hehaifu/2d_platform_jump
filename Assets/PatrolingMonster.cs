using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolingMonster : MonoBehaviour,Enemy
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int life = 100;
    [SerializeField] float maxLife = 100f;
    [SerializeField] float disapperSpeed = 1f;
    [SerializeField] Transform healthTransform;
    [SerializeField] GameObject healthBarObj;
    [SerializeField] int damage = 20;
    Color hitColor =new Color(244f / 255f, 85f / 255f, 85f / 255f);
    SpriteRenderer renderer;
    float healthBarShowTime = 0f;
    float healthBarShowTimeDuration = 1f;
    Transform playerTransform;
    [SerializeField] bool isAlive = true;
    Rigidbody2D rigidbody;
    [SerializeField] float alpha = 1f;
    int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
        renderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        updataHealthUI();
        AutoHideHealthBar();
        if (!isAlive)
        {
            Disappear();
        }
        FlipFace();
        Patrol();
    }

    void Patrol()
    {
        rigidbody.velocity = new Vector2(moveSpeed * direction * -1, rigidbody.velocity.y);
    }
    void FlipFace()
    {
        transform.localScale = new Vector3(direction, 1, 1);
        healthBarObj.transform.localScale = new Vector3(direction, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            direction *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "platform")
        {
            direction *= -1;
        }
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
    void ResetColor()
    {
        renderer.color = new Color(1f, 1f, 1f);
    }

    public void RecoverSpeedInSecounds(float secounds)
    {
        Invoke("RecoverSpeed", secounds);
    }
    void RecoverSpeed()
    {
        rigidbody.velocity = Vector2.zero;
    }
    void Disappear()
    {
        Color bodyColor = renderer.color;
        alpha -= disapperSpeed * Time.deltaTime;
        renderer.color = new Color(bodyColor.r, bodyColor.g, bodyColor.b, alpha);
        if (alpha <= 0)
        {
            //healthBarObj.SetActive(false);
            Destroy(gameObject);
        }
    }
    void AutoHideHealthBar()
    {
        if (healthBarShowTime < healthBarShowTimeDuration)
        {
            healthBarObj.SetActive(true);
        }
        else
        {
            healthBarObj.SetActive(false);
        }
        healthBarShowTime += Time.deltaTime;
    }
    public int GetDamage()
    {
        return damage;
    }
    void updataHealthUI()
    {
        float healthPercentage = life / maxLife;
        healthTransform.localScale = new Vector3(healthPercentage, 1f, 1f);
    }
}
