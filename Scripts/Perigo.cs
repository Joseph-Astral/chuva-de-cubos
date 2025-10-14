using UnityEngine;

public class Perigo : MonoBehaviour
{
    Vector3 rotation;
    public ParticleSystem efeitoQuebra;
    void Start()
    {
        var xRotation = UnityEngine.Random.Range(90f, 180f);
        rotation = new Vector3(-xRotation, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ilha"))
        {
            Destroy(gameObject);
            Instantiate(efeitoQuebra, transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }
}
