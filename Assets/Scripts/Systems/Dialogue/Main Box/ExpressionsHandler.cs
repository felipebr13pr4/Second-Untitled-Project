using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.UI;

public class ExpressionsHandler : MonoBehaviour
{
    [SerializeField] private Image m_portrait;
    [SerializeField] private Image m_fullBody;
    [SerializeField] private ExpressionsHolder m_holder;

    private void OnEnable()
    {
        TextBox.OnExpression += HandleExpression;
    }

    private void OnDisable()
    {
        TextBox.OnExpression -= HandleExpression;
    }

    public void HandleExpression(LineExpression expression)
    {
        ExpressionSprites? sprites = m_holder.GetExpressionSprite(expression.AssignedExpression);
        if (sprites == null) return;

        m_portrait.enabled = expression.AssignedExpression != Expressions.None;
        m_fullBody.enabled = expression.HasFullBody;

        if (expression.AssignedExpression == Expressions.None)
        {
            m_portrait.enabled = false;
            m_fullBody.enabled = false;
            return;
        }

        if (sprites.Value.Portrait != null)
            m_portrait.sprite = sprites.Value.Portrait;

        if (sprites.Value.FullBody != null)
            m_fullBody.sprite = sprites.Value.FullBody;
    }
}
