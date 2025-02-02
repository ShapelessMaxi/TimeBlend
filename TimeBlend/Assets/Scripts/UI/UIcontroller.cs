using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems; // Required for UI navigation

public class UIcontroller : MonoBehaviour
{
    public List<ScreenData> screens = new List<ScreenData>();
    public int currentScreen = 0;

    public TMP_Text menuText;
    public GameObject menuObject;
    public GameObject lockObject;

    public GameObject leftButtonHighlight;
    public GameObject rightButtonHighlight;

    public GameObject lockParent;
    public GameObject messagesParent;
    public GameObject mapParent;


    private bool isLeftSelected = false;
    private bool isUnlocked = false;

    void Start()
    {
        // Create screen objects
        screens.Add(new ScreenData("lock", lockParent));
        screens.Add(new ScreenData("msgs", messagesParent));
        screens.Add(new ScreenData("map", mapParent));

        // make sure the right objects are hidden or shown
        lockObject.SetActive(true);
        lockParent.SetActive(true);
        menuObject.SetActive(false);
        leftButtonHighlight.SetActive(false);
        rightButtonHighlight.SetActive(false);
        foreach (ScreenData screen in screens)
        {
            screen.emptyParent.SetActive(false);
        }
        
        UpdateContent();
    }

    void Update()
    {   
        if (!isUnlocked) 
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                UnlockPhone();
            }
        }
        else 
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
    
    public void UnlockPhone()
    {
        // keep status of lockscreen opened
        isUnlocked = true;
        // remove lockscreen from the list of screens available
        screens.RemoveAt(0);
        
        // hide the clock + lockscreen and show the menu
        lockParent.SetActive(false);
        lockObject.SetActive(false);
        menuObject.SetActive(true);

        // decide which screen is opened first
        currentScreen = 0;
        // update the content of the menu and main screen
        UpdateContent();
    }
    public void UpdateContent()
    {
        // update the content of the menu and main screen
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
