using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    private GameObject currentCube;
    public TextMeshProUGUI cubeNameText;

    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void SpawnItems()
    {
        List<ARPlane> planes = new List<ARPlane>();
        foreach (var plane in planeManager.trackables)
        {
            planes.Add(plane);
        }

        if (planes.Count > 0)
        {
            ARPlane randomPlane = planes[Random.Range(0, planes.Count)];
            Vector3 center = randomPlane.transform.position + Vector3.up;
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            if (currentCube == null)
            {
                currentCube = Instantiate(cubePrefab, center, Quaternion.identity);
                return;
            }
            currentCube.transform.position = center + randomOffset;
        }
    }

    private void SpawnOnTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            if (raycastManager.Raycast(touchPosition, hits, TrackableType.None))
            {
                // Raycast sonucunda bir yer tespit edildiyse
                Pose hitPose = hits[0].pose;
                Vector3 randomOffset = new Vector3(Random.Range(-0.1f, 0.1f), 0f, Random.Range(-0.1f, 0.1f));
                Quaternion randomRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
                GameObject newCube = Instantiate(cubePrefab, hitPose.position + randomOffset, randomRotation);
            }
        }
    }

    void TouchDetection()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Cube"))
                    {
                        cubeNameText.text = hit.collider.name;
                        SpawnItems();
                    }
                }
            }
        }
    }
    void Update()
    {
        if (currentCube == null)
        {
            SpawnItems();
        }
        TouchDetection();
    }
}
