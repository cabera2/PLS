using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lumia
{
    public partial class LumiaSC
    {
        [Header("Prefabs")][SerializeField] private SpriteRenderer targetMarkPrefab;
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
}
