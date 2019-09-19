using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

namespace MVRPlugin {
    public class VAM_HUD : MVRScript
    {

        // IMPORTANT - DO NOT make custom enums. The dynamic C# complier crashes Unity when it encounters these for
        // some reason

        // IMPORTANT - DO NOT OVERRIDE Awake() as it is used internally by MVRScript - instead use Init() function which
        // is called right after creation
        public override void Init()
        {
            try
            {
                SetupPluginUI();
                SetupHUD();
            }
            catch (Exception e)
            {
                SuperController.LogError("Exception in VAM-HUD Init: " + e);
            }
        }

        // Start is called once before Update or FixedUpdate is called and after Init()
        void Start()
        {
            try
            {
                fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            }
            catch (Exception e)
            {
                SuperController.LogError("Exception in VAM-HUD Start: " + e);
            }
        }

        // Update is called with each rendered frame by Unity
        float deltaTime = 0.0f;
        void Update()
        {
            try
            {
                //deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

                // measure average frames per second
                fpsAccumulator++;
                if (Time.realtimeSinceStartup > fpsNextPeriod)
                {
                    CurrentFps = (int)(fpsAccumulator / fpsMeasurePeriod);
                    fpsAccumulator = 0;
                    fpsNextPeriod += fpsMeasurePeriod;

                    UpdateDisplay(labelTopLeft, typeTopLeft);
                    UpdateDisplay(labelTopRight, typeTopRight);
                    UpdateDisplay(labelBottomLeft, typeBottomLeft);
                    UpdateDisplay(labelBottomRight, typeBottomRight);
                }
            }
            catch (Exception e)
            {
                SuperController.LogError("Exception in VAM HUD Update: " + e);
            }
        }

        void OnDestroy()
        {
            Destroy(hud);
        }

        protected GameObject hud;
        protected float hudScale = 100f;
        protected float topOffset = -.1f;
        const float scaleFactor = 0.000006f;

        protected float fontSize = 36.0f;
        protected Color textColor = Color.white;

        protected Text labelTopLeft;
        protected Text labelTopRight;
        protected Text labelBottomLeft;
        protected Text labelBottomRight;
        protected string typeTopLeft = "None";
        protected string typeTopRight = "None";
        protected string typeBottomLeft = "None";
        protected string typeBottomRight = "FPS";

        const float fpsMeasurePeriod = 0.5f;
        private int fpsAccumulator = 0;
        private float fpsNextPeriod = 0;
        private int CurrentFps;

        protected UIDynamicSlider sliderScale;
        protected UIDynamicSlider sliderTopOffset;
        protected UIDynamicSlider sliderFontSize;

        protected void SetupHUD()
        {
            hud = new GameObject();

            Canvas canvas = hud.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            // display only HUD doe snot need to be added to scene
            //SuperController.singleton.AddCanvas(dynCanvas);
            hud.name = "VAM HUD";

            CanvasScaler cs = hud.AddComponent<CanvasScaler>();
            cs.scaleFactor = 100.0f;
            cs.dynamicPixelsPerUnit = 1f;

            GraphicRaycaster gr = hud.AddComponent<GraphicRaycaster>();
            RectTransform rt = hud.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500f);

            hud.transform.localScale = new Vector3(hudScale * scaleFactor, hudScale * scaleFactor, hudScale * scaleFactor);
            hud.transform.localPosition = new Vector3(0f, topOffset, .7f);

            // anchor to head for HUD effect
            Transform headCenter = SuperController.singleton.centerCameraTarget.transform;
            rt.SetParent(headCenter, false);

            SetupTopLeftLabel(hud);
            SetupTopRightLabel(hud);
            SetupBottomLeftLabel(hud);
            SetupBottomRightLabel(hud);
        }
        protected void SetupTopLeftLabel(GameObject canvas)
        {

            GameObject g = new GameObject();
            g.name = "VAM HUD Top Left";
            g.transform.parent = canvas.transform;
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = new Vector3(-500f, 500f, 0f);
            g.transform.localRotation = Quaternion.identity;

            labelTopLeft = g.AddComponent<Text>();

            RectTransform rt = g.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);

