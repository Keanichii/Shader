using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [SerializeField]
    public Transform pos1;
    [SerializeField]
    public Transform pos2;
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool movingEnemy;

    Vector3 target;
    float direction;
    Rigidbody2D rb;
    bool movingLeft;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = pos1.position;
        movingLeft = true;
        direction = -1f;
    }


    private void Update()
    {
        if (movingEnemy)
        {

            float distanceToStop = Mathf.Abs(target.x - transform.position.x);
            


            if (distanceToStop <= 0.5)
            {
                
                direction *= -1f;
                if (movingLeft)
                {
                    target = pos2.position;
                    Vector3 rotation = new Vector3(transform.rotation.x, 180f, 0);
                    transform.rotation = Quaternion.Euler(rotation);
                    movingLeft = !movingLeft;
                }
                else
                {
                    target = pos1.position;
                    Vector3 rotation = new Vector3(transform.rotation.x, 0f, 0);
                    transform.rotation = Quaternion.Euler(rotation);
                    movingLeft = !movingLeft;

                }
            }

            rb.velocity = new Vector2(direction * speed, rb.velocity.y);


        }
    }

}
