using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ManipulateObject : MonoBehaviour
{
    ARTrackedImageManager arTrackedImageManager;
    [SerializeField]
    PrefabCreator prefabCreator;

    GameObject spawnedPrefab;
    bool objectExists = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Forward()
    {
        spawnedPrefab = prefabCreator.spawnedPrefab;
        if (spawnedPrefab != null)
        {
            // spawnedPrefab.transform.position += new Vector3 ()
        }
        else
        {
            
        }
    }
}
