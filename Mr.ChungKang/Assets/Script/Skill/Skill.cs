using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface Skill
{
    void Activate(GameObject caster);
}
public class SkillExecutor : MonoBehaviour
{
    public SkillData data;

    public void UseSkill(GameObject user)
    {
        Instantiate(data.effectPrefab, user.transform.position, Quaternion.identity);
        // 추가 로직은 여기에 (범위 데미지 등)
    }
}