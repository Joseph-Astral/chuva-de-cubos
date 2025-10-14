using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameManager gm;
    public RectTransform scoreConteiner;

    public void Start()
    {
        scoreConteiner.anchoredPosition = new Vector2(scoreConteiner.anchoredPosition.x, 55);
        GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject
            .LeanScale(new Vector3(1.2f, 1.2f), 0.5f).setLoopPingPong();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Play();
        }
    }

    public void Play()
    {
        GetComponent<CanvasGroup>().LeanAlpha(0, 1).setOnComplete(OnComplete);
    }

    private void OnComplete()
    {
        gm.Enabled();
        Destroy(gameObject);
    }
}
