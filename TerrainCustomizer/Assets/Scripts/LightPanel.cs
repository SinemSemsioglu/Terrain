using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit.Examples.UI;
using UnityEngine.UI;

namespace ProceduralToolkit.Examples
{
    public class LightPanel : ConfiguratorBase
    {

        public RectTransform leftPanel;
        public RectTransform rightPanel;

        public Material skyboxMat;
        public Material seaMat;
        // min max atmosphere thickness values for the skybox material
        private const int minThickness = 0;
        private const int maxThickness = 5;

        // to be set as 9 lights in the scene, 0: low arousal neutral valence, going counter clockwise for the next 7 and ending with neutral.
        public Light[] lights = new Light[9];

        // min max values for hue, saturation and brigthness values
        public float minHue = 0;
        public float maxHue = 1;
        public float minSat = 0;
        public float maxSat = 1;
        public float minBri = 0;
        public float maxBri = 1;

        // hue, saturation and brightness values for each light
        private float[] hues = new float[9];
        private float[] sats = new float[9];
        private float[] bris = new float[9];

        // labels for light hue controls, essentially the position of each light on the valence/arousal diagram
        private string[] labels = new string[9] { "low valence", "low arousal low valence", "low arousal", "low arousal high valence", "high valence", "high arousal high valence", "high arousal", "high arousal low valence", "neutral" };

        // 4 potential configs for saturation: increase with valence, decrease with valence, increase with arousal, decrease with arousal
        private bool[] satConfig = new bool[4];
        public ToggleGroup satToggleGroup;

        // same for brightness
        private bool[] briConfig = new bool[4];
        public ToggleGroup briToggleGroup;

         
        // Use this for initialization
        void Start()
        {
            InstantiateControl<SliderControl>(rightPanel)
             .Initialize("Atmospheric color", minThickness, maxThickness, skyboxMat.GetFloat("_AtmosphereThickness"), value =>
             {
                 skyboxMat.SetFloat("_AtmosphereThickness", value);
             });

            InstantiateControl<ButtonControl>(rightPanel)
             .Initialize("Sea color Cyan", () =>
             {
                 seaMat.SetColor("_EmissionColor", Color.cyan);
             });

            InstantiateControl<ButtonControl>(rightPanel)
             .Initialize("Sea color Magenta", () =>
             {
                 seaMat.SetColor("_EmissionColor", Color.magenta);
             });

            InstantiateControl<ButtonControl>(rightPanel)
             .Initialize("Sea color Black", () =>
             {
                 seaMat.SetColor("_EmissionColor", Color.black);
             });
             
            InstantiateControl<ButtonControl>(rightPanel)
             .Initialize("Sea color Gray", () =>
             {
                 seaMat.SetColor("_EmissionColor", Color.gray);
             });

            InstantiateControl<TextControl>(leftPanel)
                .Initialize("Set how saturation relates to valence and arousal");

            InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Increase with valence", satConfig[0], value =>
            {
                satConfig[0] = value;

                if (value)
                {
                    setSaturation(true, true);
                } else
                {
                    setSaturationDefault();
                }
            }, satToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Decrease with valence", satConfig[1], value =>
            {
                satConfig[1] = value;

                if (value)
                {
                    setSaturation(true, false);
                } else
                {
                    setSaturationDefault();
                }
            }, satToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Increase with arousal", satConfig[2], value =>
            {
                satConfig[2] = value;

                if (value)
                {
                    setSaturation(false, true);
                } else
                {
                    setSaturationDefault();
                }
            }, satToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Decrease with arousal", satConfig[3], value =>
            {
                satConfig[3] = value;

                if (value)
                {
                    setSaturation(false, false);
                } else
                {
                    setSaturationDefault();
                }
            }, satToggleGroup);

            InstantiateControl<TextControl>(leftPanel)
                .Initialize("Set how brightness relates to valence and arousal");

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Increase with valence", briConfig[0], value =>
            {
                briConfig[0] = value;

                if (value)
                {
                    setBrightness(true, true);
                } else
                {
                    setBrightnessDefault();
                }
            }, briToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Decrease with valence", briConfig[1], value =>
            {
                briConfig[1] = value;

                if (value)
                {
                    setBrightness(true, false);
                } else
                {
                    setBrightnessDefault();
                }
            }, briToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Increase with arousal",briConfig[2], value =>
            {
                briConfig[2] = value;

                if (value)
                {
                    setBrightness(false, true);
                } else
                {
                    setBrightnessDefault();
                }
            }, briToggleGroup);

