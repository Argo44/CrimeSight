using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool sight;
		public bool sprint;
		public bool flashlight = false;
		public KeyCode QTEkey;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		//GameUI Script
		public GameUI uiScript;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			if (GameManager.State == GameState.Game && !uiScript.inTrapQTE)
			{
				MoveInput(value.Get<Vector2>());
			}
		}

		public void OnLook(InputValue value)
		{
			if (GameManager.State == GameState.Game && !uiScript.inTrapQTE)
			{
				if(cursorInputForLook)
				{
					LookInput(value.Get<Vector2>());
				}
			}
		}

		public void OnUseSight(InputValue value)
		{
			if (GameManager.State == GameState.Game && !uiScript.inTrapQTE)
			{
				SightInput(value.isPressed);
			}
		}

		public void OnFlashlight(InputValue value)
        {
			if (GameManager.State == GameState.Game && !uiScript.inTrapQTE)
            {
				flashlight = !flashlight;
            }
        }

		public void OnSprint(InputValue value)
		{
			if (GameManager.State == GameState.Game && !uiScript.inTrapQTE)
			{
				SprintInput(value.isPressed);
			}
		}

		public void OnPause(InputValue value)
		{
			uiScript.PauseGame();
			move = Vector2.zero;
			look = Vector2.zero;
		}

		public void OnNotebook(InputValue value)
		{
			uiScript.NotebookToggle();
			move = Vector2.zero;
			look = Vector2.zero;
		}

		public void OnInteract(InputValue value)
		{
			if (GameManager.SelectedObject != null && !uiScript.inTrapQTE)
				GameManager.SelectedObject.OnInteract();
		}

		public void OnQTEc(InputValue value)
        {
			QTEkey = KeyCode.C;
		}

		public void OnQTEx(InputValue value)
		{
			QTEkey = KeyCode.X;
		}

		public void OnQTEt(InputValue value)
		{
			QTEkey = KeyCode.T;
		}

		public void OnQTEg(InputValue value)
		{
			QTEkey = KeyCode.G;
		}

		public void OnQTEh(InputValue value)
		{
			QTEkey = KeyCode.H;
		}

		public void OnQTEy(InputValue value)
		{
			QTEkey = KeyCode.Y;
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void SightInput(bool newJumpState)
		{
			sight = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void QTEInput(KeyCode newKey)
		{
			QTEkey = newKey;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}