using System;

namespace MineFields
{
    public class MineField : IMineField
    {
        public string[,] GetHintField(MineFieldSign[,] mineField)
        {
            if (mineField is null)
            {
                throw new ArgumentNullException(nameof(mineField));
            }

            var rows = FieldHelper<MineFieldSign>.GetRowsNumber(mineField);
            var columns = FieldHelper<MineFieldSign>.GetColumnsNumber(mineField);

            var hintField = new int[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    if (mineField[i, j] == MineFieldSign.Star)
                    {
                        hintField[i, j] = -1;

                        if (i - 1 >= 0 && j - 1 >= 0 && mineField[i - 1, j - 1] != MineFieldSign.Star)
                        {
                            hintField[i - 1, j - 1] += 1;
                        }

                        if (i - 1 >= 0 && j >= 0 && mineField[i - 1, j] != MineFieldSign.Star)
                        {
                            hintField[i - 1, j] += 1;
                        }

                        if (i - 1 >= 0 && j + 1 >= 0 && mineField[i - 1, j + 1] != MineFieldSign.Star)
                        {
                            hintField[i - 1, j + 1] += 1;
                        }

                        if (i >= 0 && j - 1 >= 0 && mineField[i, j - 1] != MineFieldSign.Star)
                        {
                            hintField[i, j - 1] += 1;
                        }

                        if (i >= 0 && j + 1 >= 0 && mineField[i, j + 1] != MineFieldSign.Star)
                        {
                            hintField[i, j + 1] += 1;
                        }

                        if (i + 1 >= 0 && j - 1 >= 0 && mineField[i + 1, j - 1] != MineFieldSign.Star)
                        {
                            hintField[i + 1, j - 1] += 1;
                        }

                        if (i + 1 >= 0 && j >= 0 && mineField[i + 1, j] != MineFieldSign.Star)
                        {
                            hintField[i + 1, j] += 1;
                        }

                        if (i + 1 >= 0 && j + 1 >= 0 && mineField[i + 1, j + 1] != MineFieldSign.Star)
                        {
                            hintField[i + 1, j + 1] += 1;
                        }
                    }
                }
            }

            return GetStringHintField(hintField);
        }

        private string[,] GetStringHintField(int[,] hintField)
        {
            var rows = FieldHelper<int>.GetRowsNumber(hintField);
            var columns = FieldHelper<int>.GetColumnsNumber(hintField);

            var result = new string[rows, columns];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    if (hintField[i, j] == -1)
                    {
                        result[i, j] = "*";
                    }
                    else
                    {
                        result[i, j] = hintField[i, j].ToString();
                    }
                }
            }

            return result;
        }
    }
}
