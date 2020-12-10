namespace Sqless.Query
{
    public enum SqlessQueryType
    {
        Equal = 1,
        NotEqual = 2,

        StartWith = 11,
        EndWith = 12,
        Contain = 13,

        LessThan = 21,
        GreaterThan = 22,
        LessEqual = 23,
        GreaterEqual = 24,

        Null = 31,
        NotNull = 32
    }
}
