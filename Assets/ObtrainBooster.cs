using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtrainBooster : MonoBehaviour
{
    PlayerController playerController;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    public void AddJumpCount()
    {
        playerController.IncreaseJumpCount();
    }

    public void EnableDash()
    {
        playerController.EnableDash();
    }
    public void AppplyEffect(string effectName)
    {
        Invoke(effectName, 0);
    }
}
