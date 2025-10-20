using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class HelpMessage:MonoBehaviour
{
    public string message;
    [SerializeField] float fadeInWait = 1.0f;
    [SerializeField] float fadeOutWait = 1.0f;
    BalloonDialog balloonDialog;
    Coroutine coroutine;
    Coroutine hideCoroutine;

    private void Awake()
    {
        balloonDialog = FindAnyObjectByType<BalloonDialog>();
    }

    public void OnPointerEnter()
    {
        balloonDialog.Show(message, this.transform.position);
    }

    public void OnPointerExit()
    {
        balloonDialog.Hide();
    }
}


