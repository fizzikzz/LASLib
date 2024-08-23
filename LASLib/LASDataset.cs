namespace LASLib {
    /// <summary>
    /// A common representation for a collection of parameter (~P, ~ParameterInformation, ~*_Parameter),
    /// definition (~C, ~CurveInformation, ~*_Definition), and data sections (~A, ~ASCII, ~*_Data) across
    /// all versions of LAS.
    /// </summary>
    public class LASDataset {
        public string SectionName { get; private set; }
        public ParameterSection? ParameterSection { get; set; }
        public OtherSection? OtherSection { get; set; }
        public DefinitionSection DefinitionSection { get; set; }
        public List<DataSection> DataSections { get; set; } = new();

        public LASDataset(string sectionName)
        {
            SectionName = sectionName;
        }
    }
}