using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingGlobal : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private PostProcessVolume globalVol;
    private Vignette vignette;


    private Vector3 screenPosition;

    private void Awake()
    {
        globalVol = GetComponent<PostProcessVolume>();
        globalVol.profile.TryGetSettings<Vignette>(out vignette);
    }

    private void Update()
    {
        TransformWorldToScreen(player.position);
        vignette.center.value = screenPosition;
    }

    private void TransformWorldToScreen(Vector3 worldPosition)
    {
        Camera cam = Camera.main;
        screenPosition = cam.WorldToScreenPoint(worldPosition);
    }
    


}
