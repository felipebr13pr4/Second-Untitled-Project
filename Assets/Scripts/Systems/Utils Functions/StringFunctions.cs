using System.Text;

public static class StringFunctions
{
    public static string SpaceString(string str)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
            {
                if (i != 0)
                {
                    sb.Append(' ');
                    sb.Append(str[i]);
                    continue;
                }
            }
            sb.Append(str[i]);
        }

        return sb.ToString();
    }
}
