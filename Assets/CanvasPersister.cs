using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPersister : MonoBehaviour
{
    private void Awake()
    {
        int numberOfSessions = FindObjectsOfType<CanvasPersister>().Length;
        if (numberOfSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
