namespace MineFields
{
    public static class FieldHelper<T>
    {
        public static int GetRowsNumber(T[,] field)
        {
            return field.GetUpperBound(0) + 1;
        }

        public static int GetColumnsNumber(T[,] field)
        {
            return field.Length / GetRowsNumber(field);
        }
    }
}
