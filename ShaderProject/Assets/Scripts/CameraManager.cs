using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras;

    [Header("Controls for lerping Y Damping during player jump/fall")]
    [SerializeField] private float fallPanAmount = 0.25f;
    [SerializeField] private float fallPanTime = 0.35f;
    public float dampingChangeThreshold = -15f;


    public bool IsLerping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine lerpYPanCoroutine;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;

    private float yPanAmount;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (allVirtualCameras[i].enabled)
            {
                currentCamera = allVirtualCameras[i];

                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        //set the damping amount to the inspectors value
        yPanAmount = framingTransposer.m_YDamping;
    }

        #region Lerp the Y Damping

    public void LerpYDamping(bool isFalling)
        {
            lerpYPanCoroutine = StartCoroutine(LerpYAction(isFalling));
        }

    private IEnumerator LerpYAction(bool isFalling)
        {
            IsLerping = true;

        //grab the starting damping amount
        float startDampAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if (isFalling)
        {
            endDampAmount = fallPanAmount;
            LerpedFromPlayerFalling = true;
        }

        else
        {
            endDampAmount = yPanAmount;
        }

        //lerp the pan amount
        float elapsedTime = 0f;
        while (elapsedTime < fallPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / fallPanTime);
            framingTransposer.m_YDamping = lerpedPanAmount; 


            yield return null;
        }

        IsLerping = false;

        }


        #endregion
    




}
