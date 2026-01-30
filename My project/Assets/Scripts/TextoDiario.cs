using UnityEngine;
using TMPro;

public class Diario : MonoBehaviour
{
    public TextMeshProUGUI textoDiario;

    public void CambiarTexto(string nuevoTexto)
    {
        textoDiario.text = nuevoTexto;
    }
}
