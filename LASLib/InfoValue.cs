namespace LASLib
{
    public class InfoValue
    {
        public string Mnemonic { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string Association { get; set; } = string.Empty;

        public static InfoValue ParseInfoValue(string line)
        {
            var result = new InfoValue();
            var idx = 0;
            bool firstPeriodFound = false;
            while (idx < line.Length - 1 && !firstPeriodFound)
            {
                if (line[idx] == '.')
                {
                    firstPeriodFound = true;
                }
                else
                {
                    result.Mnemonic += line[idx];
                }
                idx++;
            }

            if (!firstPeriodFound)
                throw new ArgumentException("Line did not contain a period delimiter.");

            if (string.IsNullOrWhiteSpace(result.Mnemonic))
                throw new ArgumentException("Mnemonic cannot be empty.");

            result.Mnemonic = result.Mnemonic.Trim();

            bool firstSpaceAfterFirstPeriodFound = false;
            while (idx < line.Length - 1 && !firstSpaceAfterFirstPeriodFound)
            {
                if (line[idx] == ' ')
                {
                    firstSpaceAfterFirstPeriodFound = true;
                }
                else
                {
                    result.Unit += line[idx];
                }
                idx++;
            }

            int reverseIdx = line.Length - 1;
            bool lastColonFound = false;
            bool inFormat = false;
            while (reverseIdx > idx && !lastColonFound)
            {
                if (line[reverseIdx] == ':' && !inFormat)
                    lastColonFound = true;
                else if (line[reverseIdx] == '}' || line[reverseIdx] == ']')
                {
                    inFormat = true;
                }
                else if (line[reverseIdx] == '{' || line[reverseIdx] == '[')
                {
                    result.Format = result.Description.Trim().Replace("DD", "dd").Replace("YY", "yy").Replace("hh", "HH");
                    result.Description = string.Empty;
                    inFormat = false;
                }
                else if (line[reverseIdx] == '|')
                {
                    result.Association = result.Description.Trim();
                    result.Description = string.Empty;
                }
                else
                    result.Description = line[reverseIdx] + result.Description;

                reverseIdx--;
            }

            if (!lastColonFound)
                throw new ArgumentException("Line did not contain colon delimiter.");

            result.Description = result.Description.Trim();

            while (idx <= reverseIdx)
            {
                result.Data += line[idx];
                idx++;
            }

            result.Data = result.Data.Trim();

            return result;
        }
    }

}
