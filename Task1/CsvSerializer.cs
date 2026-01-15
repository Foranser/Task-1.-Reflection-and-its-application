using System;
using System.Globalization;
using System.Text;

namespace Task1;

public static class CsvSerializer
{
    private static readonly TypeModel Model = new(
        typeof(F),
        ["i1", "i2", "i3", "i4", "i5", "mas"]
    );

    public static string Serialize(F obj)
    {
        var fields = Model.Fields;

        var i1 = (int)fields[0].GetValue(obj)!;
        var i2 = (int)fields[1].GetValue(obj)!;
        var i3 = (int)fields[2].GetValue(obj)!;
        var i4 = (int)fields[3].GetValue(obj)!;
        var i5 = (int)fields[4].GetValue(obj)!;
        var mas = (int[])fields[5].GetValue(obj)!;

        var sb = new StringBuilder(64);
        sb.Append(i1.ToString(CultureInfo.InvariantCulture)).Append(',');
        sb.Append(i2.ToString(CultureInfo.InvariantCulture)).Append(',');
        sb.Append(i3.ToString(CultureInfo.InvariantCulture)).Append(',');
        sb.Append(i4.ToString(CultureInfo.InvariantCulture)).Append(',');
        sb.Append(i5.ToString(CultureInfo.InvariantCulture)).Append(',');
        AppendArray(sb, mas);

        return sb.ToString();
    }

    public static F Deserialize(string csv)
    {
        var parts = Split6(csv);

        int i1 = int.Parse(parts[0], CultureInfo.InvariantCulture);
        int i2 = int.Parse(parts[1], CultureInfo.InvariantCulture);
        int i3 = int.Parse(parts[2], CultureInfo.InvariantCulture);
        int i4 = int.Parse(parts[3], CultureInfo.InvariantCulture);
        int i5 = int.Parse(parts[4], CultureInfo.InvariantCulture);
        int[] mas = ParseArray(parts[5]);

        return new F(i1, i2, i3, i4, i5, mas);
    }

    private static void AppendArray(StringBuilder sb, int[] arr)
    {
        if (arr.Length == 0)
            return;

        sb.Append(arr[0].ToString(CultureInfo.InvariantCulture));
        for (int i = 1; i < arr.Length; i++)
        {
            sb.Append('|');
            sb.Append(arr[i].ToString(CultureInfo.InvariantCulture));
        }
    }

    private static int[] ParseArray(string s)
    {
        if (string.IsNullOrEmpty(s))
            return Array.Empty<int>();

        int count = 1;
        for (int i = 0; i < s.Length; i++)
            if (s[i] == '|')
                count++;

        var result = new int[count];
        int start = 0;
        int idx = 0;

        for (int i = 0; i <= s.Length; i++)
        {
            if (i == s.Length || s[i] == '|')
            {
                var span = s.AsSpan(start, i - start);
                result[idx++] = int.Parse(span, CultureInfo.InvariantCulture);
                start = i + 1;
            }
        }

        return result;
    }

    private static string[] Split6(string s)
    {
        var parts = new string[6];
        int part = 0;
        int start = 0;

        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == ',')
            {
                parts[part++] = s.Substring(start, i - start);
                start = i + 1;
                if (part == 5)
                    break;
            }
        }

        parts[5] = s.Substring(start);
        return parts;
    }
}
