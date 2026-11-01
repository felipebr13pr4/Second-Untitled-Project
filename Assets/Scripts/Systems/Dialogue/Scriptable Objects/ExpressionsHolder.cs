using UnityEngine;

[CreateAssetMenu(fileName = "New Expression Holder", menuName = "Dialogue/Expression Holder")]
public class ExpressionsHolder : ScriptableObject
{
    [SerializeField] private ExpressionSprites[] m_sprites;

    private void OnValidate()
    {
        for (int i = 0; i < m_sprites.Length; i++)
        {
            m_sprites[i].Name = m_sprites[i].Expression.ToString();
        }
    }

    public bool HasExpression(LineExpression expression)
    {
        bool found = false;

        foreach (ExpressionSprites data in m_sprites)
            if (expression.AssignedExpression == data.Expression)
                found = true;

        if (!found)
            ErrorLogger.LogError("Something went wrong, a expression not detected has been received in the holder. Make sure theres a data for each expression.");
        return found;
    }

    public ExpressionSprites? GetExpressionSprite(Expressions expression)
    {
        foreach (ExpressionSprites data in m_sprites)
        {
            if (expression == data.Expression)
            {
                return data;
            }
        }

        ErrorLogger.LogError("Something went wrong, a expression enum not detected has been received in the holder. Make sure theres a enum entry for each expression.");

        return null;
    }
}
