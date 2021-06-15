using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    void Update() {
        onClick();
    }
    public static void onDeath(){
        SceneManager.LoadScene(1);
    }
    public static void onClick(){
        if(Input.GetButtonDown("Fire1")){
            SceneManager.LoadScene(0);
        }
    }
}
