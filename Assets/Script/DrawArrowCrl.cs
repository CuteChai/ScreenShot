using System.Collections.Generic;
using UnityEngine;

public struct trianglePos
{
    public Vector3 tr1;
    public Vector3 tr2;
    public Vector3 tr3;
    public Vector3 vector;
}
public class DrawArrowCrl : ToolsBaseController
{
    private GameObject g;                                   //便于查看当前点的坐标信息
    public Vector3[] lp;                                    //存储的点
    private Vector3 s;                                      //开始点
    public GameObject Arrow;                                //画箭头头部预制体
    private List<GameObject> ArrowObject = new List<GameObject>();
    private GameObject go;                                  //参考物体
    private Vector3 Vxyz;
    public trianglePos trianglePos;
    public List<trianglePos> list = new List<trianglePos>();
    void Start()
    {
        my_Material.color = Color.red;
        My_LineWidth = 15;
        g = new GameObject("g");
        Vxyz = new Vector3(2, 0, 0);//平行于X轴向量
        trianglePos = new trianglePos();
    }
    void Update()
    {
        CollectingPoints();
    }

    protected override void CollectingPoints()
    {
        if (BegainDraw)
        {
            if (ScreenshotToolsCrl.instance.LimitDrawArea() == false) return;
            Vector3 e;//结束点
            if (Input.GetMouseButtonDown(0))
            {
                trianglePos.tr1 = Input.mousePosition;
                s = GetNewPoint();
                //显示的z轴看自己需求赋值，只要在相机前面就行

                Vector2 SpawnPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenshotToolsCrl.instance.MainCanvas.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out SpawnPos);
                go = Instantiate(Arrow) as GameObject;
                go.transform.SetParent(ScreenshotToolsCrl.instance.ScreenShotTools.transform);
                go.transform.localPosition = new Vector3(SpawnPos.x, SpawnPos.y, 0);
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                ArrowObject.Add(go);
            }
            //拖拽便于确定箭头方向
            if (Input.GetMouseButton(0))
            {
                trianglePos.tr2 = Camera.main.WorldToScreenPoint(go.transform.GetChild(0).position);
                trianglePos.tr3 = Camera.main.WorldToScreenPoint(go.transform.GetChild(1).position);
                Debug.Log(trianglePos.tr2 + "====" + trianglePos.tr3);
                trianglePos.vector = trianglePos.tr3 - trianglePos.tr2;
                e = GetNewPoint();
                lp = AddLine(lp, s, e, true);
                Vector2 SpawnPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(ScreenshotToolsCrl.instance.MainCanvas.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out SpawnPos);
                go.transform.localPosition = new Vector3(SpawnPos.x, SpawnPos.y, 0);
                Vector3 Vangle = lp[lp.Length - 1] - lp[lp.Length - 2];//最后一条线段
                float angle = Vector3.Angle(Vangle, Vxyz) - 90;//三角箭头绕着Z轴旋转 那么就限制与X轴夹角为旋转角度吧
                if (Vangle.y > 0)
                {
                    go.transform.eulerAngles = new Vector3(0, 0, angle);
                }
                else
                {
                    go.transform.eulerAngles = new Vector3(0, 0, 180 - angle);
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                if (list == null)
                {
                    list = new List<trianglePos>();
                }
                list.Add(trianglePos);
                trianglePos = new trianglePos();
            }
        }
    }
    Vector3[] AddLine(Vector3[] l, Vector3 s, Vector3 e, bool tmp)
    {
        int vl = l.Length;
        if (!tmp || vl == 0) l = resizeVertices(l, 2);
        else vl -= 2;

        l[vl] = s;
        l[vl + 1] = e;
        return l;
    }

    Vector3[] resizeVertices(Vector3[] ovs, int ns)
    {
        Vector3[] nvs = new Vector3[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

    Vector3 GetNewPoint()
    {
        return g.transform.InverseTransformPoint(
            Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1.0f)
            )
        );
    }

    private void OnGUI()
    {
        if (list != null || trianglePos.tr1 != null)
        {
            GL.LoadOrtho();
            GL.PushMatrix();
            my_Material.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Begin(GL.LINES);
            for (int i = 0; i < My_LineWidth; i++)
            {
                GL.Vertex3(trianglePos.tr1.x, trianglePos.tr1.y, 0);
                GL.Vertex3(trianglePos.tr2.x + trianglePos.vector.normalized.x * i, trianglePos.tr2.y + trianglePos.vector.normalized.y * i, 0);

                GL.Vertex3(trianglePos.tr2.x + trianglePos.vector.normalized.x * i, trianglePos.tr2.y + +trianglePos.vector.normalized.y * i, 0);
                GL.Vertex3(trianglePos.tr3.x - trianglePos.vector.normalized.x * i, trianglePos.tr3.y - trianglePos.vector.normalized.y * i, 0);

                GL.Vertex3(trianglePos.tr3.x - trianglePos.vector.normalized.x * i, trianglePos.tr3.y - trianglePos.vector.normalized.y * i, 0);
                GL.Vertex3(trianglePos.tr1.x, trianglePos.tr1.y, 0);
            }

            for (int j = 0; j < list.Count; j++)
            {
                for (int i = 0; i < My_LineWidth; i++)
                {
                    GL.Vertex3(list[j].tr1.x, list[j].tr1.y, 0);
                    GL.Vertex3(list[j].tr2.x + list[j].vector.normalized.x * i, list[j].tr2.y + list[j].vector.normalized.y * i, 0);

                    GL.Vertex3(list[j].tr2.x + list[j].vector.normalized.x * i, list[j].tr2.y + list[j].vector.normalized.y * i, 0);
                    GL.Vertex3(list[j].tr3.x - list[j].vector.normalized.x * i, list[j].tr3.y - list[j].vector.normalized.y * i, 0);

                    GL.Vertex3(list[j].tr3.x - list[j].vector.normalized.x * i, list[j].tr3.y - list[j].vector.normalized.y * i, 0);
                    GL.Vertex3(list[j].tr1.x, list[j].tr1.y, 0);
                }
            }
            GL.End();
            GL.PopMatrix();
        }

    }


    public override void Clear()
    {
        list.Clear();
        foreach (var item in ArrowObject)
        {
            Destroy(item);
        }
    }
}


