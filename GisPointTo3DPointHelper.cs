using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GisPointTo3DPointHelper  
{
    
    /// <summary>
    /// 由经纬度得到位置点 
    /// </summary>
    /// <param name="BottomRightPoint">Unity右下角的点</param>
    /// <param name="TopLeftPoint">Unity左上角的点</param>
    /// <param name="BottomRightLatLon">右下角的点经纬度</param>
    /// <param name="TopLeftofWorld">左上角的点经纬度</param>
    /// <param name="LatLon">计算点经纬度 x 是东经  y是北纬</param>
    /// <param name="height">计算点高度</param>
    /// <returns></returns>
    public static Vector3 GetWorldPoint(Vector3 BottomRightPoint, Vector3 TopLeftPoint, Vector2 BottomRightLatLon, Vector2 TopLefLatLon, Vector2 LatLon, float height)
    {
        float z_offset = BottomRightLatLon.y - TopLefLatLon.y;//地图中的维度差  
        float x_offset = BottomRightLatLon.x - TopLefLatLon.x;//地图中的经度差  
        float z_w_offset = BottomRightPoint.z - TopLeftPoint.z;//unity中的维度差  
        float x_w_offset = BottomRightPoint.x - TopLeftPoint.x;//unity中的经度差  

        float tempX = LatLon.x - TopLefLatLon.x;
        float tempZ = LatLon.y - BottomRightLatLon.y;
        float _tempX = (tempX * x_w_offset / x_offset + TopLeftPoint.x);
        float _tempZ = (tempZ * z_w_offset / z_offset + BottomRightPoint.z);

        float distance1 = GetDistance(BottomRightLatLon, TopLefLatLon);
        float distance2 = Vector3.Distance(BottomRightPoint, TopLeftPoint);

        float h = distance2 * height / distance1;


        //坐标偏差（在Unity中的坐标）
        return new Vector3(_tempX, h, _tempZ);
    }
    /// <summary>
    /// 由位置点得到经纬度 
    /// </summary>
    /// <param name="BottomRightPoint">Unity右下角的点</param>
    /// <param name="TopLeftPoint">Unity左上角的点</param>
    /// <param name="BottomRightLatLon">右下角的点经纬度</param>
    /// <param name="TopLeftofWorld">左上角的点经纬度</param>
    /// <param name="curPoint">计算点的三维坐标</param>
    /// <returns>返回计算点的经纬度</returns>
    public static Vector3 GetLatLon(Vector3 BottomRightPoint, Vector3 TopLeftPoint, Vector2 BottomRightLatLon, Vector2 TopLefLatLon, Vector3 curPoint)
    {
        float z_offset = BottomRightLatLon.y - TopLefLatLon.y;//地图中的维度差  
        float x_offset = BottomRightLatLon.x - TopLefLatLon.x;//地图中的经度差  
        float z_w_offset = BottomRightPoint.z - TopLeftPoint.z;//unity中的维度差  
        float x_w_offset = BottomRightPoint.x - TopLeftPoint.x;//unity中的经度差  
        //坐标偏差
        float _x_offset = (curPoint.x - BottomRightPoint.x) * x_offset / x_w_offset;
        float _z_offset = (curPoint.z - TopLeftPoint.z) * z_offset / z_w_offset;
        float resultX = _x_offset + BottomRightLatLon.x;
        float resultZ = _z_offset + TopLefLatLon.y;
        return new Vector2(resultX, resultZ);
    }


    /// <summary>  
    /// 计算两点位置的距离，返回两点的距离，单位：米 
    /// 该公式为GOOGLE提供，误差小于0.2米    
    /// </summary>   
    /// <param 第一点经度="lon1">第一点经度</param>  
    /// <param 第一点纬度="lat1">第一点纬度</param>          
    /// <param 第二点经度="lon2">第二点经度</param>   
    /// <param 第二点纬度="lat2">第二点纬度</param>    
    /// <returns>返回两点的世界距离</returns>   
    public static float GetDistance(Vector2 pos1, Vector2 pos2)
    {
        float EARTH_RADIUS = 6378137.0f;
        float radLat1 = Rad(pos1.x);
        float radLon1 = Rad(pos1.y);
        float radLat2 = Rad(pos2.x);
        float radLon2 = Rad(pos2.y);
        float a = radLat1 - radLat2;
        float b = radLon1 - radLon2;
        float result = 2 * Mathf.Asin(Mathf.Sqrt(Mathf.Pow(Mathf.Sin(a / 2), 2) + Mathf.Cos(radLat1) * Mathf.Cos(radLat2) * Mathf.Pow(Mathf.Sin(b / 2), 2))) * EARTH_RADIUS;

        return result;//米
    }


    /// <summary>  
    /// 经纬度转化成弧度  
    /// </summary>  
    /// <param name="d"></param>  
    /// <returns></returns>  
    private static float Rad(float d)
    {
        return (float)(d * Mathf.PI / 180f);
    }




}
