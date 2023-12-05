using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMaterial : MonoBehaviour
{
    public SpriteRenderer spriteRender;
    //array of materials used by this object
    public Material[] materials;

    public void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    //swap current material to the material from the array
    public void SwapMaterials(int materialIndex)
    {
        //Debug.Log("Swap to" + materials[materialIndex].ToString());
        spriteRender.material = materials[materialIndex];
    }



}
