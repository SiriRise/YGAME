using UnityEngine;

public class PointsScript : MonoBehaviour
{


    public GameObject explosionEffect; // Префаб эффекта взрыва
    public LayerMask groundLayer; // Слой земли

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем столкновение с землей
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        // Создаем эффект взрыва
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        // Уничтожаем снаряд
        Destroy(gameObject);
    }
}

