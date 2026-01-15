using System;

namespace Task1;

public sealed class F
{
    private int i1;
    private int i2;
    private int i3;
    private int i4;
    private int i5;
    private int[] mas;

    public F(int i1, int i2, int i3, int i4, int i5, int[] mas)
    {
        this.i1 = i1;
        this.i2 = i2;
        this.i3 = i3;
        this.i4 = i4;
        this.i5 = i5;
        this.mas = mas ?? Array.Empty<int>();
    }

    public override string ToString()
    {
        return $"{i1} {i2} {i3} {i4} {i5} [{string.Join(",", mas)}]";
    }
}
