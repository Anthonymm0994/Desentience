﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserRobotScript : MonoBehaviour
{

    public Healthy healthComponent;
    public float maxHealth = 100;
    public float currentHealth = 100;

    public ParticleSystem smoke_emitter;
    public GameObject explosionPrefab;
    public int explosionDamage = 40;
    public Material dyingBodyMaterial;
    public Renderer body;
    private bool healthLow = false;
    public bool dying = false;

    private Rigidbody rb;
    private Animator anim;

    public GameObject shrapnelPrefab;
    public Material[] shrapnelMaterials;
    public Transform robotModel;

    private bool exploded = false;
    private bool explosionGrace = false;

    public int incomingBulletDamage = 20;
    public int incomingLaserDamage = 4;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (smoke_emitter == null)
        {
            Debug.LogError("No smoke emitter connected");
        }
        else
        {
            smoke_emitter.Stop();
        }

    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        healthComponent = GetComponent<Healthy>();
    }

    void Update()
    {
        if (healthComponent.currentHealth <= 0 && !dying)
        {
            dying = true;
        }
        if (!dying)
        {
            float healthRatio = healthComponent.currentHealth / healthComponent.maxHealth;

            if (!healthLow && healthRatio < 0.5f) healthLow = true;
            if (healthLow && healthRatio >= 0.5f) healthLow = false;

            if (healthLow && smoke_emitter.isStopped) smoke_emitter.Play();
            if (!healthLow && smoke_emitter.isPlaying) smoke_emitter.Stop();

        }
        else
        {
            body.material = dyingBodyMaterial;
            anim.SetBool("dying", true);
            /*if (gameObject.GetComponentInParent<ChaserAI>())
            {
                gameObject.GetComponentInParent<ChaserAI>().agent.isStopped = true;
            }*/
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                if (exploded == false && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    GameObject explosion = Instantiate(explosionPrefab);
                    explosion.transform.position = gameObject.transform.position;
                    explosion.GetComponent<ExplosionScript>().damage = explosionDamage;
                    exploded = true;

                }
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }
    }

    private void SpawnShrapnel()
    {
        for (int i = 0; i < Random.Range(10, 20); i++)
        {
            GameObject shrapnel = Instantiate(shrapnelPrefab);
            shrapnel.transform.SetParent(transform);
            shrapnel.transform.position = robotModel.position + Random.onUnitSphere;
            shrapnel.transform.localScale = Vector3.Scale(new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)), shrapnelPrefab.transform.localScale) * 2;
            Vector3 velocity = Random.insideUnitSphere * 5;
            Rigidbody projRb = shrapnel.GetComponent<Rigidbody>();
            projRb.velocity = velocity;
            shrapnel.GetComponent<Renderer>().material = shrapnelMaterials[Random.Range(0, shrapnelMaterials.Length)];
            shrapnel.GetComponent<ShrapnelScript>().destroyDelay = Random.Range(1.0f, 3.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            BooletScript hitBooletScript = collision.collider.GetComponent<BooletScript>();
            BulletScript bulletScript = collision.collider.GetComponent<BulletScript>();
            if (hitBooletScript != null)
            {
                if (!hitBooletScript.hasCollided && hitBooletScript.shooter != gameObject.transform.parent.gameObject)
                {
                    hitBooletScript.hasCollided = true;
                    if (currentHealth > 0)
                    {
                        healthComponent.TakeDamage(incomingBulletDamage);
                    }
                }
            } else if (bulletScript != null)
            {
                healthComponent.TakeDamage(bulletScript.damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Explosion" && explosionGrace == false)
        {
            if (other.GetComponent<ExplosionScript>().damage > 0)
            {
                explosionGrace = true;
                StartCoroutine(ExplosionCoroutine());
                healthComponent.TakeDamage(other.GetComponent<ExplosionScript>().damage);
                SpawnShrapnel();
            }
        }
        else if (other.tag == "Laserbeam")
        {
            if (incomingLaserDamage > 0)
            {
                healthComponent.TakeDamage(incomingLaserDamage);
                SpawnShrapnel();
            }
        }
    }

    IEnumerator ExplosionCoroutine()
    {
        yield return new WaitForSeconds(1);

        explosionGrace = false;
    }
}
