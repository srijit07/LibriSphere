using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject infoPanel;
    [SerializeField]
    Image buttonImage;
    [SerializeField]
    GameObject objectNameText;
    [SerializeField]
    Sprite hide, unhide;
    PrefabCreator prefabCreator;
    [SerializeField]
    Material scanningMaterial;
    [SerializeField]
    float scanningEffectTransitionSpeed = 0.5f;
    [SerializeField]
    TextMeshProUGUI scanningText;

    bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        prefabCreator = GetComponent<PrefabCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (prefabCreator.spawnedPrefab == null)
        {
            infoPanel.GetComponent<Animator>().SetBool("opened", false);
            buttonImage.gameObject.transform.parent.gameObject.SetActive(false);
            objectNameText.SetActive(false);
        }
        else
        {
            buttonImage.gameObject.transform.parent.gameObject.SetActive(true);
            objectNameText.SetActive(true);
        }

        if (prefabCreator.isScanning)
        {
            scanningText.text = "Scanning";
        }
        else scanningText.text = "Stand By";
    }

    public void ToggleHide()
    {
        if (!opened) buttonImage.sprite = hide;
        else buttonImage.sprite = unhide;
        opened = !opened;
        infoPanel.GetComponent<Animator>().SetBool("opened", opened);
    }

    public void Refresh()
    {
        prefabCreator.ResetTrackedImages(); // This will reset the ARTrackedImageManager and destroy the prefab
        prefabCreator.isScanning = true;    // Re-enable scanning mode
        StartCoroutine(DelayResubscribe(0.1f));
        StartCoroutine(SmoothTransition(true)); // Transition to scanning mode
        Debug.Log("Refreshed");
    }

    public IEnumerator SmoothTransition(bool toIncrease)
    {
        float targetValue = toIncrease ? 1.5f : 0f;

        while ((toIncrease && scanningMaterial.GetFloat("_Transition") < 1.5f) || (!toIncrease && scanningMaterial.GetFloat("_Transition") > 0f))
        {
            // Lerp the value smoothly
            scanningMaterial.SetFloat("_Transition", Mathf.MoveTowards(scanningMaterial.GetFloat("_Transition"), targetValue, scanningEffectTransitionSpeed * Time.deltaTime));

            // You can use the value here, e.g., apply it to another property like a light intensity or scale
            // Debug.Log("Current Value: " + scanningMaterial.GetFloat("_Transition"));

            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator DelayResubscribe(float delay)
    {
        yield return new WaitForSeconds(delay); // Add a small delay before resubscribing
        prefabCreator.Subscribe(true); // Resubscribe after delay
    }
}
