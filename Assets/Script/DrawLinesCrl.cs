using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLinesCrl : ToolsBaseController
{
    private GameObject g;

    private Vector3[] lp;

    private Vector3[] sp;

    private Vector3 s;

    void Start()
    {
        // my_Material.color = Color.red;
        g = new GameObject("c");

        lp = new Vector3[0];

        sp = new Vector3[0];
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
            if (Input.GetMouseButton(0))
            {
                if (s != Vector3.zero)
                {
                    for (int i = 0; i < lp.Length; i += 2)
                    {
                        float d = Vector3.Distance(lp[i], Input.mousePosition);
                        // if (d < 1 && Random.value > 0.9f) sp = AddLine(sp, lp[i], e, true);
                    }
                    lp = AddLine(lp, s, Input.mousePosition, false);
                }
                s = Input.mousePosition;
            }
            else
            {
                s = Vector3.zero;
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
    void OnGUI()
    {

        GL.LoadOrtho();
        GL.PushMatrix();
        my_Material.SetPass(0);
        GL.LoadPixelMatrix();
        GL.MultMatrix(g.transform.transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        // GL.Color(Color.red);
        for (int i = 0; i < lp.Length; i++)
        {
            GL.Vertex3(lp[i].x, lp[i].y, 0);
        }
        GL.Color(my_Material.color);
        for (int i = 0; i < sp.Length; i++)
        {
            GL.Vertex3(sp[i].x, sp[i].y, 0);
        }
        GL.End();
        GL.PopMatrix();
    }

    public override void Clear()
    {
        g.transform.rotation = Quaternion.identity;
        lp = new Vector3[0];
        sp = new Vector3[0];
    }
}

