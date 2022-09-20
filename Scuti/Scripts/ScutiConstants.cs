using System.Collections.Generic;
using System.Security;

namespace Scuti
{
    public static class ScutiConstants
    {
        public const string KEY_WELCOME = "welcome";
        public const string SCUTI_RESOURSES = "Scuti/Resources";
        public const string SCUTI_SETTINGS_FILE = "ScutiSettings.asset";
        public const string SCUTI_SETTINGS_PATH_FILE = "Scuti/";
        public const string MAIN_PREFAB_NAME = "ScutiSDK";
        public const string UI_PREFAB_NAME = "ScutiStore";
        public const string BUTTON_PREFAB_NAME = "ScutiButton";
        public const string BUTTON_PREFAB_3D_NAME = "ScutiButton3D";
        public const string SCUTI_SETTINGS_FILE_WITHOUTEXTENSION = "ScutiSettings";
        public static string PRIVACY_POLICY_URL = "https://www.scuti.store/privacy-policy";

        public const string INTERNAL_URL_PREFIX = "SCT:";
        public const string PUBLIC_KEY_CACHE = "PUBLIC_KEY_CACHE";
        public const string PUBLIC_KEY_ID_CACHE = "PUBLIC_KEY_ID_CACHE";
        public const string PUBLIC_KEY_CACHE_TIME = "PUBLIC_KEY_CACHE_TIME";

        public const string SCUTI_IMPRESSION_ID = "impression_key";
        public const float SCUTI_VALID_IMPRESSION_DURATION = 1f;
        public const float SCUTI_TIMEOUT = 60f;

        public const string CheckoutURL = "https://api.sandbox.checkout.com";
        public const string DOCUMENTATION_URL = "https://github.com/scuti-ai/scuti-unity";
        public const string DASHBOARD_URL = "https://scuti.store/";
        public const string VERSION = "1.5.1";

        public const double CACHE_PRODUCT_DURATION = 1d;

        //public static string[] SUPPORTED_COUNTRY_CODES = { "CA", "DE", "US", "GB", "RU", "AT", "FR", "CH", "AU", "JP", "CZ", "HU", "PL", "IE", "NL", "SE", "FI", "UA", "BE", "IT", "MX", "ES", "CL", "NO", "SK", "ID", "LU", "MY", "AF", "AR" };
        public static string[] SUPPORTED_COUNTRY_CODES = { "CA", "DE", "US", "GB" };
        //public static string[] COUNTRIES = new string[] { "Canada", "Germany", "US", "UK", "Russia", "Austria", "France", "Switzerland", "Australia", "Japan", "Czechia", "Hungary", "Poland", "Ireland", "Netherlands", "Sweden", "Finland", "Ukraine", "Belgium", "Italy", "Mexico", "Spain", "Chile", "Norway", "Slovakia", "Indonesia", "Luxembourg", "Malaysia", "Afghanistan", "Argentina" };
        //public static string[] PROVINCES_LABEL = new string[] { "Province", "State", "State", "Region", "Federal subjects", "State", "Region", "Canton", "State", "Prefecture", "Region", "County", "Voivodeship", "County", "Province", "County", "Region", "Region", "Region", "Region", "State", "Province", "Region", "County", "Kraje", "Provinces", "Canton", "State", "Province", "Province" };
        public static SupportedCountries SUPPORTED_COUNTRIES = new SupportedCountries();

        public static string[] STATES = new string[] { "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY" };
        public static string[] PROVINCES = new string[] { "AB", "BC", "MB", "NB", "NL", "NT", "NS", "NU", "ON", "PE", "QC", "SK", "YT" };

        public struct CustomCategoryData
        {
            public string DisplayName;
            public string QueryName;
        }
        public static CustomCategoryData[] CUSTOM_CATEGORIES = new CustomCategoryData[] { new CustomCategoryData() { DisplayName = "Today's Deals", QueryName = "isTodayDeal" }, new CustomCategoryData() { DisplayName = "Latest Products", QueryName = "isLatest" }, new CustomCategoryData() { DisplayName = "Miscellaneous", QueryName = "isMisc" } };

