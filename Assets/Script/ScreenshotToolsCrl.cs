using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//工具类型
public enum ScreenShotToolType
{
    DrawLine,
    Drawcircle,
    Drawframe,
    Drawarrow,
    Null,
}

/// 框的四个点
public struct FramePoint
{
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;
    public Vector3 p4;
}

public class ScreenshotToolsCrl : MonoBehaviour
{
    public static ScreenshotToolsCrl instance = null;
    public Material Frame_Material;                         //线的材质球
    public FramePoint framePoint;                           //当前正在画的框
    public List<FramePoint> list = new List<FramePoint>();  //以确定的框
    [Header("线框粗细"), Range(1, 5)]
    public int FrameLineWidth = 2;                               //画框线宽
    public bool IsOnScreenshot = false;
    public RectTransform MainCanvas;
    public GameObject ScreenshotToolbar;
    [SerializeField] ToolsBaseController[] toolsBaseControllers;
    [SerializeField] Toggle[] ScreenshotTools;
    [SerializeField] Toggle ScreenshotSwitch;
    Vector3 limitPoint_0;
    Vector3 limitPoint_1;
    Camera sceneCamera;
    bool BeginDraw = true;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        sceneCamera = Camera.main;
        transform.GetComponent<ScreenshotToolsCrl>().enabled = false;
        ScreenshotSwitch.onValueChanged.AddListener((bool value) => { transform.GetComponent<ScreenshotToolsCrl>().enabled = value; if (!value) EndScreenshot(); });
    }
    private void OnEnable()
    {
        BeginDraw = true;
    }

    private void Update()
    {
        if (BeginDraw)
        {
            DrawScreenShotFrames();
        }
    }

    //画截屏区域框
    void DrawScreenShotFrames()
    {
        Vector2 vector = new Vector2(Input.mousePosition.x / ((float)Screen.width), Input.mousePosition.y / ((float)Screen.height));
        if (IsOnScreenshot == false)
        {
            //记录第一个点
            if (Input.GetMouseButtonDown(0))
            {
                if (!sceneCamera.rect.Contains(vector)) return;
                framePoint.p1 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
            //获取计算其它三个点
            if (Input.GetMouseButton(0))
            {
                if (!sceneCamera.rect.Contains(vector)) return;
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
                ScreenshotToolbar.SetActive(true);
                ScreenshotToolbar.transform.position = new Vector3(Input.mousePosition.x - 180, Input.mousePosition.y - 30, 0);
                IsOnScreenshot = true;
                limitPoint_0 = list[list.Count - 1].p1;
                limitPoint_1 = list[list.Count - 1].p4;
            }
        }
        //  Debug.Log(ScreenshotToolbar.transform.localPosition);
    }

    public bool LimitDrawArea()
    {
        if (limitPoint_0.y < limitPoint_1.y)
        {
            if (Input.mousePosition.x < limitPoint_0.x || Input.mousePosition.x > limitPoint_1.x || Input.mousePosition.y < limitPoint_0.y || Input.mousePosition.y > limitPoint_1.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (Input.mousePosition.x < limitPoint_0.x || Input.mousePosition.x > limitPoint_1.x || Input.mousePosition.y > limitPoint_0.y || Input.mousePosition.y < limitPoint_1.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public bool LimitDrawArea(Vector3 point)
    {
        if (limitPoint_0.y < limitPoint_1.y)
        {
            if (point.x < limitPoint_0.x || point.x > limitPoint_1.x || point.y < limitPoint_0.y || point.y > limitPoint_1.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (point.x < limitPoint_0.x || point.x > limitPoint_1.x || point.y > limitPoint_0.y || point.y < limitPoint_1.y)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    void OnGUI()
    {
        GLDrawFrame();
    }

    void GLDrawFrame()
    {
        if ((list != null) || (framePoint.p1 != null))
        {
            GL.LoadOrtho();
            GL.PushMatrix();
            Frame_Material.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Begin(GL.LINES);
            //画当前正在点击的框
            if (framePoint.p1 != null)
            {
                for (int i = 0; i < FrameLineWidth; i++)
                {
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

                    for (int k = 0; k < FrameLineWidth; k++)
                    {
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

    public void EndScreenshot()
    {
        list = new List<FramePoint>(); //清空确定的点
        for (int i = 0; i < toolsBaseControllers.Length; i++)
        {
            toolsBaseControllers[i].Clear();
        }
         for (int i = 0; i < ScreenshotTools.Length; i++)
        {
            ScreenshotTools[i].isOn=false;
        }
        ScreenshotToolbar.SetActive(false);
        ScreenshotSwitch.isOn = false;
        IsOnScreenshot = false;
    }

    public void LineToggel(bool ison)
    {
        toolsBaseControllers[0].BegainDraw = ison;
    }

    public void ArrowToggel(bool ison)
    {
        toolsBaseControllers[1].BegainDraw = ison;
    }
    public void FrameToggel(bool ison)
    {
        toolsBaseControllers[2].BegainDraw = ison;
    }
    public void CircleToggel(bool ison)
    {
        toolsBaseControllers[3].BegainDraw = ison;
    }

    public void ScreenshotOnEnterTrigger()
    {
        BeginDraw = false;
    }

    public void ScreenshotOnExitTrigger()
    {
        BeginDraw = true;
    }

}
