using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LumiaSC
{
    [Serializable]
    public class LevelData
    {
        [Space(10)]
        public int attackLv;
        public int[] attackValues;
        [Space(10)]
        public int shotLv;
        public float[] shotValues;
        [Space(10)]
        public int atkSpeedLv;
        public float[] atkSpeedValues;
        [Space(10)]
        public int swordSizeLv;
        public float[] swordSizeValues;
        [Space(10)]
        public int warpLv;
        public int[] warpValues;
    }
}
