using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    [SerializeField]
    private float time;

    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
