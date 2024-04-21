
using System.Runtime.CompilerServices;

namespace LASLib
{
    /// <summary>
    /// The data section is a common representation of the ~A, ~ASCII, or ~*_DATA sections found across all versions of LAS.
    /// If you supply the constructor with a null delimiter (DLM was introduced in LAS3.0) it assumes data will be separated
    /// by whitespace of variable length.
    /// If you supply a delimiter, it will parse on that delimiter unless it is contained inside of quotation marks
    /// (e.g. string data that is surrounded by quotes)
    /// </summary>
    public class DataSection : ILASSection
    {
        private readonly int _columnCount;
        private readonly char _delimiter;
        private readonly bool _isLegacy;
        private readonly StringSplitOptions _stringSplitOptions;
        private readonly bool _wrapped;
        private int _valueCount;
        private List<string> _currentResult;
        private bool _isValid;
        public List<string[]> DataValues { get; } = new();

        public string Comments { get; set; } = string.Empty;

        public List<string> Errors { get; } = new();

        public DataSection(int columnCount, bool wrapped, string delimiter = null)
        {
            if (delimiter is null)
            {
                delimiter = "SPACE";
                _isLegacy = true;
                _stringSplitOptions = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
            }
            else
            {
                _stringSplitOptions = StringSplitOptions.TrimEntries;
            }
            _columnCount = columnCount;
            _delimiter = 
                delimiter.Equals("SPACE", StringComparison.OrdinalIgnoreCase) ? ' ' : 
                delimiter.Equals("COMMA", StringComparison.OrdinalIgnoreCase) ? ',' :
                delimiter.Equals("TAB", StringComparison.OrdinalIgnoreCase) ? '\t' : throw new ArgumentException("Delimiter must be \"SPACE\", \"COMMA\", or \"TAB\".");
            _wrapped = wrapped;
        }

        private List<string> ParseValues(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            string currentItem = string.Empty;

            if (_isLegacy)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '\"')
                    {
                        currentItem += line[i];
                        inQuotes = !inQuotes;
                    }
                    else if (line[i] == _delimiter && !inQuotes)
                    {
                        if (!string.IsNullOrEmpty(currentItem))
                        {
                            result.Add(currentItem.Trim());
                            currentItem = string.Empty;
                        }
                    }
                    else
                    {
                        currentItem += line[i];
                    }
                }
                if(!string.IsNullOrEmpty(currentItem))
                    result.Add(currentItem.Trim());
            }
            else
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i].Equals('\"'))
                    {
                        currentItem += line[i];
                        inQuotes = !inQuotes;
                    }
                    else if (line[i].Equals(_delimiter) && !inQuotes)
                    {
                        result.Add(currentItem.Trim());
                        currentItem = string.Empty;
                    }
                    else
                    {
                        currentItem += line[i];
                    }
                }
                result.Add(currentItem.Trim());
            }

            return result;
        }
        
        private void ParseWrapped(string line)
        {
            var lineValues = ParseValues(line);
            if (_valueCount == 0)
            {
                _currentResult = new();
                if (lineValues.Count != 1)
                {
                    Errors.Add("Badly formed wrapped ascii data.");
                }
            }
            
            for (var i = 0; i < lineValues.Count; i++)
            {
                _currentResult.Add(lineValues[i]);
                _valueCount++;

                if (_valueCount != _columnCount) continue;
                
                DataValues.Add(_currentResult.ToArray());
                _valueCount = 0;
                break;
            }
        }

        private void ParseUnwrapped(string line)
        {
            var lineValues = ParseValues(line);
            if(lineValues.Count > _columnCount)
            {
                Errors.Add($"Ascii data expecting {_columnCount} columns of data. Received {lineValues.Count}.");
            }
            else
            {
                var result = new string[_columnCount];
                for (var i = 0; i < lineValues.Count; i++)
                    result[i] = lineValues[i];
                DataValues.Add(result);
            }
        }
        public bool CheckIsValid()
        {
            return true;
        }
        public void ParseLine(string line)
        {
            if (_wrapped)
                ParseWrapped(line);
            else
                ParseUnwrapped(line);
        }
    }
}
