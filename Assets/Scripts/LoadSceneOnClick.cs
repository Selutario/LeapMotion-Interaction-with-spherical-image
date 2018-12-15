using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneOnClick : MonoBehaviour
{
    // Toggles scene
    public Toggle Escena1;
    public Toggle Escena2;
    public Toggle Escena3;

    public Toggle ManoIzq;
    public Toggle ManoDer;

    public void LoadByIndex(int sceneIndex)
    {
        if (Escena1.isOn)
            PlayerPrefs.SetInt("Escena", 0);
        else if (Escena2.isOn)
            PlayerPrefs.SetInt("Escena", 1);
        else if (Escena3.isOn)
            PlayerPrefs.SetInt("Escena", 2);

        if (ManoDer.isOn)
            PlayerPrefs.SetInt("ManoPrincipal", 0);
        else if (ManoIzq.isOn)
            PlayerPrefs.SetInt("ManoPrincipal", 1);


        SceneManager.LoadScene(sceneIndex);
    }
}