            StyleLabel(labelTopLeft);

        }
        protected void SetupTopRightLabel(GameObject canvas)
        {

            GameObject g = new GameObject();
            g.name = "VAM HUD Top Right";
            g.transform.parent = canvas.transform;
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = new Vector3(500f, 500f, 0f);
            g.transform.localRotation = Quaternion.identity;

            labelTopRight = g.AddComponent<Text>();

            RectTransform rt = g.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);

            StyleLabel(labelTopRight);

        }
        protected void SetupBottomLeftLabel(GameObject canvas)
        {

            GameObject g = new GameObject();
            g.name = "VAM HUD Bottom Left";
            g.transform.parent = canvas.transform;
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = new Vector3(-500f, -500f, 0f);
            g.transform.localRotation = Quaternion.identity;

            labelBottomLeft = g.AddComponent<Text>();

            RectTransform rt = g.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);

            StyleLabel(labelBottomLeft);

        }
        protected void SetupBottomRightLabel(GameObject canvas)
        {

            GameObject g = new GameObject();
            g.name = "VAM HUD BottomRight";
            g.transform.parent = canvas.transform;
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = new Vector3(500f, -500f, 0f);
            g.transform.localRotation = Quaternion.identity;

            labelBottomRight = g.AddComponent<Text>();

            RectTransform rt = g.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);

            StyleLabel(labelBottomRight);

        }
        protected void StyleLabel(Text label) { 

            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            label.alignment = TextAnchor.MiddleCenter;
            label.horizontalOverflow = HorizontalWrapMode.Overflow;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            label.font = ArialFont;
            label.fontSize = (int) fontSize;
            label.enabled = true;
            label.color = textColor;
        }
        protected void SetupPluginUI()
        {
            List<string> choices = new List<string>();
            choices.Add("None");
            choices.Add("FPS");
            choices.Add("Load Dir");
            choices.Add("Total Atoms");

            JSONStorableStringChooser jsonTypeTopLeft = new JSONStorableStringChooser("TopLeft", choices, typeTopLeft, "Top Left", SetTopLeft);
            UIDynamicPopup dropdownTopLeft = CreatePopup(jsonTypeTopLeft);
            dropdownTopLeft.labelWidth = 300f;

            JSONStorableStringChooser jsonTypeTopRight = new JSONStorableStringChooser("TopRight", choices, typeTopRight, "Top Right", SetTopRight);
            UIDynamicPopup dropdownTopRight = CreatePopup(jsonTypeTopRight, true);
            dropdownTopRight.labelWidth = 300f;

            JSONStorableStringChooser jsonTypeBottomLeft = new JSONStorableStringChooser("BottomLeft", choices, typeBottomLeft, "Bottom Left", SetBottomLeft);
            UIDynamicPopup dropdownBottomLeft = CreatePopup(jsonTypeBottomLeft);
            dropdownBottomLeft.labelWidth = 300f;

            JSONStorableStringChooser jsonTypeBottomRight = new JSONStorableStringChooser("BottomRight", choices, typeBottomRight, "Bottom Right", SetBottomRight);
            UIDynamicPopup dropdownBottomRight = CreatePopup(jsonTypeBottomRight, true);
            dropdownBottomRight.labelWidth = 300f;

            // let user adjust scale and top for HMD viewport differences and personal preference.
            JSONStorableFloat jsonTopOffset = new JSONStorableFloat("Top Offset", topOffset, SetTopOffset, -2f, +2f, true);
            RegisterFloat(jsonTopOffset);
            sliderTopOffset = CreateSlider(jsonTopOffset);

            JSONStorableFloat jsonScale = new JSONStorableFloat("HUD Scale", hudScale, SetScale, 10f, 200f, true);
            RegisterFloat(jsonScale);
            sliderScale = CreateSlider(jsonScale);

            JSONStorableFloat jsonFontSize = new JSONStorableFloat("Text Size", fontSize, SetFontSize, 10f, 100f, true);
            RegisterFloat(jsonFontSize);
            sliderFontSize = CreateSlider(jsonFontSize);

            
            // JSONStorableColor example
            HSVColor hsvc = HSVColorPicker.RGBToHSV(textColor.r, textColor.g, textColor.b);
            JSONStorableColor jsonTextColor = new JSONStorableColor("Text Color", hsvc, SetTextColor);
            RegisterColor(jsonTextColor);
            CreateColorPicker(jsonTextColor, true);
        }
        protected void SetFontSize(JSONStorableFloat storable)
        {
            fontSize = (int) storable.val;
            StyleLabel(labelTopLeft);
            StyleLabel(labelTopRight);
            StyleLabel(labelBottomLeft);
            StyleLabel(labelBottomRight);
        }
        protected void SetScale(JSONStorableFloat storable)
        {
            hudScale = storable.val;
            hud.transform.localScale = new Vector3(hudScale * scaleFactor, hudScale * scaleFactor, hudScale * scaleFactor);
        }
        protected void SetTopOffset(JSONStorableFloat storable)
        {
            topOffset = storable.val;
            hud.transform.localPosition = new Vector3(0f, topOffset, .7f);
        }
        protected void SetTextColor(JSONStorableColor jcolor)
        {
            textColor = jcolor.colorPicker.currentColor;
            StyleLabel(labelTopLeft);
            StyleLabel(labelTopRight);
            StyleLabel(labelBottomLeft);
            StyleLabel(labelBottomRight);
        }
        protected void SetTopLeft(string type)
        {
            typeTopLeft = type;
        }
        protected void SetTopRight(string type)
        {
            typeTopRight = type;
        }
        protected void SetBottomLeft(string type)
        {
            typeBottomLeft = type;
        }
        protected void SetBottomRight(string type)
        {
            typeBottomRight = type;
        }

        void UpdateDisplay(Text label, string type)
        {
            if (type == "FPS")
            {
                label.text = string.Format("{0} FPS", CurrentFps);
            }
            else if (type == "Load Dir")
            {
                label.text = SuperController.singleton.currentLoadDir;
            }
            else if (type == "Total Atoms")
            {
                label.text = SuperController.singleton.GetAtoms().Count + " atoms";
            }
            else
            {
                label.text = "";
            }
            
        }
    }
}