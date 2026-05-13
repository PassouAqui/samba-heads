using UnityEngine;
using System.Collections; 

public class AnimacaoGol : MonoBehaviour
{
    public static AnimacaoGol instancia;
    
    [Header("Elementos do Gol")]
    public GameObject painelGol; 
    
    [Header("Arraste o seu .MP3 aqui embaixo!")]
    public AudioClip musicaDoGol; // Aqui entra o arquivo de áudio

    private AudioSource tocadorInvisivel;

    void Awake()
    {
        if (instancia == null) instancia = this;

        // O script cria o AudioSource sozinho para você não ter dor de cabeça
        tocadorInvisivel = gameObject.AddComponent<AudioSource>();
        tocadorInvisivel.playOnAwake = false; // Garante que não vai tocar do nada ao abrir o jogo
    }

    public void ExibirGol()
    {
        // 1. Liga a imagem do GOL na tela
        if (painelGol != null)
        {
            painelGol.SetActive(true);
        }
        
        // 2. Toca o seu MP3
        if (musicaDoGol != null && tocadorInvisivel != null)
        {
            tocadorInvisivel.PlayOneShot(musicaDoGol);
        }

        // 3. Aciona o cronômetro invisível de 5 segundos
        StartCoroutine(EsconderGolTempoReal());
    }

    private IEnumerator EsconderGolTempoReal()
    {
        // Espera 5 segundos cravados (ignorando o tempo congelado da Unity)
        yield return new WaitForSecondsRealtime(5f); 

        // Desliga a imagem do GOL
        if (painelGol != null)
        {
            painelGol.SetActive(false);
        }
    }
}