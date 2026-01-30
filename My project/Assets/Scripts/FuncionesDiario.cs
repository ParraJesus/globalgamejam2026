using UnityEngine;
using TMPro;

public class DiarioUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject diarioUI;
    public TMP_InputField paginaIzquierda;
    public TMP_InputField paginaDerecha;

    void Start()
    {
        diarioUI.SetActive(false);
        CargarTexto();
    }

    public void AbrirDiario()
    {
        diarioUI.SetActive(true);
        CargarTexto();
    }

    public void CerrarDiario()
    {
        GuardarTexto();
        diarioUI.SetActive(false);
    }

    void GuardarTexto()
    {
        PlayerPrefs.SetString("Diario_Izquierda", paginaIzquierda.text);
        PlayerPrefs.SetString("Diario_Derecha", paginaDerecha.text);
        PlayerPrefs.Save();
    }

    void CargarTexto()
    {
        paginaIzquierda.text = PlayerPrefs.GetString("Diario_Izquierda", "");
        paginaDerecha.text = PlayerPrefs.GetString("Diario_Derecha", "");
    }
}