        public const string MAIL_TO_IOS = "support-ios@scuti2632.zendesk.com";
        public const string MAIL_TO_ANDROID = "support-android@scuti2632.zendesk.com";
        public const string MAIL_TO_PC = "support-pc@scuti2632.zendesk.com";
        public const string MAIL_FROM = "noreply@scuti.store";
        public const string MAIL_CLIENT = "smtp.gmail.com";
        public const int MAIL_PORT = 587;
        public const string MAIL_P = "MindtrustScuti123";

        // for scuti-commerce
        //public const string SENTRY_DSN = "https://03f1922891e94358a6251d2424b46d38:37f3c9dbf31d4fa19e3436af6d7c469a@o920227.ingest.sentry.io/5865652";
        // for unity-sdk
        public const string SENTRY_DSN = "https://1e8d2d595461422fa149d415cd5a6dbe:3c3631f6b55242b59ba34189f34b3b2e@o920227.ingest.sentry.io/6198586";

    }

    public class SupportedCountry
    {
        public string Name;
        public string Code;
        public string DivisionName;
        public List<Division> Divisions;
    }

    public class Division
    {
        public string Name;
        public string Code;

        public Division(string name, string code)
        {
            this.Name = name;
            this.Code = code;
        }
    }

    public class SupportedCountries
    {
        public List<SupportedCountry> Countries = new List<SupportedCountry>();

