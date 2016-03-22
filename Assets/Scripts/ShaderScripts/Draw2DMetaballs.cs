using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.ImageEffects
{
    [RequireComponent(typeof(Camera))]
    public class Draw2DMetaballs : PostEffectsBase
    {
        public float radius;

        public float testCenterX;
        public float testCenterY;
        public float test2ndX;
        public float test2ndY;
        public Texture2D positionsTexture;
        // resources
        public Shader shader;
        public Color waterColor;
        public List<Vector3> positions;
        private Material metaballMaterial = null;
        private Camera _camera;
        
        new void Start()
        {
            CheckResources();
            testCenterX = Screen.width / 2;
            testCenterY = Screen.height / 2;
            positions = new List<Vector3>();

            if (_camera == null)
                _camera = GetComponent<Camera>();
        }

        void OnEnable()
        {

            if (_camera == null)
                _camera = GetComponent<Camera>();
        }

        void OnDisable()
        {
            
            if (null != metaballMaterial)
            {
                DestroyImmediate(metaballMaterial);
                metaballMaterial = null;
            }
        }
        void CreatePositionsTexture()
        {
           
            //Color[] positionsAsColors = new Color[positions.Count];
            //for(int i = 0)
             
            for (int i = 0; i < positions.Count; i++)
            {
                Vector3 pos = _camera.WorldToScreenPoint(positions[i]);
               
                Color positionAsColor = new Color((pos.x / (Screen.width * 1.17f) ), pos.y / (Screen.height * 1.17f), pos.z);
              //  Debug.Log(positionAsColor);
                positionsTexture.SetPixel(i + 1, 1, positionAsColor);
            }
           // positionsTexture.m
            positionsTexture.Apply();
            Color c = positionsTexture.GetPixelBilinear(1f / 100f, 0.5f);
            Debug.Log(c.r * 1000 + " " + c.g * 1000);
        }

        void SetTestPositions()
        {
            positions.Add(_camera.ScreenToWorldPoint(new Vector3(testCenterX, testCenterY, 0)));
            positions.Add(_camera.ScreenToWorldPoint(new Vector3(test2ndX, test2ndY, 0)));
        }
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            positions.Clear();
                        
            test2ndX = Input.mousePosition.x;
            test2ndY = Input.mousePosition.y;
            testCenterX = Screen.width / 2;
            testCenterY = Screen.height / 2;
            SetTestPositions();
            CreatePositionsTexture();
            metaballMaterial.SetColor("_Color", waterColor);
            metaballMaterial.SetFloat("_Radius", radius);
            metaballMaterial.SetFloat("_TestX0", testCenterX);
            metaballMaterial.SetFloat("_TestY0", testCenterY);
            metaballMaterial.SetFloat("_TestX1", test2ndX);
            metaballMaterial.SetFloat("_TestY1", test2ndY);

            metaballMaterial.SetTexture("_PositionsTex", positionsTexture);
            metaballMaterial.SetInt("_width", positionsTexture.width);
            metaballMaterial.SetInt("_ScreenHeight", Screen.height);
            metaballMaterial.SetInt("_ScreenWidth", Screen.width);


            Graphics.Blit(source, destination, metaballMaterial);
        }
        public override bool CheckResources()
        {
            CheckSupport(true, true); // depth & hdr needed
            metaballMaterial = CheckShaderAndCreateMaterial(shader, metaballMaterial);

            return isSupported;
        }


    }
}
