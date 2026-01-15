using System;
using System.Reflection;

namespace Task1;

public sealed class TypeModel
{
    public FieldInfo[] Fields { get; }

    public TypeModel(Type type, string[] fieldOrder)
    {
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var fields = new FieldInfo[fieldOrder.Length];

        for (int i = 0; i < fieldOrder.Length; i++)
        {
            var f = type.GetField(fieldOrder[i], flags);
            if (f == null)
                throw new InvalidOperationException($"Field not found: {fieldOrder[i]}");
            fields[i] = f;
        }

        Fields = fields;
    }
}
