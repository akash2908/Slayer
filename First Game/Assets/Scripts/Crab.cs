using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour {

    public int health;
    public GameObject particleEffect;
    SpriteRenderer spriteRenderer;
    int direction;
    float timer = 1f;
    public float speed;
    public Sprite facingUp;
    public Sprite facingDown;
    public Sprite facingLeft;
    public Sprite facingRight;
    float changeTimer = 0.2f;
    bool shouldChange;

	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = Random.Range(0, 3);
        shouldChange = false;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1.5f;
            direction = Random.Range(0, 3);
        }
        Movement();
        if (shouldChange)
        {
            changeTimer -= Time.deltaTime;
            if (changeTimer <= 0)
            {
                shouldChange = false;
                changeTimer = 0.2f;
            }
        }
	}

    void Movement()
    {
        if (direction == 0)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            spriteRenderer.sprite = facingDown;
        }
        else if (direction == 1)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            spriteRenderer.sprite = facingLeft;
        }
        if (direction == 2)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            spriteRenderer.sprite = facingRight;
        }
        if (direction == 3)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            spriteRenderer.sprite = facingUp;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sword")
        {
            health--;
            if (health <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            col.GetComponent<Sword>().createParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            health--;
            if (!col.gameObject.GetComponent<Player>().iniFrames)
            {
                col.gameObject.GetComponent<Player>().currentHealth--;
                col.gameObject.GetComponent<Player>().iniFrames = true;
            }
            if (health <= 0)
            {
                Instantiate(particleEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        if (col.gameObject.tag == "Wall")
        {
            if(shouldChange)
            { return; }
            if (direction == 0)
            { direction = 3; }
            else if (direction == 1)
            { direction = 2; }
            else if (direction == 2)
            { direction = 1; }
            else if (direction == 3)
            { direction = 0; }
            shouldChange = true;
        }
    }
}
