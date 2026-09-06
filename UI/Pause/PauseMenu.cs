using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Animator animator;
    [SerializeField] private Image image;

    public bool isPaused = false;
    private bool isSettings = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        image.enabled = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (InputManager.instance.PausePressed())
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        animator.SetTrigger("Open");
        StartCoroutine(FadeScreen(0f, 100f / 255f, 0.5f));
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        if (isSettings) return;
        
        animator.SetTrigger("Close");
        StartCoroutine(FadeScreen(100f / 255f, 0f, 0.5f));
        StartCoroutine(DisableAfterAnimation());
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);

        isSettings = true;
    }

    public void Back()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        isSettings = false;
    }

    private IEnumerator DisableAfterAnimation()
    {
        yield return new WaitForSecondsRealtime(0.55f);
        pauseMenu.SetActive(false);
        image.enabled = false;
    }

    private IEnumerator FadeScreen(float from, float to, float duration)
    {
        image.enabled = true;
        float elapsed = 0f;
        Color color = image.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        image.color = new Color(color.r, color.g, color.b, to);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
