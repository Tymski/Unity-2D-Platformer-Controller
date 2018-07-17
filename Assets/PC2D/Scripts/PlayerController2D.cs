using UnityEngine;
namespace PC2D {
	/// <summary>
	/// This class is a simple example of how to build a controller that interacts with PC2DMotor.
	/// </summary>
	[RequireComponent(typeof(PC2DMotor))]
	public class PlayerController2D : MonoBehaviour {
		private PC2DMotor _motor;
		private bool _restored = true;
		private bool _enableOneWayPlatforms;
		private bool _oneWayPlatformsAreWalls;

		// Use this for initialization
		void Start() {
			_motor = GetComponent<PC2DMotor>();
		}

		// before enter en freedom state for ladders
		void FreedomStateSave(PC2DMotor motor) {
			if (!_restored) // do not enter twice
				return;

			_restored = false;
			_enableOneWayPlatforms = _motor.enableOneWayPlatforms;
			_oneWayPlatformsAreWalls = _motor.oneWayPlatformsAreWalls;
		}
		// after leave freedom state for ladders
		void FreedomStateRestore(PC2DMotor motor) {
			if (_restored) // do not enter twice
				return;

			_restored = true;
			_motor.enableOneWayPlatforms = _enableOneWayPlatforms;
			_motor.oneWayPlatformsAreWalls = _oneWayPlatformsAreWalls;
		}

		// Update is called once per frame
		void Update() {
			// use last state to restore some ladder specific values
			if (_motor.motorState != PC2DMotor.MotorState.FreedomState) {
				// try to restore, sometimes states are a bit messy because change too much in one frame
				FreedomStateRestore(_motor);
			}

			// Jump?
			// If you want to jump in ladders, leave it here, otherwise move it down
			if (Input.GetButtonDown(InputButtonNames.JUMP)) {
				_motor.Jump();
				_motor.DisableRestrictedArea();
			}

			_motor.jumpingHeld = Input.GetButton(InputButtonNames.JUMP);

			// XY freedom movement
			if (_motor.motorState == PC2DMotor.MotorState.FreedomState) {
				_motor.normalizedXMovement = Input.GetAxis(InputButtonNames.HORIZONTAL);
				_motor.normalizedYMovement = Input.GetAxis(InputButtonNames.VERTICAL);

				return; // do nothing more
			}

			// X axis movement
			if (Mathf.Abs(Input.GetAxis(InputButtonNames.HORIZONTAL)) > PC2D.Globals.INPUT_THRESHOLD) {
				_motor.normalizedXMovement = Input.GetAxis(InputButtonNames.HORIZONTAL);
			}
			else {
				_motor.normalizedXMovement = 0;
			}

			if (Input.GetAxis(InputButtonNames.VERTICAL) != 0) {
				bool up_pressed = Input.GetAxis(InputButtonNames.VERTICAL) > 0;
				if (_motor.IsOnLadder()) {
					if (
						(up_pressed && _motor.ladderZone == PC2DMotor.LadderZone.Top)
						||
						(!up_pressed && _motor.ladderZone == PC2DMotor.LadderZone.Bottom)
					 ) {
						// do nothing!
					}
					// if player hit up, while on the top do not enter in freeMode or a nasty short jump occurs
					else {
						// example ladder behaviour

						_motor.FreedomStateEnter(); // enter freedomState to disable gravity
						_motor.EnableRestrictedArea();  // movements is retricted to a specific sprite bounds

						// now disable OWP completely in a "trasactional way"
						FreedomStateSave(_motor);
						_motor.enableOneWayPlatforms = false;
						_motor.oneWayPlatformsAreWalls = false;

						// start XY movement
						_motor.normalizedXMovement = Input.GetAxis(InputButtonNames.HORIZONTAL);
						_motor.normalizedYMovement = Input.GetAxis(InputButtonNames.VERTICAL);
					}
				}
			}
			else if (Input.GetAxis(InputButtonNames.VERTICAL) < -PC2D.Globals.FAST_FALL_THRESHOLD) {
				_motor.fallFast = false;
			}

			if (Input.GetButtonDown(InputButtonNames.DASH)) {
				_motor.Dash();
			}
		}
	}
}