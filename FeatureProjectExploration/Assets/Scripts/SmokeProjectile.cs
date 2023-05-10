using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeProjectile : MonoBehaviour
{
    public GameObject smokeBallPrefab;
    Camera playerCamera;

    public bool isControlled;
    Vector3 startingPos;
    float movementSpeed = 20.0f;
    float maxDistance = 50.0f;
    float distanceTraveled = 0f;
    float dropForce = -2.0f;
    float dropForceIncrement = -3.0f;

    public void Start()
    {
        startingPos = transform.position;
    }
    public void Update()
    {
        if (isControlled)
        {
            transform.rotation = playerCamera.transform.rotation;
        }
        Vector3 moveVec = (transform.forward * movementSpeed * Time.deltaTime);
        if (!isControlled)
        {
            dropForce += dropForceIncrement * Time.deltaTime;
            moveVec += (transform.up * dropForce * Time.deltaTime);
        }
        Vector3 newPos = transform.position + moveVec;
        distanceTraveled = Vector3.Distance(startingPos, newPos);
        if (distanceTraveled > maxDistance)
        {
            CreateSmoke(transform.position);
        }
        else
        {
            transform.position += moveVec;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        CreateSmoke(collision.contacts[0].point);
    }
    public void StartValues(bool _isControlled, Camera _playerCamera)
    {
        isControlled = _isControlled;
        playerCamera = _playerCamera;
    }
    public void SetIsControlled(bool _isControlled)
    {
        isControlled = _isControlled;
    }
    public void CreateSmoke(Vector3 position)
    {
        Instantiate(smokeBallPrefab, position, transform.rotation);
        Destroy(this.gameObject);
    }
}
