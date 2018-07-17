using UnityEngine;
using System.Collections;

namespace PC2D {
	public class DifferentSpeeds : MonoBehaviour {
		public KeyCode keyCodeToStart;
		public float distanceToGo;

		private bool _hasStarted;
		private Vector3 _startPosition;
		private PC2DMotor _motor;

		// Use this for initialization
		void Start() {
			_motor = GetComponent<PC2DMotor>();
		}

		// Update is called once per frame
		void Update() {
			if (!_hasStarted && Input.GetKeyDown(keyCodeToStart)) {
				_hasStarted = true;
				_startPosition = transform.position;
				_motor.normalizedXMovement = 1f;
			}

			if (_hasStarted) {
				if (Vector3.Distance(transform.position, _startPosition) >= distanceToGo) {
					_motor.normalizedXMovement *= -1;
					_startPosition = transform.position;
				}
			}
		}
	}
}
