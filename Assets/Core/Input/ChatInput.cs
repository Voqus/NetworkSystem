using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

using Core.Network.Packets;
using Core.Entities;

namespace Core.Input
{
    public class ChatInput : MonoBehaviour
    {
        private Player client;
        private InputField inputField;
        private UnityAction inputAction;

        void Start()
        {
            client = GameObject.Find("Client").GetComponent<Player>();
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
                        break;
                    }
                default:
                    {
                        if (inputField.text != "")
                        {
                            client.Client.sendPacket(new MessagePacket(client.Client, client, new Message(MessageType.WORLD, inputField.text)));
                            
                            inputField.text = "";
                            inputField.DeactivateInputField();
                        }
                        break;
                    }
            }
        }


    }
}