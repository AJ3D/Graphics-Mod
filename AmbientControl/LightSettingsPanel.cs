using System;
using UnityEngine;
using ColossalFramework.UI;

namespace LightControl
{
    class LightSettingsPanel : UIPanel
    {
      

        public UIButton save;
        public UIButton reset;


        public UITitleBar m_title;

        public ColorOptions colorPanel;
        public SunOptions sunOptions;

        private const float LEFT_WIDTH = 250;
        private const float RIGHT_WIDTH = 250;
        private const float HEIGHT = 225;
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

            padding = new RectOffset(10, 10, 4, 4);
           

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
            colorPanel.width = RIGHT_WIDTH - 4;
            colorPanel.height = 220;
            colorPanel.relativePosition = new Vector3(0, 0);

            sunOptions = left.AddUIComponent<SunOptions>();
            sunOptions.width = LEFT_WIDTH - 4;
            sunOptions.height = 220;
            sunOptions.relativePosition = new Vector3(0, 0);

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
        public UIButton reset;
        public UIButton save;

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
    public class SunOptions : UIPanel
    {

        UISlider heightSlider;
        UISlider rotationSlider;
        UISlider intensitySlider;
        UISlider ambientSlider;

        private static SunOptions _instance;
        public static SunOptions instance
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
