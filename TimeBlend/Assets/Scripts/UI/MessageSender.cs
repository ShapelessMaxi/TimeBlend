using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Send Messages to the in-world phone ui
// Min and MaxTime
// Access Messages to send from MessageLoader.cs
public class MessageSender : MonoBehaviour
{
    // instance of MessageLoader.cs
    private MessageLoader messageLoader;

    // all messages sent are stored here in order: oldest message index = 0, 2nd received = 1...
    public List<MessageData> sentMessages = new List<MessageData>();

    // How long before you get a new message
    public float minTime = 0f;
    public float maxTime = 5f;

    void Start()
    {
        messageLoader = FindObjectOfType<MessageLoader>();

        // coroutine for almost randomly sent messages
        StartCoroutine(MessageRoutine());
    }

    IEnumerator MessageRoutine()
    {
        while (true)
        {

            // produce a random amount of time in a range
            float waitTime = Random.Range(minTime, maxTime);
            
            // Wait
            yield return new WaitForSeconds(waitTime);

            // produce a random integer in the range of the array
            int r = UnityEngine.Random.Range(0, messageLoader.randomMessages.Count);

            // Check the status of the randomly selected message 
            // hasn't been sent already
            // can be sent
            if (!messageLoader.randomMessages[r].isSent && messageLoader.randomMessages[r].canBeSent) 
            {
                // keep track of the message status
                messageLoader.randomMessages[r].isSent = true;
                messageLoader.randomMessages[r].canBeSent = false;
                sentMessages.Add(messageLoader.randomMessages[r]);
            }    
            
        }
    }
}