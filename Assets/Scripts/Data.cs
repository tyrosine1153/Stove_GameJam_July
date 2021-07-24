using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public enum ICE { NONE, WATER, STRAWBERRY_MILK, WHITE_MILK, CHOCO_MILK, MINTCHOCO_MILK, SEAWATER }
    public enum SYRUP { NONE, RED, YELLOW, BLUE, WHITE, ORANGE, PINK, PURPLE, SKYBLUE, GREEN, LEMON }
    public enum TOPPING { NONE, CHOCOLATE, STRAWBERRY, LEMON, REDBEAN, FRUIT_COCK, ALMOND }

    public class Stage
    {
        public StageJson[] stage;
    }
    [System.Serializable]
    public class StageJson
    {
        public int Day;
        public int mermaid_one;
        public int mermaid_two;
        public int mermaid_three;
        public int mermaid_four;
        public int mermaid_five;
        public int ice_one;
    }
}