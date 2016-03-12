using System;
using UnityEngine;
using ColossalFramework.UI;

namespace AmbientControl
{
    class LightSettingsPanel : UIPanel
    {
        public UIButton save;
        public UISlider ambiantIntensity;
        public UISlider sunIntensity;
        public UIButton reset;
        public UIPanel dropdowns;
        public UIDropDown lud;
        public UITitleBar m_title;
        public ColorOptions colorPanel;

        private const float LEFT_WIDTH = 250;
        private const float RIGHT_WIDTH = 250;
        private const float HEIGHT = 300;
        private const float SPACING = 5;
        private const float TITLE_HEIGHT = 40;

        private float originalSun;
        private float originalExposure;
        private float sunIntensityOffset;

        private static GameObject _gameObject;

        private static LightSettingsPanel _instance;
        public static LightSettingsPanel instance

        {
            get { return _instance; }
        }
    
        public static void Initialize()
        {
            try
            {
                // Destroy the UI if already exists
                _gameObject = GameObject.Find("LightSettingsPanel");
                Destroy();

                // Creating our own gameObect, helps finding the UI in ModTools
                _gameObject = new GameObject("LightSettingsPanel");
                _gameObject.transform.parent = UIView.GetAView().transform;
                _instance = _gameObject.AddComponent<LightSettingsPanel>();
            }
            catch (Exception e)
            {
                // Catching any exception to not block the loading process of other mods
                Debug.Log("Building Themes: An error has happened during the UI creation.");
                Debug.LogException(e);
            }
        }

        public override void Start()
        {
            base.Start();

            backgroundSprite = "UnlockingPanel2";
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = SPACING + LEFT_WIDTH + SPACING + SPACING + RIGHT_WIDTH + SPACING;
            height = TITLE_HEIGHT + HEIGHT + SPACING;
            relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2), Mathf.Floor((GetUIView().fixedHeight - height) / 2));

            // InitBuildingLists();
            SetupControls();
        }

        public static void Destroy()
        {
            try
            {
                if (_gameObject != null)
                    GameObject.Destroy(_gameObject);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void SetupControls()
        {
       
            m_title = AddUIComponent<UITitleBar>();
            m_title.title = "Light Settings";
            m_title.iconSprite = "ToolbarIconZoomOutCity";

            UIPanel left = AddUIComponent<UIPanel>();
            left.width = LEFT_WIDTH;
            left.height = HEIGHT ;
            left.relativePosition = new Vector3(SPACING, TITLE_HEIGHT +  SPACING);

            UIPanel right = AddUIComponent<UIPanel>();
            right.width = RIGHT_WIDTH;
            right.height = HEIGHT;
            right.relativePosition = new Vector3(LEFT_WIDTH + SPACING, TITLE_HEIGHT + SPACING);

            colorPanel = right.AddUIComponent<ColorOptions>();
            colorPanel.width = RIGHT_WIDTH;
            colorPanel.height = 120;
            colorPanel.relativePosition = new Vector3(0, 0);

        }

        public void Toggle()
        {
            if (isVisible)
            {
                Hide();
            }
            else
            {
                Show(true);
            }
        }

    }

    public class ColorOptions : UIPanel
    {


        private UICheckBox m_include;
        public UIDropDown ludDropdown;
        public UIDropDown subService;
        public UIDropDown level;
        public UITextField homeCount;

        private static ColorOptions _instance;
        public static ColorOptions instance
        {
            get { return _instance; }
        }


        public override void Start()
        {
            base.Start();
            _instance = this;
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            backgroundSprite = "UnlockingPanel";
            padding = new RectOffset(5, 5, 5, 0);
            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            autoLayoutPadding.top = 5;
            SetupControls();
        }

        private void SetupControls()
        {

            // Include
            m_include = UIUtils.CreateCheckBox(this);
            m_include.text = "Include";
            m_include.isVisible = false;

            // Level
            UILabel levelLabel = AddUIComponent<UILabel>();
            levelLabel.textScale = 0.8f;
            levelLabel.padding = new RectOffset(0, 0, 8, 0);
            levelLabel.text = "Service: ";
            //levelLabel.relativePosition = new Vector3(status.relativePosition.x + status.width + 10, 40);

            ludDropdown = UIUtils.CreateDropDown(this);
            ludDropdown.width = 180;

            foreach (var lud in ColorCorrectionManager.instance.items)
            {
                ludDropdown.AddItem(lud);
            }

            ludDropdown.selectedIndex = ColorCorrectionManager.instance.lastSelection;

            ludDropdown.eventSelectedIndexChanged += (c, i) => ColorCorrectionManager.instance.currentSelection = i;

            //service.relativePosition = new Vector3(levelLabel.relativePosition.x + levelLabel.width + 5, 40);

            subService = UIUtils.CreateDropDown(this);
            subService.width = 180;
            subService.AddItem("None");
            subService.AddItem("Residential");
            subService.AddItem("Industrial");
            subService.AddItem("Office");
            subService.AddItem("Commercial");
            subService.AddItem("Extractor");
            subService.selectedIndex = 0;

            //lud.selectedIndex = ColorCorrectionManager.instance.lastSelection;
           // lud.eventSelectedIndexChanged += (c, i) => ColorCorrectionManager.instance.currentSelection = i;

            homeCount = UIUtils.CreateTextField(this);
            homeCount.width = 180;

        }

        public override void Update()
        {

        }


        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            p.Use();
            //UIThemeManager.instance.ChangeUpgradeBuilding(m_building);

            base.OnMouseDown(p);
        }

        protected override void OnMouseEnter(UIMouseEventParameter p)
        {
            base.OnMouseEnter(p);
            //UIThemeManager.instance.buildingPreview.Show(m_building);
        }


        protected override void OnMouseLeave(UIMouseEventParameter p)
        {
            base.OnMouseLeave(p);
            //UIThemeManager.instance.buildingPreview.Show(UIThemeManager.instance.selectedBuilding);
        }

    }

}
