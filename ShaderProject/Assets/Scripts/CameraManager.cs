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
    private Coroutine panCameraCoroutine;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;

    private float yPanAmount;

    private Vector2 startingOffset;

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

        //set the starting position
        startingOffset = framingTransposer.m_TrackedObjectOffset;
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

    #region Pan Camera

    public void PanCameraStart(float panDistance, float panTime, PanDirection panDirection, bool panToStartPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        //handle pan from trigger
        if (panToStartPos)
        {
            //set the direction and distance 
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
                default:
                    break;

            }

            endPos *= panDistance;

            startingPos = startingOffset;

            endPos += startingPos;
        }

        else
        {
            startingPos = framingTransposer.m_TrackedObjectOffset;
            endPos = startingOffset;
        }


        float elapseTime = 0f;
        while (elapseTime < panTime)
        {
            elapseTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, elapseTime/panTime);
            framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }

    }

    #endregion

    #region Swap Cameras

    public void SwapCamera(CinemachineVirtualCamera cameraOnLeft, CinemachineVirtualCamera cameraOnRight)
    {

    }


    #endregion

}
