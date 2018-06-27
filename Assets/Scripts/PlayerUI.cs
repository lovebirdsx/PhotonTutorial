using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonTutorial {

	public class PlayerUI : MonoBehaviour {

		public Text playerNameText;
		public Slider playerHealthSlider;
		public Vector3 ScreenOffset = new Vector3(0f, 30f, 0f);

		PlayerManager target;
		float characterControllerHeight = 0f;
		Transform targetTransform;
		Vector3 targetPosition;

		void Awake() {
			Transform transform = GetComponent<Transform>();
			transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
		}

		public void SetTarget(PlayerManager target) {
			this.target = target;
			targetTransform = target.GetComponent<Transform>();

			if (target.photonView.owner != null)
				playerNameText.text = target.photonView.owner.NickName;
			else
				playerNameText.text = "Unknown";

			CharacterController controller = target.GetComponent<CharacterController>();
			characterControllerHeight = controller.height;
		}

		void Update() {
			if (target == null) {
				Destroy(gameObject);
				return;
			}

			playerHealthSlider.value = target.health;
		}

		void LateUpdate() {			
			if (targetTransform != null) {
				targetPosition = targetTransform.position;
				targetPosition.y += characterControllerHeight;
				this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + ScreenOffset;
			}
		}
	}

}