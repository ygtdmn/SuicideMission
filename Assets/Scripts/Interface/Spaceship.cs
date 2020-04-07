﻿using Enums;
using UnityEngine;

namespace SuicideMission.Interface
{
    public abstract class Spaceship : MonoBehaviour
    {
        [Header("Specs")]
        [SerializeField] protected int health = 200;
        [SerializeField] protected float moveSpeed = 10f;
        [SerializeField] protected int projectileDamage = 100;
        [SerializeField] protected int collisionDamage = 500;

        [Header("Projectile")]
        [SerializeField] protected GameObject laser;
        [SerializeField] protected float projectileSpeed = 20f;

        [Header("Sounds")]
        [SerializeField] protected AudioClip[] deathSounds;
        [SerializeField] protected AudioClip shootSound;
        [SerializeField] [Range(0, 1)] protected float deathSoundVolume = 0.75f;
        [SerializeField] [Range(0, 1)] protected float shootSoundVolume = 0.5f;

        [Header("Particles")]
        [SerializeField] protected GameObject deathExplosion;

        [Header("Death Animation")]
        [SerializeField] protected GameObject deathAnimation;
        [SerializeField] protected float animationSpeed = 2f;
        [SerializeField] protected float destoryAnimationAfterSeconds = 0.5f;

        protected float destroyBulletAfterSeconds;
        public Vector3 cameraPosition;

        protected virtual void Start()
        {
            cameraPosition = Camera.main.transform.position;

            destroyBulletAfterSeconds = Camera.main.orthographicSize * 2 / projectileSpeed;

            if (GetComponent<DamageDealer>() != null)
            {
                GetComponent<DamageDealer>().SetDamage(collisionDamage);
            }
        }

        protected abstract void Update();
        protected abstract void Fire();

        protected virtual void Shoot(Direction direction)
        {
            GameObject laserBean = Instantiate(laser, transform.position, laser.transform.rotation);
            laserBean.GetComponent<DamageDealer>().SetDamage(projectileDamage);
            laserBean.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed * (int) direction);
            Destroy(laserBean, destroyBulletAfterSeconds);
            AudioSource.PlayClipAtPoint(shootSound, cameraPosition, shootSoundVolume);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (damageDealer == null) return;
            ProcessHit(damageDealer);
        }

        private void ProcessHit(DamageDealer damageDealer)
        {
            health = Mathf.Max(0, health - damageDealer.GetDamage());
            damageDealer.Hit();
            if (health <= 0)
            {
                Death();
            }
            else
            {
                if (GetComponent<HitIndicator>() != null)
                {
                    GetComponent<HitIndicator>().IndicateHit();
                }
            }
        }

        protected virtual void Death()
        {
            DeathExplosion();
            DeathAnimation();
            Destroy(gameObject);
            DeathSound();
        }

        private void DeathSound()
        {
            if (deathSounds.Length > 0)
            {
                AudioSource.PlayClipAtPoint(deathSounds[Random.Range(0, deathSounds.Length)],
                    cameraPosition, deathSoundVolume);
            }
        }

        private void DeathExplosion()
        {
            if (deathExplosion != null)
            {
                Utils.PlayParticle(deathExplosion, transform.position);
            }
        }

        private void DeathAnimation()
        {
            if (deathAnimation != null)
            {
                var anim = Instantiate(deathAnimation, transform.position, Quaternion.identity);
                anim.GetComponent<Animator>().speed = animationSpeed;
                Destroy(anim, destoryAnimationAfterSeconds);
            }
        }

        // Getters
        public int GetHealth() => health;
        public float GetMoveSpeed() => moveSpeed;

        // Setters
        public int SetHealth(int health) => this.health = health;
        public float SetMoveSpeed(float moveSpeed) => this.moveSpeed = moveSpeed;
    }
}