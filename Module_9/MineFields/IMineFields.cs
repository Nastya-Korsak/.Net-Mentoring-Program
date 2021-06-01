using System.Collections.Generic;

namespace MineFields
{
    public interface IMineField
    {
        public string[,] GetHintField(MFSings[,] mineField);
    }
}
