namespace LASLib;
public class OtherSection : ILASSection
{
    public string Comments { get; set; } = string.Empty;

    public List<string> Lines = new();

    public List<string> Errors { get; } = new();

    public bool CheckIsValid()
    {
        return true;
    }
    public void ParseLine(string line)
    {
        try
        {
            Lines.Add(line);
        }
        catch (Exception e)
        {
            Errors.Add(e.Message);
        }
    }
}
