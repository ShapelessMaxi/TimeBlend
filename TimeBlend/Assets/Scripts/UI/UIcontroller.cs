using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

// UI controls            
    // Lock Screen:
        // displays screen saver, amount of notifications
        // enter = unlock phone (1st screen is active, menu is active)

    // Menu: 
        // Select and change phone screens
        // left/right = scroll through items/screens
        // enter = select menu item (selected screen is active)
        // down = menu is inactive, select something on the screen

    // Msgs Screen: 
        // display 3 last mesages received
        // down = select and scroll through messages (1st is always selected)
        // up = select menu left arrow
    
    // Conversation Screen:
        // display the full message received, and answer it (a few options)
        // left/right: select answer
        // enter: send answer

    // Map Screen:

    // Fishing minigame Screen:

public class UIcontroller : MonoBehaviour
{
    // access to list of sent messages
    public MessageSender messageSender;
    public List<MessageData> receivedMessagesMod = new List<MessageData>();

    // list containing all Screen objects
    private List<ScreenData> screens = new List<ScreenData>();

    // Clock component (small screen)
    public GameObject clockParent;
    public TMP_Text clockText;
    // having the reference to the actual clock script would be useful
    // public ClockScript = $"{Clock.currentHour:D2}:{Clock.currentMinute:D2}:{Clock.currentSecond:D2}";

    // Menu component (small screen)
    public GameObject menuParent;
    public TMP_Text menuText;
    public GameObject leftMenu_higlight;
    public GameObject rightMenu_higlight;

    // lock screen parent (main screen)
    public GameObject lockParent;

    // message screen parent (main screen)
    public GameObject messagesParent;

    // Received Messages top Notification
    public GameObject notifA;
    public TMP_Text notifA_name;
    public TMP_Text notifA_msg;
    public TMP_Text notifA_time;
    // public GameObject notifA_pic;
    // only need a highligther on the 1st msg
    public GameObject notifA_highlight;

    // Received Messages middle Notification
    public GameObject notifB;
    public TMP_Text notifB_name;
    public TMP_Text notifB_msg;
    public TMP_Text notifB_time;
    // public GameObject notifB_pic;

    // Received Messages bottom Notification
    public GameObject notifC;
    public TMP_Text notifC_name;
    public TMP_Text notifC_msg;
    public TMP_Text notifC_time;
    // public GameObject notifC_pic;

    // map screen parent (main screen)
    public GameObject mapParent;

    // UI status 
    public int currentScreen = 0;
    private bool menuIsActive = false;
    private bool messageSelected = false;
    private bool rightMenuSelected = false;
    private bool leftMenuSelected = false;

    // Status reset and setup
    // create screen objects
    // hide/show game objects
    void Start()
    {
        // get the list of sent messages
        messageSender = GetComponent<MessageSender>();
        
        // Create screen objects
        // displayed menu title
        // empty parent game object containing all related game objects
        screens.Add(new ScreenData("lock", lockParent));
        screens.Add(new ScreenData("msgs", messagesParent));
        screens.Add(new ScreenData("map", mapParent));

        // make sure the right objects are active
        clockParent.SetActive(true);
        lockParent.SetActive(true);

        // make sure the right objects are desactivated
        menuParent.SetActive(false);
        leftMenu_higlight.SetActive(false);
        rightMenu_higlight.SetActive(false);
        screens.ForEach(screen => screen.emptyParent.SetActive(false));
        notifA_highlight.SetActive(false);
        notifA.SetActive(false);
        notifC.SetActive(false);
        notifB.SetActive(false);
        
        UpdateContent();
    }
    
    // runs each frame
    // key mapped logic for interaction witht the phone
    void Update()
    {   
        // menu is active
        if (menuIsActive)
        {
            MenuControls();
            if (currentScreen == 1)
            { 
                NotifDisplay(messageSender.sentMessages, false);
            }                        
        }
        
        // menu is not active
        else
        {
            switch(currentScreen)
            {    
                // Lock Screen;
                case 0:
                    // Enter
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        // hide the clock and lockscreen
                        clockParent.SetActive(false);
                        lockParent.SetActive(false);
                        // show the menu
                        menuParent.SetActive(true);

                        // activate the menu, status check, hide/show objects 
                        ActivateMenu();

                        // change to message screen
                        currentScreen = 1;

                        // update the content of the small and main screen
                        UpdateContent();
                    }

                    return;

                // Message Screen
                case 1:
                    // Up                        
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        messageSelected = false;
                        ActivateMenu();
                    }

                    // Down
                    else if (Input.GetKeyDown(KeyCode.DownArrow) && messageSender.sentMessages != null)
                    {
                        // scroll through messages (shift list)
                        if (messageSelected)
                        {
                            // display notifications (true for shift bool)
                            NotifDisplay(receivedMessagesMod, true);
                        }
                        
                        // no message selected (coming from menu)
                        // select 1st message
                        else 
                        {
                            // re copy the list of received messages to be modified
                            receivedMessagesMod = new List<MessageData>(messageSender.sentMessages);

                            // display the notifications
                            NotifDisplay(receivedMessagesMod, false);

                            // status check
                            messageSelected = true;

                            menuIsActive = false;
                            rightMenuSelected = false;
                            leftMenuSelected = false;

                            // game object activation
                            notifA_highlight.SetActive(true);

                            // game object desactivation
                            rightMenu_higlight.SetActive(false);
                            leftMenu_higlight.SetActive(false);
                        }
                    }

                    return;
                        
                // Map Screen
                case 2:
                    // Up                        
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        ActivateMenu();
                    }

