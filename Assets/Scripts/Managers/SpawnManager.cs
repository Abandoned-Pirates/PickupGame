using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public GameObject cubePrefab;
    private GameObject currentCube;


    [SerializeField] private GameObject treePrefab;
    private Tree spawnedTree;
    public TextMeshProUGUI cubeNameText;

    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    private Pose placementPose;
    public Camera ARCam;
    //private List<ARRaycastHit> hits = new List<ARRaycastHit>();


    public void SpawnItems()
    {
         if (spawnedTree == null)
         {
             return;
         }
         foreach (var spawnLocation in spawnedTree.spawnLocations)
         {
             Instantiate(cubePrefab, spawnLocation.position,Quaternion.identity);
         }
    }
    void SpawnTreeWithTouch()
    {
        var screenCenter = ARCam.ViewportToScreenPoint(new Vector3(0.5f,0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits,TrackableType.Planes);
        if (hits.Count>0)
        {
            placementPose = hits[0].pose;
            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        }

    }
    public void ShowTreeModel()
    {
        Instantiate(treePrefab,placementPose.position,placementPose.rotation);
    }
    void SpawnTree()
    {
        List<ARPlane> planes = new List<ARPlane>();
        foreach (var plane in planeManager.trackables)
        {
            planes.Add(plane);
        } 

        if (planes.Count > 0)
        {
            Vector3 center = planes[0].transform.position;
            GameObject go = Instantiate(treePrefab, center, Quaternion.identity, planes[0].transform);
            spawnedTree = go.GetComponent<Tree>();
            cubeNameText.text = "Tree Spawned";
            return;
        }
    }

    void CollectItems()
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
                    if (hit.collider.CompareTag("Fruit"))
                    {
                        FruitControlManager.instance.SetFruitAmount(hit.collider.gameObject.GetComponent<Fruit>().fruit);
                    }
                }
            }
        }
    }
    void Update()
    {
        //if (spawnedTree == null) { SpawnTree(); }
        //SpawnItems();
        CollectItems();
    }
}
