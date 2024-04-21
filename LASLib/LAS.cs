namespace LASLib
{
    /// <summary>
    /// Contains the information of a LAS file parsed into a structured object.
    /// </summary>
    public class LAS
    {
        #region properties
        public string Comments { get; private set; } = string.Empty;
        public List<Exception> Errors { get; } = new();

        public VersionInformation VersionInformation { get; private set; }
        public WellInformation WellInformation { get; private set; }
        public List<LASDataset> LASDatasets { get; private set; } = new() { new LASDataset("Standard") };
        #endregion
        #region constructors
        private LAS() 
        {
            
        }
        #endregion
        #region publicMethods
        public static LAS ParseFromFile(string fileName)
        {
            using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            return ParseFromStream(fs);
        }
        public static LAS ParseFromStream(Stream stream)
        {
            var result = new LAS();
            using var sr = new StreamReader(stream);

            ILASSection currentSection = null!;
            var line = string.Empty;
            int lineCount = -1;
            while((line = sr.ReadLine()?.Trim(' ')) is not null)
            {
                lineCount++;
                if (line.Equals(""))
                    continue;
                else if (line[0] == '#')
                {
                    if(currentSection is null)
                    {
                        result.Comments += line + "\r\n";
                    }
                    else
                    {
                        currentSection.Comments += line + "\r\n";
                    }
                }
                else if (line[0] == '~')
                {
                    if (currentSection is not null) currentSection.CheckIsValid();
                    currentSection = null!;
                    if (!line.Contains('_'))
                    {
                        if (line.StartsWith("~V", StringComparison.OrdinalIgnoreCase))
                        {
                            result.VersionInformation = new();
                            currentSection = result.VersionInformation;
                        }
                        else if (line.StartsWith("~W", StringComparison.OrdinalIgnoreCase))
                        {
                            result.WellInformation = new();
                            currentSection = result.WellInformation;
                        }
                        else if (line.StartsWith("~C", StringComparison.OrdinalIgnoreCase))
                        {
                            result.LASDatasets[0].DefinitionSection = new();
                            currentSection = result.LASDatasets[0].DefinitionSection;
                        }
                        else if (line.StartsWith("~P", StringComparison.OrdinalIgnoreCase))
                        {
                            result.LASDatasets[0].ParameterSection = new ParameterSection();
                            currentSection = result.LASDatasets[0].ParameterSection;
                        }
                        else if (line.StartsWith("~A", StringComparison.OrdinalIgnoreCase))
                        {
                            var dataSection = new DataSection(
                                    result.LASDatasets[0].DefinitionSection.DefinitionValues.Count,
                                    result.VersionInformation.Wrap.Data.Equals("YES", StringComparison.OrdinalIgnoreCase) ? true : false,
                                    result.VersionInformation.Delimiter?.Data
                                );
                            result.LASDatasets[0].DataSections.Add(dataSection);
                            currentSection = dataSection;
                        }
                    }
                    else
                    {
                        var sectionSplit = line.TrimStart('~').Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                        
                        // if there is one item in the split, then this is either a paraeter
                        // or definition section in an optional LAS3 dataset
                        if (sectionSplit.Length == 1)
                        {
                            var sectionHeader = SectionHeader.Parse(sectionSplit[0]);
                            if (string.IsNullOrWhiteSpace(sectionHeader.SectionType))
                                throw new ArgumentException("Section header must be in the format \"SectionName_SectionType\" with an optional \"[#]\" at the end");

                            if (sectionHeader.SectionType.Equals("Parameter", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!result.LASDatasets[result.LASDatasets.Count - 1].SectionName.Equals(sectionHeader.SectionName, StringComparison.OrdinalIgnoreCase))
                                {
                                    var lasDataset = new LASDataset(sectionHeader.SectionName + sectionHeader.SectionNumber);
                                    result.LASDatasets.Add(lasDataset);
                                }
                                result.LASDatasets[result.LASDatasets.Count - 1].ParameterSection = new();
                                currentSection = result.LASDatasets[result.LASDatasets.Count - 1].ParameterSection;
                            }
                            else if (sectionHeader.SectionType.Equals("Definition", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!result.LASDatasets[result.LASDatasets.Count - 1].SectionName.Equals(sectionHeader.SectionName + sectionHeader.SectionNumber, StringComparison.OrdinalIgnoreCase))
                                {
                                    var lasDataset = new LASDataset(sectionHeader.SectionName + sectionHeader.SectionNumber);
                                    result.LASDatasets.Add(lasDataset);
                                }
                                result.LASDatasets[result.LASDatasets.Count - 1].DefinitionSection = new();
                                currentSection = result.LASDatasets[result.LASDatasets.Count - 1].DefinitionSection;
                            }
                            else
                                throw new ArgumentException("Non-ascii section type must be \"Parameter\" or \"Definition\"");
                        }
                        // if there are two items in the split, it is an ascii data section
                        // that must be following a definition section (or else it's a badly formatted LAS3)
                        else if(sectionSplit.Length == 2)
                        {
                            var asciiHeader = SectionHeader.Parse(sectionSplit[0]);
                            var definitionHeader = SectionHeader.Parse(sectionSplit[1]);
                            var totalName = definitionHeader.SectionName + definitionHeader.SectionNumber;
                            LASDataset matchingLASDataset = null!;
                            if (!totalName.Equals(
                                    result.LASDatasets[result.LASDatasets.Count - 1].SectionName,
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                matchingLASDataset = result.LASDatasets
                                    .Where(ds => ds.SectionName.Equals(totalName, StringComparison.OrdinalIgnoreCase))
                                    .FirstOrDefault();

                                if (matchingLASDataset is null)
                                    throw new ArgumentException(
                                        "Ascii data section name does not match the name of the current option LAS dataset.");
                            }
                            else
                                matchingLASDataset = result.LASDatasets[result.LASDatasets.Count - 1];

                            var dataSection = new DataSection(
                            matchingLASDataset.DefinitionSection.DefinitionValues.Count,
                            result.VersionInformation.Wrap.Data.Equals("YES", StringComparison.OrdinalIgnoreCase) ? true : false,
                            result.VersionInformation.Delimiter?.Data
                            );
                            matchingLASDataset.DataSections.Add(dataSection);
                            currentSection = dataSection;
                        }
                    }
                }
                else
                {
                    if (currentSection is null)
                        continue;

                    try
                    {
                        currentSection.ParseLine(line);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
