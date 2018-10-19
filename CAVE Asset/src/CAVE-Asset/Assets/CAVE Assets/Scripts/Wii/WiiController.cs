using UnityEngine;
using System.Collections;
using WiimoteApi;

namespace CaveAsset
{
	namespace Wii
	{
		public class WiiController : MonoBehaviour
		{
			private Wiimote wiimote = null;
			private WiiRemote wiiRemote = null;

			private void Start()
			{
				wiiRemote = new WiiRemote();
			}

			private void Update()
			{
				if (wiimote == null)
				{
					if (WiimoteManager.HasWiimote())
						wiimote = WiimoteManager.Wiimotes[0];
					else
						WiimoteManager.FindWiimotes();
				}
				else
				{
					while (wiimote.ReadWiimoteData() > 0)
						;

					wiiRemote.Update(wiimote.Button);
				}
			}

			private void OnApplicationQuit()
			{
				if (wiimote != null)
					WiimoteManager.Cleanup(wiimote);
			}

			/// <summary>
			/// Coroutine which let the Wiimote vibrate for the given time in milliseconds.
			/// </summary>
			/// <param name="timeInMilliSeconds">Float</param>
			/// <returns>IEnumerator</returns>
			private IEnumerator VibrateWiimoteCoroutine(float timeInMilliSeconds)
			{
				wiimote.RumbleOn = true;
				wiimote.SendStatusInfoRequest();

				yield return new WaitForSeconds(timeInMilliSeconds / 1000.0f);

				wiimote.RumbleOn = false;
				wiimote.SendStatusInfoRequest();
			}

			/// <summary>
			/// Let the Wiimote vibrate for the given time in milliseconds.
			/// </summary>
			/// <param name="timeInMilliSeconds">Float</param>
			public void VibrateWiimote(float timeInMilliSeconds = 500.0f)
			{
				StopCoroutine(VibrateWiimoteCoroutine(timeInMilliSeconds));
				StartCoroutine(VibrateWiimoteCoroutine(timeInMilliSeconds));
			}

			/// <summary>
			/// Returns true if the given button is pressed down, else false.
			/// </summary>
			/// <param name="wiiRemoteButton">WiiRemoteButton</param>
			/// <returns>true if the given button is pressed down, else false</returns>
			public bool GetWiimoteButtonDown(WiiRemoteButton wiiRemoteButton)
			{
				return wiiRemote.GetButton(wiiRemoteButton, WiiRemoteButtonState.DOWN);
			}

			/// <summary>
			/// Returns true if the given button is hold down, else false.
			/// </summary>
			/// <param name="wiiRemoteButton">WiiRemoteButton</param>
			/// <returns>true if the given button is hold down, else false</returns>
			public bool GetWiimoteButtonHold(WiiRemoteButton wiiRemoteButton)
			{
				return wiiRemote.GetButton(wiiRemoteButton, WiiRemoteButtonState.HOLD);
			}

			/// <summary>
			/// Returns true if the given button is released, else false.
			/// </summary>
			/// <param name="wiiRemoteButton">WiiRemoteButton</param>
			/// <returns>true if the given button is released, else false</returns>
			public bool GetWiimoteButtonUp(WiiRemoteButton wiiRemoteButton)
			{
				return wiiRemote.GetButton(wiiRemoteButton, WiiRemoteButtonState.UP);
			}
		}
	}
}
