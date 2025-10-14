using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject perigoPrefab;
    public TMPro.TextMeshProUGUI textoPlacar;
    public TMPro.TextMeshProUGUI textoPause;
    public TMPro.TextMeshProUGUI textoContador;
    public Image backgroundMenu;
    public GameObject jogador;
    public RectTransform scoreConteiner;

    private int score;
    private float timer;
    private bool gameOver;
    private Coroutine rotinaPerigo;

    private static GameManager instance;
    public static GameManager Instance => instance;

    void Start()
    {
        instance = this;
    }

    public void SpawnPerigoGuiado(float posicaoX)
    {
        var perigoGuiado = Instantiate(perigoPrefab, new Vector3(posicaoX, -12, 12),
            Quaternion.identity);
        perigoGuiado.GetComponent<Rigidbody>().linearDamping = 0.5f;
    }

    private IEnumerator showContagem(int segundos)
    {
        textoContador.gameObject.SetActive(true);

        while (segundos > 0)
        {
            textoContador.text = segundos.ToString();
            yield return new WaitForSeconds(1f);
            segundos--;

            score = 0;
            timer = 0;
            textoPlacar.text = "0";
        }
        textoContador.text = "Vai!";
        yield return new WaitForSeconds(1f);
        textoContador.gameObject.SetActive(false);
        scoreConteiner.LeanMoveY(0f, 0.75f).setEaseOutBounce();
        rotinaPerigo = StartCoroutine(SpawnPerigo());
    }

    private void OnEnable()
    {
        jogador.SetActive(true);
        gameOver = false;
        StartCoroutine(showContagem(5));
    }

    public void GameOver()
    {
        StopCoroutine(rotinaPerigo);
        gameOver = true;
        gameObject.SetActive(false);
    }

    void Update()
    {
        //Quando ESC for pressionado
        if (Input.GetKeyUp(KeyCode.Escape)){
            if (Time.timeScale == 0) //pausado
            {
                StartCoroutine(ScaleTime(0, 1, 0.5f));
                backgroundMenu.gameObject.SetActive(false);
                textoPause.gameObject.SetActive(false);
            }
            if (Time.timeScale == 1)
            {
                StartCoroutine(ScaleTime(1, 0, 0.5f));
                backgroundMenu.gameObject.SetActive(true);
                textoPause.gameObject.SetActive(true);
            }
        }

        if (gameOver) {
            return;
        }

        timer += Time.deltaTime;

        if (timer > 1) {
            timer = 0f;
            score++;
            textoPlacar.text = score.ToString();
        }
    }

    private IEnumerator SpawnPerigo()
    {
        var perigosToSpawn = UnityEngine.Random.Range(1, 3);

        for (int i = 0; i < perigosToSpawn; i++)
        {
            var x = UnityEngine.Random.Range(-12f, 12f); //onde cai fica aleatório (eixo x)
            var damping = UnityEngine.Random.Range(0f, 2f); //velocidade de queda fica aleatória
            var perigo = Instantiate(perigoPrefab, new Vector3(x, 11, 0), Quaternion.identity);
            perigo.GetComponent<Rigidbody>().linearDamping = damping; //velocidade fica aleatória para cada um
        }

        yield return new WaitForSeconds(0.5f); //tempo para surgir
        yield return SpawnPerigo(); //chama recursivo para criar outros cubos
    }

    IEnumerator ScaleTime(float start, float end, float duration)
    {
        float lastTime = Time.realtimeSinceStartup;
        float timer = 0.0f;

        while (timer < duration)
        {
            //Time.timeScale é onde mexe na velocidade do jogo
            //Time.fixedDeltaTime é o tempo que a física afeta os G.O
            Time.timeScale = Mathf.Lerp(start, end, timer / duration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            timer += (Time.realtimeSinceStartup - lastTime);
            lastTime = Time.realtimeSinceStartup;
            yield return null;
        }
        Time.timeScale = end;
        Time.fixedDeltaTime = 0.02f * end;
    }

    public void Enabled()
    {
        gameObject.SetActive(true);
    }   
}