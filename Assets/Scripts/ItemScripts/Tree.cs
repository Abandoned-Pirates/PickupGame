using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Transform[] spawnLocations;
    void Start()
    {
        //Projenin ilerleyi�ine g�re SpawnItems fonksiyonuna direkt olarak A�ac�n kendisini parametre olarak g�nderebiliriz.
        SpawnManager.Instance.SpawnItems();
    }

}
