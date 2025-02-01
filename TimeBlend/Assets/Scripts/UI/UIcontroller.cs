using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems; // Required for UI navigation

public class UIcontroller : MonoBehaviour
{
    public List<ScreenData> screens = new List<ScreenData>();
    public int currentScreen = 0;

    public TMP_Text menuText;
    public GameObject leftButtonHighlight;
    public GameObject rightButtonHighlight;

    void Start()
    {
        // Create screen objects
        screens.Add(new ScreenData("msgs", GameObject.Find("Messages")));
        screens.Add(new ScreenData("map", GameObject.Find("Map")));
        screens.Add(new ScreenData("test01", GameObject.Find("Map")));
        screens.Add(new ScreenData("test02", GameObject.Find("Map")));
        
        UpdateContent();
    }

    void Update()
    {
        // Detect arrow key presses
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightArrowPressed();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftArrowPressed();
        }
    }

    public void LeftArrowPressed()
    {
        leftHighlight();
        PreviousScreen();
    }

    public void RightArrowPressed()
    {
        rightHighlight();
        NextScreen();
    }

    public void leftHighlight()
    {
        leftButtonHighlight.SetActive(true);
        Invoke("leftUnHighlight", 0.2f); // Brief highlight effect
    }
    public void leftUnHighlight()
    {
        leftButtonHighlight.SetActive(false);
    }
    public void rightHighlight()
    {
        rightButtonHighlight.SetActive(true);
        Invoke("rightUnHighlight", 0.2f); // Brief highlight effect
    }
    public void rightUnHighlight()
    {
        rightButtonHighlight.SetActive(false);
    }

    public void HidePreviousContent()
    {
        screens[currentScreen].emptyParent.SetActive(false);
    }
    public void UpdateContent()
    {
        menuText.text = screens[currentScreen].title;
        screens[currentScreen].emptyParent.SetActive(true);
    }

    public void NextScreen()
    {
        HidePreviousContent();
        currentScreen = (currentScreen + 1) % screens.Count;
        UpdateContent();
    }
    public void PreviousScreen()
    {
        HidePreviousContent();
        currentScreen = (currentScreen - 1 + screens.Count) % screens.Count;
        UpdateContent();
    }
}
