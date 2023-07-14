using System;
using System.Collections.Generic;
using Battlers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.BattleUI
{
    public class SkillBarManager : MonoBehaviour
    {
        [SerializeField] private BattleChannel battleChannel;
        [SerializeField] private GameObject skillPrefab;

        private void OnCurrentBattlerChanged(BattlerInstance battler)
        {
            RemoveCurrentBattlerSkills();
            
            if (battler.Team == Team.Party) 
                InitSkillBar(battler);
        }

        private void InitSkillBar(BattlerInstance battler)
        {
            foreach (var skill in battler.battler.Skills)
            {
                GameObject skillInstance = Instantiate(skillPrefab, transform);
                skillInstance.name = $"[{skill.name}]";
                Button btn = skillInstance.GetComponentInChildren<Button>();
                btn.onClick.AddListener(() => OnSkillButtonClicked(skill));
                btn.gameObject.GetComponent<Image>().sprite = skill.icon;
            }
        }

        private void OnSkillButtonClicked(Skill skill) => battleChannel.RaiseSkillSelected(skill);

        private void RemoveCurrentBattlerSkills() => transform.DestroyChildren();

        private void OnEnable() => battleChannel.currentBattlerChangedEvent += OnCurrentBattlerChanged;
        private void OnDisable() => battleChannel.currentBattlerChangedEvent -= OnCurrentBattlerChanged;
    }
}