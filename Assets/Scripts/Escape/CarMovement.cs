using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float carSpeed;
    public Camera carCamera;
    public Camera mainCamera;

    public void CarMovementStart()
    {
        UIManager.Instance.FadeCamera(false, 0.1f);
        StartCoroutine(CarMovementCoroutine());
    }

    private IEnumerator CarMovementCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SwitchCameras();
        UIManager.Instance.FadeCamera(true, 0.5f);
        while (true)
        {
            transform.Translate(Vector3.down * carSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void SwitchCameras()
    {
        if (carCamera != null && mainCamera != null)
        {
            mainCamera.enabled = false;
            carCamera.enabled = true;
        }
    }
}
