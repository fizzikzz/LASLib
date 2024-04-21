namespace LASLib
{
    public interface ILASSection
    {
        string Comments { get; set; }
        List<string> Errors { get; }
        bool CheckIsValid();
        void ParseLine(string line);
    }
}
