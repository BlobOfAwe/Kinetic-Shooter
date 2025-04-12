using UnityEngine;
using UnityEngine.InputSystem;

public class CheatCode : MonoBehaviour
{
    private PlayerInput input;
    private InputAction navigate;
    private InputAction submit;
    private InputAction cancel;
    private int increment = 0;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInput>();
        navigate = input.actions["Navigate"];
        submit = input.actions["Submit"];
        cancel = input.actions["Cancel"];
    }

    private void Update()
    {

    }
}
