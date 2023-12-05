using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
       if (col != null && col.CompareTag("Player"))
        {
            GameManager.instance.CollectSoul();
            UIBar.instance.CollectSoulUI();
            Destroy(gameObject);
        }
    }
}
