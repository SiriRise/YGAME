using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 15f;

    private void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(
            projectilePrefab, firePoint.position,
            firePoint.rotation);
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * projectileForce, ForceMode2D.Impulse);
    }
}
