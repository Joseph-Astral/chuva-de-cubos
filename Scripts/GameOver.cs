using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void Restart()
    {
        gameObject.SetActive(false);
        GameManager.Instance.Enabled();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
