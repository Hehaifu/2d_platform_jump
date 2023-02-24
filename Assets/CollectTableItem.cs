using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollectTableItem : MonoBehaviour
{
    Animator animator;
    UIController uiController;
    ObtrainBooster booster;
    SpriteRenderer[] childrenRenderer;
    [SerializeField] string title;
    [SerializeField] string content;
    [SerializeField] string bootsterName;
    bool insideTrigger = false;
    bool used = false;
    // Start is called before the first frame update
    void Start()
    {
        uiController = FindObjectOfType<UIController>();
        animator = GetComponent<Animator>();
        childrenRenderer = gameObject.GetComponentsInChildren<SpriteRenderer>();
        booster = FindObjectOfType<ObtrainBooster>();
        foreach (var render in childrenRenderer)
        {
            if (render.gameObject == gameObject) continue;
            render.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (used) return;
        if (!insideTrigger) return;
        if (Input.GetKey(KeyCode.UpArrow))//如果按下上键
        {
            uiController.ShowDialog(title, content);
            animator.SetTrigger("open");
            booster.AppplyEffect(bootsterName);
            used = true;
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used) return;
        if (collision.gameObject.tag == "Player")
        {
            insideTrigger = true;
            foreach (var render in childrenRenderer)
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
