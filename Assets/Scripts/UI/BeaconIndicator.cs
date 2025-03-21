using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconIndicator : MonoBehaviour
{
    public Transform target;
    public RectTransform arrowRectTransform;
    // Offset as percentage of screen (0.5 = center)
    public Vector2 relativeOffset = new Vector2(0f, 0.1f); // 10% from center

    void Update()
    {
        if (target == null) return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 proportionalOffset = new Vector3(Screen.width * relativeOffset.x,Screen.height * relativeOffset.y,0);
        arrowRectTransform.position = screenPos + proportionalOffset;
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowRectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}