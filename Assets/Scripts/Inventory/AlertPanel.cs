using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum AlertType
{
    INFO,
    CONFIRMATION
}

public class AlertPanel : MonoBehaviour
{
    public GameObject alertPanel;

    [Header("Components")]
    public TMP_Text messageText;
    public Button confirmButton;
    public Button cancelButton;
    public Button closeButton;

    private UnityEvent OnConfirmButton = new UnityEvent();
    private UnityEvent OnCancelButton = new UnityEvent();
    private UnityEvent OnCloseButton = new UnityEvent();

    private void Awake()
    {
        CloseAlert();
    }

    private void Start()
    {
        confirmButton.onClick.AddListener(() => OnConfirmButton?.Invoke());
        cancelButton.onClick.AddListener(() => OnCancelButton?.Invoke());
        closeButton.onClick.AddListener(() => OnCloseButton?.Invoke());
    }

    public void ShowAlert(AlertType type, string message, Action confirmAction = null, Action cancelAction = null, Action closeAction = null)
    {
        // set message
        messageText.text = message;
        
        // set button
        switch (type)
        {
            case AlertType.INFO:
                OnCloseButton.AddListener(() =>
                {
                    closeAction?.Invoke();
                    CloseAlert();
                });
                closeButton.gameObject.SetActive(true);
                break;
            case AlertType.CONFIRMATION:
                OnConfirmButton.AddListener(() =>
                {
                    confirmAction?.Invoke();
                    CloseAlert();
                });
                OnCancelButton.AddListener(() =>
                {
                    cancelAction?.Invoke();
                    CloseAlert();
                });
                confirmButton.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(true);
                break;
        }
        
        messageText.text = message;
        alertPanel.SetActive(true);
    }

    private void CloseAlert()
    {
        // reset button
        confirmButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        OnConfirmButton.RemoveAllListeners();
        OnCancelButton.RemoveAllListeners();
        OnCloseButton.RemoveAllListeners();

        // hide alert panel
        alertPanel.SetActive(false);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AlertPanel))]
public class AlertPanelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Show Info Alert"))
        {
            AlertPanel alertPanel = (AlertPanel)target;
            alertPanel.ShowAlert(AlertType.INFO, "This is an information message.");
        }

        if (GUILayout.Button("Show Confirmation"))
        {
            AlertPanel alertPanel = (AlertPanel)target;
            alertPanel.ShowAlert(AlertType.CONFIRMATION, "This is a confirmation message.", () => { Debug.Log("Confirm"); }, () => { Debug.Log("Cancel"); });
        }
    }
}

#endif