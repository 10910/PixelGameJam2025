using UnityEngine;

public class GamePlayPanel : MonoBehaviour
{

    private void OnEnable()
    {
        PlayGamePlayMusic();
    }

    private void OnDisable()
    {
        AudioManager.Instance.PauseMusic();
    }

    public void PlayGamePlayMusic()
    {
        Debug.Log("GamePlayPanel PlayGamePlayMusic");
        AudioManager.Instance.PlayGamePlayMusic();
    }

}
