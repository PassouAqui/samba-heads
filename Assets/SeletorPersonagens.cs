using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SeletorPersonagens : MonoBehaviour
{
    public RectTransform bordaP1, bordaP2;
    public GameObject avisoEnter;
    public GameObject[] todosBotoes;

    [Header("Sprites por slot (mesma ordem de todosBotoes)")]
    public Sprite[] spritesP1Esq;
    public Sprite[] spritesP2Dir;

    private int indexP1 = 0, indexP2 = 1;
    private bool p1Confirmou = false, p2Confirmou = false;

    void Start()
    {
        if (avisoEnter) avisoEnter.SetActive(false);
        bordaP2.gameObject.SetActive(false);
        AtualizarPosicao();
    }

    void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        if (!p1Confirmou)
        {
            if (teclado.dKey.wasPressedThisFrame) MoverHorizontalP1(1);
            if (teclado.aKey.wasPressedThisFrame) MoverHorizontalP1(-1);
            if (teclado.sKey.wasPressedThisFrame) indexP1 = BuscarMaisProximo(indexP1, Vector2.down);
            if (teclado.wKey.wasPressedThisFrame) indexP1 = BuscarMaisProximo(indexP1, Vector2.up);

            if (EnterPressionado(teclado))
            {
                p1Confirmou = true;
                bordaP2.gameObject.SetActive(true);
            }
        }
        else if (!p2Confirmou)
        {
            if (teclado.escapeKey.wasPressedThisFrame)
            {
                p1Confirmou = false;
                bordaP2.gameObject.SetActive(false);
                return;
            }

            if (teclado.rightArrowKey.wasPressedThisFrame) MoverHorizontalP2(1);
            if (teclado.leftArrowKey.wasPressedThisFrame) MoverHorizontalP2(-1);
            if (teclado.downArrowKey.wasPressedThisFrame) indexP2 = BuscarMaisProximo(indexP2, Vector2.down);
            if (teclado.upArrowKey.wasPressedThisFrame) indexP2 = BuscarMaisProximo(indexP2, Vector2.up);

            if (EnterPressionado(teclado)) p2Confirmou = true;
        }

        if (p1Confirmou && p2Confirmou)
        {
            if (avisoEnter) avisoEnter.SetActive(true);
            if (EnterPressionado(teclado))
            {
                SalvarEscolhaParaPartida();
                SceneManager.LoadScene("Charge");
            }
        }

        AtualizarPosicao();
    }

    void MoverHorizontalP1(int dir)
    {
        if (todosBotoes == null || todosBotoes.Length == 0) return;
        indexP1 = (indexP1 + dir + todosBotoes.Length) % todosBotoes.Length;
    }

    void MoverHorizontalP2(int dir)
    {
        if (todosBotoes == null || todosBotoes.Length == 0) return;
        indexP2 = (indexP2 + dir + todosBotoes.Length) % todosBotoes.Length;
    }

    int BuscarMaisProximo(int atual, Vector2 direcao)
    {
        if (todosBotoes == null || todosBotoes.Length == 0) return atual;
        int melhorEscolha = atual;
        float menorDistancia = float.MaxValue;
        Vector3 posAtual = todosBotoes[atual].transform.position;

        for (int i = 0; i < todosBotoes.Length; i++)
        {
            if (i == atual) continue;

            Vector3 direcaoParaOAlvo = (todosBotoes[i].transform.position - posAtual).normalized;
            float alinhamento = Vector3.Dot(direcaoParaOAlvo, direcao);

            if (alinhamento > 0.7f)
            {
                float dist = Vector3.Distance(posAtual, todosBotoes[i].transform.position);
                if (dist < menorDistancia)
                {
                    menorDistancia = dist;
                    melhorEscolha = i;
                }
            }
        }
        return melhorEscolha;
    }

    void AtualizarPosicao()
    {
        if (todosBotoes == null || todosBotoes.Length == 0) return;
        if (todosBotoes[indexP1] != null && bordaP1 != null)
        {
            bordaP1.position = todosBotoes[indexP1].transform.position;
        }

        if (todosBotoes[indexP2] != null && bordaP2 != null)
        {
            bordaP2.position = todosBotoes[indexP2].transform.position;
        }
    }

    private void SalvarEscolhaParaPartida()
    {
        if (spritesP1Esq != null && indexP1 >= 0 && indexP1 < spritesP1Esq.Length)
        {
            SelecaoPersonagens.SpriteP1 = spritesP1Esq[indexP1];
        }

        if (spritesP2Dir != null && indexP2 >= 0 && indexP2 < spritesP2Dir.Length)
        {
            SelecaoPersonagens.SpriteP2 = spritesP2Dir[indexP2];
        }
    }

    bool EnterPressionado(Keyboard teclado)
    {
        return teclado.enterKey.wasPressedThisFrame || teclado.numpadEnterKey.wasPressedThisFrame;
    }
}
