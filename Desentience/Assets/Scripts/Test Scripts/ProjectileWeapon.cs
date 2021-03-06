﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float bulletSpeed;
    public float bulletDamage;
    public AudioClip gunshot;
    public float timeOfLastShot;
    public float fireRate;
    
    public override bool Action()
    {
        if ((Time.time - timeOfLastShot) < (1 / fireRate)) {
            return false;
        }
        timeOfLastShot = Time.time;
        GameObject projectile = Instantiate(projectilePrefab);
        BulletScript bs = projectile.GetComponent<BulletScript>();
        if (bs != null)
        {
            bs.damage = bulletDamage;
        }
        projectile.transform.position = projectileSpawnPoint.position;
        projectile.transform.rotation = projectileSpawnPoint.rotation;
        Vector3 forward_vector = projectile.transform.rotation * Vector3.forward;
        forward_vector.y = 0;
        Vector3 velocity = forward_vector.normalized * bulletSpeed;
        Rigidbody projRb = projectile.GetComponent<Rigidbody>();
        projRb.velocity = velocity;

        if (gunshot != null)
        {
            AudioManager.Instance.PlayClipAtPoint(gunshot, projectileSpawnPoint.position, volume: 0.15f);
        }
        // end of Action()
        return true;
    }
}
