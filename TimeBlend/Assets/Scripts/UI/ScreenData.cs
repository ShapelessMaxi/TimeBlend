using UnityEngine;

public class ScreenData
{
    public string title;
    public GameObject emptyParent;

    public ScreenData(string title, GameObject emptyParent)
    {
        this.title = title;
        this.emptyParent = emptyParent;
    }

    public class MessageData
    {
        public bool isSent = false;
        public bool canBeSent = false;
        public string senderName;
        // public (int Hour, int Minute, int Second) timeReceived = (10, 08, 35);
        public string abreviatedMessage;
        public string fullMessage;

        public MessageData(bool isSent, bool canBeSent, string senderName, string abreviatedMessage, string fullMessage)
        {
            this.isSent = isSent;
            this.canBeSent = canBeSent;
            this.senderName = senderName;
            // this.timeReceived = timeReceived;
            this.abreviatedMessage = abreviatedMessage;
            this.fullMessage = fullMessage;
        }
    }
}