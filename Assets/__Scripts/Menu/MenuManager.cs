using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] Screens;

    #region Singleton
    public static MenuManager Instance { get; private set; }
    private void Awake() => Instance = this;
    #endregion

    public void TransitionToScreenByName(string screenName)
    {
        if (string.IsNullOrEmpty(screenName))
        {
            Debug.LogError("MenuNetwork :: Ao transitar, o nome da cena <b>NÃO</b> pode ser nula ou vazia!");
            return;
        }

        foreach (var screen in Screens)
        {
            screen.SetActive(screen.name == screenName);
        }
    }
}
