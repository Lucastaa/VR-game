using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    
    // Danh sách các Prefab nhân vật của bạn
    public GameObject[] playerPrefabs; 
    private readonly Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

    void Awake() => trackedImageManager = GetComponent<ARTrackedImageManager>();

    void OnEnable() => trackedImageManager.trackedImagesChanged += OnChanged;
    void OnDisable() => trackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Khi nhận diện thẻ bài mới
        foreach (var newImage in eventArgs.added)
        {
            foreach (var prefab in playerPrefabs)
            {
                // Nếu tên thẻ bài trong Library khớp với tên Prefab
                if (newImage.referenceImage.name == prefab.name && !spawnedPrefabs.ContainsKey(newImage.referenceImage.name))
                {
                    var newPrefab = Instantiate(prefab, newImage.transform);
                    spawnedPrefabs.Add(newImage.referenceImage.name, newPrefab);
                }
            }
        }
        
        // Khi thẻ bài di chuyển ngoài đời thực, cập nhật vị trí model trong game
        foreach (var updatedImage in eventArgs.updated)
        {
            if (spawnedPrefabs.ContainsKey(updatedImage.referenceImage.name))
            {
                spawnedPrefabs[updatedImage.referenceImage.name].SetActive(updatedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking);
            }
        }
    }
}