                    return;

                default:
                    return;
            }
        }
    }
    
    // handle the logic of displaying the message notifications
    void NotifDisplay(List<MessageData> messageList, bool shift)
    {
        // Compte le nombre de messages recus
        int messageCount = messageList.Count;
        
        // Afficher un maximum de 3 notification
        // Retourne le nombre entier le plus petit entre 3 et le nombres de messages recus
        int displayCount = Mathf.Min(3, messageCount);
        
        // Cacher les notifications superflues
        notifA.SetActive(displayCount > 0);
        notifB.SetActive(displayCount > 1);
        notifC.SetActive(displayCount > 2);
        
        int shiftingIndex = 0;
        if (shift) 
        {
          shiftingIndex = 1;
        }
    
        // count up to 3 (displayed notification)
        for (int i = 0; i < displayCount; i++)
        {
            // Access messages in reverse order (newest first)
            int msgIndex = messageCount - 1 - i - shiftingIndex;
            // i = 0 = A, 1 = B, 2 = C
            WriteNotification(i, messageList[msgIndex]);
        }
    }
    
    // Write the correct info on the correct game object
    void WriteNotification(int index, MessageData message)
    {
        switch(index) {
            case 0:
                notifA.SetActive(true);
                notifA_name.text = message.senderName;
                notifA_time.text = clockText.text;
                notifA_msg.text = message.fullMessage.Substring(0, 12);
                return;

            case 1:
                notifB.SetActive(true);
                notifB_name.text = message.senderName;
                notifB_time.text = clockText.text;
                notifB_msg.text = message.fullMessage.Substring(0, 12);
                return;
            
            case 2:
                notifC.SetActive(true);
                notifC_name.text = message.senderName;
                notifC_time.text = clockText.text;
                notifC_msg.text = message.fullMessage.Substring(0, 12);
                return;

            default:
                // should never be anything else other than 1, 2 or 3
                return;
        }
    }

    // ui controls when the menu is active
    void MenuControls()
    {
        // Right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // status check
            menuIsActive = true;
            rightMenuSelected = true;
            leftMenuSelected = false;

            // game object activation
            rightMenu_higlight.SetActive(true);
            // game object desactivation
            leftMenu_higlight.SetActive(false);
        }

        // Left
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // status check
            menuIsActive = true;
            rightMenuSelected = false;
            leftMenuSelected = true;

            // game object activation
            leftMenu_higlight.SetActive(true);
            // game object desactivation
            rightMenu_higlight.SetActive(false);
        }

        // Down
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // status check
            menuIsActive = false;
            rightMenuSelected = false;
            leftMenuSelected = false;

            // game object desactivation
            rightMenu_higlight.SetActive(false);
            leftMenu_higlight.SetActive(false);

            switch(currentScreen)
            {   
                // message screen
                case 1:
                    NotifDisplay(messageSender.sentMessages, false);
                    notifA_highlight.SetActive(true);
                    return;

                default:
                    return;
            }
        }

        // Enter
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            screens[currentScreen].emptyParent.SetActive(false);
            
            // select left 
            if (leftMenuSelected) 
            {
                // change screens
                currentScreen = (currentScreen - 1 + screens.Count) % screens.Count;
                UpdateContent();
            }
            // select rgiht
            else if (rightMenuSelected)
            {
                // change screens
                currentScreen = (currentScreen + 1) % screens.Count;
                UpdateContent();
            }
        }
    }

    // ui controls to reactivate menu when on other screens
    void ActivateMenu() 
    {
            // status check
            menuIsActive = true;
            rightMenuSelected = false;
            leftMenuSelected = true;

            // game object activation
            leftMenu_higlight.SetActive(true);

            // game object desactivation
            notifA_highlight.SetActive(false);
            rightMenu_higlight.SetActive(false);
    }

    // display main screen content
    void UpdateContent()
    {
        // update the content of the menu and main screen
        menuText.text = screens[currentScreen].title;
        screens[currentScreen].emptyParent.SetActive(true);
    }
}