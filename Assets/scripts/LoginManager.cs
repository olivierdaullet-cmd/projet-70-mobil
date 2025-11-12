using UnityEngine;
using UnityEngine.UIElements;
using System.Text.RegularExpressions;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private TextField mailField;
    private TextField passField;
    private Button loginButton;
    private Label messageErreur;

    private bool ignoreValueChange;   // <- nouveau

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        mailField = root.Q<TextField>("SaisieMail");
        passField = root.Q<TextField>("SaisiePass");
        loginButton = root.Q<Button>("login");
        messageErreur = root.Q<Label>("MessageErreur");

        if (messageErreur != null)
            messageErreur.style.display = DisplayStyle.None;

        if (loginButton != null)
            loginButton.clicked += OnLoginClicked;

        if (mailField != null)
        {
            mailField.isDelayed = false;
            mailField.RegisterValueChangedCallback(evt =>
            {
                if (!ignoreValueChange)
                    CacherMessageErreur();
            });
        }

        if (passField != null)
        {
            passField.isDelayed = false;
            passField.RegisterValueChangedCallback(evt =>
            {
                if (!ignoreValueChange)
                    CacherMessageErreur();
            });
        }
    }

    private void OnLoginClicked()
    {
        var mail = mailField?.value?.Trim() ?? "";
        var pass = passField?.value?.Trim() ?? "";

        if (mail == "test@mail.com" && pass == "1234")
        {
            Debug.Log($"[DEBUG] OK : {mail} / {pass}");
            Application.Quit();
        }
        else
        {
            Debug.LogWarning("Erreur : identifiants invalides !");

            ignoreValueChange = true;
            mailField?.SetValueWithoutNotify("");   // ne déclenche pas les callbacks
            passField?.SetValueWithoutNotify("");
            ignoreValueChange = false;

            AfficherMessageErreur("Mail ou mot de passe incorrect.");
        }
    }

    private void AfficherMessageErreur(string msg)
    {
        if (messageErreur == null) return;
        messageErreur.text = msg;
        messageErreur.style.display = DisplayStyle.Flex;
    }

    private void CacherMessageErreur()
    {
        if (messageErreur == null) return;
        messageErreur.style.display = DisplayStyle.None;
    }
}
