using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    GameSession session;
    UIController uIController;
    // Start is called before the first frame update
    void Start()
    {
        uIController = FindObjectOfType<UIController>();
        session = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            uIController.ShowDialog("Congratulations on passing this level of the game!", "About to move on to the next level");
            Invoke("GoToNext", 3f);
        }
    }
    void GoToNext()
    {
        session.GoToNextScene();
        uIController.CloseDialog();
    }
}
