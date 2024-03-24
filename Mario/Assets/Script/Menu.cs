using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Choimoi()
    {
        SceneManager.LoadScene(1);
    }
    public void Thoat()
    {
        Application.Quit();
    }
    public void ThoatraMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void man2()
    {
        SceneManager.LoadScene(2);
    }
    public void man3()
    {
        SceneManager.LoadScene(3);
    }
}