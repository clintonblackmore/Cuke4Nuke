using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Cuke4Nuke.Core;

namespace Cuke4Nuke.Framework
{
    [TypeConverter(typeof(TableConverter))]
    public class Table
    {
        public List<Dictionary<string, string>> Hashes()
        {
            var hashes = new List<Dictionary<string, string>>();
            if (_data.Count == 0)
                return hashes;

            var keys = Headers();

            for (int i = 1; i < _data.Count; i++)
            {
                var hash = new Dictionary<string, string>();
                for (int j = 0; j < _data[i].Count; j++)
                {
                    hash.Add(keys[j], _data[i][j]);
                }
                hashes.Add(hash);
            }

            return hashes;
        }

        List<List<string>> _data = new List<List<string>>();
        public List<List<string>> Data
        {
            get
            {
                return _data;
            }
        }

        public void AssertSameAs(Table expectedTable)
        {
            throw new TableAssertionException(this, expectedTable);
        }

        public bool Includes(Table expectedTable)
        {
            foreach (var exRow in expectedTable.Hashes())
            {
                bool hasMatchingRow = false;
                foreach (var acRow in this.Hashes())
                {
                    foreach (var key in exRow.Keys)
                    {
                        if (exRow[key] == acRow[key])
                        {
                            hasMatchingRow = true;
                        }
                        else
                        {
                            hasMatchingRow = false;
                            break;
                        }
                    }
                    if (hasMatchingRow)
                        break;
                }
                if (!hasMatchingRow)
                    return false;
            }
            return true;
        }

        public List<string> Headers()
        {
            if (Data.Count == 0)
                return new List<string>();
            else
                return Data[0];
        }

        public Dictionary<string, string> RowHashes()
        {
            if (_data.Count == 0 || _data[0].Count != 2)
                throw new InvalidOperationException("Table must contain exactly two columns to use RowHashes().");

            var rowHashes = new Dictionary<string, string>();
            foreach (var row in _data)
            {
                rowHashes.Add(row[0], row[1]);
            }
            return rowHashes;
        }

        public bool Equals(Table other)
        {
            if (other == null) return false;

            int rowCount = Data.Count;
            if (rowCount != other.Data.Count)
            {
                return false;
            }

            for (int i = 0; i < rowCount; ++i)
            {
                int columnCount = Data[i].Count;
                if (other.Data[i].Count != columnCount) return false;

                for (int j = 0; j < columnCount; ++j)
                {
                    if (Data[i][j].Equals(other.Data[i][j]) == false) return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Table);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // This function shows what is inside the table, so that you can 'Debug.Log' it
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var row in Data)
            {
                RowToString(row, sb);
            }

            return sb.ToString();
        }

        private void RowToString(List<string> row, StringBuilder builder)
        {
            int columns = row.Count;
            for (int i = 0; i < columns; ++i)
            {
                builder.Append(row[i]);
                if (i < columns -1)
                {
                    builder.Append(" | ");
                }
            }
            builder.Append("\n");
        }
    }
}
