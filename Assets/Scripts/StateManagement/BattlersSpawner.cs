using Battlers;
using System.Collections.Generic;
using UnityEngine;

namespace StateManagement
{
    public class BattlersSpawner
    {
        public List<BattlerInstance> SpawnBattlers(List<Battler> battlersData, List<Vector2> battlersPlacements, Team team)
        {
            List<BattlerInstance> battlerInstances = new List<BattlerInstance>();
            for (var i = 0; i < battlersData.Count; i++)
            {
                Vector2 position = battlersPlacements[i];
                BattlerInstance battlerInstance = SpawnBattler(battlersData[i], position);
                battlerInstance.Team = team;
                battlerInstances.Add(battlerInstance);
            }

            return battlerInstances;
        }

        private BattlerInstance SpawnBattler(Battler battler, Vector2 position)
        {
            GameObject battlerPrefab = Resources.Load<GameObject>("Prefabs/Battler");
            BattlerInstance battlerInstance = Object.Instantiate(battlerPrefab).GetComponent<BattlerInstance>();
            battlerInstance.gameObject.name = battler.Name;
            battlerInstance.battler = battler;
            SetPositionAndScale(position, battlerInstance);
            SetSprites(battler, battlerInstance);
            return battlerInstance;
        }

        private void SetSprites(Battler battler, BattlerInstance battlerInstance)
        {
            SpriteRenderer spriteRenderer = battlerInstance.GetComponent<SpriteRenderer>();
            var battlerAnimator = battlerInstance.gameObject.GetComponent<BattlerAnimator>();
            battlerAnimator.SetAnimationSprites(battler.Sprites);
            spriteRenderer.sortingOrder = 2;
            spriteRenderer.enabled = true;
        }

        private void SetPositionAndScale(Vector2 position, BattlerInstance battlerInstance)
        {
            battlerInstance.transform.position = position;
            battlerInstance.transform.localScale = new Vector3(1f, 1f, 1f);
            battlerInstance.transform.parent = BattleManager.Instance.transform;
        }
    }
}