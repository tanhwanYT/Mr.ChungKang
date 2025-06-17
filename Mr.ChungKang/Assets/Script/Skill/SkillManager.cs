using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<Skill> skills;

    public void UseSkill(int index, GameObject caster)
    {
        if (index >= 0 && index < skills.Count)
            skills[index].Activate(caster);
    }
}