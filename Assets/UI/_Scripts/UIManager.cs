using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // --- SINGLETON ---
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    // --- UI OBJECTS ---
    public GameObject menuUI;

    [Header("MainMenu")]
    public Button Btn_newgame;
    public Button Btn_options;
    public Button Btn_aboutus;
    public Button Btn_exit;

    [Header("Option")]
    public Slider Sen;
    public Button Btn_LowG;
    public Button Btn_UltraG;
    public Button Btn_back;

    [Header("Panel General")]
    public GameObject MainMenu;
    public GameObject OptionMenu;
    public GameObject Txt_AnyBtn;

    [Header("Loading")]
    public GameObject LoadingPanel;
    public Slider LoadingSlider;
    public Image LoadingBG;

    [Header("Settings")]
    public float minFadeDuration = 1.5f;
    public float uiFadeDuration = 0.5f;

    private bool isGameStarted = false;
    private bool returningFromGame = false;
    private bool startingNewGame = false;
    public GameObject Canvas;
    public GameObject Credit;


    // -----------------------
    // SCENE EVENT LISTENERS
    // -----------------------
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ================
        // LOAD SCENE_B
        // ================
        if (scene.name == "Scene_B")
        {
            menuUI.SetActive(false);

            int savedLevel = PlayerPrefs.GetInt("GraphicLevel", 1);
            ApplyGraphicsToScene(savedLevel);
            
            // === APPLY SENSITIVITY RETURNED FROM UI ===
            float sen = PlayerPrefs.GetFloat("MouseSensitivity", 50);
            MouseFollowCamera cam = FindObjectOfType<MouseFollowCamera>();
            if (cam != null)
                cam.mouseSensitivity = sen;

            return;
        }


        // ================
        // LOAD SCENE UI
        // ================
        if (scene.name == "UI")
        {
            if (returningFromGame)
            {
                returningFromGame = false;

                // Bỏ qua Press Any Key
                isGameStarted = true;

                Txt_AnyBtn.SetActive(false);
                MainMenu.SetActive(true);
                OptionMenu.SetActive(false);

                SetupPanelAlpha(MainMenu, 1f);

                return;
            }

            // Lần đầu chạy game
            isGameStarted = false;

            Txt_AnyBtn.SetActive(true);
            MainMenu.SetActive(false);
            OptionMenu.SetActive(false);

            SetupPanelAlpha(Txt_AnyBtn, 1f);

            int savedLevel = PlayerPrefs.GetInt("GraphicLevel", 1);
            UpdateButtonVisuals(savedLevel);
        }
    }



    private void Start()
    {
        SetupPanelAlpha(MainMenu, 0f);
        SetupPanelAlpha(OptionMenu, 0f);
        SetupPanelAlpha(Txt_AnyBtn, 1f);

        MainMenu.SetActive(false);
        OptionMenu.SetActive(false);
        Txt_AnyBtn.SetActive(true);

        if (LoadingPanel != null) LoadingPanel.SetActive(false);

        Btn_newgame.onClick.AddListener(NewGame);
        Btn_options.onClick.AddListener(ShowOptionMenu);
        Btn_aboutus.onClick.AddListener(AboutUs);
        Btn_exit.onClick.AddListener(ExitGame);

        Btn_back.onClick.AddListener(BackToMainMenuPanel);

        Btn_LowG.onClick.AddListener(() => SetGraphics(0));
        Btn_UltraG.onClick.AddListener(() => SetGraphics(1));

        int currentLevel = PlayerPrefs.GetInt("GraphicLevel", 1);
        UpdateButtonVisuals(currentLevel);
        Sen.onValueChanged.AddListener(OnSensitivityChanged);
        Sen.value = PlayerPrefs.GetFloat("MouseSensitivity", 300f);
    }


    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "UI" && !isGameStarted)
        {
            if (Input.anyKeyDown)
            {
                isGameStarted = true;
                StartCoroutine(SwitchPanelsCoroutine(Txt_AnyBtn, MainMenu));
            }
        }
    }


    // ========================
    // GRAPHIC LEVEL HANDLING
    // ========================
    public void SetGraphics(int level)
    {
        PlayerPrefs.SetInt("GraphicLevel", level);
        PlayerPrefs.Save();

        if (SceneManager.GetActiveScene().name == "Scene_B")
            ApplyGraphicsToScene(level);

        UpdateButtonVisuals(level);
    }


    void UpdateButtonVisuals(int activeLevel)
    {
        float activeAlpha = 1f;
        float dimAlpha = 0.5f;

        if (activeLevel == 0)
        {
            SetButtonState(Btn_LowG, activeAlpha);
            SetButtonState(Btn_UltraG, dimAlpha);
        }
        else
        {
            SetButtonState(Btn_LowG, dimAlpha);
            SetButtonState(Btn_UltraG, activeAlpha);
        }
    }


    void SetButtonState(Button btn, float alpha)
    {
        if (btn == null) return;

        ColorBlock cb = btn.colors;
        Color c = cb.normalColor;
        c.a = alpha;

        cb.normalColor = c;
        cb.selectedColor = c;

        btn.colors = cb;
    }


    void ApplyGraphicsToScene(int level)
    {
        GameObject sunObj = GameObject.Find("SUN");
        if (sunObj == null) return;

        Light sunLight = sunObj.GetComponent<Light>();
        if (sunLight == null) return;

        if (level == 0)
            sunLight.shadows = LightShadows.None;
        else
            sunLight.shadows = LightShadows.Soft;
    }


    // ========================
    // UI HANDLERS
    // ========================
    public void NewGame()
    {
        startingNewGame = true;

        LoadingPanel.SetActive(true);
        LoadingSlider.value = 0f;

        MainMenu.SetActive(false);
        OptionMenu.SetActive(false);
        Txt_AnyBtn.SetActive(false);

        StartCoroutine(NewGameSequence());
    }


    public void ReturnMenu()
    {
        returningFromGame = true;

        menuUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(LoadSceneAsyncCoroutine("UI"));
    }


    IEnumerator NewGameSequence()
    {
        yield return StartCoroutine(LoadSceneAsyncCoroutine("Scene_B"));
        
        
        menuUI.SetActive(false);
        startingNewGame = false;
    }


    public void ShowOptionMenu() =>
        StartCoroutine(SwitchPanelsCoroutine(MainMenu, OptionMenu));

    public void BackToMainMenuPanel() =>
        StartCoroutine(SwitchPanelsCoroutine(OptionMenu, MainMenu));


    IEnumerator SwitchPanelsCoroutine(GameObject outPanel, GameObject inPanel)
    {
        if (outPanel != null && outPanel.activeSelf)
        {
            yield return StartCoroutine(FadeCanvasGroup(outPanel, 1f, 0f, uiFadeDuration));
            outPanel.SetActive(false);
        }

        if (inPanel != null)
        {
            inPanel.SetActive(true);
            yield return StartCoroutine(FadeCanvasGroup(inPanel, 0f, 1f, uiFadeDuration));
        }
    }


    IEnumerator FadeCanvasGroup(GameObject targetObj, float startAlpha, float endAlpha, float duration)
    {
        CanvasGroup cg = targetObj.GetComponent<CanvasGroup>();
        if (cg == null) yield break;

        float timer = 0f;
        cg.alpha = startAlpha;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            yield return null;
        }

        cg.alpha = endAlpha;
    }


    void SetupPanelAlpha(GameObject obj, float alpha)
    {
        if (obj == null) return;
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg != null) cg.alpha = alpha;
    }


    public void AboutUs()
    {
        this.Canvas.gameObject.SetActive(false);
        Credit.SetActive(true);
        DOVirtual.DelayedCall(17f, () =>
        {
            this.Canvas.gameObject.SetActive(true);
            Credit.SetActive(false);
        });

    }
    public void ExitGame() => Application.Quit();


    // ========================
    // ASYNC SCENE LOADER
    // ========================
    public IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        LoadingPanel.SetActive(true);
        LoadingSlider.value = 0f;

        Color bg = LoadingBG.color;
        bg.a = 0f;
        LoadingBG.color = bg;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float timer = 0f;

        while (!operation.isDone)
        {
            timer += Time.deltaTime;

            float currentAlpha = Mathf.Clamp01(timer / minFadeDuration);
            bg = LoadingBG.color;
            bg.a = currentAlpha;
            LoadingBG.color = bg;

            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingSlider.value = progressValue;

            if (operation.progress >= 0.9f && LoadingBG.color.a >= 0.99f)
            {
                LoadingSlider.value = 1f;
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        LoadingPanel.SetActive(false);
        
    }
    void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        PlayerPrefs.Save();

        // Nếu đang trong Scene_B thì apply ngay
        MouseFollowCamera cam = FindObjectOfType<MouseFollowCamera>();
        if (cam != null)
            cam.mouseSensitivity = value;
    }

}
