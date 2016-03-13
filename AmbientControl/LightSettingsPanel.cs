using System;
using UnityEngine;
using ColossalFramework.UI;

namespace LightControl
{
    class LightSettingsPanel : UIPanel
    {
      

        public UITitleBar m_title;

        public ColorPanel colorPanel;
        public LightPanel lightPanel;

        public UITabstrip panelTabs;

        public UIButton lightButton;
        public UIButton colorButton;
        public UIButton engineButton;
        public UIButton presetButton;

        private const float WIDTH = 270;
        private const float HEIGHT = 350;
        private const float SPACING = 5;
        private const float TITLE_HEIGHT = 36;
        private const float TABS_HEIGHT = 28;

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
                Debug.LogException(e);
            }
        }

        public override void Start()
        {
            base.Start();

            backgroundSprite = "PoliciesBubble";
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            padding = new RectOffset(10, 10, 4, 4);
            width = SPACING + WIDTH;
            height = TITLE_HEIGHT + HEIGHT + TABS_HEIGHT + SPACING;
            relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2), Mathf.Floor((GetUIView().fixedHeight - height) / 2));

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
            m_title.title = "Graphics Settings";

            panelTabs = AddUIComponent<UITabstrip>();
            panelTabs.relativePosition = new Vector2(SPACING, TITLE_HEIGHT + SPACING);
            panelTabs.size = new Vector2(WIDTH, TABS_HEIGHT);
            panelTabs.padding = new RectOffset(2, 2, 2, 2);

            lightButton = UIUtils.CreateTab(panelTabs, "Light");
            colorButton = UIUtils.CreateTab(panelTabs, "Color");
            engineButton = UIUtils.CreateTab(panelTabs, "Engine");
            presetButton = UIUtils.CreateTab(panelTabs, "Presets");

            UIPanel body = AddUIComponent<UIPanel>();
            body.width = WIDTH;
            body.height = HEIGHT ;
            body.relativePosition = new Vector3(SPACING, TITLE_HEIGHT + TABS_HEIGHT +  SPACING);

            lightPanel = body.AddUIComponent<LightPanel>();
            lightPanel.width = WIDTH - SPACING;
            lightPanel.height = HEIGHT;
            lightPanel.relativePosition = new Vector3(0, 0);
            lightPanel.isVisible = false;

            colorPanel = body.AddUIComponent<ColorPanel>();
            colorPanel.width = WIDTH - SPACING;
            colorPanel.height = HEIGHT;
            colorPanel.relativePosition = new Vector3(0, 0);
            colorPanel.isVisible = false;

            lightButton.eventClick += (sender, e) => TabClicked(sender, e);
            colorButton.eventClick += (sender, e) => TabClicked(sender, e);
            engineButton.eventClick += (sender, e) => TabClicked(sender, e);
            presetButton.eventClick += (sender, e) => TabClicked(sender, e);

        }
        void TabClicked(UIComponent component, UIMouseEventParameter eventParam)
        {
            lightPanel.isVisible = false;
            colorPanel.isVisible = false;

            if (component == lightButton) {
                lightPanel.isVisible = true;
            }
            if (component == colorButton)
            {
                colorPanel.isVisible = true;
            }
            if (component == engineButton)
            {
                lightPanel.isVisible = true;
            }
            if (component == presetButton)
            {
                lightPanel.isVisible = true;
            }
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

    public class ColorPanel : UIPanel
    {

        private UICheckBox m_include;
        public UIDropDown ludDropdown;
        public UIButton reset;
        public UIButton save;

        private static ColorPanel _instance;
        public static ColorPanel instance
        {
            get { return _instance; }
        }

        public override void Start()
        {
            base.Start();
            _instance = this;
            //isVisible = true;
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

            ludDropdown = UIUtils.CreateDropDown(this);
            ludDropdown.width = 180;

            foreach (var lud in ColorCorrectionManager.instance.items)
            {
                ludDropdown.AddItem(lud);
            }

            ludDropdown.selectedIndex = ColorCorrectionManager.instance.lastSelection;
            ludDropdown.eventSelectedIndexChanged += (c, i) => ColorCorrectionManager.instance.currentSelection = i; 

            reset = UIUtils.CreateButton(this);
            reset.width = 90;
            reset.text = "Reset";

            save = UIUtils.CreateButton(this);
            save.width = 90;
            save.text = "Save";
        }
    }

    public class LightPanel : UIPanel
    {

        UISlider heightSlider;
        UISlider rotationSlider;
        UISlider intensitySlider;
        UISlider ambientSlider;

        private static LightPanel _instance;
        public static LightPanel instance
        {
            get { return _instance; }
        }

        public override void Start()
        {
            base.Start();
            _instance = this;
            //isVisible = true;
            canFocus = true;
            isInteractive = true;
            backgroundSprite = "UnlockingPanel";

            autoLayoutPadding = new RectOffset(10, 10, 4, 4);
            autoLayout = true;
            //autoFitChildrenVertically = true;
            autoLayoutDirection = LayoutDirection.Vertical;

            //autoLayoutPadding.top = 5;
            SetupControls();
        }

        private void SetupControls()
        {

            AddUIComponent<UILabel>().text = "Height";
            heightSlider = UIUtils.CreateSlider(this, -80f, 80f);
            heightSlider.eventValueChanged += ValueChanged;

            AddUIComponent<UILabel>().text = "Rotation";
            rotationSlider = UIUtils.CreateSlider(this, -180f, 180f);
            rotationSlider.eventValueChanged += ValueChanged;

            AddUIComponent<UILabel>().text = "Intensity";
            intensitySlider = UIUtils.CreateSlider(this, 0f, 10f);
            intensitySlider.eventValueChanged += ValueChanged;
            intensitySlider.stepSize = 0.1f;

            AddUIComponent<UILabel>().text = "Ambient";
            ambientSlider = UIUtils.CreateSlider(this, 0f, 2f);
            ambientSlider.eventValueChanged += ValueChanged;
            ambientSlider.stepSize = 0.1f;
        }

        void ValueChanged(UIComponent component, float value)
        {
            if (component == heightSlider)
            {
                DayNightProperties.instance.m_Latitude = value;
            }

            if (component == rotationSlider)
            {
                DayNightProperties.instance.m_Longitude = value;
            }

            if (component == ambientSlider)
            {
                DayNightProperties.instance.m_Exposure = value;
            }

            if (component == intensitySlider)
            {
                DayNightProperties.instance.m_SunIntensity = value;
            }
        }
    }

    }
