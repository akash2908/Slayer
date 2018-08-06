﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{

    Animator anim;
    public Transform rewardPosition;
    public GameObject potion;
    public float speed;
    public int dir;
    float dirTimer = 1.5f;
    public int health;
    public GameObject deathParticle;
    bool canAttack;
    float attackTimer = 2f;
    public GameObject projectile;
    public float thrustPower;
    float changeTimer = 0.2f;
    float specialTimer = 0.5f;
    bool shouldChange;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        canAttack = false;
        shouldChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        specialTimer -= Time.deltaTime;
        if (specialTimer <= 0)
        {
            specialAttack();
            specialAttack();
            specialTimer = 0.5f;
        }

        dirTimer -= Time.deltaTime;
        if (dirTimer <= 0)
        {
            dirTimer = 1.5f;
            switch (dir)
            {
                case 1: dir = 0;
                    break;
                case 2: dir = 1;
                    break;
                case 3: dir = 2;
                    break;
                case 0: dir = 3;
                    break;
                default: dir = 1;
                    break;
            }
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
        if (dir == 0)
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
                Instantiate(potion, rewardPosition.transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
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

    void specialAttack()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        int randomDir = Random.Range(0, 3);
        switch (randomDir)
        {        
            case 0: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * thrustPower);
                break;
            case 1: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * thrustPower);
                break;
            case 2: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -thrustPower);
                break;
            case 3: newProjectile.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -thrustPower);
                break;
        }
    }
}