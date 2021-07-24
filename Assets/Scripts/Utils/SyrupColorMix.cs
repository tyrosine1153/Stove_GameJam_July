using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SyrupColorMix
{
    // 조합 개수가 많지 않으므로 하드코딩으로 빠르게 구현한다.
    private static Dictionary<(Data.SYRUP, Data.SYRUP), Data.SYRUP> syrupMixPair = new Dictionary<(Data.SYRUP, Data.SYRUP), Data.SYRUP>()
    {
        {(Data.SYRUP.RED, Data.SYRUP.YELLOW), Data.SYRUP.ORANGE },
        {(Data.SYRUP.RED, Data.SYRUP.WHITE), Data.SYRUP.PINK },
        {(Data.SYRUP.RED, Data.SYRUP.BLUE), Data.SYRUP.PURPLE },
        {(Data.SYRUP.BLUE, Data.SYRUP.WHITE), Data.SYRUP.SKYBLUE },
    };

    public static bool TryGetMixedSyrup(Data.SYRUP syrup1, Data.SYRUP syrup2, out Data.SYRUP resultSyrup)
    {
        if (syrup1 == Data.SYRUP.NONE || syrup2 == Data.SYRUP.NONE)
        {
            resultSyrup = Data.SYRUP.NONE;
            return false;
        }

        // Swap을 이용해 두 수를 오름차순으로 정렬한다.
        if (syrup1 > syrup2)
        {
            var temp = syrup1;
            syrup1 = syrup2;
            syrup2 = temp;
        }

        if (!syrupMixPair.TryGetValue((syrup1, syrup2), out var matchSyrup))
        {
            resultSyrup = Data.SYRUP.NONE;
            return false;
        }

        resultSyrup = matchSyrup;
        return true;
    }
}
