using UnityEngine;

public class PlaylistSamba : MonoBehaviour
{
    public AudioSource meuRadio;
    public AudioClip[] listaDeMusicas;

    void Start()
    {
        
        DontDestroyOnLoad(this.gameObject);

        if (listaDeMusicas.Length > 0)
        {
            
            int indiceAleatorio = Random.Range(0, listaDeMusicas.Length);
            meuRadio.clip = listaDeMusicas[indiceAleatorio];
            meuRadio.Play();
        }
    }
}