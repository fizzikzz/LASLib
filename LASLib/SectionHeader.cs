using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LASLib
{
    public class SectionHeader
    {
        public string SectionName { get; private set; } = string.Empty;
        public string SectionType { get; private set; } = string.Empty;
        public string SectionNumber { get; private set; } = string.Empty;

        public static SectionHeader Parse(string line)
        {
            var result = new SectionHeader();
            bool underscoreFound = false;
            for (int i = line.Length - 1; i >= 0; i--)
            {
                if (line[i].Equals('['))
                {
                    result.SectionNumber = (line[i] + result.SectionName).Trim();
                    result.SectionName = string.Empty;
                }
                else if (line[i].Equals('_') && !underscoreFound)
                {
                    underscoreFound = true;
                    result.SectionType = result.SectionName.Trim();
                    result.SectionName = string.Empty;
                }
                else
                {
                    result.SectionName = line[i] + result.SectionName;
                }
            }
            result.SectionName = result.SectionName.Trim();

            return result;
        }
    }
}
