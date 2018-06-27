using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhotonTutorial {
	public class GameManager : Photon.PunBehaviour {
		static GameManager instance;
		static GameObject localPlayer;

		public GameObject playerPrefab;

		public static GameManager Instance { get {return instance;} }
		public static GameObject LocalPlayer { get {return localPlayer;} set {localPlayer = value;} }

		void Start() {
			instance = this;

			if (LocalPlayer == null) {
				PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
			}
		}

		void Update() {			
		}

		public override void OnLeftRoom() {
			SceneManager.LoadScene(0);
		}

		public void LeaveRoom() {
			PhotonNetwork.LeaveRoom();
		}

		void LoadArena() {
			if (!PhotonNetwork.isMasterClient) {
				Debug.LogError("Trying load a level but we are not the master client");
			}

			Debug.LogFormat("PhotonNetwork: Loading Level: {0}", PhotonNetwork.room.PlayerCount);
			PhotonNetwork.LoadLevel("Room For " + PhotonNetwork.room.PlayerCount);
		}

		public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
			Debug.LogFormat("OnPhotonPlayerConnected() {0}", newPlayer.NickName);

			if (PhotonNetwork.isMasterClient) {
				Debug.LogFormat("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient);

				LoadArena();
			}
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
			Debug.LogFormat("OnPhotonPlayerDisconnected() {0}", otherPlayer.NickName);

			if (PhotonNetwork.isMasterClient) {
				Debug.Log("OnPhotonPlayerDisconnected isMasterClient " + PhotonNetwork.isMasterClient);

				LoadArena();
			}
		}
	}
}