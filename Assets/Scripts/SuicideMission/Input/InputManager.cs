using SuicideMission.Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SuicideMission.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private LevelLoader levelLoader;
        [SerializeField] private InputActionAsset inputActionAsset;

        [Header("Movement Speed")]
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float touchSpeedMultiplier = 0.1f;
        [SerializeField] private float keyboardSpeedMultiplier = 1f;
        [SerializeField] private float gamepadSpeedMultiplier = 1f;

        private InputAction moveAction;
        private bool moving;
        private bool firing;

        private void Awake()
        {
            InputAction move = inputActionAsset.FindAction("Move");

            InputBinding touchMoveBinding = new InputBinding
            {
                overrideProcessors = "ScaleVector2(x=" + movementSpeed * touchSpeedMultiplier + ",y=" +
                                     movementSpeed * touchSpeedMultiplier + ")"
            };

            InputBinding gamepadMoveBinding = new InputBinding
            {
                overrideProcessors = "ScaleVector2(x=" + movementSpeed * gamepadSpeedMultiplier + ",y=" +
                                     movementSpeed * gamepadSpeedMultiplier + ")"
            };

            InputBinding keyboardMoveBinding = new InputBinding
            {
                overrideProcessors = "ScaleVector2(x=" + movementSpeed * keyboardSpeedMultiplier + ",y=" +
                                     movementSpeed * keyboardSpeedMultiplier + ")"
            };
            
            move.ApplyBindingOverride(0, gamepadMoveBinding);
            move.ApplyBindingOverride(1, touchMoveBinding);
            move.ApplyBindingOverride(2, keyboardMoveBinding); //TODO Change this if you change order in input action asset.
        }

        private void Update()
        {
            if (player == null) return;

            if (moving)
            {
                Vector2 movement = moveAction.ReadValue<Vector2>();
                player.Move(movement.x, movement.y);
            }

            if (firing)
            {
                player.Fire();
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            moveAction = context.action;

            if (context.started)
            {
                moving = true;
            }
            else if (context.canceled)
            {
                moving = false;
            }
        }

        public void Fire(InputAction.CallbackContext context)
        {
            levelLoader.Input();

            if (context.started)
            {
                firing = true;
            }
            else if (context.canceled)
            {
                if (player == null) return;

                if (context.canceled)
                    firing = false;
            }
        }
    }
}