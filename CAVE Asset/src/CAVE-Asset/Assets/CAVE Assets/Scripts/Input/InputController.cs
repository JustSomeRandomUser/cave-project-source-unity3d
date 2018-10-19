using CaveAsset.CavePlayer;
using CaveAsset.Kinect;
using CaveAsset.Wii;
using UnityEngine;

namespace CaveAsset
{
	namespace Input
	{
		public class InputController : MonoBehaviour
		{
			[Header("Input Typs")]
			[Tooltip("Enables the classic input way with WSAD and mouse look")]
			public bool enableMouseAndKeyboard = false;

			[Header("Input Sensitivity")]
			[Tooltip("Walking speed modifier")]
			public float walkingSpeed = 5.0f;
			[Tooltip("Turning speed modifier")]
			public float turningSpeed = 3.0f;

			private CavePlayerController cavePlayerController = null;
			private KinectController kinectController = null;
			private WiiController wiiController = null;

			private void Awake()
			{
				cavePlayerController = GetComponent<CavePlayerController>();
				kinectController = GetComponent<KinectController>();
				wiiController = GetComponent<WiiController>();

				if (cavePlayerController.testMode)
				{
					kinectController.enabled = false;
					wiiController.enabled = false;
				}
			}

			private void Update()
			{
				if (enableMouseAndKeyboard)
					MouseAndKeyboardMovement();

				if (!cavePlayerController.testMode)
				{
					WiimoteMovement();

					if (wiiController.GetWiimoteButtonHold(WiiRemoteButton.HOME))
						Application.Quit();
				}
			}

			/// <summary>
			/// Performs the movement based on the input that comes from mouse and keyboard.
			/// W, Arrow up -> moving forward
			/// S, Arrow down -> moving backward
			/// A, Arrow left -> moving left
			/// D, Arrow right -> moving right
			/// Mouse input -> turning up/down/left/right
			/// </summary>
			private void MouseAndKeyboardMovement()
			{
				float strafe = UnityEngine.Input.GetAxis("Horizontal") * Time.deltaTime * walkingSpeed;
				float forwardBackward = UnityEngine.Input.GetAxis("Vertical") * Time.deltaTime * walkingSpeed;
				float mouseX = UnityEngine.Input.GetAxis("Mouse X") * turningSpeed;

				transform.Translate(strafe, 0.0f, forwardBackward);
				transform.Rotate(0.0f, mouseX, 0.0f);
			}

			/// <summary>
			/// Performs the movement based on the input that comes from the Wiimote.
			/// Cross up -> moving forward
			/// Cross down -> moving backward
			/// Cross left -> truning left
			/// Cross right -> turning right
			/// </summary>
			private void WiimoteMovement()
			{
				float rotateY = 0.0f;
				float forwardBackward = 0.0f;

				if (wiiController.GetWiimoteButtonHold(WiiRemoteButton.CROSS_UP))
					forwardBackward = Time.deltaTime * walkingSpeed;
				if (wiiController.GetWiimoteButtonHold(WiiRemoteButton.CROSS_DOWN))
					forwardBackward = -1.0f * Time.deltaTime * walkingSpeed;
				if (wiiController.GetWiimoteButtonHold(WiiRemoteButton.CROSS_RIGHT))
					rotateY = turningSpeed;
				if (wiiController.GetWiimoteButtonHold(WiiRemoteButton.CROSS_LEFT))
					rotateY = -1.0f * turningSpeed;

				transform.Translate(0.0f, 0.0f, forwardBackward);
				transform.Rotate(0.0f, rotateY, 0.0f);
			}

			/// <summary>
			/// Returns the KinectController component. When in test mode returns null.
			/// </summary>
			/// <returns>KinectController component or null when in test mode</returns>
			public KinectController GetKinectController()
			{
				if (cavePlayerController.testMode)
					return null;
				else
					return kinectController;
			}

			/// <summary>
			/// Returns the WiiController component. When in test mode returns null.
			/// </summary>
			/// <returns>WiiController component or null when in test mode</returns>
			public WiiController GetWiiController()
			{
				if (cavePlayerController.testMode)
					return null;
				else
					return wiiController;
			}
		}
	}
}
