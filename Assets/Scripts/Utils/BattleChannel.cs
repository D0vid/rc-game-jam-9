﻿using System;
using System.Collections.Generic;
using Battlers;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Channels/BattleChannel")]
    public class BattleChannel : ScriptableObject
    {
        public event UnityAction<Queue<BattlerInstance>> turnOrderResolvedEvent;
        public event UnityAction<BattlerInstance> currentBattlerChangedEvent;

        public void RaiseTurnOrderResolved(Queue<BattlerInstance> battlersQueue)
        {
            turnOrderResolvedEvent?.Invoke(battlersQueue);
        }

        public void RaiseCurrentBattlerChanged(BattlerInstance battler)
        {
            currentBattlerChangedEvent?.Invoke(battler);
        }
    }
}