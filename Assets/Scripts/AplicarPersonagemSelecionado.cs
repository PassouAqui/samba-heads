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
        // Mantido apenas para nao quebrar componentes antigos na cena.
        // A selecao atual usa AtivarPersonagensSelecionados.
    }
}
