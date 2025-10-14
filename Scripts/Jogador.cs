using UnityEngine;
using Unity.Cinemachine;

public class Jogador : MonoBehaviour
{
    public float forceMultiplier = 5f; //força aplicada no movimento do jogador
    public float velocidadeMaxima = 6f; //velocidade maxima de movimento
    public GameObject followCam; //atributo de camera que segue
    public GameObject zoomCam; //atributo de camera do zoom
    public GameObject efeitoCinza; //efeito de acinzentar quando morre
    public GameObject gameOverMenu;

    public ParticleSystem particulasMorte;
    private Rigidbody rb; //caching para não ficar dando getcomponent no update
    private CinemachineImpulseSource impulseSource; //cria o objeto que vai gerar o ruido para a camera

    /*public float tempoParaPenalidade = 3f;
    public float penalidadeAbaixaPlacar = 10f;
    public float penalidadeDesacelera = 5f;

    private float tempoInput = 0f;
    private bool jogadorInativo = false;*/

    private float ultimaPosicaoX;
    private float tempoParado = 0f;
    public float tempoLimiteParado = 4f;
    public float limiteMovimento = 0.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        ultimaPosicaoX = transform.position.x;
    }

    private void geraMorte()
    {
        GameManager.Instance.GameOver(); //para contagem de pontos
        gameObject.SetActive(false); //desabilita o jogador
        gameOverMenu.SetActive(true); //habilita o menu game over

        Instantiate(particulasMorte, transform.position, Quaternion.identity); //gera particulas de morte
        impulseSource.GenerateImpulse(); //gera impulso para tremer a camera

        followCam.SetActive(false); //desabilitando camera que treme
        zoomCam.SetActive(true); //habilitando camera do zoom
        efeitoCinza.SetActive(true); //habilita efeito cinza
    }

    void Update()
    {
        // quando o jogador cair da plataforma chamamos a nova função de morte
        if(transform.position.y < -2)
        {
            geraMorte();
        }

        float posicaoAtualX = transform.position.x;
        float distanciaMovida = Mathf.Abs(posicaoAtualX - ultimaPosicaoX);

        if (distanciaMovida >= limiteMovimento)
        {
            tempoParado = 0f;
            ultimaPosicaoX = posicaoAtualX;
        }
        else
        {
            if (!GameManager.Instance.textoContador.gameObject.activeSelf) 
            {
                tempoParado += Time.deltaTime;
            }
            tempoParado += Time.deltaTime;
            if (tempoParado >= tempoLimiteParado)
            {
                GameManager.Instance.SpawnPerigoGuiado(posicaoAtualX);
                tempoParado = 0f;
            }
        }

        if(GameManager.Instance == null)
        {
            return; 
        }

        var horizontalInput = Input.GetAxis("Horizontal"); //pegando input do teclado na horizontal

        if(rb.linearVelocity.magnitude <= velocidadeMaxima)
        {
            rb.AddForce(new Vector3(horizontalInput * forceMultiplier, 0, 0)); //movendo o jogador
        }

        /*if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            tempoInput = 0f;
            jogadorInativo = false;
        }
        else
        {
            tempoInput += Time.deltaTime;

            if (tempoInput >= tempoParaPenalidade && !jogadorInativo)
            {
                AtivaPenalidade();
                jogadorInativo = true;
            }
        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Perigo"))
        {
            geraMorte();
        }
    }

    private void OnEnable()
    {
        transform.position = new Vector3(0, 1.91f, 0);
        transform.rotation = Quaternion.identity;

        efeitoCinza.SetActive(false);
        followCam.SetActive(true);
        zoomCam.SetActive(false);
    }

    /*public void AtivaPenalidade()
    {
        if (rb != null)
        {
            
        }
    }*/
}
