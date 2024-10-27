using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{

    public Vector2 offset;
    public Tilemap map;
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float smoothSpeed = 0.125f;
    
    private float _cameraHalfHeight;
    private float _cameraHalfWidth;

    private void Start()
    {
        _cameraHalfHeight = Camera.main.orthographicSize;
        _cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
    }

    private void FixedUpdate()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //  영역 지정
        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(transform.position.x + horizontal * moveSpeed,  map.localBounds.min.x+ _cameraHalfWidth, map.localBounds.max.x - _cameraHalfWidth),
            Mathf.Clamp(transform.position.y + vertical * moveSpeed, map.localBounds.min.y + _cameraHalfHeight, map.localBounds.max.y - _cameraHalfHeight),
            transform.position.z);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
