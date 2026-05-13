using UnityEngine;
using UnityEngine.U2D.Animation;

public enum JogadorAlvo
{
    Player1 = 0,
    Player2 = 1
}

/// <summary>
/// No estádio, aplica o sprite escolhido no menu (mesma hierarquia de bones nos PSDs).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class AplicarPersonagemSelecionado : MonoBehaviour
{
    public JogadorAlvo jogador;

    private void Start()
    {
        Sprite escolhido = jogador == JogadorAlvo.Player1
            ? SelecaoPersonagens.SpriteP1
            : SelecaoPersonagens.SpriteP2;

        if (escolhido == null)
        {
            return;
        }

        var sr = GetComponent<SpriteRenderer>();
        var skin = GetComponent<SpriteSkin>();
        if (skin != null)
        {
            skin.autoRebind = true;
        }

        sr.sprite = escolhido;
    }
}
