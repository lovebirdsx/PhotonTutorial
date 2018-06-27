using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhotonTutorial {

	public class PlayerManager : Photon.PunBehaviour, IPunObservable {

		public GameObject playerUIPrefab;
		public GameObject beams;
		public float health = 1f;

		bool isFiring;

		void Awake() {
			beams.SetActive(false);

			if (photonView.isMine) {
				GameManager.LocalPlayer = gameObject;
			}

			DontDestroyOnLoad(gameObject);
		}

		void SetupUI() {
			GameObject uiGo = Instantiate(playerUIPrefab) as GameObject;
			PlayerUI ui = uiGo.GetComponent<PlayerUI>();
			ui.SetTarget(this);
		}

		void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
		{
			if (!Physics.Raycast(transform.position, -Vector3.up, 5f)) {
				transform.position = new Vector3(0f, 5f, 0f);
			}

			SetupUI();
		}

		void Start() {
			CameraWork cameraWork = GetComponent<CameraWork>();
			if (photonView.isMine || !PhotonNetwork.connected) {
				cameraWork.OnStartFollowing();
			}

			SetupUI();

			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnDestroy() {
			UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		void Update() {
			if (photonView.isMine || !PhotonNetwork.connected)
				ProcessInputs();

			if (isFiring != beams.GetActive()) {
				beams.SetActive(isFiring);
			}

			if (health <= 0f && PhotonNetwork.connected) {
				GameManager.Instance.LeaveRoom();
			}
		}

		void ProcessInputs() {
			if (Input.GetButtonDown("Fire1")) {
				if (!isFiring) {
					isFiring = true;
				}
			}

			if (Input.GetButtonUp("Fire1")) {
				if (isFiring) {
					isFiring = false;
				}
			}
		}

		bool CanTakeDamage(Collider other) {
			if (!photonView.isMine && PhotonNetwork.connected)
				return false;

			if (!other.name.Contains("Beam") || other.transform.parent == beams.transform)
				return false;

			return true;
		}

		void OnTriggerEnter(Collider other) {
			if (CanTakeDamage(other)) {
				health -= 0.1f;
			}
		}

		void OnTriggerStay(Collider other) {
			if (CanTakeDamage(other)) {
				health -= 0.1f * Time.deltaTime;
			}
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
			if (stream.isWriting) {
				stream.SendNext(isFiring);
				stream.SendNext(health);
			} else {
				isFiring = (bool) stream.ReceiveNext();
				health = (float) stream.ReceiveNext();
			}
		}
	}

}