using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class FlightController : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivityX = 2f;
    public float rotationZ = 20f;
    public RectTransform area;
    private Vector3 startAcceleration;
    private bool calibrated = false;
    

    private void Start()
    {
        if(Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
            StartCoroutine(CalibrateAccelerometer());
        }
        else
        {
            Debug.LogError("KEIN ACCELEROMETER!");
        }
    }

    IEnumerator CalibrateAccelerometer()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 sum = Vector3.zero;
        int samples = 30;

        for (int i = 0; i < samples; i++)
        {
            sum += Accelerometer.current.acceleration.ReadValue();
            yield return null;
        }

        startAcceleration = sum / samples;
        calibrated = true;

        Debug.Log("Startposition: " + startAcceleration);
    }

    private void Update()
    {
        if(Accelerometer.current == null || !calibrated)
        {
            return;
        }
        Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();

        float horizontal = (acceleration.x - startAcceleration.x) * sensitivityX;
        Vector3 movement = new Vector3(horizontal, 0f, 0f);
        transform.position += movement * speed * Time.deltaTime;
        float targetRotationZ = -horizontal * rotationZ;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetRotationZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        if(area != null)
        {
            Vector3[] corners = new Vector3[4];
            area.GetWorldCorners(corners);

            Vector3 pos = transform.position;

            pos.x = Mathf.Clamp(pos.x, corners[0].x, corners[2].x);
            transform.position = pos;
        }
    }
}