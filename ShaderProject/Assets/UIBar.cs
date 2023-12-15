using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public static UIBar instance;
    private RectTransform rect;
    private float xPos;
    private float width;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();   
        instance = this;

        xPos = 18;
        rect.anchoredPosition = new Vector2(xPos, 0);
        width = rect.sizeDelta.x;
    }

    public void CollectSoulUI()
    {
        xPos = rect.anchoredPosition.x;
        xPos += 5;
        rect.anchoredPosition = new Vector2(Mathf.Clamp(xPos, 18, width), 0); 
    }

    public void LoseSoulUI()
    {
        xPos = rect.anchoredPosition.x;
        xPos -= 25;
        rect.anchoredPosition = new Vector2(Mathf.Clamp(xPos, 18, width), 0);
    }

}
