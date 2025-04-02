using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public float Speed = 1f;
    private Rigidbody2D rg;
    private float Move;



    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move = Input.GetAxis("Horizontal");

        rg.linearVelocity = new Vector2(Move * Speed, rg.linearVelocity.y);
    }

}
