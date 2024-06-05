using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using V.UI;

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
        [SerializeField] private InputType _startInputType;
    
        public event Action<Vector2> MoveEvent;
        public event Action AcclerateEvent;
        public event Action AcclerateCanceledEvent;

        public event Action PauseEvent;
        public event Action ResumeEvent;

        public event Action SubmitEvent;

        public event Action AimEvent;
        public event Action AimCanceledEvent;
        public event Action<Vector2> AimDirectionEvent;

        #region Input Value
        public Vector2 NavigationInput {get; set;}
        public event Action OnSubmitEvent; // Any Key
        public event Action OnConfirmEvent; // Enter
        #endregion

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
            SetActionMap(_startInputType);
        }

        private void OnDisable() 
        {
 
        }
        #endregion

        public void SetActionMap(InputType inputType)
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
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
        
        public void OnAcclerate(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                AcclerateEvent?.Invoke();
            }

            if(context.canceled)
            {
                AcclerateCanceledEvent?.Invoke();
            }
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
            if(context.started)
            {
                PauseEvent?.Invoke();

                Debug.Log("Pause");
                SetActionMap(InputType.UI);
            }
        }
        #endregion

        #region UI
        public void OnResume(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                ResumeEvent?.Invoke();

                SetActionMap(InputType.GamePlay);
                Debug.Log("REsume");
            }            
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                OnSubmitEvent?.Invoke();
                Debug.Log("sub");
            }
        }
        public void OnConfirm(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                OnConfirmEvent?.Invoke();
                Debug.Log("com");
            }            
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            
        }
        #endregion

    }
}
