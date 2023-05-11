using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public int totalKnives = 5;
    float shootCooldown = 0.1f;
    float shootForce = 100f;
    float shootUpwardForce;

    KeyCode leftShootKey = KeyCode.Mouse0;
    KeyCode rightShootKey = KeyCode.Mouse1;

    [SerializeField]
    bool isDashing;
    [SerializeField]
    bool isThrowingSmoke = false;
    [SerializeField]
    bool readyToThrow = false;

    public GameObject knifeProjectile;
    public GameObject smokeProjectile;
    public GameObject smokeFireTransform;
    public Camera playerCamera;
    public GameObject attackPoint1;
    public GameObject attackPoint2;
    public GameObject attackPoint3;
    public GameObject attackPoint4;
    public GameObject attackPoint5;

    SmokeProjectile currentSmokeProjectile;
    float lastTimeSmokeEnded = 0f;
    float smokeDelaySeconds = 0.5f;
    public void Start()
    {
        attackPoint1.SetActive(false);
        attackPoint2.SetActive(false);
        attackPoint3.SetActive(false);
        attackPoint4.SetActive(false);
        attackPoint5.SetActive(false);
        readyToThrow = false;
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
        Shoot();
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
    #region Knives

    public void Shoot()
    {

        bool knivesOn = Input.GetKeyDown(KeyCode.X);
        if (knivesOn && !readyToThrow)
        {
            MoreKnives();
            attackPoint1.SetActive(true);
            attackPoint2.SetActive(true);
            attackPoint3.SetActive(true);
            attackPoint4.SetActive(true);
            attackPoint5.SetActive(true);
            readyToThrow = true;
        }
        bool knivesOff = Input.GetKey(KeyCode.Z);
        if (knivesOff && readyToThrow)
        {
            attackPoint1.SetActive(false);
            attackPoint2.SetActive(false);
            attackPoint3.SetActive(false);
            attackPoint4.SetActive(false);
            attackPoint5.SetActive(false);
            readyToThrow = false;
            MoreKnives();
        }
        if (Input.GetKeyDown(leftShootKey) && readyToThrow && totalKnives > 4)
        {
            shootKnife1();

        }
        else if(Input.GetKeyDown(leftShootKey) && readyToThrow && totalKnives > 3)
        {
            shootKnife2();

        }
        else if (Input.GetKeyDown(leftShootKey) && readyToThrow && totalKnives > 2)
        {
            shootKnife3();

        }
        else if (Input.GetKeyDown(leftShootKey) && readyToThrow && totalKnives > 1)
        {
            shootKnife4();

        }
        else if (Input.GetKeyDown(leftShootKey) && readyToThrow && totalKnives > 0)
        {
            shootKnife5();

        }
        if(Input.GetKeyDown(rightShootKey)&& readyToThrow && totalKnives > 0)
        {
            ScatterShot();
        }
    }
    public void shootKnife1()
    {
        readyToThrow = false;
        GameObject _projectile = Instantiate((knifeProjectile), attackPoint1.transform.position, playerCamera.transform.rotation);
        Rigidbody _projectileRB = _projectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = playerCamera.transform.forward * shootForce + transform.up * shootUpwardForce;
        _projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
        totalKnives--;
        Invoke(nameof(ResetKnife), shootCooldown);
        attackPoint1.SetActive(false);
    }
    public void shootKnife2()
    {
        readyToThrow = false;
        GameObject _projectile = Instantiate((knifeProjectile), attackPoint2.transform.position, playerCamera.transform.rotation);
        Rigidbody _projectileRB = _projectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = playerCamera.transform.forward * shootForce + transform.up * shootUpwardForce;
        _projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
        totalKnives--;
        Invoke(nameof(ResetKnife), shootCooldown);
        attackPoint2.SetActive(false);
    }
    public void shootKnife3()
    {
        readyToThrow = false;
        GameObject _projectile = Instantiate((knifeProjectile), attackPoint3.transform.position, playerCamera.transform.rotation);
        Rigidbody _projectileRB = _projectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = playerCamera.transform.forward * shootForce + transform.up * shootUpwardForce;
        _projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
        totalKnives--;
        Invoke(nameof(ResetKnife), shootCooldown);
        attackPoint3.SetActive(false);
    }
    public void shootKnife4()
    {
        readyToThrow = false;
        GameObject _projectile = Instantiate((knifeProjectile), attackPoint4.transform.position, playerCamera.transform.rotation);
        Rigidbody _projectileRB = _projectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = playerCamera.transform.forward * shootForce + transform.up * shootUpwardForce;
        _projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
        totalKnives--;
        Invoke(nameof(ResetKnife), shootCooldown);
        attackPoint4.SetActive(false);
    }
    public void shootKnife5()
    {
        readyToThrow = false;
        GameObject _projectile = Instantiate((knifeProjectile), attackPoint5.transform.position, playerCamera.transform.rotation);
        Rigidbody _projectileRB = _projectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = playerCamera.transform.forward * shootForce + transform.up * shootUpwardForce;
        _projectileRB.AddForce(forceToAdd, ForceMode.Impulse);
        totalKnives--;
        Invoke(nameof(ResetKnife), shootCooldown);
        attackPoint5.SetActive(false);
    }
    public void ScatterShot()
    {
        readyToThrow = false;
        GameObject _projectile1 = Instantiate((knifeProjectile), attackPoint1.transform.position, playerCamera.transform.rotation);
        GameObject _projectile2 = Instantiate((knifeProjectile), attackPoint2.transform.position, playerCamera.transform.rotation);
        GameObject _projectile3 = Instantiate((knifeProjectile), attackPoint3.transform.position, playerCamera.transform.rotation);
        GameObject _projectile4 = Instantiate((knifeProjectile), attackPoint4.transform.position, playerCamera.transform.rotation);
        GameObject _projectile5 = Instantiate((knifeProjectile), attackPoint5.transform.position, playerCamera.transform.rotation);
        Rigidbody _projectileRB1 = _projectile1.GetComponent<Rigidbody>();
        Rigidbody _projectileRB2 = _projectile2.GetComponent<Rigidbody>();
        Rigidbody _projectileRB3 = _projectile3.GetComponent<Rigidbody>();
        Rigidbody _projectileRB4 = _projectile4.GetComponent<Rigidbody>();
        Rigidbody _projectileRB5 = _projectile5.GetComponent<Rigidbody>();
        Vector3 forceToAdd = playerCamera.transform.forward * shootForce + transform.up * shootUpwardForce;
        _projectileRB1.AddForce(forceToAdd, ForceMode.Impulse);
        _projectileRB2.AddForce(forceToAdd, ForceMode.Impulse);
        _projectileRB3.AddForce(forceToAdd, ForceMode.Impulse);
        _projectileRB4.AddForce(forceToAdd, ForceMode.Impulse);
        _projectileRB5.AddForce(forceToAdd, ForceMode.Impulse);
        totalKnives = 0;
        Invoke(nameof(ResetKnife), shootCooldown);
        attackPoint1.SetActive(false);
        attackPoint2.SetActive(false);
        attackPoint3.SetActive(false);
        attackPoint4.SetActive(false);
        attackPoint5.SetActive(false);
    }

    public void ResetKnife()
    {
        readyToThrow = true;
    }
    public void MoreKnives()
    {
        totalKnives = 5;
    }
    #endregion
}
