namespace LASLib
{
    /// <summary>
    /// The required first non-whitespace, non-comment section across all versions of LAS.
    /// </summary>
    public class VersionInformation : ILASSection
    {
        public const string MNEMONIC_VERSION = "VERS";
        public const string MNEMONIC_WRAP = "WRAP";
        public const string MNEMONIC_DELIMITER = "DLM";
        public string Comments { get; set; } = string.Empty;
        public List<InfoValue> Values { get; } = new List<InfoValue>();
        public List<string> Errors { get; } = new();

        public InfoValue Version { get; private set; }
        public InfoValue Wrap { get; private set; }
        public InfoValue? Delimiter { get; private set; }
        public bool CheckIsValid()
        {
            bool versionFound = false;
            bool wrapFound = false;
            bool delimiterFound = false;
            for (int i = 0; i < Values.Count; i++)
            {
                if (Values[i].Mnemonic.Equals(MNEMONIC_VERSION, StringComparison.OrdinalIgnoreCase))
                {
                    versionFound = true;
                    Version = Values[i];
                    if (
                        !Values[i].Data.Equals("1.2", StringComparison.OrdinalIgnoreCase) &&
                        !Values[i].Data.Equals("2.0", StringComparison.OrdinalIgnoreCase) &&
                        !Values[i].Data.Equals("3.0", StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        Errors.Add($"Version \"{Values[i].Data}\" invalid. Must be 1.2, 2.0, or 3.0.");
                    }
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_WRAP, StringComparison.OrdinalIgnoreCase))
                {
                    wrapFound = true;
                    Wrap = Values[i];
                    if (
                    !Values[i].Data.Equals("YES", StringComparison.OrdinalIgnoreCase) &&
                    !Values[i].Data.Equals("NO", StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        Errors.Add($"Wrap \"{Values[i].Data}\" invalid. Must be \"Yes\" or \"No\".");
                    }
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_DELIMITER, StringComparison.OrdinalIgnoreCase))
                {
                    delimiterFound = true;
                    Delimiter = Values[i];
                    if (
                        !Values[i].Data.Equals("COMMA", StringComparison.OrdinalIgnoreCase) &&
                        !Values[i].Data.Equals("SPACE", StringComparison.OrdinalIgnoreCase) &&
                        !Values[i].Data.Equals("TAB", StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        Errors.Add($"{MNEMONIC_DELIMITER} invalid. Must be \"COMMA\", \"SPACE\", \"TAB\".");
                    }
                }
            }

            if (!versionFound)
                Errors.Add($"\"{MNEMONIC_VERSION}\" not found.");
            else
            {
                if (Version.Data.Equals("3.0", StringComparison.OrdinalIgnoreCase) &&
                    !delimiterFound)
                    Errors.Add($"Version 3.0 requires {MNEMONIC_DELIMITER} field to be present.");
            }
            if (!wrapFound)
                Errors.Add($"\"{MNEMONIC_WRAP}\" not found.");

            return Errors.Count == 0;
        }
        public void ParseLine(string line)
        {
            Values.Add(InfoValue.ParseInfoValue(line));
        }
    }
}
