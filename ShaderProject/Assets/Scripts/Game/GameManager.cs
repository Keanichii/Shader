using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static float shards;

    private void Awake()
    {
        instance = this;
        shards = 0;
    }


    public void EndGame()
    {
        Debug.Log("GAME OVER");
        SceneManager.LoadScene("GameOver");
    }
    
    public void CollectSoul()
    {
        shards ++;
        Debug.Log("You now have " + shards + " shards");
    }



}
