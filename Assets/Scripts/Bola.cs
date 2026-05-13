using UnityEngine;

public class ControleBola : MonoBehaviour
{
    [Header("Posicao Inicial da Bola")]
    public Vector3 posicaoInicial = new Vector3(0f, 9.97f, 0f);

    [Header("Fisica Arcade")]
    public float impulsoAoBaterNoJogador = 5f;
    public float impulsoVerticalMinimo = 3f;
    public float velocidadeMaxima = 18f;
    public float multiplicadorQueda = 1.15f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetarPosicao();
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        if (rb.linearVelocity.y < 0f)
        {
            rb.AddForce(Vector2.down * (multiplicadorQueda - 1f), ForceMode2D.Force);
        }

        if (rb.linearVelocity.magnitude > velocidadeMaxima)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * velocidadeMaxima;
        }
    }

    public void ResetarPosicao()
    {
        transform.position = posicaoInicial;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb == null) return;

        ControleJogador jogador = collision.collider.GetComponentInParent<ControleJogador>();
        if (jogador == null) return;

        Vector2 direcao = ((Vector2)transform.position - (Vector2)jogador.transform.position).normalized;
        direcao.y = Mathf.Max(direcao.y, impulsoVerticalMinimo / impulsoAoBaterNoJogador);
        direcao.Normalize();

        rb.AddForce(direcao * impulsoAoBaterNoJogador, ForceMode2D.Impulse);
    }
}
