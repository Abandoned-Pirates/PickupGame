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
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //[SerializeField]
    //Transform[] spawnLocations;

    [Header("Max-Min Values")]
    private float minX = -2.5f;
    private float maxX = 2.5f;
    private float minY = 5.5f;
    private float maxY = 9.5f;
    private float minZ = -2.5f;
    private float maxZ = 2.5f;

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
            spawnedTree = Instantiate(treePrefab, center, Quaternion.identity).GetComponent<Tree>();
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
                    if (hit.collider.CompareTag("Cube"))
                    {
                        cubeNameText.text = hit.collider.name;
                    }
                }
            }
        }
    }
    void Update()
    {
        if (spawnedTree == null) { SpawnTree(); }
        //SpawnItems();
        CollectItems();
    }
}
