using UnityEngine;
using System.Collections;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float dashDuration;
    public float maxDashYSpeed;

    [Header("CameraEffects")]
    public PlayerCam cam;
    public float dashFov;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("UI")]
    public UnityEngine.UI.Slider dashSlider;

    private Color availableColor = Color.white;
    private Color cooldownColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
            Dash();

        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
            dashSlider.value = 1 - (dashCdTimer / dashCd); 
        }
        else
        {
            dashSlider.value = 1;
        }
    }

    private void Dash() 
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        cam.DoFov(dashFov);

        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam;
        else
            forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);    
        
        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;
        
        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        if (resetVel)
            rb.linearVelocity = Vector3.zero; 

        StartCoroutine(SmoothDash());
    }

    private IEnumerator SmoothDash()
    {
        float elapsed = 0;
        Vector3 targetVelocity = orientation.forward * dashForce + orientation.up * dashUpwardForce;

        while (elapsed < dashDuration)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, elapsed / dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void ResetDash() 
    { 
        pm.dashing = false;
        pm.maxYSpeed = 0;

        cam.DoFov(90f);

        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        return orientation.forward;
    }

    /* private Vector3 GetDirection(Transform forwardT) 
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            direction = forwardT.forward;

        if (verticalInput == 0 && horizontalInput == 0)
            direction = forwardT.forward;

        return direction.normalized;
    }
    */
}
