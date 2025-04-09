using UnityEngine;

public class PointsScript : MonoBehaviour
{


    public GameObject explosionEffect; // ������ ������� ������
    public LayerMask groundLayer; // ���� �����

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ��������� ������������ � ������
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        // ������� ������ ������
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        
        // ���������� ������
        Destroy(gameObject);
    }
}

