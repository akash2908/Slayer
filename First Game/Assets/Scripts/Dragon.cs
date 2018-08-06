using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour {

    Animator anim;
    public float speed;
    int dir;
    float dirTimer = 0.7f;
    public int health;
    public GameObject deathParticle;
    bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = 0.2f;
    bool shouldChange;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        dir = Random.Range(0, 3);
        canAttack = false;
        shouldChange = false;
	}
	
	// Update is called once per frame
	void Update () {
        dirTimer -= Time.deltaTime;
        if (dirTimer <= 0)
        {
            dirTimer = 0.7f;
            dir = Random.Range(0, 3);
        }
        Movement();
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = 2f;
            canAttack = true;
        }
        Attack();
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

    void Attack()
    {
        if (!canAttack)
        {
            return;
        }
        canAttack = false;
        if (dir == 0)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
        }
        else if (dir == 1)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -thrustPower);
        }
        else if (dir == 2)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -thrustPower);
        }
        else if (dir == 3)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
        }
    }

    void Movement()
    {
        if(dir == 0)
        { transform.Translate(Vector3.up * speed * Time.deltaTime); anim.SetInteger("dir", dir); }
        else if (dir == 1)
        { transform.Translate(Vector3.left * speed * Time.deltaTime); anim.SetInteger("dir", dir); }
        else if (dir == 2)
        { transform.Translate(Vector3.down * speed * Time.deltaTime); anim.SetInteger("dir", dir); }
        else if (dir == 3)
        { transform.Translate(Vector3.right * speed * Time.deltaTime); anim.SetInteger("dir", dir); }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Sword")
        {
            health--;
            col.gameObject.GetComponent<Sword>().createParticle();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canAttack = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canMove = true;
            Destroy(col.gameObject);
            if (health <= 0)
            {
                Instantiate(deathParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
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
                Instantiate(deathParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

        if (col.gameObject.tag == "Wall")
        {
            if (shouldChange)
            { return; }
            if (dir == 0)
            { dir = 2; }
            else if (dir == 1)
            { dir = 3; }
            else if (dir == 2)
            { dir = 0; }
            else if (dir == 3)
            { dir = 1; }
            shouldChange = true;
        }
    }
}
