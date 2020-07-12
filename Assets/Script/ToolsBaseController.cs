using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ToolsBaseController : MonoBehaviour
{
    public Material my_Material;
    public int My_LineWidth;
    public bool BegainDraw = false;
    protected abstract void CollectingPoints();
    public abstract void Clear();

}


