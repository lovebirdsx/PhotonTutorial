using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonTutorial {

	[RequireComponent(typeof(InputField))]
	public class PlayerNameInputField : MonoBehaviour {

		static string playerNamePrefKey;

		string GetInstanceName() {
			string[] args = System.Environment.GetCommandLineArgs();
			for (int i = 0; i < args.Length; i++) {
				if (args[i] == "-InstanceName") {
					return args[i + 1];
				}
			}
			return "PhotonTutorial";
		}

		void Awake() {
			if (playerNamePrefKey == null)
				playerNamePrefKey = GetInstanceName() + ".PlayerName";
		}

		// Use this for initialization
		void Start() {
			string defaultName = "";
			InputField inputField = this.GetComponent<InputField>();
			if (inputField != null) {
				defaultName = PlayerPrefs.GetString(playerNamePrefKey);
				inputField.text = defaultName;
			}

			PhotonNetwork.playerName = defaultName;

			Debug.Log(playerNamePrefKey);
		}

		// Update is called once per frame
		public void SetPlayerName(string value) {
			PhotonNetwork.playerName = value + " ";
			PlayerPrefs.SetString(playerNamePrefKey, value);
		}
	}
}