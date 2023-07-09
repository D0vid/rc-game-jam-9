using System.Collections.Generic;

namespace Battlers
{
    public class SpeedComparer : IComparer<BattlerInstance>
    {
        public int Compare(BattlerInstance x, BattlerInstance y)
        {
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            return -x.Battler.Speed.CompareTo(y.Battler.Speed);
        }
    }
}