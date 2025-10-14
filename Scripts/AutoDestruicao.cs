using UnityEngine;

public class AutoDestruicao : MonoBehaviour
{
    public float delay;
    void Start()
    {
        Destroy(gameObject, delay);
    }
}
