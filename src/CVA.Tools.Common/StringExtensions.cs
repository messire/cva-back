using System.Text;

namespace CVA.Tools.Common;

/// <summary>
/// Extensions for string.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Convert to snake case.
    /// </summary>
    /// <param name="text">Input text.</param>
    /// <returns>Snake case text.</returns>
    /// <exception cref="ArgumentNullException">If input text is null.</exception>
    public static string ToSnakeCase(this string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));
        for (var i = 1; i < text.Length; ++i)
        {
            var c = text[i];
            if (char.IsUpper(c))
            {
                sb.Append('_').Append(char.ToLowerInvariant(c));
                continue;
            }

            sb.Append(c);
        }

        return sb.ToString();
    }
}