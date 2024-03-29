using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    private float speed;
    private bool restoreTime;

    private ParticleSystem darkBubble;
    //anim to trigger "Damaged" stage (for later on)
    //private Animator anim;

    private void Awake()
    {
        darkBubble = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        restoreTime = false;
    }

    private void Update()
    {
        if (restoreTime)
        {
            if(Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }
    }

    public void StopTime (float changeTime, float restoreSpeed, float delay)
    {
        speed = restoreSpeed;


        if (delay > 0)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            restoreTime = true;
        }

        darkBubble.Play();
        Time.timeScale = changeTime;
        Debug.Log("Damaged");
    }


    public IEnumerator StartTimeAgain(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        restoreTime = true;
    }



}
