using SuicideMission.Behavior;
using SuicideMission.Enums;
using SuicideMission.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace SuicideMission.Objects
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
        [SerializeField] protected float destroyAnimationAfterSeconds = 0.5f;

        protected float destroyBulletAfterSeconds;
        public Vector3 cameraPosition;


        protected virtual void Start()
        {
            cameraPosition = Camera.main.transform.position;

            destroyBulletAfterSeconds = Camera.main.orthographicSize * 2 / ProjectileSpeed;

            if (GetComponent<DamageDealer>() != null) GetComponent<DamageDealer>().SetDamage(CollisionDamage);
        }

        protected abstract void Update();
        public abstract void Fire();

        protected virtual void Shoot(Direction direction)
        {
            var laserBean = Instantiate(laser, transform.position, laser.transform.rotation);
            laserBean.GetComponent<DamageDealer>().SetDamage(ProjectileDamage);
            laserBean.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ProjectileSpeed * (int) direction);
            Destroy(laserBean, destroyBulletAfterSeconds);
            AudioSource.PlayClipAtPoint(shootSound, cameraPosition, shootSoundVolume);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            var damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (damageDealer == null) return;
            ProcessHit(damageDealer);
        }

        protected virtual void ProcessHit(DamageDealer damageDealer)
        {
            Health = Mathf.Max(0, Health - damageDealer.GetDamage());
            damageDealer.Hit();
            if (Health <= 0)
            {
                TryDeath();
            }
            else
            {
                if (GetComponent<HitIndicator>() != null) GetComponent<HitIndicator>().IndicateHit();
            }
        }

        protected virtual void TryDeath()
        {
            Death();
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
                AudioSource.PlayClipAtPoint(deathSounds[Random.Range(0, deathSounds.Length)],
                    cameraPosition, deathSoundVolume);
        }

        private void DeathExplosion()
        {
            if (deathExplosion != null) ParticleUtils.PlayParticle(deathExplosion, transform.position);
        }

        private void DeathAnimation()
        {
            if (deathAnimation != null)
            {
                var anim = Instantiate(deathAnimation, transform.position, Quaternion.identity);
                anim.GetComponent<Animator>().speed = animationSpeed;
                Destroy(anim, destroyAnimationAfterSeconds);
            }
        }


        public int Health
        {
            get => health;
            set => health = value;
        }
        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }
        public int ProjectileDamage
        {
            get => projectileDamage;
            set => projectileDamage = value;
        }
        public int CollisionDamage
        {
            get => collisionDamage;
            set => collisionDamage = value;
        }
        public float ProjectileSpeed
        {
            get => projectileSpeed;
            set => projectileSpeed = value;
        }
    }
}