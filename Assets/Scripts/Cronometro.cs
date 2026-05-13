using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cronometro : MonoBehaviour
{
    public static Cronometro instancia;

    [Header("Textos do Cronômetro (UI)")]
    public TextMeshProUGUI minDezena;
    public TextMeshProUGUI minUnidade;
    public TextMeshProUGUI segDezena;
    public TextMeshProUGUI segUnidade;
    public TextMeshProUGUI textoPeriodo;

    [Header("Configurações da Partida")]
    public float tempoLimite = 180f; 
    private float tempoDecorrido = 0f;
    private int periodoAtual = 1;
    private int golsP1 = 0;
    private int golsP2 = 0;
    private bool jogoFinalizado = false;
    private bool golEmAndamento = false; 

    [Header("Objetos para Resetar no Gol")]
    public Transform bola;
    public Transform player1;
    public Transform player2;

    [Header("Tela de Vitoria")]
    public GameObject painelVitoria;
    public TextMeshProUGUI textoVitoria;
    public float tempoTelaVitoria = 4f;

    [Header("Tela de Empate")]
    public GameObject painelEmpate;
    public GameObject botoesEmpate;
    public TextMeshProUGUI textoEmpate;
    public TextMeshProUGUI textoContagemEmpate;
    public int tempoContagemGolDeOuro = 3;

    [Header("Nomes dos Jogadores")]
    public string nomePlayer1 = "PLAYER 1";
    public string nomePlayer2 = "PLAYER 2";
    public TextMeshProUGUI textoNomePlayer1Placar;
    public TextMeshProUGUI textoNomePlayer2Placar;
    
    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    void Start()
    {
        Time.timeScale = 1f; 

        AplicarNomesSelecionados();

        if (painelVitoria != null) painelVitoria.SetActive(false);
        if (painelEmpate != null) painelEmpate.SetActive(false);
        if (botoesEmpate != null) botoesEmpate.SetActive(false);

        AtualizarPeriodoVisual();
    }

    private void AplicarNomesSelecionados()
    {
        if (!string.IsNullOrWhiteSpace(SelecaoPersonagens.NomeP1))
        {
            nomePlayer1 = SelecaoPersonagens.NomeP1;
        }

        if (!string.IsNullOrWhiteSpace(SelecaoPersonagens.NomeP2))
        {
            nomePlayer2 = SelecaoPersonagens.NomeP2;
        }

        if (textoNomePlayer1Placar != null) textoNomePlayer1Placar.text = nomePlayer1;
        if (textoNomePlayer2Placar != null) textoNomePlayer2Placar.text = nomePlayer2;
    }

    public void AdicionarGol(int numeroDoPlayer)
    {
        if (jogoFinalizado || golEmAndamento) return;

        golEmAndamento = true;

        if (numeroDoPlayer == 1) golsP1++;
        else if (numeroDoPlayer == 2) golsP2++;

        Debug.Log($"Gol do Player {numeroDoPlayer}! Placar: {golsP1}x{golsP2}");

        StartCoroutine(SequenciaPosGol());
    }

    private IEnumerator SequenciaPosGol()
    {
        // 1. Congela o jogo e a física
        Time.timeScale = 0f;
        TravarFisica(true);

        // 2. Dispara o painel de GOL (que deve sumir em 5s no script AnimacaoGol)
        if (AnimacaoGol.instancia != null) 
        {
            AnimacaoGol.instancia.ExibirGol();
        }

        // 3. ESPERA 6 SEGUNDOS: 
        // 5 segundos da animação + 1 segundo de respiro para o reset
        yield return new WaitForSecondsRealtime(6f); 

        // 4. Reseta os personagens e a bola (via ControleBola)
        if (periodoAtual == 2)
        {
            FinalizarPartida();
            yield break; 
        }

        ResetarCampo();

        // 5. Devolve o tempo ao normal e libera o jogo
        golEmAndamento = false; 
        Time.timeScale = 1f;
    }

    private void ResetarCampo()
    {
        // Manda a bola para a posição inicial (0, 9.97, 0) definida no ControleBola
        if (bola != null)
        {
            ControleBola scriptBola = bola.GetComponent<ControleBola>();
            if (scriptBola != null)
            {
                scriptBola.ResetarPosicao();
            }
        }

        if (player1 != null)
        {
            ControlePlayer1 scriptPlayer1 = player1.GetComponent<ControlePlayer1>();
            if (scriptPlayer1 != null)
            {
                scriptPlayer1.ResetarPosicao();
            }
        }

        if (player2 != null)
        {
            ControlePlayer2 scriptPlayer2 = player2.GetComponent<ControlePlayer2>();
            if (scriptPlayer2 != null)
            {
                scriptPlayer2.ResetarPosicao();
            }
        }

        // Religamos a simulação física
        TravarFisica(false);
    }

    private void TravarFisica(bool travar)
    {
        bool ativarFisica = !travar;

        if (bola != null)
        {
            Rigidbody2D rb = bola.GetComponent<Rigidbody2D>();
            if (rb != null) { rb.linearVelocity = Vector2.zero; rb.simulated = ativarFisica; }
        }
        if (player1 != null)
        {
            Rigidbody2D rb = player1.GetComponent<Rigidbody2D>();
            if (rb != null) { rb.linearVelocity = Vector2.zero; rb.simulated = ativarFisica; }
        }
        if (player2 != null)
        {
            Rigidbody2D rb = player2.GetComponent<Rigidbody2D>();
            if (rb != null) { rb.linearVelocity = Vector2.zero; rb.simulated = ativarFisica; }
        }
    }

    void Update()
    {
        if (jogoFinalizado || golEmAndamento) return;

        tempoDecorrido += Time.deltaTime;

        if (tempoDecorrido >= tempoLimite)
        {
            tempoDecorrido = tempoLimite;
            VerificarFimDePeriodo();
        }

        AtualizarCronometroVisual();
    }

    void AtualizarCronometroVisual()
    {
        int minutos = Mathf.FloorToInt(tempoDecorrido / 60);
        int segundos = Mathf.FloorToInt(tempoDecorrido % 60);

        if(minDezena != null) minDezena.text = (minutos / 10).ToString();
        if(minUnidade != null) minUnidade.text = (minutos % 10).ToString();
        if(segDezena != null) segDezena.text = (segundos / 10).ToString();
        if(segUnidade != null) segUnidade.text = (segundos % 10).ToString();
    }

    void VerificarFimDePeriodo()
    {
        StartCoroutine(SequenciaFimPeriodo());
    }

    void IniciarPeriodo2()
    {
        periodoAtual = 2;
        tempoDecorrido = 0f;
        AtualizarPeriodoVisual();
        ResetarCampo();
    }

    void AtualizarPeriodoVisual()
    {
        if (textoPeriodo != null) textoPeriodo.text = periodoAtual.ToString();
    }

    void FinalizarPartida()
    {
        jogoFinalizado = true;
        Time.timeScale = 0f;
        TravarFisica(true);

        if (textoVitoria != null)
        {
            if (golsP1 > golsP2) textoVitoria.text = nomePlayer1 + " VENCEU";
            else if (golsP2 > golsP1) textoVitoria.text = nomePlayer2 + " VENCEU";
        }

        if (golsP1 == golsP2)
        {
            if (textoEmpate != null) textoEmpate.text = "EMPATE\nPARTIDA ENCERRADA";
            if (textoContagemEmpate != null) textoContagemEmpate.text = "";
            if (painelEmpate != null) painelEmpate.SetActive(true);
            if (botoesEmpate != null) botoesEmpate.SetActive(true);
        }
        else if (painelVitoria != null)
        {
            painelVitoria.SetActive(true);
        }

        Debug.Log("Fim de Jogo!");
    }

    private IEnumerator SequenciaFimPeriodo()
    {
        jogoFinalizado = true;
        Time.timeScale = 0f;
        TravarFisica(true);

        if (periodoAtual == 1 && golsP1 == golsP2)
        {
            if (textoEmpate != null) textoEmpate.text = "EMPATE";
            if (painelEmpate != null) painelEmpate.SetActive(true);
            if (botoesEmpate != null) botoesEmpate.SetActive(false);

            for (int segundos = tempoContagemGolDeOuro; segundos > 0; segundos--)
            {
                string mensagemContagem = "MOMENTO GOL DE OURO EM " + segundos;

                if (textoContagemEmpate != null) textoContagemEmpate.text = mensagemContagem;
                else if (textoEmpate != null) textoEmpate.text = "EMPATE\n" + mensagemContagem;

                yield return new WaitForSecondsRealtime(1f);
            }

            if (painelEmpate != null) painelEmpate.SetActive(false);
            if (textoContagemEmpate != null) textoContagemEmpate.text = "";
            jogoFinalizado = false;
            IniciarPeriodo2();
            Time.timeScale = 1f;
            yield break;
        }

        if (periodoAtual == 2 && golsP1 == golsP2)
        {
            FinalizarPartida();
            yield break;
        }

        if (textoVitoria != null)
        {
            if (golsP1 > golsP2) textoVitoria.text = nomePlayer1 + " VENCEU";
            else if (golsP2 > golsP1) textoVitoria.text = nomePlayer2 + " VENCEU";
        }

        if (painelVitoria != null)
        {
            painelVitoria.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(tempoTelaVitoria);

        FinalizarPartida();
    }

    public void NovaRevanche()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NovoJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}
