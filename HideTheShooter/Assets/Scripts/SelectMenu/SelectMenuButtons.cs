using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMenuButtons : MonoBehaviour
{
    public void playMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerNameSelect");
    }

    public void goBack()
    {
        SceneManager.LoadScene("StarterMenu");
    }
}
