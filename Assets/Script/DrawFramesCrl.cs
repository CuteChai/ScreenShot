using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawFramesCrl : ToolsBaseController
{
    public FramePoint framePoint;
    public List<FramePoint> list = new List<FramePoint>();
    void Start()
    {
        // my_Material.color = Color.red;
        My_LineWidth = 1;
        framePoint = new FramePoint();
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
            //记录第一个点
            if (Input.GetMouseButtonDown(0))
            {
                framePoint.p1 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
            //获取计算其它三个点
            if (Input.GetMouseButton(0))
            {
                if (ScreenshotToolsCrl.instance.LimitDrawArea() == false) return;
                framePoint.p4 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                framePoint.p2 = new Vector3(framePoint.p1.x, framePoint.p4.y, 0);
                framePoint.p3 = new Vector3(framePoint.p4.x, framePoint.p1.y, 0);
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (list == null)
                {
                    list = new List<FramePoint>();
                }
                list.Add(framePoint);
                framePoint = new FramePoint();
            }
        }
    }

    void OnGUI()
    {
        if ((list != null) || (framePoint.p1 != null))
        {
            GL.LoadOrtho();
            GL.PushMatrix();
            my_Material.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Begin(GL.LINES);
            // GL.Color(Color.red);
            //画当前正在点击的框
            if (framePoint.p1 != null)
            {
                for (int i = 0; i < My_LineWidth; i++)
                {
                    // Debug.Log(framePoint.p1 + "--" + framePoint.p4);
                    GL.Vertex3(framePoint.p1.x + i, framePoint.p1.y - i, 0);
                    GL.Vertex3(framePoint.p2.x + i, framePoint.p2.y + i, 0);

                    GL.Vertex3(framePoint.p1.x + i, framePoint.p1.y - i, 0);
                    GL.Vertex3(framePoint.p3.x - i, framePoint.p3.y - i, 0);

                    GL.Vertex3(framePoint.p2.x + i, framePoint.p2.y + i, 0);
                    GL.Vertex3(framePoint.p4.x - i, framePoint.p4.y + i, 0);

                    GL.Vertex3(framePoint.p3.x - i, framePoint.p3.y - i, 0);
                    GL.Vertex3(framePoint.p4.x - i, framePoint.p4.y + i, 0);
                }
            }

            //画已经确定的框
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    for (int k = 0; k < My_LineWidth; k++)
                    {
                        Debug.Log(framePoint.p1 + "--" + framePoint.p4);
                        GL.Vertex3(list[i].p1.x + k, list[i].p1.y - k, 0);
                        GL.Vertex3(list[i].p2.x + k, list[i].p2.y + k, 0);

                        GL.Vertex3(list[i].p1.x + k, list[i].p1.y - k, 0);
                        GL.Vertex3(list[i].p3.x - k, list[i].p3.y - k, 0);

                        GL.Vertex3(list[i].p2.x + k, list[i].p2.y + k, 0);
                        GL.Vertex3(list[i].p4.x - k, list[i].p4.y + k, 0);

                        GL.Vertex3(list[i].p3.x - k, list[i].p3.y - k, 0);
                        GL.Vertex3(list[i].p4.x - k, list[i].p4.y + k, 0);
                    }
                }
            }
            GL.End();
            GL.PopMatrix();
        }
    }

    public override void Clear()
    {
        list.Clear();
    }
}


