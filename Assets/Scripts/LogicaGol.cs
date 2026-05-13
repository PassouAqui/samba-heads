using UnityEngine;
using TMPro;

public class LogicaGol : MonoBehaviour
{
    [Header("Textos do Placar (Arraste do Inspector)")]
    public TextMeshProUGUI dezena;
    public TextMeshProUGUI unidade;

    [Header("Qual jogador pontua aqui? (Digite 1 ou 2)")]
    public int jogadorQuePontua;

    private int pontos = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Debug.Log("Gol validado para o Player " + jogadorQuePontua); 

            // Atualiza a matemática visual da trave
            pontos++;
            if (dezena != null) dezena.text = (pontos / 10).ToString();
            if (unidade != null) unidade.text = (pontos % 10).ToString();
            
            // CHAMA O JUIZ DIRETAMENTE (Garante que a animação, o tempo e a física parem)
            if (Cronometro.instancia != null)
            {
                Cronometro.instancia.AdicionarGol(jogadorQuePontua);
            }
            else
            {
                Debug.LogError("ERRO: O Cronômetro não foi encontrado! A instância está nula.");
            }
        }
    }
}