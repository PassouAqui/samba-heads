using UnityEngine;

public class ControlePausa : MonoBehaviour
{
    [Header("Tela de Pausa")]
    public GameObject telaPausa;

    [Header("Controles")]
    public KeyCode teclaPausa = KeyCode.Escape;
    public KeyCode teclaPausaAlternativa = KeyCode.RightControl;

    private bool jogoPausado = false;

    void Start()
    {
        if (telaPausa != null) telaPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(teclaPausa) || Input.GetKeyDown(teclaPausaAlternativa))
        {
            AlternarPausa();
        }
    }

    public void AlternarPausa()
    {
        if (jogoPausado) ContinuarJogo();
        else PausarJogo();
    }

    public void PausarJogo()
    {
        jogoPausado = true;
        Time.timeScale = 0f;

        if (telaPausa != null) telaPausa.SetActive(true);
    }

    public void ContinuarJogo()
    {
        jogoPausado = false;
        Time.timeScale = 1f;

        if (telaPausa != null) telaPausa.SetActive(false);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}
