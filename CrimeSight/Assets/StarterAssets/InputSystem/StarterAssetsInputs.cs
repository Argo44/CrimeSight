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
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		//GameUI Script
		public GameUI uiScript;
		//GameManager Script
		public GameManager gameManagerScript;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			if (GameManager.State == GameState.Game)
			{
				MoveInput(value.Get<Vector2>());
			}
		}

		public void OnLook(InputValue value)
		{
			if (GameManager.State == GameState.Game)
			{
				if(cursorInputForLook)
				{
					LookInput(value.Get<Vector2>());
				}
			}
		}

		public void OnJump(InputValue value)
		{
			if (GameManager.State == GameState.Game)
			{
				JumpInput(value.isPressed);
			}
		}

		public void OnSprint(InputValue value)
		{
			if (GameManager.State == GameState.Game)
			{
				SprintInput(value.isPressed);
			}
		}

		public void OnPause(InputValue value)
		{
			uiScript.PauseGame();
		}

		public void OnNotebook(InputValue value)
		{
			uiScript.NotebookToggle();
		}

		public void OnInteract(InputValue value)
		{
			uiScript.numOfNewClues++;
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

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
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