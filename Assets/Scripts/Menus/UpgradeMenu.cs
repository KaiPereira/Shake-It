using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MainMenu : MonoBehaviour
{
    private UIDocument _document;
    private Button _exit_button;
    public VisualTreeAsset upgradeTemplate;
    private VisualElement upgradeContainer;

    private GameManager gameManager;
    private ScoreManager scoreManager;

    public AudioSource buySound;
    public AudioSource errorSound;

    private float buttonErrorLength = 0.25f;

    public Color successColor;
    public Color errorColor;

    [System.Serializable]
    public class Upgrade
    {
        public VisualElement[] outlines;
        public float[] upgradePrices;
        public int currentLevel = 0;
        public System.Action<int> onUpgradeAction;
        public Button upgradeButton;
        public Label upgradeText;
    }

    public List<Upgrade> upgrades = new List<Upgrade>();

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        gameManager = FindObjectOfType<GameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnEnable()
    {
        var root = _document.rootVisualElement;
        _exit_button = root.Q<Button>("ExitMenuButton");
        upgradeContainer = root.Q<GroupBox>("UpgradesContainer");

        _exit_button.RegisterCallback<ClickEvent>(ExitMenuClick);

        foreach (Upgrade upgrade in upgrades)
        {
            var upgradeElement = upgradeTemplate.CloneTree();

            upgradeContainer.Add(upgradeElement);

            // Get button, outlines, sprites
            upgrade.upgradeButton = upgradeElement.Q<Button>("UpgradeButton");
            upgrade.upgradeText = upgradeElement.Q<Label>("UpgradeText");

            upgrade.outlines =new VisualElement[4]
            {
                root.Q<VisualElement>("outline1"),
                root.Q<VisualElement>("outline2"),
                root.Q<VisualElement>("outline3"),
                root.Q<VisualElement>("outline4")
            };

            upgrade.upgradeButton.RegisterCallback<ClickEvent>(evt => PerformUpgrade(upgrade));
        }
    }

    private void OnDisable()
    {
        _exit_button.UnregisterCallback<ClickEvent>(ExitMenuClick);
    }

    private void ExitMenuClick(ClickEvent evt)
    {
        gameObject.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void PerformUpgrade(Upgrade upgrade)
    {
        bool canUpgrade = scoreManager.BuyUpgrade(upgrade.upgradePrices[upgrade.currentLevel]);

        if (canUpgrade)
        {
            buySound.Play();

            if (upgrade.currentLevel < upgrade.outlines.Length)
            {
                upgrade.currentLevel++;
            }

            // Change the price
            if (upgrade.currentLevel == upgrade.outlines.Length) {
                upgrade.upgradeText.text = "max"; 
            } else {
                upgrade.upgradeText.text = upgrade.upgradePrices[upgrade.currentLevel].ToString();
            }

            // Fill the outlines
            for (int i = 0; i < upgrade.currentLevel; i++)
            {
                upgrade.outlines[i].style.backgroundColor = successColor;
            }

            gameManager.customerLevelUpgrade = upgrade.currentLevel;
        } else {
            StartCoroutine(ButtonError(upgrade));
        }
    }

    private IEnumerator ButtonError(Upgrade upgrade)
    {
        errorSound.Play();

        upgrade.upgradeButton.style.backgroundColor = errorColor;

        yield return new WaitForSeconds(buttonErrorLength);

        upgrade.upgradeButton.style.backgroundColor = successColor;
    }

    /*private void OnEnable()
    {
        var root = _document.rootVisualElement;
        _upgrade_customer_button = root.Q<Button>("UpgradeCustomers");

        _exit_button.RegisterCallback<ClickEvent>(ExitMenuClick);
        _upgrade_customer_button.RegisterCallback<ClickEvent>(UpgradeCustomer);

        outlines = new VisualElement[4]
        {
            root.Q<VisualElement>("outline1"),
            root.Q<VisualElement>("outline2"),
            root.Q<VisualElement>("outline3"),
            root.Q<VisualElement>("outline4")
        };

        FillOutlines();
        CustomerUpgradePrice();
    }

    private void UpgradeBuilding()
    {

    }

    private void FillOutlines()
    {
        for (int i = 0; i < outline_index; i++)
        {
            outlines[i].style.backgroundColor = customerUpgradeColor;
        }
    }

    private void CustomerUpgradePrice()
    {
    }

    */
}
