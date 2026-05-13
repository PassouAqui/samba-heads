using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[System.Serializable]
public class DadosEstadio
{
    public GameObject objetoNoMenu;
    public string nomeDaCena;
}

public class SeletorEstadios : MonoBehaviour
{
    public RectTransform bordaSelecao;
    public DadosEstadio[] listaEstadios;
    public string cenaAnterior = "SampleScene";
    private int index = 0;

    void Start()
    {
        ConfigurarEventosMouse();
        AtualizarPosicao();
    }

    void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        if (teclado.escapeKey.wasPressedThisFrame) SceneManager.LoadScene(cenaAnterior);
        if (teclado.dKey.wasPressedThisFrame || teclado.rightArrowKey.wasPressedThisFrame) Mover(1);
        if (teclado.aKey.wasPressedThisFrame || teclado.leftArrowKey.wasPressedThisFrame) Mover(-1);
        if (teclado.enterKey.wasPressedThisFrame) ConfirmarSelecao();
    }

    public void Mover(int dir)
    {
        if (listaEstadios.Length == 0) return;
        index = (index + dir + listaEstadios.Length) % listaEstadios.Length;
        AtualizarPosicao();
    }

    public void ConfirmarSelecao()
    {
        if (listaEstadios.Length > 0) SceneManager.LoadScene(listaEstadios[index].nomeDaCena);
    }

    void AtualizarPosicao()
    {
        if (listaEstadios.Length > 0 && bordaSelecao != null)
        {
            bordaSelecao.position = listaEstadios[index].objetoNoMenu.transform.position;
        }
    }

    void ConfigurarEventosMouse()
    {
        for (int i = 0; i < listaEstadios.Length; i++)
        {
            int tempIndex = i;
            EventTrigger trigger = listaEstadios[i].objetoNoMenu.GetComponent<EventTrigger>();
            if (trigger == null) trigger = listaEstadios[i].objetoNoMenu.AddComponent<EventTrigger>();

            EventTrigger.Entry entryHover = new EventTrigger.Entry();
            entryHover.eventID = EventTriggerType.PointerEnter;
            entryHover.callback.AddListener((data) => { index = tempIndex; AtualizarPosicao(); });
            trigger.triggers.Add(entryHover);

            EventTrigger.Entry entryClick = new EventTrigger.Entry();
            entryClick.eventID = EventTriggerType.PointerClick;
            entryClick.callback.AddListener((data) => { ConfirmarSelecao(); });
            trigger.triggers.Add(entryClick);
        }
    }
}