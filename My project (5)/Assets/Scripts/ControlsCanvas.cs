using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlsCanvas : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup canvasGroup;

    [Header("Settings")]
    public float displayTime = 3f;   // cuánto tiempo se muestra
    public float fadeDuration = 1f;  // cuánto tarda el fade

    private void Start()
    {
        // bloquea el juego mientras se muestran los controles
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(ShowThenFade());
    }

    private IEnumerator ShowThenFade()
    {
        // espera el tiempo de display (usando unscaledTime para ignorar timeScale)
        yield return new WaitForSecondsRealtime(displayTime);

        // fade out
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        // desactiva el canvas y arranca el juego
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}