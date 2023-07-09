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
            GameObject gameObject = new GameObject(battler.Name);
            SetSprite(battler, gameObject);
            SetPositionAndScale(position, gameObject);
            return SetAndGetBattler(gameObject, battler); // TODO separate method
        }

        private void SetSprite(Battler battler, GameObject battlerInstance)
        {
            SpriteRenderer spriteRenderer = battlerInstance.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = battler.Sprite;
            spriteRenderer.sortingOrder = 2;
        }

        private void SetPositionAndScale(Vector2 position, GameObject battlerInstance)
        {
            battlerInstance.transform.position = position;
            battlerInstance.transform.localScale = new Vector3(1f, 1f, 1f);
            battlerInstance.transform.parent = BattleManager.Instance.transform;
        }

        private BattlerInstance SetAndGetBattler(GameObject battlerObject, Battler battler)
        {
            BattlerInstance battlerInstance = battlerObject.AddComponent<BattlerInstance>();
            battlerObject.AddComponent<Animator>();
            battlerObject.AddComponent<AudioSource>();
            battlerInstance.Battler = battler;
            return battlerInstance;
        }
    }
}