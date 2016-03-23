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
        public float testX;
        public Texture2D positionsTexture;
        // resources
        public Shader shader;
        public Color waterColor;
        public List<Vector3> positions;
        private Material metaballMaterial = null;
        private Camera _camera;
        private Vector2 ratio;
        
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
        void CreatePositionsTexture() // position in uv coordinates
        {           
            for (int i = 0; i < positions.Count; i++)
            {
                Vector3 pos = WorldTOUV(positions[i]);                
                Color positionAsColor = new Color(pos.x, pos.y, pos.z);
                positionsTexture.SetPixel(i, 1, positionAsColor);
            }
            positionsTexture.Apply();
        }

        Vector2 WorldTOUV(Vector3 worldP)
        {
            
            float x = (worldP.x + ratio.x) / (2 * ratio.x);
            float y = ((worldP.y + ratio.y) / (2 * ratio.y)) + .1f - 0.017f;
            
            return new Vector2(x, y);
        }

        void SetTestPositions()
        {
            test2ndX = Input.mousePosition.x;
            test2ndY = Input.mousePosition.y;
            testCenterX = Screen.width / 2;
            testCenterY = Screen.height / 2;

            positions.Add(_camera.ScreenToWorldPoint(new Vector3(testCenterX, testCenterY, 0)));
            positions.Add(_camera.ScreenToWorldPoint(new Vector3(test2ndX, test2ndY, 0)));
        }
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            positions.Clear();
            float xRatio = _camera.orthographicSize * _camera.aspect;
            float yRatio = _camera.orthographicSize;
            ratio = new Vector2(xRatio, yRatio);

            SetTestPositions();
            CreatePositionsTexture();
            //Debug.Log(WorldTOUV(_camera.ScreenToWorldPoint(new Vector3(test2ndX, test2ndY, 0))).y);
            metaballMaterial.SetColor("_Color", waterColor);
            metaballMaterial.SetFloat("_Radius", radius);
            metaballMaterial.SetTexture("_PositionsTex", positionsTexture);
            metaballMaterial.SetInt("_width",  positions.Count);
            //Debug.Log(testX);
            metaballMaterial.SetFloat("_TestX", testX);
            metaballMaterial.SetFloat("_XScale", ratio.x);
            metaballMaterial.SetFloat("_YScale", ratio.y);
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
