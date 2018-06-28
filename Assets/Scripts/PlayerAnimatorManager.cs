using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhotonTutorial {

	[RequireComponent(typeof(Animator))]
	public class PlayerAnimatorManager : Photon.MonoBehaviour {

		public float directionDampTime = .25f;
		public float moveSpeed = 6.0f;
		public float rotationSpeed = 360;
		
		Animator animator;

		// Use this for initialization
		void Start() {
			animator = GetComponent<Animator>();
		}

		void UpdateMove() {
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			if (h * h + v * v > 0.01f) {
				Vector3 direction = new Vector3(h, 0, v);
				direction.Normalize();
				Vector3 targetPosition = transform.position + direction * moveSpeed * Time.deltaTime;
				transform.LookAt(targetPosition);
				transform.position += direction * moveSpeed * Time.deltaTime;
				animator.SetFloat("Speed", moveSpeed);
			} else {
				animator.SetFloat("Speed", 0f);
			}
		}

		void UpdateMoveByAnimation() {
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			if (v < 0) v = 0;

			animator.SetFloat("Speed", h * h + v * v);
			animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
		}

		// Update is called once per frame
		void Update() {
			if (!photonView.isMine && PhotonNetwork.connected)
				return;

			if (animator.applyRootMotion) {
				UpdateMoveByAnimation();
			} else {
				UpdateMove();
			}

			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (stateInfo.IsName("Base Layer.Run")) {
				if (Input.GetButtonDown("Fire2")) {
					animator.SetTrigger("Jump");
				}
			}
		}
	}
}