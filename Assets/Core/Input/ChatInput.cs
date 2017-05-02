using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

using Core.Network;

namespace Core.Input
{
    public class ChatInput : MonoBehaviour
    {

        private InputField inputField;
        private ClientConnection clientCon;
        private UnityAction inputAction;
        // Use this for initialization

        void Awake()
        {
        }
        void Start()
        {
            clientCon = GameObject.Find("Client").GetComponent<ClientConnection>();
            inputField = GameObject.Find("InputField").GetComponent<InputField>();

            inputField.onEndEdit.AddListener(delegate
            {
                InputListener();
            });
        }

        private void InputListener()
        {
            string prefix = inputField.text.Split(' ')[0];

            switch (prefix)
            {
                case "!spawn":
                    {
                        clientCon.spawn();
                        break;
                    }
                default:
                    {
                        if (inputField.text != "")
                        {
                            clientCon.sendMessage(inputField.text);
                            // Server will send back the message to all clients and will initialize the message
                            // on the output ui field on their screen.
                            // Check ClientConnection.cs @sendMessage function

                            inputField.text = "";
                            inputField.DeactivateInputField();
                        }
                        break;
                    }

            }
        }


    }
}