using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") )
        {
            GameManager.instance.EndGame();
            this.gameObject.SetActive(false);
        }
    }


}
