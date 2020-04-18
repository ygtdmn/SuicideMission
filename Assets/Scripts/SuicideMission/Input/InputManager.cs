using SuicideMission.Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SuicideMission.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private LevelLoader levelLoader;

        private InputAction moveAction;
        private bool moving;
        private bool firing;

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
            Debug.Log("Move request");
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
            Debug.Log("Fire request");
            if (context.performed)
            {
                levelLoader.Input();
            }

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