using System.Collections.Generic;
using UnityEngine;
public class DrawCircleCrl : ToolsBaseController
{
    private bool _Draw;
    private Vector3 _StartPos;
    private Vector3 _OverPos;
    public List<Vector3> _Points;
    public List<List<Vector3>> circelPoints = new List<List<Vector3>>();
    private float angle, w, h;
    void Start()
    {
        // my_Material.color = Color.red;
        My_LineWidth = 10;
        _Points = new List<Vector3>();

    }
    void Update()
    {
        CollectingPoints();
    }
    bool Limit = false;
    protected override void CollectingPoints()
    {
        if (BegainDraw)
        {
            if (ScreenshotToolsCrl.instance.LimitDrawArea() == false) return;
            if (Input.GetMouseButtonDown(0))
            {
                _StartPos = Input.mousePosition;
                Limit = false;
            }
            if (Limit == false)
            {
                if (Input.GetMouseButton(0))
                {
                    _OverPos = Input.mousePosition;
                    w = Mathf.Abs(_StartPos.x - _OverPos.x);
                    h = Mathf.Abs(_StartPos.y - _OverPos.y);
                    //j控制线的粗细
                    for (int j = 0; j < My_LineWidth; j++)
                    {
                        w += j / 4; h += j / 4;
                        _Points = new List<Vector3>();
                        for (float i = 0; i <= 360; i++)
                        {
                            //Mathf里面应该可以控制线的锯齿，目前不知道怎么调整
                            //_StartPos,圆心位置
                            float x = w * Mathf.Cos(i * Mathf.Deg2Rad) + _StartPos.x;
                            float y = h * Mathf.Sin(i * Mathf.Deg2Rad) + _StartPos.y;

                            _Points.Add(new Vector3(x, y, 0));
                            if (ScreenshotToolsCrl.instance.LimitDrawArea(new Vector3(x, y, 0)) == false)
                            {
                                Limit = true;
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                circelPoints.Add(_Points);
                _Points = new List<Vector3>();
                Limit = false;
            }
        }
    }

    private void OnGUI()
    {
        if (_Points != null)
        {
            GL.PushMatrix();
            my_Material.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Begin(GL.LINES);
            for (int i = 0; i < _Points.Count - 1; i++)
            {
                GL.Vertex(_Points[i]);
                GL.Vertex(_Points[i + 1]);
            }
        }
        if (circelPoints != null)
        {
            for (int i = 0; i < circelPoints.Count; i++)
            {
                for (int j = 0; j < circelPoints[i].Count - 1; j++)
                {
                    GL.Vertex(circelPoints[i][j]);
                    GL.Vertex(circelPoints[i][j + 1]);
                }
            }
        }
        GL.End();
        GL.PopMatrix();

    }
    public override void Clear()
    {
        foreach (var item in circelPoints)
        {
            item.Clear();
        }
    }
}

