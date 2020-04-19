using System.Collections;
using SuicideMission.Behavior;
using SuicideMission.Enums;
using UnityEngine;

namespace SuicideMission.Objects
{
    public class Player : Spaceship
    {
        [Header("Player Specific Specs")]
        [SerializeField] private float firingDelay = 0.1f;
        [SerializeField] private float paddingX = 0.75f;
        [SerializeField] private float paddingY = 1.25f;
        [SerializeField] private int initialLife = 2;
        [SerializeField] private int coinNeededForExtraLife = 6;

        [Header("Weapon Heat Settings")]
        [SerializeField] private float weaponHeatFactor = 20;
        [SerializeField] private float weaponHeatWaitingTime = 25f;
        [SerializeField] private float weaponHeatReduceFactor = 6;
        [SerializeField] private GameObject heatIndicator;

        private int coins;

        private int life;
        private int initialHealth;
        private Vector3 initialPosition;

        private float shootingSpeedBoost = 1f;
        private float shootingSpeedBoostRemainingTime = 0f;

        private float laserSizeBoost = 1f;
        private float laserSizeBoostRemainingTime = 0f;

        private float trippleLaserBoostRemainingTime = 0f;

        private float weaponHeat;

        private LevelLoader levelLoader;

        private bool invincible;

        private float lastShot;

        protected override void Start()
        {
            base.Start();
            life = initialLife;
            initialHealth = health;
            initialPosition = transform.position;
            levelLoader = FindObjectOfType<LevelLoader>();
        }

        protected override void Update()
        {
            UpdateBoostTimers();
        }

        private void UpdateBoostTimers()
        {
            if (shootingSpeedBoostRemainingTime > 0) shootingSpeedBoostRemainingTime -= Time.deltaTime;

            if (laserSizeBoostRemainingTime > 0) laserSizeBoostRemainingTime -= Time.deltaTime;

            if (shootingSpeedBoostRemainingTime <= 0)
            {
                weaponHeatFactor /= shootingSpeedBoost;
                weaponHeatWaitingTime /= shootingSpeedBoost;
                shootingSpeedBoostRemainingTime = 0;
                shootingSpeedBoost = 1f;
            }

            if (laserSizeBoostRemainingTime <= 1)
            {
                laserSizeBoostRemainingTime = 0;
                laserSizeBoost = 1f;
            }

            if (trippleLaserBoostRemainingTime > 0)
                trippleLaserBoostRemainingTime -= Time.deltaTime;
            else
                trippleLaserBoostRemainingTime = 0;
        }

        public void Move(float deltaX, float deltaY)
        {
            deltaX *= Time.deltaTime;
            deltaY *= Time.deltaTime;

            var minX = levelLoader.MinX + paddingX;
            var maxX = levelLoader.MaxX - paddingX;

            var minY = levelLoader.MinY + paddingY;
            var maxY = levelLoader.MaxY - paddingY;

            var newXPos = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
            var newYPos = Mathf.Clamp(transform.position.y + deltaY, minY, maxY);
            
            transform.position = new Vector2(newXPos, newYPos);
            Fire();
        }

        public override void Fire() //Todo change name
        {
            if (lastShot + firingDelay / shootingSpeedBoost >= Time.time) return;

            Shoot(Direction.Up);
            lastShot = Time.time;

            if (weaponHeat >= 0) weaponHeat -= Time.deltaTime * weaponHeatReduceFactor;
            heatIndicator.transform.localScale = new Vector3(1, Mathf.Clamp(weaponHeat / weaponHeatFactor, 0, 1), 1);
        }

        protected override void Shoot(Direction direction)
        {
            if (weaponHeat >= weaponHeatFactor) return;

            var laserBeanCount = 1;
            if (trippleLaserBoostRemainingTime > 0) laserBeanCount = 3;

            for (var i = 0; i < laserBeanCount; i++)
            {
                var laserBean = Instantiate(laser, transform.position, laser.transform.rotation);
                laserBean.transform.localScale *= laserSizeBoost;

                if (laserBeanCount == 3)
                    switch (i)
                    {
                        case 0:
                        {
                            var pos = laserBean.transform.position;
                            pos.x -= 0.3f;
                            pos.y -= 0.2f;
                            laserBean.transform.position = pos;
                            break;
                        }
                        case 2:
                        {
                            var pos = laserBean.transform.position;
                            pos.x += 0.3f;
                            pos.y -= 0.2f;
                            laserBean.transform.position = pos;
                            break;
                        }
                    }

                laserBean.GetComponent<DamageDealer>().SetDamage(projectileDamage);
                laserBean.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed * (int) direction);
                Destroy(laserBean, destroyBulletAfterSeconds);
            }

            HeatWeapon();
            AudioSource.PlayClipAtPoint(shootSound, cameraPosition, shootSoundVolume);
        }

        private void HeatWeapon()
        {
            weaponHeat++;
            if (weaponHeat >= weaponHeatFactor) weaponHeat = weaponHeatWaitingTime;
        }

        protected override void TryDeath()
        {
            if (--life <= 0)
            {
                Death();
            }
            else
            {
                StartCoroutine(LoseLife());
                GetComponent<HitIndicator>().IndicateHit();
            }
        }

        protected override void ProcessHit(DamageDealer damageDealer)
        {
            if (invincible) return;
            base.ProcessHit(damageDealer);
        }

        private IEnumerator LoseLife()
        {
            SetInvincible(true);
            transform.position = initialPosition;
            health = initialHealth;
            GetComponent<Blink>().SetActive(true);
            yield return new WaitForSeconds(2);
            GetComponent<Blink>().SetActive(false);
            SetInvincible(false);
        }

        protected override void Death()
        {
            base.Death();
            levelLoader.LoadGameOver();
        }

        public void GiveSpeedBoost(float boost, float duration)
        {
            if (shootingSpeedBoostRemainingTime <= 0 || shootingSpeedBoost != boost)
            {
                weaponHeatFactor *= boost;
                weaponHeatWaitingTime *= boost;

                shootingSpeedBoost = boost;
                shootingSpeedBoostRemainingTime = duration;
            }
            else
            {
                shootingSpeedBoostRemainingTime += duration;
            }
        }

        public void GiveLaserSizeBoost(float boost, float duration)
        {
            if (laserSizeBoostRemainingTime <= 0 || laserSizeBoost != boost)
            {
                laserSizeBoost = boost;
                laserSizeBoostRemainingTime = duration;
            }
            else
            {
                laserSizeBoostRemainingTime += duration;
            }
        }

        public void GiveTrippleLaserBoost(float duration)
        {
            trippleLaserBoostRemainingTime += duration;
        }

        public float GetFiringDelay()
        {
            return firingDelay;
        }

        public float SetFiringDelay(float firingDelay)
        {
            return this.firingDelay = firingDelay;
        }

        private void SetInvincible(bool val)
        {
            invincible = val;
        }

        public int GetLifes()
        {
            return life;
        }

        public int GetTotalHealth()
        {
            return initialHealth;
        }

        public int GetCoins() => coins;

        public void AddCoins(int coin)
        {
            coins += coin;
            if (coins >= coinNeededForExtraLife)
            {
                coins = 0;
                life++;
            }
        }
    }
}