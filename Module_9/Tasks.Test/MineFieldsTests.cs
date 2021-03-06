using System;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace MineFields
{
    public class MineFieldsTests
    {
        private readonly MineFieldSign[,] _newMineField =
        {
            { MineFieldSign.Star, MineFieldSign.Point, MineFieldSign.Point, MineFieldSign.Point },
            { MineFieldSign.Point, MineFieldSign.Point, MineFieldSign.Star, MineFieldSign.Point },
            { MineFieldSign.Point, MineFieldSign.Point, MineFieldSign.Point, MineFieldSign.Point }
        };

        private readonly string[,] _expectedField =
        {
            { "*", "2", "1", "1" },
            { "1", "2", "*", "1" },
            { "0", "1", "1", "1" }
        };

        private IMineField _mineField;

        private int _mineFieldRows;

        private int _mineFieldColumns;

        [SetUp]
        public void Setup()
        {
            _mineField = new MineField();
            _mineFieldRows = FieldHelper<MineFieldSign>.GetRowsNumber(_newMineField);
            _mineFieldColumns = FieldHelper<MineFieldSign>.GetColumnsNumber(_newMineField);
        }

        [Test]
        public void GetHintField_MineField_HintFieldSizeIsMineFieldSize()
        {
            var actualField = _mineField.GetHintField(_newMineField);

            int hintFieldRows = FieldHelper<string>.GetRowsNumber(actualField);
            int hintFieldColumns = FieldHelper<string>.GetColumnsNumber(actualField);

            using (new AssertionScope())
            {
                hintFieldRows.Should().Be(_mineFieldRows);
                hintFieldColumns.Should().Be(_mineFieldColumns);
            }
        }

        [Test]
        public void GetHintField_MineField_HintField()
        {
            var actualField = _mineField.GetHintField(_newMineField);

            for (int i = 0; i < _mineFieldRows; i++)
            {
                for (int j = 0; j < _mineFieldColumns; j++)
                {
                    actualField[i, j].Should().Be(_expectedField[i, j]);
                }
            }
        }

        [Test]
        public void GetHintField_Null_ArgumentNullException()
        {
            Action act = () => _mineField.GetHintField(null);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}
