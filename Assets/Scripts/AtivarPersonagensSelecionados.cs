using UnityEngine;

public class AtivarPersonagensSelecionados : MonoBehaviour
{
    [Header("Personagens do Player 1")]
    public GameObject[] personagensPlayer1;
    public ControleJogador controlePlayer1;

    [Header("Personagens do Player 2")]
    public GameObject[] personagensPlayer2;
    public ControleJogador controlePlayer2;

    void Start()
    {
        GarantirGrupoComoFilhoDoPlayer(personagensPlayer1, controlePlayer1);
        GarantirGrupoComoFilhoDoPlayer(personagensPlayer2, controlePlayer2);

        AtivarPersonagem(personagensPlayer1, SelecaoPersonagens.IndexP1, controlePlayer1);
        AtivarPersonagem(personagensPlayer2, SelecaoPersonagens.IndexP2, controlePlayer2);
    }

    private void GarantirGrupoComoFilhoDoPlayer(GameObject[] personagens, ControleJogador controleJogador)
    {
        if (personagens == null || personagens.Length == 0 || controleJogador == null) return;

        Transform grupoPersonagens = null;

        foreach (GameObject personagem in personagens)
        {
            if (personagem == null) continue;
            grupoPersonagens = personagem.transform.parent;
            break;
        }

        if (grupoPersonagens == null) return;
        if (grupoPersonagens.parent == controleJogador.transform) return;

        grupoPersonagens.SetParent(controleJogador.transform, true);
    }

    private void AtivarPersonagem(GameObject[] personagens, int indexEscolhido, ControleJogador controleJogador)
    {
        if (personagens == null || personagens.Length == 0) return;

        indexEscolhido = Mathf.Clamp(indexEscolhido, 0, personagens.Length - 1);

        for (int i = 0; i < personagens.Length; i++)
        {
            if (personagens[i] != null)
            {
                personagens[i].SetActive(i == indexEscolhido);
            }
        }

        GameObject personagemAtivo = personagens[indexEscolhido];
        if (personagemAtivo == null || controleJogador == null) return;

        Animator animator = personagemAtivo.GetComponent<Animator>();
        if (animator == null)
        {
            animator = personagemAtivo.GetComponentInChildren<Animator>();
        }

        controleJogador.animator = animator;
    }
}
