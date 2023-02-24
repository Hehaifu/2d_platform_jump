using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    PlayerController playerController;
    UIController uiController;
    [SerializeField] int playerLives = 3;
    public bool isDashEnabled = false;
    public int maxJumpCount = 1;
    private void Awake()
    {
        int numberOfSessions = FindObjectsOfType<GameSession>().Length;
        playerController = FindObjectOfType<PlayerController>();
        if (numberOfSessions>1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    //public void SetPlayerController(PlayerController playerController) {
    //    this.playerController = playerController;
    //}

    void Start()
    {
        uiController = GetComponent<UIController>();
        uiController.UpdataPlayerLives(playerLives);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToNextScene()
    {
        int currentSceneID = SceneManager.GetActiveScene().buildIndex;
        int totalNumScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneID = (currentSceneID + 1) % totalNumScenes;
        SceneManager.LoadScene(nextSceneID);
    }

    public void DeductPlayerlife()
    {
        playerLives--;
        uiController.UpdataPlayerLives(playerLives);
        if (playerLives <= 0) 
        {
            Invoke("ResetGameSession", 3f);
        }
        else 
        {
            Invoke("Reborn",3f);
        }
    }
    void Reborn()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.Reborn();
    }
    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        uiController.UpdataPlayerLives(3);
        uiController.UpdatePlayerhealthy(1f);
        isDashEnabled = false;
        maxJumpCount = 1;
    }
}
