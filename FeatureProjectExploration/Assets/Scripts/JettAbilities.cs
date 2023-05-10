using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JettAbilities : MonoBehaviour
{
    public PlayerMove pm;
    public CharacterController characterController;
    public SmokeProjectile sp;

    float normalGravity;
    float strafeGravity = -2f;
    float jumpHeight = 1.5f;
    float dashSpeed = 30f;
    int dashAttempts;
    float dashStartTime;
    [SerializeField]
    bool isDashing;
    [SerializeField]
    bool isThrowingSmoke = false;

    public GameObject smokeProjectile;
    public GameObject smokeFireTransform;
    public Camera playerCamera;

    SmokeProjectile currentSmokeProjectile;
    float lastTimeSmokeEnded = 0f;
    float smokeDelaySeconds = 0.5f;

    public void Start()
    {
        normalGravity = pm.gravity;
        pm.GetComponent<PlayerMove>();
        characterController.GetComponent<CharacterController>();
    }
    public void Update()
    {
        Updraft();
        SlowFall();
        Dash();
        Smoke();
    }
    #region Updraft & Float
    //Updraft or Q
    public void Updraft()
    {
        bool jump = Input.GetKeyDown(KeyCode.Q);
        if (jump)
        {
            pm.isJumping = true;
        }
        else
        {
            pm.isJumping = false;
        }
        if (pm.isGrounded && pm.jumpVelo.y < 0)
        {
            pm.jumpVelo.y = -2f;
        }
        if (pm.isJumping)
        {
            pm.jumpVelo.y = Mathf.Sqrt(jumpHeight * 2 * -2f * pm.gravity);
        }
        characterController.Move(pm.jumpVelo * Time.deltaTime);
    }
    //Passive Float
    public void SlowFall()
    {
        bool slowFall = Input.GetKey(KeyCode.Space);
        if (pm.jumpVelo.y < 0)
        {
            if (!pm.isGrounded && slowFall)
            {
                pm.gravity = strafeGravity;
            }
            else
            {
                pm.gravity = normalGravity;
            }
        }
    }
    #endregion
    #region Dash
    //Dash
    public void Dash()
    {
        bool dash = Input.GetKeyDown(KeyCode.E);
        if (dash && !isDashing)
        {
            if (dashAttempts <= 500)
            {
                OnStartDash();
            }
        }
        if (isDashing)
        {
            if (Time.time - dashStartTime <= 0.4f)
            {
                if (pm.movementVector.Equals(Vector3.zero))
                {
                    characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
                }
                else
                {
                    characterController.Move(pm.movementVector.normalized * dashSpeed * Time.deltaTime);
                }
            }
            else
            {
                OnEndDash();
            }
        }
    }
    public void OnStartDash()
    {
        isDashing = true;
        dashStartTime = Time.time;
        dashAttempts += 1;
    }
    public void OnEndDash()
    {
        isDashing = false;
        dashStartTime = 0;
    }
    #endregion
    #region Smoke
    public void Smoke()
    {
        bool isSmoking = Input.GetKeyDown(KeyCode.C);
        if(isSmoking && Time.time - lastTimeSmokeEnded >= smokeDelaySeconds)
        {
            ThrowSmoke();
        }
        if (isThrowingSmoke)
        {
            bool isControlled = Input.GetKey(KeyCode.C);
            currentSmokeProjectile.SetIsControlled(isControlled);

            bool isStoppingControl = Input.GetKeyUp(KeyCode.C);
            if (isStoppingControl)
            {
                SmokeEnd();
            }
        }
    }
    public void ThrowSmoke()
    {
        isThrowingSmoke = true;
        GameObject _smokeProjectile = Instantiate(smokeProjectile, smokeFireTransform.transform.position, playerCamera.transform.rotation);
        currentSmokeProjectile = _smokeProjectile.GetComponent<SmokeProjectile>();
        currentSmokeProjectile.StartValues(false, playerCamera);
    }
    public void SmokeEnd()
    {
        lastTimeSmokeEnded = Time.time;
        isThrowingSmoke = false;
        currentSmokeProjectile.SetIsControlled(false);
        currentSmokeProjectile = null;
    }
    #endregion
}
