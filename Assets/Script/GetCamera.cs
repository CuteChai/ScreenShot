using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

//获取屏幕截图 和 使用PC摄像机拍照
public class GetCamera : MonoBehaviour
{
    private string filename;      //要截取的图片名
    private Vector4 vector4;       //截图范围
                                   //截图
    public void ScreenShot()
    {
        GetSize();
        StartCoroutine(getTexture());
    }

    /// <summary>
    /// 计算截图范围
    /// </summary>
    private void GetSize()
    {
        if ((ScreenshotToolsCrl.instance.list[0].p1.y - ScreenshotToolsCrl.instance.list[0].p4.y) > 0)
        {
            vector4 = new Vector4(ScreenshotToolsCrl.instance.list[0].p2.x, ScreenshotToolsCrl.instance.list[0].p2.y,
                ScreenshotToolsCrl.instance.list[0].p4.x - ScreenshotToolsCrl.instance.list[0].p2.x, ScreenshotToolsCrl.instance.list[0].p1.y - ScreenshotToolsCrl.instance.list[0].p4.y);
        }
        else
        {
            vector4 = new Vector4(ScreenshotToolsCrl.instance.list[0].p3.x, ScreenshotToolsCrl.instance.list[0].p3.y,
                ScreenshotToolsCrl.instance.list[0].p1.x - ScreenshotToolsCrl.instance.list[0].p3.x, ScreenshotToolsCrl.instance.list[0].p4.y - ScreenshotToolsCrl.instance.list[0].p1.y);
        }
    }

    /// <summary>
    /// 获取屏幕截图
    /// </summary>
    /// <returns></returns>
    public IEnumerator getTexture()
    {
        //等待当前帧结束
        yield return new WaitForEndOfFrame();
        //创建一个Texture2D，用来保存截图，并设置图片的宽高
        Texture2D screenShot = new Texture2D(Mathf.Abs((int)vector4.z), Mathf.Abs((int)vector4.w), TextureFormat.RGB24, false);
        //读取当前屏幕像素
        screenShot.ReadPixels(new Rect(Mathf.Abs(vector4.x), Mathf.Abs(vector4.y), Mathf.Abs(vector4.z), Mathf.Abs(vector4.w)), 0, 0, true);
        yield return screenShot;

        //储存为字节
        byte[] bytes = screenShot.EncodeToJPG();
        screenShot.Compress(false);
        //获取系统时间  
        System.DateTime now = new System.DateTime();
        now = System.DateTime.Now;
        filename = string.Format("IMG_{0}{1}{2}_{3}{4}{5}.jpg", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        Debug.Log(filename);
        //储存文件到指定路径下
        File.WriteAllBytes(Application.dataPath + "/Photos/ " + filename, bytes);
        Debug.Log("截图完成,已保存至" + Application.dataPath + "/Photos");
        ScreenshotToolsCrl.instance.EndScreenshot();
         StopCoroutine("getTexture");
    }
}
