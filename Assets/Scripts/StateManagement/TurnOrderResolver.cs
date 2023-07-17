using System.Collections.Generic;
using Battlers;
using UnityEngine;
using Utils;
using Utils.Channels;

namespace StateManagement
{
    public class TurnOrderResolver
    {
        private readonly BattleChannel _battleChannel;
        
        public TurnOrderResolver() => _battleChannel = Resources.Load<BattleChannel>("Channels/BattleChannel");

        public void ResolveTurnOrder(List<BattlerInstance> party, List<BattlerInstance> enemies)
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

            _battleChannel.RaiseTurnOrderResolved(finalQueue);
        }

        private Queue<BattlerInstance> SortByDescendingSpeed(List<BattlerInstance> battlers)
        {
            var speedComparer = new SpeedComparer();
            battlers.Sort(speedComparer);
            return new Queue<BattlerInstance>(battlers);
        }
    }
}