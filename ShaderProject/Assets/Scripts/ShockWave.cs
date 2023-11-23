using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField]
    private float shockwaveTime = 0.75f;

    private Coroutine shockwaveCoroutine;

    private Material shockwaveMat;

    private static int waveDistanceFromCenter = Shader.PropertyToID("waveDistanceFromCenter");

    void Awake()
    {
        shockwaveMat = GetComponent<SpriteRenderer>().material;
    }

    public void Start()
    {
        CallShockWave();
    }

    public void CallShockWave()
    {
        shockwaveCoroutine = StartCoroutine(ShockWaveAction(-0.1f, 1.75f));
    }

    private IEnumerator ShockWaveAction(float startPos, float endPos)
    {
        shockwaveMat.SetFloat(waveDistanceFromCenter, startPos);

        float lerpedAmount = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < shockwaveTime)
        {
            elapsedTime += Time.deltaTime;

            lerpedAmount = Mathf.Lerp(startPos, endPos, elapsedTime/ shockwaveTime);
            shockwaveMat.SetFloat(waveDistanceFromCenter, lerpedAmount);


            yield return null;
        }

    }

}
