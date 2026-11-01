using System;
using System.Collections.Generic;
using static TagsConvention;

public static class TagFunctions
{
    public static TextData Scan(string rawText)
    {
        List<TagCue> cues = new();
        bool inTag = false;
        string cleanText = "";
        string tagText = "";
        for (int i = 0; i < rawText.Length; i++)
        {
            char c = rawText[i];
            if (c == '<')
            {
                inTag = true;
            }
            else if (c == '>')
            {
                TagCue? result = HandleTag(tagText, cleanText.Length);
                if (result != null) cues.Add(result.Value);
                inTag = false;
                tagText = "";
            }
            if (c == '<' || c == '>')
                continue;

            if (!inTag)
            {
                cleanText += c;
            }
            else
            {
                tagText += c;
            }
        }

        if (inTag)
            ErrorLogger.LogWarning($"Uncompleted tag detected. Make sure all tags are closed properly.\n" +
                $"Infriging whole text: {cleanText}\n Is in tag: {inTag}");

        return new(cleanText, cues.ToArray());
    }

    private static TagCue? HandleTag(string tag, int pos)
    {
        if (tag.StartsWith(Speed))
        {
            return new TagCue(TagType.Speed, GetFloatNumber(tag), pos);
        }
        else if (tag.StartsWith(Sound))
        {
            return new TagCue(TagType.Sound, GetIntNumber(tag), pos);
        }
        else if (tag.StartsWith("color") || tag.StartsWith("/color"))
        {
            return null;
        }
        ErrorLogger.LogError("A tag cue was received to Handle Tag but it doesn't match any of the tag constants. Make sure everything is updated correctly or that the tag was correctly typed.");
        return null;
    }

    private static float GetFloatNumber(string tag)
    {
        return float.Parse(GetNumber(tag));
    }

    private static int GetIntNumber(string tag)
    {
        return int.Parse(GetNumber(tag));
    }

    private static string GetNumber(string tag)
    {
        string number = "";
        bool inNumber = false;
        foreach (char c in tag)
        {
            if (c == '=' && !inNumber)
            {
                inNumber = true;
                continue;
            }
            else if (inNumber)
            {
                number += c;
            }
        }
        return number;
    }

    
}
