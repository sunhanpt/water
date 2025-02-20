using System;
using UnityEngine;

namespace Game
{
    public class WaterItem : MonoBehaviour
    {
        public Transform Root;
        public Transform Bottle;
        public int WaterId;
        
        
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private Quaternion _bottleRotation;
        private Mesh _mesh;
        private Color _color;
        private bool _bInit = false;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _meshRenderer.material.color = value;
            }
        }
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
            _bottleRotation = Bottle.rotation;
            _mesh = new Mesh();
        }
        
        private void Start()
        {
            _bInit = false;
        }

        public void Update()
        {
            //if (_bInit == false || _bottleRotation != Bottle.rotation)
            {
                _bInit = true;
                var rotation = Bottle.rotation;
                transform.localRotation = Quaternion.Inverse(rotation);
                _bottleRotation = rotation;
                rotation.ToAngleAxis(out float angle, out _);
                Bottle.localPosition = Vector3.zero;
                // 重新构建mesh
                // 上面的三个
                //if (WaterId <= 3)
                {
                    float waterCenterY = DataConf.BottleBottom +  WaterId * DataConf.WaterHeight +
                                         DataConf.WaterHeight / 2;
                    float oneDivCos = 1.0f / Mathf.Max(Mathf.Cos(angle * Mathf.Deg2Rad), 0.001f);
                    float halfWaterWidth = 0.5f * DataConf.BottleWidth * oneDivCos;
                    Vector3 center = Bottle.TransformPoint(new Vector3(0, waterCenterY, 0));
                    Vector3 topCenter = center + Bottle.up * DataConf.WaterHeight / 2 * oneDivCos;
                    Vector3 bottomCenter = center - Bottle.up * DataConf.WaterHeight / 2 * oneDivCos;
                    var verts = new Vector3[4];
                    verts[0] = bottomCenter + new Vector3(-halfWaterWidth,  - DataConf.WaterHeight / 2, 0);
                    verts[1] = bottomCenter + new Vector3(halfWaterWidth,  - DataConf.WaterHeight / 2, 0);
                    verts[2] = topCenter + new Vector3(halfWaterWidth,  DataConf.WaterHeight / 2, 0);
                    verts[3] = topCenter + new Vector3(-halfWaterWidth,  DataConf.WaterHeight / 2, 0);
                    _mesh.vertices = verts;
                    _mesh.triangles = new int[] {0, 1, 2, 0, 2, 3};
                    _mesh.uv = new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(1, 0),
                        new Vector2(1, 1),
                        new Vector2(0, 1)
                    };
                    _mesh.UploadMeshData(false);
                    _meshFilter.mesh = _mesh;
                }
                // TODO: 要处理只有一个水块的时候，它的导出过程不是一个四边形，会变形成三角形。
            }
        }
    }
}