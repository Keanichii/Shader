using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private PlayerMovement player;

    private bool isFacingRight;

    [SerializeField] private float flipYRotationTime = 0.5f;

    private Coroutine turnCoroutine;

   
    private void Start()
    {
        player = playerTransform.gameObject.GetComponent<PlayerMovement>();

        isFacingRight = player.IsFacingRight;

    }

    
    void Update()
    {
        //make this object follow the player's position
        transform.position = playerTransform.position;

    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = CalculateEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            //lerp y rotation for smooth transition
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, elapsedTime/ flipYRotationTime);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float CalculateEndRotation()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            return 0f;
        }
        else
            return 180f;
    }


}
