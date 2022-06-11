using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
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

    private void Awake()
    {
        CloseAlert();
    }

    public void ShowAlert(AlertType type, string message, System.Action confirmAction = null, System.Action cancelAction = null, System.Action closeAction = null)
    {
        // set message
        messageText.text = message;
        
        // set button
        switch (type)
        {
            case AlertType.INFO:
                closeButton.onClick.AddListener(() =>
                {
                    closeAction?.Invoke();
                    CloseAlert();
                });
                closeButton.gameObject.SetActive(true);
                break;
            case AlertType.CONFIRMATION:
                confirmButton.onClick.AddListener(() =>
                {
                    confirmAction?.Invoke();
                    CloseAlert();
                });
                cancelButton.onClick.AddListener(() =>
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
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();

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