using Battlers;
using System.Collections.Generic;
using UnityEngine;

namespace StateManagement
{
    public class BattlersSpawner
    {
        public List<BattlerInstance> SpawnBattlers(List<Battler> battlersData, List<Vector2> battlersPlacements)
        {
            List<BattlerInstance> battlersObjects = new List<BattlerInstance>();
            for (var i = 0; i < battlersData.Count; i++)
            {
                Vector2 position = battlersPlacements[i];
                BattlerInstance battler = SpawnBattler(battlersData[i], position);
                battlersObjects.Add(battler);
            }

            return battlersObjects;
        }

        private BattlerInstance SpawnBattler(Battler battler, Vector2 position)
        {
            GameObject battlerInstance = new GameObject(battler.Name);
            SetSprite(battler, battlerInstance);
            SetPositionAndScale(position, battlerInstance);
            return SetAndGetBattler(battlerInstance); // TODO separate method
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

        private BattlerInstance SetAndGetBattler(GameObject battlerObject)
        {
            BattlerInstance battlerInstance = battlerObject.AddComponent<BattlerInstance>();
            battlerObject.AddComponent<Animator>();
            battlerObject.AddComponent<AudioSource>();
            return battlerInstance;
        }
    }
}