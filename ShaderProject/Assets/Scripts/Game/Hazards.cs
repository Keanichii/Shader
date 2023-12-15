using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    //this script is in charge of player-hit actions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<TimeStop>().StopTime(0.01f, 10, 0.1f);
            UIBar.instance.LoseSoulUI();
            GameManager.instance.LoseSoul();
        }
    }
}
