using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceImageVideo : MonoBehaviour
{
    ARTrackedImageManager ImageTrack;
    public GameObject[] InteractablePrefabs;
    readonly Dictionary<string, GameObject> InstPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        ImageTrack = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        ImageTrack.trackedImagesChanged += OnTrackedImagesChanged;
    }
    private void OnDisable()
    {
        ImageTrack.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs ImageArgs)
    {
        foreach (var TrackedARImage in ImageArgs.added)
        {
            var NameOfImage = TrackedARImage.referenceImage.name;
            
            foreach (var CurrentPref in InteractablePrefabs)
            {
                if(string.Compare(CurrentPref.name, NameOfImage, System.StringComparison.OrdinalIgnoreCase) == 00 && !InstPrefabs.ContainsKey(NameOfImage))
                {
                    var NewPref = Instantiate(CurrentPref, TrackedARImage.transform);
                    InstPrefabs[NameOfImage] = NewPref;
                }
            }

        }
        foreach (var TrackedARImage in ImageArgs.updated)
        {
            InstPrefabs[TrackedARImage.referenceImage.name].SetActive(TrackedARImage.trackingState == TrackingState.Tracking);
        }

        foreach (var TrackedARImage in ImageArgs.removed)
        {
            Destroy(InstPrefabs[TrackedARImage.referenceImage.name]);
            InstPrefabs.Remove(TrackedARImage.referenceImage.name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
