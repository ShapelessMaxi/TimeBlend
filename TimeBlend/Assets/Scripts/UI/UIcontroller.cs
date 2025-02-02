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
    public float highlightTiming = 0.5f;

    public GameObject mapParent;
    public GameObject messagesParent;

    private bool isLeftSelected = false;

    void Start()
    {
        // Create screen objects
        screens.Add(new ScreenData("msgs", messagesParent));
        screens.Add(new ScreenData("map", mapParent));
        screens.Add(new ScreenData("test01", mapParent));
        screens.Add(new ScreenData("test02", mapParent));
        
        UpdateContent();
    }

    void Update()
    {
        // Detect arrow key presses
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HighlightRightButton();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HighlightLeftButton();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmSelection();
        }
    }

    public void HighlightLeftButton()
    {
        isLeftSelected = true;
        leftHighlight();
        rightUnHighlight();
    }

    public void HighlightRightButton()
    {
        isLeftSelected = false;
        rightHighlight();
        leftUnHighlight();
    }

    public void leftHighlight()
    {
        leftButtonHighlight.SetActive(true);
    }
    public void leftUnHighlight()
    {
        leftButtonHighlight.SetActive(false);
    }
    public void rightHighlight()
    {
        rightButtonHighlight.SetActive(true);
    }
    public void rightUnHighlight()
    {
        rightButtonHighlight.SetActive(false);
    }

    public void UpdateContent()
    {
        menuText.text = screens[currentScreen].title;
        screens[currentScreen].emptyParent.SetActive(true);
    }

    public void ConfirmSelection()
    {
        if (isLeftSelected)
        {
            PreviousScreen();
        }
        else
        {
            NextScreen();
        }
    }

    public void NextScreen()
    {
        screens[currentScreen].emptyParent.SetActive(false);
        currentScreen = (currentScreen + 1) % screens.Count;
        UpdateContent();
    }

    public void PreviousScreen()
    {
        screens[currentScreen].emptyParent.SetActive(false);
        currentScreen = (currentScreen - 1 + screens.Count) % screens.Count;
        UpdateContent();
    }
}
