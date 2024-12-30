using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class StageInfoManager : MonoBehaviour
{
    public static List<int> StageInfo = new List<int>();
    public static List<List<int>> WaveInfo = new List<List<int>>();

    public static void StageInfoSet()
    {
        //  Stage 당 Wave 수 정보
        StageInfo.Add(5);
        StageInfo.Add(5);
        StageInfo.Add(5);

        //  Wave 당 총 몬스터 수 정보
        WaveInfo.Add(new List<int> { 100, 100, 100, 100, 100 });
        WaveInfo.Add(new List<int> { 100, 100, 100, 100, 100 });
        WaveInfo.Add(new List<int> { 100, 100, 100, 100, 100 });
    }
}
