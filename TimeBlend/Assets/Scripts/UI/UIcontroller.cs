using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems; // Required for UI navigation

public class UIcontroller : MonoBehaviour
{
    public List<ScreenData> screens = new List<ScreenData>();
    public int currentScreen = 0;
    private CreateMessage CreateMessage;

    public TMP_Text menuText;
    public GameObject menuObject;
    public GameObject lockObject;
    public TMP_Text currentClock;

    public GameObject leftButtonHighlight;
    public GameObject rightButtonHighlight;

    public GameObject lockParent;
    public GameObject messagesParent;
    public GameObject mapParent;

    
    public GameObject notificationA;
    public GameObject notificationA_highlight;
    public GameObject notificationA_profilepic;
    public TMP_Text notificationA_name;
    public TMP_Text notificationA_abreviatedMessage;
    public TMP_Text notificationA_timestamp;

    public GameObject notificationB;
    public GameObject notificationB_profilepic;
    public TMP_Text notificationB_name;
    public TMP_Text notificationB_abreviatedMessage;
    public TMP_Text notificationB_timestamp;

    public GameObject notificationC;
    public GameObject notificationC_profilepic;
    public TMP_Text notificationC_name;
    public TMP_Text notificationC_abreviatedMessage;
    public TMP_Text notificationC_timestamp;

    private bool isLeftSelected = false;
    private bool isUnlocked = false;

    void Start()
    {
        // find the list of sent messages
        CreateMessage = GetComponent<CreateMessage>();
        
        // Create screen objects
        // displayed menu title
        //empty aprent game object containing all related game objects to hide/display
        screens.Add(new ScreenData("lock", lockParent));
        screens.Add(new ScreenData("msgs", messagesParent));
        screens.Add(new ScreenData("map", mapParent));

        // make sure the right objects are hidden or shown
        lockObject.SetActive(true);
        lockParent.SetActive(true);
        menuObject.SetActive(false);
        leftButtonHighlight.SetActive(false);
        rightButtonHighlight.SetActive(false);
        notificationA.SetActive(false);
        notificationC.SetActive(false);
        notificationB.SetActive(false);
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

        int messageCount = CreateMessage.sentMessages.Count;

        // Loop through the first 3 entries or as many entries as exist in the list
        for (int i = 0; i < Mathf.Min(3, messageCount); i++)
        {
            // Assuming you're updating notifications for positions "A", "B", "C"
            string notificationPosition = GetNotificationPosition(i);
            UpdateNotification(notificationPosition, CreateMessage.sentMessages[i]);
        }
      
    }

    public void UpdateNotification(string motificationIdentifier, ScreenData.MessageData message)
    {
        // target the correct game object
        if (motificationIdentifier == "A") {
            notificationA.SetActive(true);
            notificationA_name.text = message.senderName;
            notificationA_timestamp.text = currentClock.text;
            notificationA_abreviatedMessage.text = message.abreviatedMessage;
        } 
        else if (motificationIdentifier == "B") 
        {
            notificationB.SetActive(true);
            notificationB_name.text = message.senderName;
            notificationB_timestamp.text = currentClock.text;
            notificationB_abreviatedMessage.text = message.abreviatedMessage;
        }
        else if (motificationIdentifier == "C") 
        {
            notificationC.SetActive(true);
            notificationC_name.text = message.senderName;
            notificationC_timestamp.text = currentClock.text;
            notificationC_abreviatedMessage.text = message.abreviatedMessage;
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
    
    // Helper method to map index to notification position
    string GetNotificationPosition(int index)
    {
        switch (index)
        {
            case 0: return "A";
            case 1: return "B";
            case 2: return "C";
            default: return "D"; 
        }
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
