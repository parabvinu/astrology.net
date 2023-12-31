using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SwissEphNet;
using CosineKitty;
using TA;


namespace webapp.Controllers
{
    public class HomeController : Controller
    {
      public string dtodms(double degree)
        {
           
            int degrees = (int)degree;
            int deg = degrees % 30;
           double minutes = (degree - degrees) * 60;
            double seconds = Math.Truncate((minutes - (int)minutes) * 60);
            string dms = string.Format("{0}°{1}'{2}\"", deg, (int)minutes, seconds.ToString("F2"));
            return dms;
        }
        public ActionResult Index()
        {
            SwissEph sweph = new SwissEph();
            sweph.swe_set_sid_mode(5,  0,  0);
            // ViewBag.path = sweph.swe_get_library_path();
            DateTime currentDate = DateTime.UtcNow; // Use UTC time to avoid time zone issues
            int year = DateTime.UtcNow.Year;
            int month = DateTime.UtcNow.Month;
            int day = DateTime.UtcNow.Day;
            int hour = DateTime.UtcNow.Hour;
            int min = DateTime.UtcNow.Minute;
            int sec = DateTime.UtcNow.Second;
            double secs = DateTime.UtcNow.Second;
            double latitude = 19.0060 + 17 / 60.0;
            double longitude = 72.4877 + 5 / 60.0;
            string estwest = (longitude > 0 ? "E" : "W");
            string northsouth = (latitude > 0 ? "N" : "S");
            double[] cusps = new double[13];
            double[] ascmc = new double[10];

            DateTime today = DateTime.Now;
            long julianDate = ConvertToJulian(today);

            double hrs = hour + (min / 60) + (secs / 3600);
            double jdt = sweph.swe_julday(year,  month,  day, hrs, 1);
            double ayanut = sweph.swe_get_ayanamsa_ut(jdt);
            string[] dms = new string[13];
            int flags1 = 65536;
            int results = sweph.swe_houses(jdt, latitude, longitude,  'P',  cusps,  ascmc);
            for(int p = 0; p < 13; p++)
            {
                dms[p] = dtodms(cusps[p]);
            }
           
           
            flags1 = 2|65536|64|256;

            
            //DateTime normalDateTime = new DateTime(year, month, day, hour, min, sec);
            //double jdt = GetJulianDate(today);

            // sweph.swe_jdet_to_utc( SwissEph.SE_GREG_CAL,1,ref year,ref month, ref day, ref hour, ref min, ref sec);
            //double jdUTC = sweph.swe_utc_time(jdET, -1);

            // double julianDate = sweph.swe_julday(currentDate.Year, currentDate.Month, currentDate.Day,currentDate.Hour, 1);

            string[] planetEng = { "Sun", "Moon", "Mercury", "Venus", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto", "Rahu", "Ketu" };
            string[] planet = { "रवी", "चन्द्र", "बुध", "शुक्र", "मंगळ", "गुरु", "शनी", "हर्षल", "नेपच्युन", "प्लूटो", "राहू", "केतू" };
            string[] nakshatra = new string[12];
            string[] lord = new string[12];
            int[] sublords = new int[12];

            string[] rashi = {"मेष","वृषभ","मिथुन","कर्क","सिंह","कन्या","तूळ","वृश्चिक","धनु","मकर","कुंभ","मीन"};
            ViewBag.Sign = rashi[(int)(ascmc[0] / 30)];
            int[] pad = new int[12];
            int nIndx = 0;
            string[] degree = new string[12];
            string serr = string.Empty;
            double[] positions = new double[23];
            string[] retro = new string[23];

            int flags = 0;
            flags = SwissEph.SEFLG_SPEED | SwissEph.SEFLG_SWIEPH;
            int flag2 = SwissEph.SEFLG_SIDEREAL | SwissEph.SE_SIDM_FAGAN_BRADLEY;
            int result = 0;
            int pos = 0;
            string[] nakshatraseng = { "Ashwini", "Bharani", "Krittika", "Rohini", "Mrigashira", "Ardra", "Punarvasu", "Pushya", "Ashlesha", "Magha", "Purva Phalguni", "Uttara Phalguni", "Hasta", "Chitra", "Swati", "Vishakha", "Anuradha", "Jyeshtha", "Mula", "Purva Ashadha", "Uttara Ashadha", "Shravana", "Dhanishta", "Shatabhisha", "Purva Bhadrapada", "Uttara Bhadrapada", "Revati" };
            string[] nklord = { "केतू", "शुक्र", "रवी", "चन्द्र", "मंगळ", "राहू", "गुरु", "शनी", "बुध" };
            string[] nakshatras = { "अश्विनी", "भरणी", "कृत्तिका", "रोहिणी", "मृगशीर्ष", "आर्द्रा", "पुनर्वसू", "पुष्य", "आश्लेषा", "मघा", "पूर्वाफाल्गुनी", "उत्तराफाल्गुनी", "हस्त", "चित्रा", "स्वाती", "विशाखा", "अनुराधा", "ज्येष्ठा", "मूळ", "पूर्वाषाढा", "उत्तराषाढा", "श्रवण", "धनिष्ठा", "शततारका", "पूर्वाभाद्रपदा", "उत्तराभाद्रपदा", "रेवती" };
            int res = 0;
            int sublord = 0;
           
            //double[] cusps = new double[13];
            //double[] ascmc = new double[10];
            for (int i = 0; i < 12; i++)
            {
                if (i == 11)
                {// for ketu
                    result = sweph.swe_calc_ut(jdt, 10, flags1, positions, ref serr);
                    if (positions[0] < 180)
                    {
                        positions[0] = 180 - positions[0];
                    }
                    if (positions[0] > 180)
                    {
                        positions[0] = positions[0] - 180;
                    }
                    if (positions[0] == 180)
                    {
                        positions[0] = 0;
                    }
                    //nIndx = (int)(positions[0] / 13.33333333);
                    nIndx = (int)((positions[0]) / 13.33333);
                    nakshatra[i] = nakshatras[nIndx];
                    pad[i] = (int)(positions[0] % 4) + 1;
                    degree[i] = dtodms(positions[0]);
                    lord[i] = nklord[nIndx % 9];
                    pos = (int)(positions[0] / 30);
                    rashi[i] = rashi[pos];
                    if (positions[3] < 0)
                    {
                        retro[i] = "R";
                    }
                    else
                    {
                        retro[i] = "D";
                    }

                    break;
                }
                result = sweph.swe_calc_ut(jdt, i, flags1, positions, ref serr);
                
                //nIndx = (int)(positions[0] / 13.33333333);
                nIndx = (int)((positions[0])/13.33333);
                nakshatra[i] = nakshatras[nIndx];
                pad[i] = (int)(positions[0] % 4)+1 ;
                degree[i] = dtodms(positions[0]);
                lord[i] = nklord[nIndx % 9];
                pos =  (int)(positions[0] / 30);
                rashi[i] = rashi[pos];
                if (positions[3] < 0)
                {
                    retro[i] = "R";
                }
                else
                {
                    retro[i] = "D";
                }

                //sublords[i] = CalculateSublord(positions[0]);

            }
            res = sweph.swe_houses(jdt, latitude, longitude, 'P', cusps, ascmc);
            if (result >= 0)
            {
                result = sweph.swe_calc(2450009.5, 1, flags, positions, ref serr);
                ViewBag.Moon = positions;
                ViewBag.Planet = planet;
                ViewBag.Nakshtra = nakshatra;
                ViewBag.Pada = pad;
                ViewBag.Degree = degree;
                ViewBag.NakLord = lord;
                ViewBag.Rashi = rashi;
                //ViewBag.Subloard = sublords;
                ViewBag.Cusps = cusps;
                ViewBag.Retro = retro;
                ViewBag.Cusps = dms;
                ViewBag.Asc = ascmc[0];
            }
            else
            {
                // Handle errors
                ViewBag.Position=  serr;
            }
            ViewBag.doubles = julianDate;
            return View();
        }
        
        public static double GetJulianDate(DateTime dateTime)
        {
            // Calculate the fractional day
            double fractionalDay = dateTime.Hour / 24.0 + dateTime.Minute / (24.0 * 60.0) + dateTime.Second / (24.0 * 60.0 * 60.0) + dateTime.Millisecond / (24.0 * 60.0 * 60.0 * 1000.0);

            // Calculate the Julian date for the given date and time
            double a = (14 - dateTime.Month) / 12;
            double y = dateTime.Year + 4800 - a;
            double m = dateTime.Month + 12 * a - 3;
            double julianDate = dateTime.Day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045 + fractionalDay;

            return julianDate;
        }


        public static long ConvertToJulian(DateTime Date)
        {
            int Month = Date.Month;
            int Day = Date.Day;
            int Year = Date.Year;
            if (Month < 3)
            {
                Month = Month + 12;
                Year = Year - 1;
            }
            long JulianDay = Day + (153 * Month - 457) / 5 + 365 * Year + (Year / 4) - (Year / 100) + (Year / 400) + 1721119;
            return JulianDay;
        }

        
        public ActionResult About()
        {
            SwissEph swissEph = new SwissEph();
            swissEph.swe_set_ephe_path(null);


            int[] pad = new int[12];
            int nIndx = 0;
            double[] degree = new double[12];
            string serr = string.Empty;
            //double[] positions = new double[23];
            int year = 2023;
            int month = 10;
            int day = 12;
            double hour = 13;
            double minute = 45;
            double second = 30;
            double timezone = 0;
            string errMsg = "";
            bool isUniversalTime = false;
            DateTime dateTime = new DateTime(year, month, day, (int)hour, (int)minute, (int)second);

            double jdn = GetJulianDate(dateTime);

            int flag = SwissEph.SEFLG_SPEED | SwissEph.SEFLG_SWIEPH;
            int flag2 = SwissEph.SEFLG_SIDEREAL | SwissEph.SE_SIDM_FAGAN_BRADLEY;
            string[] nakshatras = { "अश्विनी", "भरणी", "कृत्तिका", "रोहिणी", "मृगशीर्ष", "आर्द्रा", "पुनर्वसू", "पुष्य", "आश्लेषा",
                "मघा", "पूर्वाफाल्गुनी", "उत्तराफाल्गुनी", "हस्त", "चित्रा", "स्वाती", "विशाखा", "अनुराधा", "ज्येष्ठा", 
                "मूळ", "पूर्वाषाढा", "उत्तराषाढा", "श्रवण", "धनिष्ठा", "शततारका", "पूर्वाभाद्रपदा", "उत्तराभाद्रपदा", "रेवती" };
            string[] planet = { "रवी", "चन्द्र", "बुध", "शुक्र", "मंगळ", "गुरु", "शनी", "हर्षल", "नेपच्युन", "प्लूटो", "राहू", "केतू" };
            string[] nklord = { "केतू", "शुक्र", "रवी", "चन्द्र", "मंगळ", "राहू", "गुरु", "शनी", "बुध"};
            string[] nakshatra = new string[12];

            double[] positions = new double[SwissEph.SE_NPLANETS];
            int result = 0;
            for (int i = 0; i < 12; i++)
            {
                swissEph.swe_calc_ut(jdn, i, flag, positions, ref errMsg);
                double moonDegree = positions[0];
            }


            for (int i = 0; i < 12; i++)
            {
                if (i == 11)
                {// for ketu
                    result = swissEph.swe_calc(jdn, 10, flag, positions, ref errMsg);
                    if (positions[0] < 180)
                    {
                        positions[0] = 180 - positions[0];
                    }
                    if (positions[0] > 180)
                    {
                        positions[0] = positions[0] - 180;
                    }
                    if (positions[0] == 180)
                    {
                        positions[0] = 0;
                    }
                    nIndx = (int)(positions[0] / 13.33333333);
                    nakshatra[i] = nakshatras[nIndx];
                    pad[i] = (int)(positions[0] % 4) + 1;
                    degree[i] = (positions[0] % 30);
                    
                    break;
                }
                result = swissEph.swe_calc(jdn, i, flag, positions, ref serr);
                nIndx = (int)(positions[0] % 13.33333333);
                nakshatra[i] = nakshatras[nIndx];
                pad[i] = (int)(positions[0] % 4) + 1;
                degree[i] = (positions[0] % 30);

            }

            if (result >= 0)
            {
                ViewBag.Planet = planet;
                ViewBag.Nakshtra = nakshatra;
                ViewBag.Pada = pad;
                ViewBag.Degree = degree;
            }
            else
            {
                // Handle errors
                ViewBag.Position = serr;
            }


            ViewBag.doubles = jdn;

            swissEph.swe_close();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Data()
        {
            AstroTime dt = null;
            DateTime date = new DateTime(1994, 04, 28, 4, 25,0);
            bool tt = CosineKitty.AstroTime.TryParse(date.ToString(), out dt);
            ViewBag.date = dt;
           // CosineKitty.Astronomy.BaryState(Body.Sun, AstroTime date);
             //CosineKitty.StateVector BaryState(Body.Moon, AstroTime time);

            return View();
        }
        //public static string[] GetGana(string nakshtra)
        //{
        //    string[] info;

            

        //    return info;
        //}
        private static readonly int[] CuspSequence = new int[] { 1, 6, 11, 4, 9, 2, 7, 12, 5, 10, 3, 8 }; // Cusp sequence in KP astrology

        private static readonly int[] SublordSequence = new int[] { 2, 7, 12, 5, 10, 3, 8, 1, 6, 11, 4, 9 }; // Sublord sequence in KP astrology

        public static int CalculateSublord(double planetDegree)
        {
            int signNumber = (int)(Math.Floor(planetDegree / 30) % 12) + 1; // Determine the sign number

            int cuspNumber = CuspSequence[signNumber - 1]; // Get the associated cusp number based on the sign

            double subDivisionDegree = planetDegree % 30; // Determine the degree within the sign

            int subDivisionNumber = (int)(subDivisionDegree / 2) + 1; // Determine the sub-division number

            int sublordNumber = (cuspNumber - 1) * 3 + subDivisionNumber; // Calculate the sublord number

            int finalSublordNumber = SublordSequence[sublordNumber - 1]; // Get the final sublord number based on the sequence

            return finalSublordNumber;
        }
    }
}
