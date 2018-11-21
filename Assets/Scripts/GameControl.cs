using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class GameControl : MonoBehaviour {

    
    public GameObject StartgamePrefab;
    public GameObject bt;
    public GameObject bt2;
   

   

    public void StartGame(string sceneName)
    {
        /* bt.SetActive(false);
         bt2.SetActive(false);
         StartCoroutine(StartGame2(sceneName,StartgamePrefab)); 
         */
        SceneManager.LoadScene(sceneName);
    }

    

    IEnumerator StartGame2(string sceneName,GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        yield return new WaitForSeconds(3.5F);
        Destroy(go);
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void CancelExitGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ConfirmExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void RankButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CloseRank(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
