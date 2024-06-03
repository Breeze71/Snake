using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace V
{
    public enum InputType
    {
        GamePlay,
        UI,
    }

    public class InputManager : MonoBehaviour, PlayerInput.IGamePlayActions, PlayerInput.IUIActions
    {
        public static InputManager Instance {get; private set;} 

        private PlayerInput _playerInput;
    
        public event Action<Vector2> MoveEvent;
        public event Action JumpEvent;
        public event Action JumpCanceledEvent;

        public event Action PauseEvent;
        public event Action ResumeEvent;

        public event Action SubmitEvent;

        public event Action AimEvent;
        public event Action AimCanceledEvent;
        public event Action<Vector2> AimDirectionEvent;

        #region LC
        private void Awake() 
        {
            if(Instance != null)
            {
                Debug.LogWarning("More than one Input Manager");
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        private void OnEnable() 
        {
            if(_playerInput == null)
            {
                _playerInput = new PlayerInput();

                _playerInput.GamePlay.SetCallbacks(this);
                _playerInput.UI.SetCallbacks(this);
            }
        }

        private void Start() 
        {    
            SetActionMap(InputType.GamePlay);
        }

        private void OnDisable() 
        {
 
        }
        #endregion

        private void SetActionMap(InputType inputType)
        {
            foreach(InputActionMap inputMap in _playerInput.asset.actionMaps)
            {
                if(inputMap.name == inputType.ToString())
                {
                    inputMap.Enable();
                }
                else
                {
                    inputMap.Disable();
                }
            }
        }


        #region GamePlay
        public void OnMove(InputAction.CallbackContext context)
        {
            // MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            // if(context.phase == InputActionPhase.Performed)
            // {
            //     JumpEvent?.Invoke();
            // }

            // if(context.phase == InputActionPhase.Canceled)
            // {
            //     JumpCanceledEvent?.Invoke();
            // }
        }

        public void OnShootDirection(InputAction.CallbackContext context)
        {
            AimDirectionEvent?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnShoot(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                AimEvent?.Invoke();
            }

            else if(context.canceled)
            {
                AimCanceledEvent?.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            // if(context.phase == InputActionPhase.Performed)
            // {
            //     PauseEvent?.Invoke();

            //     SetActionMap(InputType.UI);
            //     Debug.Log("Pause");
            // }
        }
        #endregion

        #region UI
        public void OnResume(InputAction.CallbackContext context)
        {
            // if(context.phase == InputActionPhase.Performed)
            // {
            //     ResumeEvent?.Invoke();

            //     SetActionMap(InputType.GamePlay);
            //     Debug.Log("REsume");
            // }            
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            // SubmitEvent?.Invoke();
        }
        #endregion
    
    }
}