        public SupportedCountries()
        {
            var us = new SupportedCountry();
            us.Name = "US";
            us.Code = "US";
            us.DivisionName = "State";
            var usDivisions = new List<Division>();
            usDivisions.Add(new Division("Alaska", "AK"));
            usDivisions.Add(new Division("Alabama", "AL"));
            usDivisions.Add(new Division("Arkansas", "AR"));
            usDivisions.Add(new Division("American Samoa", "AS"));
            usDivisions.Add(new Division("Arizona", "AZ"));
            usDivisions.Add(new Division("California", "CA"));
            usDivisions.Add(new Division("Colorado", "CO"));
            usDivisions.Add(new Division("Connecticut", "CT"));
            usDivisions.Add(new Division("District of Columbia", "DC"));
            usDivisions.Add(new Division("Delaware", "DE"));
            usDivisions.Add(new Division("Florida", "FL"));
            usDivisions.Add(new Division("Georgia", "GA"));
            usDivisions.Add(new Division("Guam", "GU"));
            usDivisions.Add(new Division("Hawaii", "HI"));
            usDivisions.Add(new Division("Iowa", "IA"));
            usDivisions.Add(new Division("Idaho", "ID"));
            usDivisions.Add(new Division("Illinois", "IL"));
            usDivisions.Add(new Division("Indiana", "IN"));
            usDivisions.Add(new Division("Kansas", "KS"));
            usDivisions.Add(new Division("Kentucky", "KY"));
            usDivisions.Add(new Division("Louisiana", "LA"));
            usDivisions.Add(new Division("Massachusetts", "MA"));
            usDivisions.Add(new Division("Maryland", "MD"));
            usDivisions.Add(new Division("Maine", "ME"));
            usDivisions.Add(new Division("Michigan", "MI"));
            usDivisions.Add(new Division("Minnesota", "MN"));
            usDivisions.Add(new Division("Missouri", "MO"));
            usDivisions.Add(new Division("Northern Mariana Islands", "MP"));
            usDivisions.Add(new Division("Mississippi", "MS"));
            usDivisions.Add(new Division("Montana", "MT"));
            usDivisions.Add(new Division("North Carolina", "NC"));
            usDivisions.Add(new Division("North Dakota", "ND"));
            usDivisions.Add(new Division("Nebraska", "NE"));
            usDivisions.Add(new Division("New Hampshire", "NH"));
            usDivisions.Add(new Division("New Jersey", "NJ"));
            usDivisions.Add(new Division("New Mexico", "NM"));
            usDivisions.Add(new Division("Nevada", "NV"));
            usDivisions.Add(new Division("New York", "NY"));
            usDivisions.Add(new Division("Ohio", "OH"));
            usDivisions.Add(new Division("Oklahoma", "OK"));
            usDivisions.Add(new Division("Oregon", "OR"));
            usDivisions.Add(new Division("Pennsylvania", "PA"));
            usDivisions.Add(new Division("Puerto Ric", "PR"));
            usDivisions.Add(new Division("Rhode Island", "RI"));
            usDivisions.Add(new Division("South Carolina", "SC"));
            usDivisions.Add(new Division("South Dakota", "SD"));
            usDivisions.Add(new Division("Tennessee", "TN"));
            usDivisions.Add(new Division("Texas", "TX"));
            usDivisions.Add(new Division("U.S. Minor Outlying Islands", "UM"));
            usDivisions.Add(new Division("Utah", "UT"));
            usDivisions.Add(new Division("Virginia", "VA"));
            usDivisions.Add(new Division("Virgin Islands of the U.S.", "VI"));
            usDivisions.Add(new Division("Vermont", "VT"));
            usDivisions.Add(new Division("Washington", "WA"));
            usDivisions.Add(new Division("Wisconsin", "WI"));
            usDivisions.Add(new Division("West Virginia", "WV"));
            usDivisions.Add(new Division("Wyoming", "WY"));
            us.Divisions = usDivisions;
            Countries.Add(us);

            var ca = new SupportedCountry();
            ca.Name = "Canada";
            ca.Code = "CA";
            ca.DivisionName = "Province";
            var caDivisions = new List<Division>();
            caDivisions.Add(new Division("Alberta", "AB"));
            caDivisions.Add(new Division("British Columbia", "BC"));
            caDivisions.Add(new Division("Manitoba", "MB"));
            caDivisions.Add(new Division("New Brunswick", "NB"));
            caDivisions.Add(new Division("Newfoundland and Labrador", "NL"));
            caDivisions.Add(new Division("Nova Scotia", "NS"));
            caDivisions.Add(new Division("Northwest Territories", "NT"));
            caDivisions.Add(new Division("Nunavut", "NU"));
            caDivisions.Add(new Division("Ontario", "ON"));
            caDivisions.Add(new Division("Prince Edward Island", "PE"));
            caDivisions.Add(new Division("Quebec", "QC"));
            caDivisions.Add(new Division("Saskatchewan", "SK"));
            caDivisions.Add(new Division("Yukon Territory", "YT"));
            ca.Divisions = caDivisions;
            Countries.Add(ca);

            var gb = new SupportedCountry();
            gb.Name = "UK";
            gb.Code = "GB";
            gb.DivisionName = "Region";
            var gbDivisions = new List<Division>();
            gbDivisions.Add(new Division("Aberdeenshire", "ABD"));
            gbDivisions.Add(new Division("Aberdeen City", "ABE"));
            gbDivisions.Add(new Division("Argyll and Bute", "AGB"));
            gbDivisions.Add(new Division("Isle of Anglese", "AGY"));
            gbDivisions.Add(new Division("Angus", "ANS"));
            gbDivisions.Add(new Division("Antrim", "ANT"));
            gbDivisions.Add(new Division("Ards", "ARD"));
            gbDivisions.Add(new Division("Armagh", "ARM"));
            gbDivisions.Add(new Division("Bath and North East Somerset", "BAS"));
            gbDivisions.Add(new Division("Blackburn with Darwen", "BBD"));
            gbDivisions.Add(new Division("Bedfordshire", "BDF"));
            gbDivisions.Add(new Division("Barking and Dagenham", "BDG"));
            gbDivisions.Add(new Division("Brent", "BEN"));
            gbDivisions.Add(new Division("Bexley", "BEX"));
            gbDivisions.Add(new Division("Belfast", "BFS"));
            gbDivisions.Add(new Division("Bridgend", "BGE"));
            gbDivisions.Add(new Division("Blaenau Gwent", "BGW"));
            gbDivisions.Add(new Division("Birmingham", "BIR"));
            gbDivisions.Add(new Division("Buckinghamshire", "BKM"));
            gbDivisions.Add(new Division("Ballymena", "BLA"));
            gbDivisions.Add(new Division("Ballymoney", "BLY"));
            gbDivisions.Add(new Division("Bournemouth", "BMH"));
            gbDivisions.Add(new Division("Banbridge", "BNB"));
            gbDivisions.Add(new Division("Barnet", "BNE"));
            gbDivisions.Add(new Division("Brighton and Hove", "BNH"));
            gbDivisions.Add(new Division("Barnsley", "BNS"));
            gbDivisions.Add(new Division("Bolton", "BOL"));
            gbDivisions.Add(new Division("Blackpool", "BPL"));
            gbDivisions.Add(new Division("Bracknell Forest", "BRC"));
            gbDivisions.Add(new Division("Bradford", "BRD"));
            gbDivisions.Add(new Division("Bromley", "BRY"));
            gbDivisions.Add(new Division("Bristol, City of", "BST"));
            gbDivisions.Add(new Division("Bury", "BUR"));
            gbDivisions.Add(new Division("Cambridgeshire", "CAM"));
            gbDivisions.Add(new Division("Caerphill", "CAY"));
            gbDivisions.Add(new Division("Central Bedfordshire", "CBF"));
            gbDivisions.Add(new Division("Ceredigion", "CGN"));
            gbDivisions.Add(new Division("Craigavon", "CGV"));
            gbDivisions.Add(new Division("Cheshire East", "CHE"));
            gbDivisions.Add(new Division("Cheshire West and Chester", "CHW"));
            gbDivisions.Add(new Division("Carrickfergus", "CKF"));
            gbDivisions.Add(new Division("Cookstown", "CKT"));
            gbDivisions.Add(new Division("Calderdale", "CLD"));
            gbDivisions.Add(new Division("Clackmannanshire", "CLK"));
            gbDivisions.Add(new Division("Coleraine", "CLR"));
            gbDivisions.Add(new Division("Cumbria", "CMA"));
            gbDivisions.Add(new Division("Camden", "CMD"));
            gbDivisions.Add(new Division("Carmarthenshire", "CMN"));
            gbDivisions.Add(new Division("Cornwall", "CON"));
            gbDivisions.Add(new Division("Coventry", "COV"));
            gbDivisions.Add(new Division("Cardiff", "CRF"));
            gbDivisions.Add(new Division("Croydon", "CRY"));
            gbDivisions.Add(new Division("Castlereagh", "CSR"));
            gbDivisions.Add(new Division("Conwy", "CWY"));
            gbDivisions.Add(new Division("Darlington", "DAL"));
            gbDivisions.Add(new Division("Derbyshire", "DBY"));
            gbDivisions.Add(new Division("Denbighshire", "DEN"));
            gbDivisions.Add(new Division("Derby", "DER"));
            gbDivisions.Add(new Division("Devon", "DEV"));
            gbDivisions.Add(new Division("Dungannon", "DGN"));
            gbDivisions.Add(new Division("Dumfries and Galloway", "DGY"));
            gbDivisions.Add(new Division("Doncaster", "DNC"));
            gbDivisions.Add(new Division("Dundee City", "DND"));
            gbDivisions.Add(new Division("Dorset", "DOR"));
            gbDivisions.Add(new Division("Down", "DOW"));
            gbDivisions.Add(new Division("Derry", "DRY"));
            gbDivisions.Add(new Division("Dudley", "DUD"));
            gbDivisions.Add(new Division("Durham", "DUR"));
            gbDivisions.Add(new Division("Ealing", "EAL"));
            gbDivisions.Add(new Division("East Ayrshire", "EAY"));
            gbDivisions.Add(new Division("Edinburgh, City of", "EDH"));
            gbDivisions.Add(new Division("East Dunbartonshire", "EDU"));
            gbDivisions.Add(new Division("East Lothian", "ELN"));
            gbDivisions.Add(new Division("Eilean Siar", "ELS"));
            gbDivisions.Add(new Division("Enfield", "ENF"));
            gbDivisions.Add(new Division("East Renfrewshire", "ERW"));
            gbDivisions.Add(new Division("East Riding of Yorkshire", "ERY"));
            gbDivisions.Add(new Division("Essex", "ESS"));
            gbDivisions.Add(new Division("East Sussex", "ESX"));
            gbDivisions.Add(new Division("Falkirk", "FAL"));
            gbDivisions.Add(new Division("Fermanagh", "FER"));
            gbDivisions.Add(new Division("Fife", "FIF"));
            gbDivisions.Add(new Division("Flintshir", "FLN"));
            gbDivisions.Add(new Division("Gateshead", "GAT"));
            gbDivisions.Add(new Division("Glasgow City", "GLG"));
            gbDivisions.Add(new Division("Gloucestershire", "GLS"));
            gbDivisions.Add(new Division("Greenwich", "GRE"));
            gbDivisions.Add(new Division("Gwynedd", "GWN"));
            gbDivisions.Add(new Division("Halton", "HAL"));
            gbDivisions.Add(new Division("Hampshire", "HAM"));
            gbDivisions.Add(new Division("Havering", "HAV"));
            gbDivisions.Add(new Division("Hackney", "HCK"));
            gbDivisions.Add(new Division("Herefordshire, County of", "HEF"));
            gbDivisions.Add(new Division("Hillingdon", "HIL"));
            gbDivisions.Add(new Division("Highland", "HLD"));
            gbDivisions.Add(new Division("Hammersmith and Fulham", "HMF"));
            gbDivisions.Add(new Division("Hounslow", "HNS"));
            gbDivisions.Add(new Division("Hartlepool", "HPL"));
            gbDivisions.Add(new Division("Hertfordshire", "HRT"));
            gbDivisions.Add(new Division("Harrow", "HRW"));
            gbDivisions.Add(new Division("Haringey", "HRY"));
            gbDivisions.Add(new Division("Isle of Wight", "IOW"));
            gbDivisions.Add(new Division("Islington", "ISL"));
            gbDivisions.Add(new Division("Inverclyde", "IVC"));
            gbDivisions.Add(new Division("Kensington and Chelsea", "KEC"));
            gbDivisions.Add(new Division("Kent", "KEN"));
            gbDivisions.Add(new Division("Kingston upon Hull, City of", "KHL"));
            gbDivisions.Add(new Division("Kirklees", "KIR"));
            gbDivisions.Add(new Division("Kingston upon Thames", "KTT"));
            gbDivisions.Add(new Division("Knowsley", "KWL"));
            gbDivisions.Add(new Division("Lancashire", "LAN"));
            gbDivisions.Add(new Division("Lambeth", "LBH"));
            gbDivisions.Add(new Division("Leicester", "LCE"));
            gbDivisions.Add(new Division("Leeds", "LDS"));
            gbDivisions.Add(new Division("Leicestershire", "LEC"));
            gbDivisions.Add(new Division("Lewisham", "LEW"));
            gbDivisions.Add(new Division("Lincolnshire", "LIN"));
            gbDivisions.Add(new Division("Liverpool", "LIV"));
            gbDivisions.Add(new Division("Limavady", "LMV"));
            gbDivisions.Add(new Division("London, City of", "LND"));
            gbDivisions.Add(new Division("Larne", "LRN"));
            gbDivisions.Add(new Division("Lisburn", "LSB"));
            gbDivisions.Add(new Division("Luton", "LUT"));
            gbDivisions.Add(new Division("Manchester", "MAN"));
            gbDivisions.Add(new Division("Middlesbrough", "MDB"));
            gbDivisions.Add(new Division("Medway", "MDW"));
            gbDivisions.Add(new Division("Magherafelt", "MFT"));
            gbDivisions.Add(new Division("Milton Keynes", "MIK"));
            gbDivisions.Add(new Division("Midlothian", "MLN"));
            gbDivisions.Add(new Division("Monmouthshire", "MON"));
            gbDivisions.Add(new Division("Merton", "MRT"));
            gbDivisions.Add(new Division("Moray", "MRY"));
            gbDivisions.Add(new Division("Merthyr Tydfil", "MTY"));
            gbDivisions.Add(new Division("Moyle", "MYL"));
            gbDivisions.Add(new Division("North Ayrshire", "NAY"));
            gbDivisions.Add(new Division("Northumberland", "NBL"));
            gbDivisions.Add(new Division("North Down", "NDN"));
            gbDivisions.Add(new Division("North East Lincolnshire", "NEL"));
            gbDivisions.Add(new Division("Newcastle upon Tyne", "NET"));
            gbDivisions.Add(new Division("Norfolk", "NFK"));
            gbDivisions.Add(new Division("Nottingham", "NGM"));
            gbDivisions.Add(new Division("North Lanarkshire", "NLK"));
            gbDivisions.Add(new Division("North Lincolnshire", "NLN"));
            gbDivisions.Add(new Division("North Somerset", "NSM"));
            gbDivisions.Add(new Division("Newtownabbey", "NTA"));
            gbDivisions.Add(new Division("Northamptonshire", "NTH"));
            gbDivisions.Add(new Division("Neath Port Talbo", "NTL"));
            gbDivisions.Add(new Division("Nottinghamshire", "NTT"));
            gbDivisions.Add(new Division("North Tyneside", "NTY"));
            gbDivisions.Add(new Division("Newham", "NWM"));
            gbDivisions.Add(new Division("Newport", "NWP"));
            gbDivisions.Add(new Division("North Yorkshire", "NYK"));
            gbDivisions.Add(new Division("Newry and Mourne", "NYM"));
            gbDivisions.Add(new Division("Oldham", "OLD"));
            gbDivisions.Add(new Division("Omagh", "OMH"));
            gbDivisions.Add(new Division("Orkney Islands", "ORK"));
            gbDivisions.Add(new Division("Oxfordshire", "OXF"));
            gbDivisions.Add(new Division("Pembrokeshir", "PEM"));
            gbDivisions.Add(new Division("Perth and Kinross", "PKN"));
            gbDivisions.Add(new Division("Plymouth", "PLY"));
            gbDivisions.Add(new Division("Poole", "POL"));
            gbDivisions.Add(new Division("Portsmouth", "POR"));
            gbDivisions.Add(new Division("Powys", "POW"));
            gbDivisions.Add(new Division("Peterborough", "PTE"));
            gbDivisions.Add(new Division("Redcar and Cleveland", "RCC"));
            gbDivisions.Add(new Division("Rochdale", "RCH"));
            gbDivisions.Add(new Division("Rhondda, Cynon, Taff", "RCT"));
            gbDivisions.Add(new Division("Redbridge", "RDB"));
            gbDivisions.Add(new Division("Reading", "RDG"));
            gbDivisions.Add(new Division("Renfrewshire", "RFW"));
            gbDivisions.Add(new Division("Richmond upon Thames", "RIC"));
            gbDivisions.Add(new Division("Rotherham", "ROT"));
            gbDivisions.Add(new Division("Rutland", "RUT"));
            gbDivisions.Add(new Division("Sandwell", "SAW"));
            gbDivisions.Add(new Division("South Ayrshire", "SAY"));
            gbDivisions.Add(new Division("Scottish Borders, The", "SCB"));
            gbDivisions.Add(new Division("Suffolk", "SFK"));
            gbDivisions.Add(new Division("Sefton", "SFT"));
            gbDivisions.Add(new Division("South Gloucestershire", "SGC"));
            gbDivisions.Add(new Division("Sheffield", "SHF"));
            gbDivisions.Add(new Division("St. Helens", "SHN"));
            gbDivisions.Add(new Division("Shropshire", "SHR"));
            gbDivisions.Add(new Division("Stockport", "SKP"));
            gbDivisions.Add(new Division("Salford", "SLF"));
            gbDivisions.Add(new Division("Slough", "SLG"));
            gbDivisions.Add(new Division("South Lanarkshire", "SLK"));
            gbDivisions.Add(new Division("Sunderland", "SND"));
            gbDivisions.Add(new Division("Solihull", "SOL"));
            gbDivisions.Add(new Division("Somerset", "SOM"));
            gbDivisions.Add(new Division("Southend-on-Sea", "SOS"));
            gbDivisions.Add(new Division("Surrey", "SRY"));
            gbDivisions.Add(new Division("Strabane", "STB"));
            gbDivisions.Add(new Division("Stoke-on-Trent", "STE"));
            gbDivisions.Add(new Division("Stirling", "STG"));
            gbDivisions.Add(new Division("Southampton", "STH"));
            gbDivisions.Add(new Division("Sutton", "STN"));
            gbDivisions.Add(new Division("Staffordshire", "STS"));
            gbDivisions.Add(new Division("Stockton-on-Tees", "STT"));
            gbDivisions.Add(new Division("South Tyneside", "STY"));
            gbDivisions.Add(new Division("Swansea", "SWA"));
            gbDivisions.Add(new Division("Swindon", "SWD"));
            gbDivisions.Add(new Division("Southwark", "SWK"));
            gbDivisions.Add(new Division("Tameside", "TAM"));
            gbDivisions.Add(new Division("Telford and Wrekin", "TFW"));
            gbDivisions.Add(new Division("Thurrock", "THR"));
            gbDivisions.Add(new Division("Torbay", "TOB"));
            gbDivisions.Add(new Division("Torfaen", "TOF"));
            gbDivisions.Add(new Division("Trafford", "TRF"));
            gbDivisions.Add(new Division("Tower Hamlets", "TWH"));
            gbDivisions.Add(new Division("Vale of Glamorgan, The", "VGL"));
            gbDivisions.Add(new Division("Warwickshire", "WAR"));
            gbDivisions.Add(new Division("West Berkshire", "WBK"));
            gbDivisions.Add(new Division("West Dunbartonshire", "WDU"));
            gbDivisions.Add(new Division("Waltham Forest", "WFT"));
            gbDivisions.Add(new Division("Wigan", "WGN"));
            gbDivisions.Add(new Division("Wiltshire", "WIL"));
            gbDivisions.Add(new Division("Wakefield", "WKF"));
            gbDivisions.Add(new Division("Walsall", "WLL"));
            gbDivisions.Add(new Division("West Lothian", "WLN"));
            gbDivisions.Add(new Division("Wolverhampton", "WLV"));
            gbDivisions.Add(new Division("Wandsworth", "WND"));
            gbDivisions.Add(new Division("Windsor and Maidenhead", "WNM"));
            gbDivisions.Add(new Division("Wokingham", "WOK"));
            gbDivisions.Add(new Division("Worcestershire", "WOR"));
            gbDivisions.Add(new Division("Wirral", "WRL"));
            gbDivisions.Add(new Division("Warrington", "WRT"));
            gbDivisions.Add(new Division("Wrexham", "WRX"));
            gbDivisions.Add(new Division("Westminster", "WSM"));
            gbDivisions.Add(new Division("West Sussex", "WSX"));
            gbDivisions.Add(new Division("York", "YOR"));
            gbDivisions.Add(new Division("Shetland Islands", "ZET"));
            gb.Divisions = gbDivisions;
            Countries.Add(gb);

            var de = new SupportedCountry();
            de.Name = "Germany";
            de.Code = "DE";
            de.DivisionName = "State";
            var deDivisions = new List<Division>();
            deDivisions.Add(new Division("Brandenburg", "BB"));
            deDivisions.Add(new Division("Berlin", "BE"));
            deDivisions.Add(new Division("Baden-Württemberg", "BW"));
            deDivisions.Add(new Division("Bayern", "BY"));
            deDivisions.Add(new Division("Bremen", "HB"));
            deDivisions.Add(new Division("Hessen", "HE"));
            deDivisions.Add(new Division("Hamburg", "HH"));
            deDivisions.Add(new Division("Mecklenburg-Vorpommern", "MV"));
            deDivisions.Add(new Division("Niedersachsen", "NI"));
            deDivisions.Add(new Division("Nordrhein-Westfalen", "NW"));
            deDivisions.Add(new Division("Rheinland-Pfalz", "RP"));
            deDivisions.Add(new Division("Schleswig-Holstein", "SH"));
            deDivisions.Add(new Division("Saarland", "SL"));
            deDivisions.Add(new Division("Sachsen", "SN"));
            deDivisions.Add(new Division("Sachsen-Anhalt", "ST"));
            deDivisions.Add(new Division("Thüringen", "TH"));
            de.Divisions = deDivisions;
            Countries.Add(de);



        }
    }

    
}