using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    [Header("Debug")]
    public AnimationCurve difficultyCurve; // Для визуальной настройки в инспекторе
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 15f;
    public GameObject explosionEffect; // Префаб эффекта взрыва

    [Header("Fall Settings")]
    public float initialFallDelay = 3f;
    public float minFallDelay = 0.5f;
    public float fallDuration = 2f;
    public float fallForce = 9.8f;
    public LayerMask groundLayer;

    [Header("Firing Settings")]
    public float initialFireRate = 0.5f;
    public float maxFireRate = 2f;
    private float currentFireRate;
    public float nextFireTime = 0f;

    [Header("Difficulty Settings")]
    public float timeToMaxDifficulty = 160f;


    [Tooltip("Множитель изменения сложности (рекомендуется 1 для линейного роста)")]
    public float difficultyCurvePower = 1f;
    private float gameTime = 0f;
    private void Start()
    {
        currentFireRate = initialFireRate;

        if (firePoint == null)
        {
            Debug.LogError("FirePoint не назначен!");
            firePoint = transform;
        }
    }

    

private void Update()
{
    // Обновляем таймер
    gameTime += Time.deltaTime;
    
    // Рассчитываем прогресс сложности от 0 до 1
    float rawProgress = Mathf.Clamp01(gameTime / timeToMaxDifficulty);
    float difficultyProgress = Mathf.Pow(rawProgress, difficultyCurvePower);
    
    // Интерполируем параметры
    currentFireRate = Mathf.Lerp(initialFireRate, maxFireRate, difficultyProgress);
    float currentFallDelay = Mathf.Lerp(initialFallDelay, minFallDelay, difficultyProgress);

        if (Time.time >= nextFireTime)
        {
            Shoot(currentFallDelay);
            nextFireTime = Time.time + 1f / currentFireRate;
        }
    }

    void Shoot(float fallDelay)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(firePoint.up * projectileForce, ForceMode2D.Impulse);
        }

        ProjectileCollisionHandler collisionHandler = projectile.AddComponent<ProjectileCollisionHandler>();
        collisionHandler.Initialize(this, fallDelay, fallDuration, fallForce);
    }

    public void CreateExplosion(Vector2 position)
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, position, Quaternion.identity);

            // Автоматическое удаление эффекта взрыва после анимации
            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(explosion, ps.main.duration);
            }
            else
            {
                // Если нет ParticleSystem, удаляем через 2 секунды
                Destroy(explosion, 0.5f);
            }
        }
    }
}

public class ProjectileCollisionHandler : MonoBehaviour
{
    private WeaponController weaponController;
    private float fallDelay;
    private float fallDuration;
    private float fallForce;
    private bool hasExploded = false;

    public void Initialize(WeaponController controller, float delay, float duration, float force)
    {
        weaponController = controller;
        fallDelay = delay;
        fallDuration = duration;
        fallForce = force;
        StartCoroutine(FallAndDestroy());
    }

    private IEnumerator FallAndDestroy()
    {
        yield return new WaitForSeconds(fallDelay);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float fallEndTime = Time.time + fallDuration;
            while (Time.time < fallEndTime)
            {
                rb.AddForce(Vector2.down * fallForce);
                yield return new WaitForFixedUpdate();
            }
        }

        if (!hasExploded)
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & weaponController.groundLayer) != 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;
        weaponController.CreateExplosion(transform.position);
        Destroy(gameObject);
    }
}