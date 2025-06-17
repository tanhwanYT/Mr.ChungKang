using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum SkillType { Offensive, Defensive, Utility }

[CreateAssetMenu(menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public SkillType type;
    public float costEP;
    public float cooldown;
    public GameObject effectPrefab;
}