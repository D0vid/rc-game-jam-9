using System.Collections.Generic;
using Battlers;

namespace StateManagement
{
    public class TurnOrderResolver
    {
        public Queue<BattlerInstance> ResolveTurnOrder(List<BattlerInstance> party, List<BattlerInstance> enemies)
        {
            var finalQueue = new Queue<BattlerInstance>();
            
            var sortedParty = SortByDescendingSpeed(party);
            var sortedEnemies = SortByDescendingSpeed(enemies);
            
            var doesPartyContainFastestBattler = sortedParty.Peek().battler.Speed > sortedEnemies.Peek().battler.Speed;
            var firstTeam = doesPartyContainFastestBattler ? sortedParty : sortedEnemies;
            var secondTeam = firstTeam.Equals(sortedParty) ? sortedEnemies : sortedParty;

            while (firstTeam.Count > 0)
            {
                finalQueue.Enqueue(firstTeam.Dequeue());
                if (secondTeam.Count > 0)
                    finalQueue.Enqueue(secondTeam.Dequeue());
            }
            while(secondTeam.Count > 0)
                finalQueue.Enqueue(secondTeam.Dequeue());

            return finalQueue;
        }

        private Queue<BattlerInstance> SortByDescendingSpeed(List<BattlerInstance> battlers)
        {
            var speedComparer = new SpeedComparer();
            battlers.Sort(speedComparer);
            return new Queue<BattlerInstance>(battlers);
        }
    }
}