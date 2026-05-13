using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarregamentoCena : MonoBehaviour
{
    [Header("Cena de Destino")]
    public string cenaDestino = "Estadio_Padrao";

    [Header("Tempo Minimo da Tela")]
    public float tempoMinimo = 2f;

    void Start()
    {
        StartCoroutine(CarregarCena());
    }

    private IEnumerator CarregarCena()
    {
        yield return new WaitForSecondsRealtime(tempoMinimo);
        SceneManager.LoadScene(cenaDestino);
    }
}
