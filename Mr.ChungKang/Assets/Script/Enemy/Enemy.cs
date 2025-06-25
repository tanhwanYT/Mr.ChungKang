using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character
{
    public EnemyAI ai;
    public EnemyData data;
    public GameObject prfhpBar;
    public GameObject canvas;
    public float height = 1.7f;

    RectTransform hpBar;
    private Image hpFillImage;

    private GameObject playerobj;
    private void Start()
    {
        hpBar = Instantiate(prfhpBar, canvas.transform).GetComponent<RectTransform>();
        characterName = data.enemyName;
        maxHP = data.maxHP;
        currentHP = maxHP;
        moveSpeed = data.moveSpeed;
        playerobj = GameObject.FindWithTag("Player");

        GetComponent<Animator>().runtimeAnimatorController = data.animator;
        
        hpBar = hpBar.GetComponent<RectTransform>();
        hpFillImage = hpBar.transform.Find("fill arena").GetComponent<Image>();
    }
    private void Update()
    {
        ai?.Execute(this);
        Vector3 hpBarpos =
            Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, transform.position.z));
        Vector2 screenPos = Camera.main.WorldToScreenPoint(hpBarpos);

        Vector2 localPoint;
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out localPoint))
        {
            hpBar.anchoredPosition = localPoint;
        }
    }

    public override void TakeDamage(float amount)
    {
        currentHP -= amount;
        
        UpdateHPBar();

        if (currentHP < 0) currentHP = 0;
        if (currentHP <= 0) Die();
    }

    private void UpdateHPBar()
    {
        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = currentHP / maxHP;
        }
    }

    public override void Die()
    {
        if (playerobj != null)
        {
            Player player = playerobj.GetComponent<Player>();
            player.GainExp(data.expReward);
        }

        Destroy(gameObject);
        Destroy(hpBar.gameObject);
    }
}