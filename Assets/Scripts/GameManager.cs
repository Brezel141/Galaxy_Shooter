using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;



    // Update is called once per frame
    private void Update()
    {
        if (_isGameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1); //Current Game Scene
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
