using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateMessage : MonoBehaviour
{
    public List<ScreenData.MessageData> randomMessages = new List<ScreenData.MessageData>();
    public List<ScreenData.MessageData> locationMessages = new List<ScreenData.MessageData>();
    public List<ScreenData.MessageData> sentMessages = new List<ScreenData.MessageData>();

    private int timer; 
    public (float min, float max) timeRange = (0f, 5f);

    public TMP_Text currentClock;

    void Start()
    {
        // hide notifications
        // notifcationA.SetActive(false);
        // notifcationB.SetActive(false);
        // notifcationC.SetActive(false);

        // Create all randomly sent message objects:
        // is sent bool
        // can be sent bool
        // sender name
        // time received at (hour, minute, second)
        // abbreviated message displayed on main msg screen
        // full mesage displayed when opening the message
        randomMessages.Add(new ScreenData.MessageData(false, true, "Kriss", "Hey where ar", "Hey where are u did you forget our date again??"));
        randomMessages.Add(new ScreenData.MessageData(false, false, "Kriss", "im literally", "im literally waiting for you here at the cafe..."));
        randomMessages.Add(new ScreenData.MessageData(false, true, "Adrien", "lol look at", "lol look at this fool.."));
        // 
        // Create all location based message objects:
        locationMessages.Add(new ScreenData.MessageData(false, false, "HungTopFun", "Hey sexy, you", "Hey sexy, your like 10 meters away, looking?"));

        // coroutine for almost randomly sent messages
        StartCoroutine(MessageRoutine());
    }

    IEnumerator MessageRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(timeRange.min, timeRange.max);
            yield return new WaitForSeconds(waitTime);
            
            SendRandomMessage();
        }
    }

    void SendRandomMessage()
    {
        int r = UnityEngine.Random.Range(0, randomMessages.Count);
        if (!randomMessages[r].isSent && randomMessages[r].canBeSent) 
        {
            // keep track of the message status
            randomMessages[r].isSent = true;
            randomMessages[r].canBeSent = false;
            sentMessages.Add(randomMessages[r]);
            // Debug.Log(sentMessages.Count);

        }
    }

    // Update is called once per frame
    void Update()
    {
        // decide on the logic behind sending the messages....
        // options:
        // (block certain message with the bool can be sent)
        // 1- randomly. set a timer, and release a message every ~1-3min. (hey are we still doing the pre at your place later?)
        // 2- location based. set spots on the map, when you walk by them, send. (heey i just saw you at the coffee shop)
        // 3- probably a mix of both actually
    }
}
