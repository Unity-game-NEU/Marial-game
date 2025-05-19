using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject item;
    public GameObject breakEffect; // 碎裂效果的预制体    
    public Sprite emptyBlock;
    public int maxHits = -1;
    public bool animating;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && collision.transform.DotTest(transform, Vector2.up)) // 检查是否从下方顶砖块
        {
            CheckForEnemiesAbove();
            if (player.big && CompareTag("Brick") && item == null) // 玩家处于“变大模式”
            {
                BreakBlock(); // 直接碎裂
            }
            else if (!animating && maxHits != 0)
            {
                Hit();
            }
        }

    }
    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true; // show if hidden

        maxHits--;

        if (maxHits == 0)
        {
            spriteRenderer.sprite = emptyBlock;
        }

        if (item != null)
        {
            Instantiate(item, transform.position, Quaternion.identity);
            if (item.CompareTag("Mushroom"))
            {
                AudioManager.Instance.PlaySound("Mushroom", transform.position);
            }
            else
            { AudioManager.Instance.PlaySound("Coin", transform.position); }
        }


        StartCoroutine(Animate());
    }
    private void BreakBlock()
    {
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }
        AudioManager.Instance.PlaySound("Break", transform.position);

        // 销毁砖块
        Destroy(gameObject);
    }
    private IEnumerator Animate()
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }
    private void CheckForEnemiesAbove()
    {
        // 定义一个检测范围，检测范围略高于砖块
        Vector2 position = transform.position;
        Vector2 size = new Vector2(0.9f, 0.5f); // 设置适当的宽度和高度

        // 检测上方区域的碰撞体
        Collider2D[] hits = Physics2D.OverlapBoxAll(position + Vector2.up * 0.5f, size, 0f);

        foreach (Collider2D hit in hits)
        {
            // 检查是否是敌人（确保敌人有 "Enemy" 标签）
            if (hit.CompareTag("Enemy"))
            {
                // 尝试获取 Goomba 组件并调用 Hit 方法
                Goomba goomba = hit.GetComponent<Goomba>();
                if (goomba != null)
                {
                    goomba.Hit(); // 调用怪物的死亡方法
                }
            }
        }
    }

}

