using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField]
    List<SpawnableItem> spawnableItems = new(); // biscuitObject, dnaObject;
    [SerializeField]
    Vector3 prefabOffset;
    [SerializeField]
    TextMeshProUGUI infoText;
    [SerializeField]
    RawImage infoImage;
    [SerializeField]
    Material scanningMaterial;
    [SerializeField]
    float scanningEffectTransitionSpeed = 0.5f;
    [SerializeField]
    TextMeshProUGUI imageName, objectName;


    [HideInInspector]
    public GameObject spawnedPrefab, biscuit, dna;
    [HideInInspector]
    public bool isScanning = true;
    ARTrackedImageManager arTrackedImageManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        arTrackedImageManager = gameObject.GetComponent<ARTrackedImageManager>();
        Subscribe(true);
    }

    private void OnDisable()
    {
        Subscribe(false);
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (isScanning)
            foreach(ARTrackedImage image in args.added)
            {
                // spawnedPrefab = Instantiate(spawnableItems[2].prefab, image.transform);
                string nameOfImageScanned = image.referenceImage.name;
                imageName.text = nameOfImageScanned;
                SpawnableItem itemToSpawn = ReturnItemToSpawn(nameOfImageScanned);
                if (itemToSpawn != null)
                    objectName.text = itemToSpawn.Name;
                else
                    objectName.text = "Null Object";
                // spawnedPrefab = Instantiate(itemToSpawn.prefab, image.transform);
                if (itemToSpawn != null) 
                {
                    // Destroy(spawnedPrefab);
                    spawnedPrefab = Instantiate(itemToSpawn.prefab, image.transform);
                    infoText.text = itemToSpawn.information;
                    infoImage.texture = itemToSpawn.image;
                    isScanning = false;
                    Subscribe(false);
                    StartCoroutine(ClearList(args));
                    StartCoroutine(SmoothTransition(false));
                }

                if (spawnedPrefab != null)
                {
                    spawnedPrefab.transform.position += prefabOffset;
                }
            }
    }

    SpawnableItem ReturnItemToSpawn(string itemName)
    {
        foreach (var item in spawnableItems)
        {
            if (item.Name == itemName) return item;
        }
        return null;
    }

    public IEnumerator SmoothTransition(bool toIncrease)
    {
        float targetValue = toIncrease ? 1f : 0f;

        while ((toIncrease && scanningMaterial.GetFloat("_Transition") < 1f) || (!toIncrease && scanningMaterial.GetFloat("_Transition") > 0f))
        {
            // Lerp the value smoothly
            scanningMaterial.SetFloat("_Transition", Mathf.MoveTowards(scanningMaterial.GetFloat("_Transition"), targetValue, scanningEffectTransitionSpeed * Time.deltaTime));

            // You can use the value here, e.g., apply it to another property like a light intensity or scale
            // Debug.Log("Current Value: " + scanningMaterial.GetFloat("_Transition"));

            yield return null; // Wait for the next frame
        }
    }

    public void ResetTrackedImages()
    {
        Debug.Log("Resetting tracked images...");

        // Disable the tracked image manager
        arTrackedImageManager.enabled = false;

        // Clear any existing spawned prefab (if any)
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);
            spawnedPrefab = null;
        }

        // Re-enable the tracked image manager to detect images again
        arTrackedImageManager.enabled = true;
    }

    public void Subscribe(bool yes)
    {
        if (yes) arTrackedImageManager.trackedImagesChanged += OnImageChanged;
        else arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    IEnumerator ClearList(ARTrackedImagesChangedEventArgs args)
    {
        yield return new WaitForSeconds(1f);
        args.added.Clear();
    }
}
