
namespace LASLib
{
    public class ParameterSection : ILASSection
    {
        public string Comments { get; set; } = string.Empty;

        public List<InfoValue> ParameterValues { get; } = new();

        public List<string> Errors { get; } = new();

        public bool CheckIsValid()
        {
            return true;
        }
        public void ParseLine(string line)
        {
            try
            {
                ParameterValues.Add(InfoValue.ParseInfoValue(line));
            }
            catch (Exception e)
            {
                Errors.Add(e.Message);
            }
        }
    }
}
