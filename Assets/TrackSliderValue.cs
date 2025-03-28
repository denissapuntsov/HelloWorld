using UnityEngine;
using UnityEngine.UI;

public class TrackSliderValue : MonoBehaviour
{
    [SerializeField] Slider controllingSlider;
    [SerializeField] float maxValue = 500f;

    public void Shrink()
    {
        Debug.Log("shrinking");
        GetComponent<RectTransform>().offsetMin = new Vector2(controllingSlider.value / 360 * maxValue, 0);
    }
}
