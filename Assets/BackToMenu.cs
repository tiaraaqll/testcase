using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void BackGame (string MainMenus) {
        SceneManager.LoadScene(MainMenus);
        Debug.Log("Ini Scene Main Menu Aktif" + MainMenus);
    }
}
