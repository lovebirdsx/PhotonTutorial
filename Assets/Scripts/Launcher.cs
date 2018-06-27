using UnityEngine;

namespace PhotonTutorial {
	public class Launcher : Photon.PunBehaviour {
		public PhotonLogLevel logLevel = PhotonLogLevel.Informational;
		public byte maxPlayersPerRoom = 4;
		public GameObject controlPanel;
		public GameObject progressLabel;

		string gameVersion = "1";
		bool isConnecting = false;

		void Awake() {
			PhotonNetwork.logLevel = logLevel;
			PhotonNetwork.autoJoinLobby = false;
			PhotonNetwork.automaticallySyncScene = true;
		}

		void Start() {
			controlPanel.SetActive(true);
			progressLabel.SetActive(false);
		}

		public override void OnConnectedToMaster() {
			Debug.Log("OnConnectedToMaster was called by PUN");

			if (isConnecting)
				PhotonNetwork.JoinRandomRoom();
		}

		public override void OnDisconnectedFromPhoton() {
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
			Debug.LogWarning("OnDisconnectedFromPhoton was called by PUN");
		}

		public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
			Debug.Log("OnPhotonRandomJoinFailed was called by PUN");

			PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom }, null);
		}

		public override void OnJoinedRoom() {
			Debug.Log("OnJoinedRoom was called by PUN");

			if (PhotonNetwork.room.PlayerCount == 1) {
				Debug.Log("We load the 'Room For 1'");

				PhotonNetwork.LoadLevel("Room For 1");
			}
		}

		public void Connect() {
			isConnecting = true;

			progressLabel.SetActive(true);
			controlPanel.SetActive(false);
			if (!PhotonNetwork.connected) {
				PhotonNetwork.ConnectUsingSettings(gameVersion);
			} else {
				PhotonNetwork.JoinRandomRoom();
			}
		}
	}
}