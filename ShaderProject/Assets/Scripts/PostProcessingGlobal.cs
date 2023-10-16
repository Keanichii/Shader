using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingGlobal : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Volume globalVol;
    private Vignette vnt;

    [SerializeField]
    private Vector3 screenPosition;

    private void Awake()
    {
        globalVol = GetComponent<Volume>();
        globalVol.profile.TryGet(out vnt);
    }

    public void Update()
    {
        //TransformWorldToScreen(player.position);
        //vnt.center.value = new Vector2(screenPosition.x, screenPosition.y);
    }

    private void TransformWorldToScreen(Vector3 worldPosition)
    {
        Camera cam = Camera.main;
        screenPosition = cam.WorldToScreenPoint(worldPosition);
    }
    


}
