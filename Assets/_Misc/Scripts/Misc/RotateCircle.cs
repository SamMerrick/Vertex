﻿using UnityEngine;

public class RotateCircle : MonoBehaviour {

    private RectTransform rectComponent;
    private float rotateSpeed = 300f;

    private void Start()
    {
        rectComponent = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
