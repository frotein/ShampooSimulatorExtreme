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
        public Transform testPosition1, testPosition2;
        //public List<Vector3> positions;
        public List<Transform> metaballs;
        private Material metaballMaterial = null;
        private Camera _camera;
        private Vector2 ratio;
        
        new void Start()
        {
            CheckResources();
            testCenterX = Screen.width / 2;
            testCenterY = Screen.height / 2;
            //positions = new List<Vector3>();
            metaballs = new List<Transform>();
            metaballs.Add(testPosition1);
            //metaballs.Add(testPosition2);
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
            Color positionAsColor = new Color(0,0,0);
            for (int i = 0; i < metaballs.Count; i++)
            {
                Vector3 pos = WorldTOUV(metaballs[i].position);
                positionAsColor = new Color(pos.x, pos.y, pos.z);
                //Debug.Log(pos.x + " " + positionAsColor.r);
                positionsTexture.SetPixel(i, 1, positionAsColor);
            }
            positionsTexture.Apply();

            Color c = positionsTexture.GetPixel(0, 1);
            Vector2 grabbed = UVToWorld(new Vector2(c.r, c.g));
           // Debug.Log(positionAsColor.r + " " + c.r);
        }

        Vector2 WorldTOUV(Vector3 worldP)
        {
            
            float x = ((worldP.x - _camera.transform.position.x) + ratio.x) / (2 * ratio.x);
            float y = (((worldP.y - _camera.transform.position.y) + ratio.y) / (2 * ratio.y));// + .1f - 0.017f;
            
            return new Vector2(x, y);
        }

        Vector2 UVToWorld(Vector2 uv)
        {
            Vector2 pt = new Vector2( (uv.x * 2.0f * ratio.x) - ratio.x, (uv.y * 2.0f * ratio.y) - ratio.y);
            return pt;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            //positions.Clear();
            float xRatio = _camera.orthographicSize * _camera.aspect;
            float yRatio = _camera.orthographicSize;
            ratio = new Vector2(xRatio, yRatio);

            CreatePositionsTexture();
            metaballMaterial.SetColor("_Color", waterColor);
            metaballMaterial.SetFloat("_Radius", radius);
            metaballMaterial.SetTexture("_PositionsTex", positionsTexture);
            metaballMaterial.SetInt("_width",  metaballs.Count);
            metaballMaterial.SetInt("_set", 0);
            //Debug.Log(testX);
            metaballMaterial.SetFloat("_XScale", ratio.x);
            metaballMaterial.SetFloat("_YScale", ratio.y);
            Graphics.Blit(source, destination, metaballMaterial);

          //  float grabbedX = metaballMaterial.GetFloat("_testX");
            
        }
        public override bool CheckResources()
        {
            CheckSupport(true, true); // depth & hdr needed
            metaballMaterial = CheckShaderAndCreateMaterial(shader, metaballMaterial);

            return isSupported;
        }


    }
}
