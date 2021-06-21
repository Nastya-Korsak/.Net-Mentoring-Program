namespace MineFields
{
    public static class FieldHelper<T>
    {
        public static int GetRowsNumber(T[,] field)
        {
            if (field is null)
            {
                throw new System.ArgumentNullException(nameof(field));
            }

            return field.GetUpperBound(0) + 1;
        }

        public static int GetColumnsNumber(T[,] field)
        {
            if (field is null)
            {
                throw new System.ArgumentNullException(nameof(field));
            }

            return field.Length / GetRowsNumber(field);
        }
    }
}
