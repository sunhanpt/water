using System;
using UnityEngine;

namespace Game
{
    public class WaterItem : MonoBehaviour
    {
        public Transform Bottle;
        public int WaterId;
        public float waterSurfaceLevel { private set; get; }

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private Quaternion bottleRotation;
        private Mesh mesh;
        private Color color;
        private bool bInit = false;
        private float waterOutDuration = 0.0f;
        private float waterInDuration = 0.0f;
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                meshRenderer.material.color = value;
            }
        }
        
        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            bottleRotation = Bottle.rotation;
            mesh = new Mesh();
        }
        
        private void Start()
        {
            bInit = false;
        }

        public void Update()
        {
            //if (_bInit == false || _bottleRotation != Bottle.rotation)
            {
                bInit = true;
                var rotation = Bottle.rotation;
                transform.localRotation = Quaternion.Inverse(rotation);
                bottleRotation = rotation;
                rotation.ToAngleAxis(out float angle, out _);
                Bottle.localPosition = Vector3.zero;
                // 重新构建mesh
                // 上面的三个
                //if (WaterId <= 3)
                {
                    
                    float waterMeshHeight = DataConf.WaterMeshHeight;
                    if (waterOutDuration > 0)
                    {
                        waterMeshHeight = DataConf.WaterMeshHeight * waterOutDuration;
                    }
                    else if (waterInDuration > 0)
                    {
                        waterMeshHeight = DataConf.WaterMeshHeight * (1 - waterInDuration);
                    }
                    float waterCenterY = DataConf.BottleBottom +  WaterId * DataConf.WaterHeight +
                                         waterMeshHeight / 2;
                    
                    float oneDivCos = 1.0f / Mathf.Max(Mathf.Cos(angle * Mathf.Deg2Rad), 0.001f);
                    float halfWaterWidth = 0.5f * DataConf.BottleWidth * oneDivCos;
                    Vector3 center = Bottle.TransformPoint(new Vector3(0, waterCenterY, 0));
                    Vector3 topCenter = center + Bottle.up * waterMeshHeight / 2 * oneDivCos;
                    Vector3 bottomCenter = center - Bottle.up * waterMeshHeight / 2 * oneDivCos;
                    var verts = new Vector3[4];
                    verts[0] = bottomCenter + new Vector3(-halfWaterWidth,  0, 0);
                    verts[1] = bottomCenter + new Vector3(halfWaterWidth,  0, 0);
                    verts[2] = topCenter + new Vector3(halfWaterWidth,  0, 0);
                    verts[3] = topCenter + new Vector3(-halfWaterWidth,  0, 0);
                    verts[1].x = verts[2].x;
                    //verts[3].x = verts[0].x;
                    if (WaterId == 0)
                    {
                        Vector3 BottleBottomPos = Bottle.TransformPoint(DataConf.BottleWidth / 2, DataConf.BottleBottom,0);
                        verts[0].y = BottleBottomPos.y;
                        verts[1].y = BottleBottomPos.y;
                    }
                    mesh.vertices = verts;
                    mesh.triangles = new int[] {0, 1, 2, 0, 2, 3};
                    mesh.uv = new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(1, 0),
                        new Vector2(1, 1),
                        new Vector2(0, 1)
                    };
                    mesh.UploadMeshData(false);
                    meshFilter.mesh = mesh;
                    waterSurfaceLevel = waterCenterY + waterMeshHeight / 2;
                }
                // TODO: 要处理只有一个水块的时候，它的导出过程不是一个四边形，会变形成三角形。
            }

            if (waterOutDuration > 0 && waterOutDuration - Time.deltaTime <= 0)
            {
                this.gameObject.SetActive(false);
            }
            waterOutDuration -= Time.deltaTime;
            waterOutDuration = Mathf.Max(waterOutDuration, 0.0f);
            
            waterInDuration -= Time.deltaTime;
            waterInDuration = Mathf.Max(waterInDuration, 0.0f);
            
        }

        public void WaterOut(float duration)
        {
            waterOutDuration = duration;
        }

        public void WaterIn(float duration)
        {
            waterInDuration = duration;
        }
    }
}