using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhotonTutorial {

	[RequireComponent(typeof(Animator))]
	public class PlayerAnimatorManager : Photon.MonoBehaviour {

		public float directionDampTime = .25f;
		
		Animator animator;		

		// Use this for initialization
		void Start() {
			animator = GetComponent<Animator>();
		}

		// Update is called once per frame
		void Update() {
			if (!photonView.isMine && PhotonNetwork.connected)
				return;

			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			if (v < 0) v = 0;

			animator.SetFloat("Speed", h * h + v * v);
			animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (stateInfo.IsName("Base Layer.Run")) {
				if (Input.GetButtonDown("Fire2")) {
					animator.SetTrigger("Jump");
				}
			}
		}
	}
}