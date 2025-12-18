using UnityEngine;
using UnityEngine.InputSystem;

public class Motor : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction m_applyForceAction;
    private InputAction m_showVelocityAction;

    public Rigidbody Body;
    
    [Tooltip("Force in Newtons (N).")]
    public Vector3 Force;
    public bool IsWorking { get; private set; }
    public float Interval = 0f;
    public float WorkingInterval = 0f;



    private void Awake()
    {
        m_applyForceAction = InputSystem.actions.FindAction("ApplyForce");
        m_showVelocityAction = InputSystem.actions.FindAction("ShowVelocity");
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("Rocket").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Rocket").Disable();
    }

    private void Update()
    {
        if (!IsWorking && m_applyForceAction.WasPressedThisFrame())
        {
            ApplyForce(Interval);
        }

        if (m_showVelocityAction.WasPressedThisFrame())
        {
            Debug.Log($"Velocity: {Body.linearVelocity}");
        }
    }

    private void FixedUpdate()
    {
        if (IsWorking)
        {
            if (WorkingInterval - Time.fixedDeltaTime > 0f)
            {
                Body.AddRelativeForce(Force, ForceMode.Force);

                WorkingInterval -= Time.fixedDeltaTime;
            }
            else if(WorkingInterval > float.Epsilon)
            {
                Body.AddRelativeForce(Force * (WorkingInterval / Time.fixedDeltaTime), ForceMode.Force);

                WorkingInterval = 0f;
                IsWorking = false;
            }
            else
            {
                WorkingInterval = 0f;
                IsWorking = false;
            }
        }
    }



    public void ApplyForce(float interval)
    {
        WorkingInterval = interval;
        IsWorking = true;
    }
}
