# LASLib
A C# library intended to have the ability to parse LAS1.2, LAS2.0, and LAS3.0 files into a structured, useable object.

Parse from a file:
```c#
var filePaths = new string[] {
    "someLAS1.2file.las",
    "someLAS2.0file.las",
    "someLAS3.0file.las"
}

foreach(var filePath in filePaths)
{
    var las = LAS.ParseFromFile(filePath);
    
    Console.WriteLine("Version Information");
    foreach (var info in las.VersionInformation.Values)
        Console.WriteLine(
            $"mnemonic({info.Mnemonic}) unit({info.Unit}) data({info.Data}) description({info.Description})"
            );
            
    Console.WriteLine("Well Information");
    foreach (var info in las.WellInformation.Values)
        Console.WriteLine(
            $"mnemonic({info.Mnemonic}) unit({info.Unit}) data({info.Data}) description({info.Description})"
            );
            
    foreach (var lasDataset in las.LASDatasets)
    {
        Console.WriteLine($"Section Name: {lasDataset.SectionName}");
        if (lasDataset.ParameterSection is not null)
        {
            Console.WriteLine("Parmeter Section");
            foreach (var parameterValue in lasDataset.ParameterSection.ParameterValues)
                Console.WriteLine(
                    $"mnemonic({parameterValue.Mnemonic}) unit({parameterValue.Unit}) data({parameterValue.Data}) description({parameterValue.Description})"
                );
        }
        
        if (lasDataset.DefinitionSection is not null)
        {
            Console.WriteLine("Definition Section");
            foreach(var definitionValue in lasDataset.DefinitionSection.DefinitionValues)
                Console.WriteLine(
                    $"mnemonic({definitionValue.Mnemonic}) unit({definitionValue.Unit}) data({definitionValue.Data}) description({definitionValue.Description} association({definitionValue.Association})"
                );
        }
        
        foreach (var dataSection in lasDataset.DataSections)
        {
            Console.WriteLine("Data Section");
            foreach(var dataValue in dataSection.DataValues)
                Console.WriteLine(string.Join(',', dataValue));
        }
    }
}
```

To parse from a stream:
```c#
using var fs = new FileStream("someLASfile.las", FileMode.Open, FileAccess.Read, FileShare.Read);

var las = LAS.ParseFromStream(fs);
```