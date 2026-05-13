using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControleJogador : MonoBehaviour
{
    [Header("Configuracoes de Movimento")]
    public float velocidade = 5f;
    public float forcaPulo = 7f;

    [Header("Controles do Jogador")]
    public KeyCode teclaEsquerda;
    public KeyCode teclaDireita;
    public KeyCode teclaPulo;
    public KeyCode teclaChute;
    public KeyCode teclaSuperChute;

    [Header("Fisica e Chao")]
    public Transform peDoJogador;
    public float raioChao = 0.2f;
    public LayerMask camadaChao;
    private bool noChao;

    [Header("Animacao")]
    public Animator animator;

    [Header("Chute e Cabeceio")]
    public Transform pontoChute;
    public Rigidbody2D bola;
    public float alcanceChute = 1.2f;
    public float forcaChute = 12f;
    public float forcaCabecada = 9f;
    public Vector2 direcaoCabecada = new Vector2(1f, 0.4f);

    [Header("Superchute")]
    public float tempoRecargaSuperChute = 21f;
    public float multiplicadorSuperChute = 2.5f;
    public Image barraSuperChute;
    public Color corDisponivel = Color.yellow;
    public Color corRecargando = new Color(0.3f, 0.3f, 0.3f);
    public TextMeshProUGUI textoSuperChute;

    private Rigidbody2D rb;
    private float movimentoX;
    private float recargaAtualSuperChute;
    private bool superChuteDisponivel = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        recargaAtualSuperChute = tempoRecargaSuperChute;
        AtualizarBarraSuperChute();
    }

    void Update()
    {
        AtualizarChao();
        LerMovimento();
        LerPulo();
        LerAcoes();
        AtualizarRecargaSuperChute();
        AtualizarAnimacoes();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimentoX * velocidade, rb.linearVelocity.y);
    }

    private void AtualizarChao()
    {
        if (peDoJogador != null)
        {
            noChao = Physics2D.OverlapCircle(peDoJogador.position, raioChao, camadaChao);
        }
    }

    private void LerMovimento()
    {
        movimentoX = 0f;
        if (Input.GetKey(teclaEsquerda)) movimentoX = -1f;
        if (Input.GetKey(teclaDireita)) movimentoX = 1f;
    }

    private void LerPulo()
    {
        if (Input.GetKeyDown(teclaPulo) && noChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
    }

    private void LerAcoes()
    {
        if (Input.GetKeyDown(teclaChute))
        {
            if (noChao) Chutar(false);
            else Cabecear(false);
        }

        if (Input.GetKeyDown(teclaSuperChute))
        {
            TentarSuperChute();
        }
    }

    private void TentarSuperChute()
    {
        if (!superChuteDisponivel) return;

        if (noChao) Chutar(true);
        else Cabecear(true);

        superChuteDisponivel = false;
        recargaAtualSuperChute = 0f;
        AtualizarBarraSuperChute();
    }

    private void AtualizarRecargaSuperChute()
    {
        if (superChuteDisponivel) return;

        recargaAtualSuperChute += Time.deltaTime;

        if (recargaAtualSuperChute >= tempoRecargaSuperChute)
        {
            recargaAtualSuperChute = tempoRecargaSuperChute;
            superChuteDisponivel = true;
        }

        AtualizarBarraSuperChute();
    }

    private void AtualizarBarraSuperChute()
    {
        if (barraSuperChute != null)
        {
            barraSuperChute.fillAmount = recargaAtualSuperChute / tempoRecargaSuperChute;
            barraSuperChute.color = superChuteDisponivel ? corDisponivel : corRecargando;
        }

        if (textoSuperChute != null)
        {
            if (superChuteDisponivel)
            {
                textoSuperChute.text = "PRONTO!";
            }
            else
            {
                textoSuperChute.text =
                    $"{Mathf.RoundToInt(recargaAtualSuperChute / tempoRecargaSuperChute * 100f)}%";
            }
        }
    }

    private void Chutar(bool super)
    {
        if (animator != null)
        {
            animator.SetTrigger("Chutando");
        }

        float forca = super ? forcaChute * multiplicadorSuperChute : forcaChute;
        float alturaAleatoria = Random.Range(0.1f, 0.8f);
        float horizontal = Mathf.Approximately(direcaoCabecada.x, 0f) ? 1f : Mathf.Sign(direcaoCabecada.x);
        AplicarForcaNaBola(forca, new Vector2(horizontal, alturaAleatoria));
    }

    private void Cabecear(bool super)
    {
        if (animator != null)
        {
            animator.SetTrigger("Cabeceando");
        }

        float forca = super ? forcaCabecada * multiplicadorSuperChute : forcaCabecada;
        AplicarForcaNaBola(forca, direcaoCabecada);
    }

    private void AplicarForcaNaBola(float forca, Vector2 direcaoBase)
    {
        if (bola == null) return;

        Vector2 origem = pontoChute != null ? pontoChute.position : transform.position;
        float distancia = Vector2.Distance(origem, bola.position);
        if (distancia > alcanceChute) return;

        float direcaoX = transform.localScale.x >= 0f ? 1f : -1f;
        Vector2 direcao = new Vector2(direcaoBase.x * direcaoX, direcaoBase.y).normalized;

        bola.linearVelocity = Vector2.zero;
        bola.AddForce(direcao * forca, ForceMode2D.Impulse);
    }

    private void AtualizarAnimacoes()
    {
        if (animator != null)
        {
            animator.SetBool("Andando", movimentoX != 0);
            animator.SetBool("NoChao", noChao);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (peDoJogador != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(peDoJogador.position, raioChao);
        }

        Transform origem = pontoChute != null ? pontoChute : transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origem.position, alcanceChute);
    }
}
