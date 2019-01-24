using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils {

    public static T Search<T>(T value, List<T> list) where T: IComparable
    {
        if (value.CompareTo(list.First()) < 0)
        {
            return list.First();
        }
        if (value.CompareTo(list.Last()) > 0)
        {
            return list.Last();
        }

        int lo = 0;
        int hi = list.Count - 1;

        while (lo <= hi)
        {
            int mid = (hi + lo) / 2;

            if (value.CompareTo(list[mid]) < 0)
            {
                hi = mid - 1;
            }
            else if (value.CompareTo(list[mid]) > 0)
            {
                lo = mid + 1;
            }
            else
            {
                return list[mid];
            }
        }

        int lowerDelta = list[lo].CompareTo(value);
        int higherDelta = value.CompareTo(list[hi]);

        return lowerDelta < higherDelta ? list[lo] : list[hi];
    }
}
