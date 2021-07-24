using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public enum ICE { NONE, WATER, STRAWBERRY_MILK, WHITE_MILK, CHOCO_MILK, MINTCHOCO_MILK, SEAWATER }
    public enum SYRUP { NONE, RED, YELLOW, BLUE, WHITE, ORANGE, PINK, PURPLE, SKYBLUE }
    public enum TOPPING { NONE, CHOCOLATE, STRAWBERRY, LEMON, REDBEAN, FRUIT_COCK, ALMOND }

    public class Stage
    {
        public StageJson[] stage;
    }
    [System.Serializable]
    public class StageJson
    {
        public int IceCount;
        public int mermaidCount;
    }
}