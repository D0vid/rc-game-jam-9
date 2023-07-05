using UnityEngine;

namespace StateManagement
{
    public class PlayerTurnState : State
    {
        public override void Enter()
        {
            base.Enter();
            Debug.Log("> Now in PlayerTurnState");
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}