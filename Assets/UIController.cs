using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{
    [SerializeField] RectTransform health;
    [SerializeField] GameObject healthFull;
    [SerializeField] float healthImageWidth = 471f;
    [SerializeField] float healthImageHeight = 70f;
    [SerializeField] GameObject dialogObj;
    [SerializeField] Image[] lifeImages;
    [SerializeField] Sprite fullLifeSprite;
    [SerializeField] Sprite emptyLifeSprite;
    TextMeshProUGUI title;
    TextMeshProUGUI content;

    private void Start()
    {
        TextMeshProUGUI[] texts = dialogObj.GetComponentsInChildren<TextMeshProUGUI>();
        foreach(var text in texts)
        {
            if (text.gameObject.name == "title")
            {
                title = text;
            }
            else
            {
                content = text;
            }
        }
        CloseDialog();
        //ShowDialog("hi","hello");
    }
    public void UpdatePlayerhealthy(float percentage)
    {
        if (percentage >=1f)
        {
            healthFull.SetActive(true);
            health.sizeDelta = new Vector2(healthImageWidth, healthImageHeight);
        }
        else
        {
            healthFull.SetActive(false);
            float width = healthImageWidth * percentage;
            health.sizeDelta = new Vector2(width, healthImageHeight);
        }
    }
    public void UpdataPlayerLives(int numberOfLives)
    {
        for (int i = 0; i < numberOfLives ; i++)
        {
            lifeImages[i].sprite = fullLifeSprite;
        }
        for (int i = numberOfLives ; i<lifeImages.Length ; i++)
        {
            lifeImages[i].sprite = emptyLifeSprite;
        }
    }

    public void ShowDialog(string title , string content)
    {
        dialogObj.SetActive(true);
        this.title.text = title;
        this.content.text = content;
    }
    public void CloseDialog()
    {
        dialogObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            CloseDialog();
        }
    }
}
