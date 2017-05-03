using UnityEngine;
using UnityEngine.UI;

using Core.Entities;

public class DynamicName : MonoBehaviour {

	private Player client;
	private InputField inputField;

	private void Start () {
		client = GameObject.Find("Client").GetComponent<Player>();
		inputField = GameObject.Find("NameField").GetComponent<InputField>();

		inputField.onEndEdit.AddListener(delegate
		{
			InputListener();
		});
	}

	private void Update () {
		
	}

	private void InputListener()
	{
		client.PlayerName = inputField.text;
		client.enabled = true;
	}
	
}
