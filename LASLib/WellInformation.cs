namespace LASLib
{
    /// <summary>
    /// The required second non-whitespace, non-comment section across all versions of LAS.
    /// </summary>
    public class WellInformation : ILASSection
    {
        public const string MNEMONIC_START = "STRT";
        public const string MNEMONIC_STOP = "STOP";
        public const string MNEMONIC_STEP = "STEP";
        public const string MNEMONIC_NULL = "NULL";
        public const string MNEMONIC_COMPANY = "COMP";
        public const string MNEMONIC_WELL = "WELL";
        public const string MNEMONIC_FIELD = "FLD";
        public const string MNEMONIC_LOCATION = "LOC";
        public const string MNEMONIC_PROVINCE = "PROV";
        public const string MNEMONIC_COUNTY = "CNTY";
        public const string MNEMONIC_STATE = "STAT";
        public const string MNEMONIC_COUNTRY = "CTRY";
        public const string MNEMONIC_SERVICECOMPANY = "SRVC";
        public const string MNEMONIC_DATE = "DATE";
        public const string MNEMONIC_UWI = "UWI";
        public const string MNEMONIC_API = "API";
        public const string MNEMONIC_LICENSE = "LIC";

        public string Comments { get; set; } = string.Empty;
        public List<InfoValue> Values { get; } = new();
        public List<string> Errors { get; } = new();

        public InfoValue Start { get; private set; }
        public InfoValue Stop { get; private set; }
        public InfoValue Step { get; private set; }
        public InfoValue Null { get; private set; }
        public InfoValue Company { get; private set; }
        public InfoValue Well { get; private set; }
        public InfoValue Field { get; private set; }
        public InfoValue Location { get; private set; }
        public InfoValue Province { get; private set; }
        public InfoValue County { get; private set; }
        public InfoValue State { get; private set; }
        public InfoValue Country { get; private set; }
        public InfoValue ServiceCompany { get; private set; }
        public InfoValue Date { get; private set; }
        public InfoValue UWI { get; private set; }
        public InfoValue API { get; private set; }

        public List<InfoValue> AdditionalValues { get; } = new();
        public bool CheckIsValid()
        {
            bool startFound = false;
            bool stopFound = false;
            bool stepFound = false;
            bool nullFound = false;
            bool companyFound = false;
            bool wellFound = false;
            bool fieldFound = false;
            bool locationFound = false;
            bool provinceFound = false;
            bool countyFound = false;
            bool stateFound = false;
            bool countryFound = false;
            bool serviceFound = false;
            bool dateFound = false;
            bool uwiFound = false;
            bool apiFound = false;

            for(int i = 0; i < Values.Count; i++)
            {
                if (Values[i].Mnemonic.Equals(MNEMONIC_START, StringComparison.OrdinalIgnoreCase))
                {
                    startFound = true;
                    Start = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_STOP, StringComparison.OrdinalIgnoreCase))
                {
                    stopFound = true;
                    Stop = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_STEP, StringComparison.OrdinalIgnoreCase))
                {
                    stepFound = true;
                    Step = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_NULL, StringComparison.OrdinalIgnoreCase))
                {
                    nullFound = true;
                    Null = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_COMPANY, StringComparison.OrdinalIgnoreCase))
                {
                    companyFound = true;
                    Company = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_WELL, StringComparison.OrdinalIgnoreCase))
                {
                    wellFound = true;
                    Well = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_FIELD, StringComparison.OrdinalIgnoreCase))
                {
                    fieldFound = true;
                    Field = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_LOCATION, StringComparison.OrdinalIgnoreCase))
                {
                    locationFound = true;
                    Location = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_PROVINCE, StringComparison.OrdinalIgnoreCase))
                {
                    provinceFound = true;
                    Province = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_COUNTY, StringComparison.OrdinalIgnoreCase))
                {
                    countyFound = true;
                    County = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_STATE, StringComparison.OrdinalIgnoreCase))
                {
                    stateFound = true;
                    State = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_COUNTRY, StringComparison.OrdinalIgnoreCase))
                {
                    countryFound = true;
                    Country = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_SERVICECOMPANY, StringComparison.OrdinalIgnoreCase))
                {
                    serviceFound = true;
                    ServiceCompany = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_DATE, StringComparison.OrdinalIgnoreCase))
                {
                    dateFound = true;
                    Date = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_UWI, StringComparison.OrdinalIgnoreCase))
                {
                    uwiFound = true;
                    UWI = Values[i];
                }
                else if (Values[i].Mnemonic.Equals(MNEMONIC_API, StringComparison.OrdinalIgnoreCase))
                {
                    apiFound = true;
                    API = Values[i];
                }
                else
                {
                    AdditionalValues.Add(Values[i]);
                }
            }

            if (!startFound)
                Errors.Add($"{MNEMONIC_START} not found.");
            if (!stopFound)
                Errors.Add($"{MNEMONIC_STOP} not found.");
            if (!stepFound)
                Errors.Add($"{MNEMONIC_STEP} not found.");
            if (!nullFound)
                Errors.Add($"{MNEMONIC_NULL} not found.");
            if (!companyFound)
                Errors.Add($"{MNEMONIC_COMPANY} not found.");
            if (!wellFound)
                Errors.Add($"{MNEMONIC_WELL} not found.");
            if (!fieldFound)
                Errors.Add($"{MNEMONIC_FIELD} not found.");
            if (!locationFound)
                Errors.Add($"{MNEMONIC_LOCATION} not found.");
            if (!provinceFound)
            {
                if (!countyFound)
                    Errors.Add($"{MNEMONIC_COUNTY} not found.");
                if (!stateFound)
                    Errors.Add($"{MNEMONIC_STATE} not found.");
                if (!countryFound)
                    Errors.Add($"{MNEMONIC_COUNTRY} not found.");
            }
            if (!serviceFound)
                Errors.Add($"{MNEMONIC_SERVICECOMPANY} not found.");
            if (!dateFound)
                Errors.Add($"{MNEMONIC_DATE} not found.");
            if (!uwiFound)
                Errors.Add($"{MNEMONIC_UWI} not found.");
            if (!apiFound)
                Errors.Add($"{MNEMONIC_API} not found.");

            return Errors.Count == 0;
        }

        public void ParseLine(string line)
        {
            Values.Add(InfoValue.ParseInfoValue(line));
        }
    }
}
