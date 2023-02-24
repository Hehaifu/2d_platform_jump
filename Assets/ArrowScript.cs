using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ArrowScript : MonoBehaviour, BulletSprite
{
    [SerializeField] float speed = 5f;
    Rigidbody2D rigidbody;
    public float arrowDirection = 1f;
    [SerializeField] bool hit = false;
    [SerializeField] SpriteRenderer body;
    [SerializeField] SpriteRenderer head;
    float alpha = 1f;
    [SerializeField] float disapperSpeed = 0.1f;
    public int damage = 20;
    [SerializeField] float shootInterval = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
            rigidbody.velocity = new Vector2(speed * arrowDirection, 0f);
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
            Disappear();
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
            hit = true;
    }
    void Disappear()
    {
        Color bodyColor = body.color;
        alpha -= disapperSpeed * Time.deltaTime;
        body.color = new Color(bodyColor.r, bodyColor.g, bodyColor.b,alpha);
        Color headColor = head.color;
        head.color = new Color(headColor.r, headColor.g, headColor.b, alpha);
        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public  float GetSpeed()
    {
        return speed;
    }

    public float GetBulletInterval()
    {
        return shootInterval;
    }
}
