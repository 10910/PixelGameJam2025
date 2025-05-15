using UnityEngine;
using UnityEngine.UI;
public class MusicSlider : MonoBehaviour
{
    [SerializeField]
    private Slider _musicSlider;

    [SerializeField]
    private Slider _sfxSlider;


    private void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }

}
