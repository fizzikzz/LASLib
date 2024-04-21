
namespace LASLib
{
    /// <summary>
    /// The definition section is a common representation of the ~C, ~Curve Information, ~*_Defintion sections across all versions of LAS.
    /// </summary>
    public class DefinitionSection : ILASSection
    {
        public List<InfoValue> DefinitionValues { get; } = new();

        public string Comments { get; set; }

        public List<string> Errors { get; } = new();

        public bool CheckIsValid()
        {
            return true;
        }

        public void ParseLine(string line)
        {
            try
            {
                DefinitionValues.Add(InfoValue.ParseInfoValue(line));
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
            }
        }
    }
}
