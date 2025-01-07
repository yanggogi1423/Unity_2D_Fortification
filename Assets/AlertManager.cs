using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    public List<string> alertText = new List<string>();

    public GameObject alertWrapper;
    public TMP_Text alert;
    
    public void Awake()
    {
        SetAlertText();
        alertWrapper.SetActive(false);
    }

    private void SetAlertText()
    {
        alertText.Add("You've not enough gold");
        alertText.Add("The population is full");
    }

    public void ShowAlert(int i)
    {
        alertWrapper.SetActive(true);
        alert.SetText(GetAlertText(i));
        StartCoroutine(ShowAlertCoroutine());
    }

    private IEnumerator ShowAlertCoroutine()
    {
        alertWrapper.GetComponent<Animator>().SetBool("show", true);
        yield return new WaitForSeconds(3f);
        alertWrapper.GetComponent<Animator>().SetBool("show",false);
        yield return new WaitForSeconds(1.2f);
        alertWrapper.SetActive(false);
    }

    public string GetAlertText(int i)
    {
        return alertText[i];
    }
}
