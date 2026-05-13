using UnityEngine;

public class ControlePlayer1 : MonoBehaviour
{
    [Header("Posicao Inicial do Player 1")]
    public Vector3 posicaoInicial = new Vector3(1.00928f, 1.23091f, 0f);

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetarPosicao();
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
}
