using System.Collections.Generic;
using Battlers;
using Utils;
using System.Linq;

namespace StateManagement
{
    public class TurnOrderResolver
    {
        public PriorityQueue<BattlerInstance, int> ResolveTurnOrder(List<BattlerInstance> battlers)
        {
            var queue = new PriorityQueue<BattlerInstance, int>();
            
            foreach (var battler in battlers)
            {
                queue.Enqueue(battler, battler.Battler.Speed);
            }

            return queue;
        }
    }
}