using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SeletorPersonagens : MonoBehaviour
{
    public RectTransform bordaP1, bordaP2;
    public GameObject avisoEnter;
    public GameObject[] todosBotoes;

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
            if (EnterPressionado(teclado)) SceneManager.LoadScene("Charge");
        }

        AtualizarPosicao();
    }

    void MoverHorizontalP1(int dir)
    {
        indexP1 = (indexP1 + dir + todosBotoes.Length) % todosBotoes.Length;
    }

    void MoverHorizontalP2(int dir)
    {
        indexP2 = (indexP2 + dir + todosBotoes.Length) % todosBotoes.Length;
    }

    int BuscarMaisProximo(int atual, Vector2 direcao)
    {
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
        if (todosBotoes.Length > 0)
        {
            bordaP1.position = todosBotoes[indexP1].transform.position;
            bordaP2.position = todosBotoes[indexP2].transform.position;
        }
    }

    bool EnterPressionado(Keyboard teclado)
    {
        return teclado.enterKey.wasPressedThisFrame || teclado.numpadEnterKey.wasPressedThisFrame;
    }
}
