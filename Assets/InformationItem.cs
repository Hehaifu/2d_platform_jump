using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationItem : MonoBehaviour
{
    UIController uiController;
    SpriteRenderer[] childrenRenderer;
    [SerializeField] string title;
    [SerializeField] string content;
    bool insideTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIController>();
        childrenRenderer = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var render in childrenRenderer)
        {
            if (render.gameObject == gameObject) continue;
            render.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!insideTrigger) return;
        if (Input.GetKey(KeyCode.UpArrow))//如果按下上键
        {
            uiController.ShowDialog(title, content);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        insideTrigger = true;
        if (collision.gameObject.tag == "Player")
        {
            foreach(var render in childrenRenderer)
            {
            if (render.gameObject == gameObject) continue;
                render.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        insideTrigger = false;
        if (collision.gameObject.tag == "Player")
        {
            foreach (var render in childrenRenderer)
            {
            if (render.gameObject == gameObject) continue;
                render.gameObject.SetActive(false);
            }
            uiController.CloseDialog();
        }
    }
}
