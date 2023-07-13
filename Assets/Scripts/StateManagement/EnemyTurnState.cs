using System.Collections;
using UnityEngine;

namespace StateManagement
{
    public class EnemyTurnState : State
    {
        private BattleManager _battleManager;
        
        public override void Enter()
        {
            base.Enter();
            _battleManager = BattleManager.Instance;
            Debug.Log($"> Now in EnemyTurnState - Battler : {_battleManager.CurrentBattler.name}");
            StartCoroutine(EndTurn());
        }

        private IEnumerator EndTurn()
        {
            yield return new WaitForSeconds(0.1f); // Hacky way to wait for the state to finish transitioning
            _battleManager.EndTurn();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}