             InstantiateControl<ToggleControl>(leftPanel)
            .Initialize("Decrease with arousal", briConfig[3], value =>
            {
                briConfig[3] = value;

                if (value)
                {
                    setBrightness(false, false);
                } else
                {
                    setBrightnessDefault();
                }
            }, briToggleGroup);

           
            /*
            for (int c=0; c<9; c++)
            {
                int ind = c;
                InstantiateControl<SliderControl>(rightPanel)
                .Initialize(labels[ind], minHue, maxHue, hues[ind], value =>
                {
                  
                });
            }
            */

        }

        public void lVSlider(float value) {
            updateHue(0, value);
        }

        public void lAlVSlider(float value) {
            updateHue(1, value);
        }

        public void lASlider(float value) {
            updateHue(2, value);
        }

        public void lAhVSlider(float value) {
            updateHue(3, value);
        }

        public void hVSlider(float value) {
            updateHue(4, value);
        }

        public void hAhVSlider(float value) {
            updateHue(5, value);
        }

        public void hASlider(float value) {
            updateHue(6, value);
        }

        public void hAlVSlider(float value) {
            updateHue(7, value);
        }

         public void nSlider(float value) {
            updateHue(8, value);
        }

        private void updateHue(int ind, float value) {
            hues[ind] = value;
            setLightColor(lights[ind], value, -1, -1);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void setSaturationDefault() {

            for(int i=0; i<lights.Length; i++) {
                setLightColor(lights[i], -1, 0.5f, -1);
            }       
        }

        private void setBrightnessDefault() {
            
            for(int i=0; i<lights.Length; i++) {
                setLightColor(lights[i], -1, -1, 0.5f);
            }       
        }

        private void setSaturation(bool valence, bool direct)
        {
            float[] satVals = getSaturationBrightnessValues(5, direct, true);
            float[] briVals = initConstantArray(new float[5], -1);

            if (valence)
            {
                setValence(satVals, briVals);
            } else
            {
                setArousal(satVals, briVals);
            }
        }

        private void setBrightness(bool valence, bool direct)
        {
            float[] briVals = getSaturationBrightnessValues(5, direct, false);
            float[] satVals = initConstantArray(new float[5], -1);

            if (valence)
            {
                setValence(satVals, briVals);
            }
            else
            {
                setArousal(satVals, briVals);
            }
        }

        private float[] getSaturationBrightnessValues(int numVals, bool direct, bool saturation)
        {
            float min;
            float max;

            if (saturation)
            {
                max = maxSat;
                min = minSat;
            } else
            {
                max = maxBri;
                min = minBri;
            }

            float[] vals = new float[numVals];
            float step = (max - min) / (numVals -1);
            float curr;
            int multiplier;

            if (direct)
            {
                curr = min;
                multiplier = 1;
            } else
            {
                curr = max;
                multiplier = -1;
            }

            for (int i=0; i < numVals; i++)
            {
                vals[i] = curr;
                curr += (multiplier * step);
            }

            return vals;
        }

        private void setValence(float[] satValues, float[] briValues)
        {
            setLightColor(lights[0], -1, satValues[0], briValues[0]);

            setLightColor(lights[1], -1, satValues[1], briValues[1]);
            setLightColor(lights[7], -1, satValues[1], briValues[1]);

            setLightColor(lights[2], -1, satValues[2], briValues[2]);
            setLightColor(lights[6], -1, satValues[2], briValues[2]);
            setLightColor(lights[8], -1, satValues[2], briValues[2]);

            setLightColor(lights[3], -1, satValues[3], briValues[3]);
            setLightColor(lights[5], -1, satValues[3], briValues[3]);

            setLightColor(lights[4], -1, satValues[4], briValues[4]);
        }

        private void setArousal(float[] satValues, float[] briValues)
        {
            setLightColor(lights[2], -1, satValues[0], briValues[0]);

            setLightColor(lights[1], -1, satValues[1], briValues[1]);
            setLightColor(lights[3], -1, satValues[1], briValues[1]);

            setLightColor(lights[0], -1, satValues[2], briValues[2]);
            setLightColor(lights[4], -1, satValues[2], briValues[2]);
            setLightColor(lights[8], -1, satValues[2], briValues[2]);

            setLightColor(lights[5], -1, satValues[3], briValues[3]);
            setLightColor(lights[7], -1, satValues[3], briValues[3]);

            setLightColor(lights[6], -1, satValues[4], briValues[4]);
        }

        private float[] initConstantArray(float[] arr, float val) {
            for (int i=0; i<arr.Length; i++)
            {
                arr[i] = val;
            }

            return arr;
        }

        // update hue, sat, brightness of a light if values of hVal, sVal, bVal >=0
        private void setLightColor(Light l, float hVal, float sVal, float bVal)
        {
            float h;
            float s;
            float b;

            Color.RGBToHSV(l.color, out h, out s, out b);

            if (hVal >= 0) h = hVal;
            if (sVal >= 0) s = sVal;
            if (bVal >= 0) b = bVal;
            l.color = Color.HSVToRGB(h, s, b);
        }

        // returns hsv colors of all lights
        public Vector3[] getLightColors() {
            Vector3[] colors = new Vector3[lights.Length];
            float h;
            float s;
            float b;

            for (int i=0; i < lights.Length; i++) {
                Color.RGBToHSV(lights[i].color, out h, out s, out b);
                colors[i] = new Vector3(h, s, b);
            }

            return colors;
        }

        public float getAtmosphereThickness() {
            return skyboxMat.GetFloat("_AtmosphereThickness");
        }

        public void setAtmosphereThickness(float atmosphereThickness) {
	        skyboxMat.SetFloat("_AtmosphereThickness", atmosphereThickness);
        }

        public Vector3 getSeaColor() {
            Color c = seaMat.GetColor("_EmissionColor");
            return new Vector3(c.r, c.g, c.b);
        }

        public void setSeaColor(Vector3 seaColor) {
            seaMat.SetColor("_EmissionColor", Color.HSVToRGB(seaColor.x, seaColor.y, seaColor.z));
        }


        public void setLightColors(Vector3[] lightHSV) {
            if (lights.Length != lightHSV.Length) {
                Debug.Log("light numbers do not match");
            } else {
                for (int i = 0; i <lights.Length; i++) {
                    Vector3 col = lightHSV[i];
                    lights[i].color = Color.HSVToRGB(col.x, col.y, col.z);
                }
            }
        }
    }
}
