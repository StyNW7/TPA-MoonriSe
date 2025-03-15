using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingStack : MonoBehaviour
{

    public PostProcessVolume postProcessVolume;
    private Bloom bloom;
    private Vignette vignette;
    private ColorGrading colorGrading;

    void Start()
    {
        if (postProcessVolume.profile.TryGetSettings(out bloom))
        {
            bloom.intensity.value = 30f;
        }

        if (postProcessVolume.profile.TryGetSettings(out colorGrading))
        {
            colorGrading.temperature.value = 40f;
        }

        if (postProcessVolume.profile.TryGetSettings(out vignette))
        {
            vignette.intensity.value = 20f;
        }

    }

}
