using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersianDate.Models
{
    public class PersianDateTime
    {
        /// <summary>
        /// Days of Week in Persian
        /// شنبه: Saturday
        /// یکشنبه: Sunday
        /// دوشنبه: Monday
        /// سه شنبه: Tuesday
        /// چهارشنبه: Wednesday
        /// پنجشنبه: Thursday
        /// جمعه: Friday
        /// </summary>
        public enum DaysOfWeek { شنبه = 6, یکشنبه = 0, دوشنبه = 1, سه_شنبه = 2, چهارشنبه = 3, پنجشنبه = 4, جمعه = 5 };
        
        /// <summary>
        /// Month Names in Persian
        /// </summary>
        public enum MonthsOfYear { فروردین = 1, اردیبهشت = 2, خرداد = 3, تیر = 4, مرداد = 5, شهریور = 6, مهر = 7, آبان = 8, آذر = 9, دی = 10, بهمن = 11, اسفند = 12 };

        static System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
        DateTime gregorianDatetime;

        #region [Constructors]
        
        /// <summary>
        /// sets minimum supported date as default 
        /// </summary>
        public PersianDateTime()
        {
            gregorianDatetime = persianCalendar.MinSupportedDateTime;
        }

        /// <summary>
        /// Initiate PersianDateTime based on given Gregorian DateTime
        /// </summary>
        /// <param name="georgianDate">Gregorian DateTime</param>
        public PersianDateTime(DateTime georgianDate)
        {
            if (georgianDate < persianCalendar.MinSupportedDateTime) georgianDate = persianCalendar.MinSupportedDateTime;
            if (georgianDate > persianCalendar.MaxSupportedDateTime) georgianDate = persianCalendar.MaxSupportedDateTime;
            if (georgianDate < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue) georgianDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            if (georgianDate > (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue) georgianDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
            gregorianDatetime = georgianDate;
        }
        #endregion

        #region [Properties]
        /// <summary>
        /// Day of Month
        /// </summary>
        public int Day
        {
            get
            {
                return persianCalendar.GetDayOfMonth(gregorianDatetime);
            }
            set
            {
                SetDate(null, null, value, null, null, null, null);
            }
        }
        
        /// <summary>
        /// enum of Persian's name of days in week
        /// </summary>
        public DaysOfWeek DayOfWeek
        {
            get
            {
                return (DaysOfWeek)persianCalendar.GetDayOfWeek(gregorianDatetime);
            }
        }
        
        /// <summary>
        /// Count of Days in Month (31,30,29 or 28 days)
        /// </summary>
        public int DaysInMonth
        {
            get
            {
                return persianCalendar.GetDaysInMonth(Year, Month);
            }
        }
        
        /// <summary>
        /// Month of year
        /// </summary>
        public int Month
        {
            get
            {
                return persianCalendar.GetMonth(gregorianDatetime);
            }
            set
            {
                SetDate(null, value, null, null, null, null, null);
            }
        }
        
        /// <summary>
        /// enum of Persian's name of months
        /// </summary>
        public MonthsOfYear MonthOfYear
        {
            get
            {
                return (MonthsOfYear)Month;
            }
            set
            {
                Month = (int)value;
            }
        }
        
        /// <summary>
        /// Persian Year
        /// </summary>
        public int Year
        {
            get
            {
                return persianCalendar.GetYear(gregorianDatetime);
            }
            set
            {
                SetDate(value, null, null, null, null, null, null);
            }
        }
        public int Hour
        {
            get
            {
                return persianCalendar.GetHour(gregorianDatetime);
            }
            set
            {
                SetDate(null, null, null, value, null, null, null);
            }
        }
        public int Minute
        {
            get
            {
                return persianCalendar.GetMinute(gregorianDatetime);
            }
            set
            {
                SetDate(null, null, null, null, value, null, null);
            }
        }
        public int Second
        {
            get
            {
                return persianCalendar.GetSecond(gregorianDatetime);
            }
            set
            {
                SetDate(null, null, null, null, null, value, null);
            }
        }
        public int Millisecond
        {
            get
            {
                return (int)persianCalendar.GetMilliseconds(gregorianDatetime);
            }
            set
            {
                SetDate(null, null, null, null, null, null, value);
            }
        }
        
        /// <summary>
        /// Gregorian DateTime
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return gregorianDatetime;
            }
            set
            {
                gregorianDatetime = value;
            }
        }
        #endregion

        #region [Methods]

        /// <summary>
        /// Convert string to PersianDateTime
        /// Supports only these formats:
        /// 1370/01/01
        /// 1370/1/1
        /// 1370-01-01
        /// 1370-1-1
        /// 1370/01/01 12:00:00
        /// 1370/1/1 12:00:00
        /// 1370-01-01 12:00:00
        /// 1370-1-1 12:00:00
        /// </summary>
        /// <param name="Input">Persian DateTime String</param>
        /// <returns>Gregorian DateTime</returns>
        public static PersianDateTime Parse(string Input)
        {
            PersianDateTime PersianDate = new PersianDateTime();
            if (!String.IsNullOrEmpty(Input))
            {
                /*default persian date*/
                var persianYear = PersianDate.Year;
                var persianMonth = PersianDate.Month;
                var persianDay = PersianDate.Day;
                var persianHour = PersianDate.Hour;
                var persianMinute = PersianDate.Minute;
                var persianSecond = PersianDate.Second;
                var persianMilisecond = PersianDate.Millisecond;

                string datePart = null;
                string timePart = null;
                if (Input.Split().Length == 1)
                {
                    datePart = Input;
                }
                else if (Input.Split().Length == 2)
                {
                    //supposed to be dateTime
                    string[] parts = Input.Split();
                    datePart = parts[0];
                    timePart = parts[1];
                }
                else
                {
                    //not valid format
                    throw new Exception(PersianConversionFail(Input));
                }

                if (!String.IsNullOrEmpty(datePart))
                {
                    //Parse date part
                    string[] parts = datePart.Split('/', '-');
                    try
                    {
                        switch (parts.Length)
                        {
                            case 3:
                                persianYear = Convert.ToInt32(parts[0]);
                                persianMonth = Convert.ToInt32(parts[1]);
                                persianDay = Convert.ToInt32(parts[2]);
                                break;
                            case 2:
                                persianYear = Convert.ToInt32(parts[0]);
                                persianMonth = Convert.ToInt32(parts[1]);
                                break;
                            case 1:
                                persianYear = Convert.ToInt32(parts[0]);
                                break;
                            default:
                                throw new Exception(PersianConversionFail(Input));
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception(PersianConversionFail(Input));
                    }

                }

                if (!String.IsNullOrEmpty(timePart))
                {
                    //parsing time part
                    try
                    {
                        var result = Convert.ToDateTime(timePart);
                        persianHour = result.Hour;
                        persianMinute = result.Minute;
                        persianSecond = result.Second;
                        persianMilisecond = result.Millisecond;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(PersianConversionFail(Input), ex);
                    }
                }
                PersianDate.Year = persianYear;
                PersianDate.Month = persianMonth;
                PersianDate.Day = persianDay;
                PersianDate.Hour = persianHour;
                PersianDate.Minute = persianMinute;
                PersianDate.Second = persianSecond;
                PersianDate.Millisecond = persianMilisecond;
            }
            else
            {
                throw new Exception(PersianConversionFail(Input));
            }
            return PersianDate;
        }
        
        /// <summary>
        /// Convert PersianDateTime to string with default format ({YYYY}/{MM}/{DD} {hh}:{mm}:{ss})
        /// </summary>
        /// <returns>string of PersianDateTime</returns>
        public override string ToString()
        {
            return ToString("{YYYY}/{MM}/{DD} {hh}:{mm}:{ss}");
        }

        /// <summary>
        /// This Method Format the output date based on input format
        /// </summary>
        /// <param name="Format">
        /// {YYYY} four-digit year (eg 1387)
        /// {YY} two-digit year (eg 87),
        /// {MM} two-digit month (01=فروردین, etc.),
        /// {Mn} name of month (eg فروردین),
        /// {DD} two-digit day of month (01 through 31),
        /// {Dn} A full textual representation of the day of the week (شنبه to جمعه)
        /// {hh} two digits of hour (00 through 23) (صبح/عصر NOT allowed)
        /// {mm} two digits of minute (00 through 59)
        /// {ss} two digits of second (00 through 59)
        /// </param>
        /// <returns>Formatted string of PersianDateTime</returns>
        public string ToString(string Format)
        {
            string Com = "";

            Com = "{YYYY}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Year.ToString("D4"));
            Com = "{YY}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Year.ToString("D2").Substring(2));//((double)Year / 100).ToString().Substring(((double)Year / 100).ToString().IndexOf('.') + 1));
            Com = "{M}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Month.ToString());
            Com = "{MM}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Month.ToString("D2"));
            Com = "{Mn}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, GetName(MonthOfYear));
            Com = "{D}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Day.ToString());
            Com = "{DD}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Day.ToString("D2"));
            Com = "{Dn}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, GetName(DayOfWeek));
            Com = "{h}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Hour.ToString());
            Com = "{hh}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Hour.ToString("D2"));
            Com = "{m}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Minute.ToString());
            Com = "{mm}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Minute.ToString("D2"));
            Com = "{s}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Second.ToString());
            Com = "{ss}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Second.ToString("D2"));

            return Format;
        }
        
        /// <summary>
        /// Set PersianDate
        /// </summary>
        /// <param name="persianYear">Persian Year</param>
        /// <param name="persianMonth">Persian Month (1 to 12)</param>
        /// <param name="persianDay">Persian Day (1 to 31)</param>
        /// <param name="Hour"></param>
        /// <param name="Minute"></param>
        /// <param name="Second"></param>
        /// <param name="Millisecond"></param>
        void SetDate(int? persianYear, int? persianMonth, int? persianDay, int? Hour, int? Minute, int? Second, int? Millisecond)
        {
            if (!persianYear.HasValue)
                persianYear = persianCalendar.GetYear(gregorianDatetime);
            if (!persianMonth.HasValue)
                persianMonth = persianCalendar.GetMonth(gregorianDatetime);
            if (!persianDay.HasValue)
                persianDay = persianCalendar.GetDayOfMonth(gregorianDatetime);
            if (!Hour.HasValue)
                Hour = persianCalendar.GetHour(gregorianDatetime);
            if (!Minute.HasValue)
                Minute = persianCalendar.GetMinute(gregorianDatetime);
            if (!Second.HasValue)
                Second = persianCalendar.GetSecond(gregorianDatetime);
            if (!Millisecond.HasValue)
                Millisecond = (int)persianCalendar.GetMilliseconds(gregorianDatetime);
            gregorianDatetime = persianCalendar.ToDateTime(persianYear.Value, persianMonth.Value, persianDay.Value, Hour.Value, Minute.Value, Second.Value, Millisecond.Value);
        }

        /// <summary>
        ///    Returns a new PersianDateTime that adds the specified number of minutes
        ///    to the value of this instance.
        //
        /// Parameters:
        ///  value:
        ///    A number of whole and fractional minutes. The value parameter can be
        ///    negative or positive.
        ///
        /// Returns:
        ///    An object whose value is the sum of the date and time represented by this
        ///    instance and the number of minutes represented by value.
        /// </summary>
        /// <param name="value">Minutes</param>
        /// <returns>PersianDateTime</returns>
        public PersianDateTime AddMinutes(double value)
        {
            return new PersianDateTime(gregorianDatetime.AddMinutes(value));
        }

        /// <summary>
        ///     Returns a new PersianDateTime that adds the specified number of months to
        ///     the value of this instance.
        ///
        /// Parameters:
        ///   months:
        ///     A number of months. The months parameter can be negative or positive.
        ///
        /// Returns:
        ///     An object whose value is the sum of the date and time represented by this
        ///     instance and months.
        /// </summary>
        /// <param name="value">Months</param>
        /// <returns>PersianDateTime</returns>
        public PersianDateTime AddMonths(int value)
        {
            return new PersianDateTime(gregorianDatetime.AddMonths(value));
        }

        /// <summary>
        ///     Returns a new PersianDateTime that adds the specified number of days to the
        ///     value of this instance.
        ///
        /// Parameters:
        ///   value:
        ///     A number of whole and fractional days. The value parameter can be negative
        ///     or positive.
        ///
        /// Returns:
        ///     An object whose value is the sum of the date and time represented by this
        ///     instance and the number of days represented by value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>PersianDateTime</returns>
        public PersianDateTime AddDays(double value)
        {
            return new PersianDateTime(gregorianDatetime.AddDays(value));
        }

        /// <summary>
        ///     Returns a new PersianDateTime that adds the specified number of hours to
        ///     the value of this instance.
        ///
        /// Parameters:
        ///   value:
        ///     A number of whole and fractional hours. The value parameter can be negative
        ///     or positive.
        ///
        /// Returns:
        ///     An object whose value is the sum of the date and time represented by this
        ///     instance and the number of hours represented by value.
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddHours(double value)
        {
            return new PersianDateTime(gregorianDatetime.AddHours(value));
        }

        /// <summary>
        ///     Returns a new PersianDateTime that adds the specified number of years to
        ///     the value of this instance.
        ///
        /// Parameters:
        ///   value:
        ///     A number of years. The value parameter can be negative or positive.
        ///
        /// Returns:
        ///     An object whose value is the sum of the date and time represented by this
        ///     instance and the number of years represented by value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddYears(int value)
        {
            return new PersianDateTime(gregorianDatetime.AddYears(value));
        }
        
        /// <summary>
        ///     Returns a new PersianDateTime that adds the specified number of milliseconds
        ///     to the value of this instance.
        ///
        /// Parameters:
        ///   value:
        ///     A number of whole and fractional milliseconds. The value parameter can be
        ///     negative or positive. Note that this value is rounded to the nearest integer.
        ///
        /// Returns:
        ///     An object whose value is the sum of the date and time represented by this
        ///     instance and the number of milliseconds represented by value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PersianDateTime AddMilliseconds(double value)
        {
            return new PersianDateTime(gregorianDatetime.AddMilliseconds(value));
        }

        #endregion

        #region [Statics]
        
        /// <summary>
        ///     Gets a PersianDateTime object that is set to the current date and time on
        ///     this computer, expressed as the local time.
        ///
        /// Returns:
        ///     An object whose value is the current local date and time (in Persian).
        /// </summary>
        public static PersianDateTime Now
        {
            get
            {
                return new PersianDateTime(DateTime.Now);
            }
        }

        /// <summary>
        ///     Returns the number of days in the specified month and year.
        /// </summary>
        /// <param name="Year">The year</param>
        /// <param name="Month">The enum of MonthOfYear.</param>
        /// <returns>The number of days in month for the specified year.For example, if month
        ///     equals 2 for اردیبهشت, the return value is 31
        ///</returns>
        public static int GetDaysInMonth(int Year, MonthsOfYear Month)
        {
            System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();
            return PC.GetDaysInMonth(Year, (int)Month);
        }
        
        /// <summary>
        ///     Returns the number of days in the specified month and year.
        /// </summary>
        /// <param name="Year">The year</param>
        /// <param name="Month">The month (a number ranging from 1 to 12).</param>
        /// <returns>The number of days in month for the specified year.For example, if month
        ///     equals 2 for اردیبهشت, the return value is 31
        ///</returns>
        public static int GetDaysInMonth(int Year, int Month)
        {
            System.Globalization.PersianCalendar PC = new System.Globalization.PersianCalendar();
            return PC.GetDaysInMonth(Year, Month);
        }
        
        /// <summary>
        ///     Returns Persian Name of Day in Week
        /// </summary>
        /// <param name="Day">enum of DaysOfWeek</param>
        /// <returns>Persian Name of Day in Week</returns>
        public static string GetName(DaysOfWeek Day)
        {
            if (Day.ToString().Contains("_"))
                return Day.ToString().Replace("_", " ");
            return Day.ToString();
        }
        
        /// <summary>
        ///     Returns Persian Name of Month
        /// </summary>
        /// <param name="Month">enum of MonthsOfYear</param>
        /// <returns>Persian Name of Month</returns>
        public static string GetName(MonthsOfYear Month)
        {
            return Month.ToString();
        }

        /// <summary>
        /// Generate string error when converting to PersianDateTime failed
        /// </summary>
        /// <param name="Input">
        ///
        /// </param>
        /// <returns>string error when converting to PersianDateTime failed</returns>
        static string PersianConversionFail(string Input)
        {
            string Temp = "";
            Temp = "Convert \"{0}\" to Persian DateTime was unsuccessful.\r\n";
            return string.Format(Temp, Input);
        }

        /// <summary>
        /// The earliest date and time supported by the PersianDateTime
        /// </summary>
        public static PersianDateTime MinValue = new PersianDateTime(persianCalendar.MinSupportedDateTime);
        
        /// <summary>
        ///     The latest date and time supported by the PersianDateTime
        /// </summary>
        public static PersianDateTime MaxValue = new PersianDateTime(persianCalendar.MaxSupportedDateTime);
        
        /// <summary>
        ///     Converts specified gregorian DateTime to string of PersianDateTime with 
        ///     specified format.
        /// </summary>
        /// <param name="Date">
        ///     The gregorian DateTime
        /// </param>
        /// <param name="Format">
        ///     The specified format for PersianDateTime
        ///     {YYYY} four-digit year (eg 1387)
        ///     {YY} two-digit year (eg 87),
        ///     {MM} two-digit month (01=فروردین, etc.),
        ///     {Mn} name of month (eg فروردین),
        ///     {DD} two-digit day of month (01 through 31),
        ///     {Dn} A full textual representation of the day of the week (شنبه to جمعه)
        ///     {hh} two digits of hour (00 through 23) (صبح/عصر NOT allowed)
        ///     {mm} two digits of minute (00 through 59)
        ///     {ss} two digits of second (00 through 59)
        /// </param>
        /// <returns>
        ///     String of PersianDateTime with specified format
        /// </returns>
        public static string DateTimeToString(DateTime Date, string Format)
        {
            string Com = "";

            Com = "{YYYY}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Date.Year.ToString("D4"));
            Com = "{YY}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, ((double)Date.Year / 100).ToString().Substring(((double)Date.Year / 100).ToString().IndexOf('.') + 1));
            Com = "{MM}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Date.Month.ToString("D2"));
            Com = "{DD}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Date.Day.ToString("D2"));
            Com = "{hh}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Date.Hour.ToString("D2"));
            Com = "{mm}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Date.Minute.ToString("D2"));
            Com = "{ss}";
            if (Format.Contains(Com))
                Format = Format.Replace(Com, Date.Second.ToString("D2"));

            return Format;
        }
        
        /// <summary>
        ///     Converts specified gregorian DateTime to string of PersianDateTime with 
        ///     default format: {YYYY}-{MM}-{DD} {hh}:{mm}:{ss}
        /// </summary>
        /// <param name="Date">
        ///     The gregorian DateTime
        /// </param>
        /// <returns>
        ///     String of PersianDateTime with default format: 
        ///     {YYYY}-{MM}-{DD} {hh}:{mm}:{ss}
        /// </returns>
        public static string DateTimeToString(DateTime Date)
        {
            return DateTimeToString(Date, "{YYYY}-{MM}-{DD} {hh}:{mm}:{ss}");
        }

        
        /// <summary>
        ///     Returns string of PersianDateTime in specified format.
        /// </summary>
        /// <param name="Format">
        ///     The specified format for PersianDateTime
        ///     {YYYY} four-digit year (eg 1387)
        ///     {YY} two-digit year (eg 87),
        ///     {MM} two-digit month (01=فروردین, etc.),
        ///     {Mn} name of month (eg فروردین),
        ///     {DD} two-digit day of month (01 through 31),
        ///     {Dn} A full textual representation of the day of the week (شنبه to جمعه)
        ///     {hh} two digits of hour (00 through 23) (صبح/عصر NOT allowed)
        ///     {mm} two digits of minute (00 through 59)
        ///     {ss} two digits of second (00 through 59)
        /// </param>
        /// <returns>
        ///     String of PersianDateTime in specified format
        /// </returns>
        public string ToStringDateTime(string Format)
        {
            return PersianDateTime.DateTimeToString(DateTime, Format);
        }

        /// <summary>
        ///     Returns string of PersianDateTime with 
        ///     default format: {YYYY}-{MM}-{DD} {hh}:{mm}:{ss}
        /// </summary>
        /// <returns>
        ///     String of PersianDateTime with default format: 
        ///     {YYYY}-{MM}-{DD} {hh}:{mm}:{ss}
        /// </returns>
        public string ToStringDateTime()
        {
            return PersianDateTime.DateTimeToString(DateTime);
        }

        /// <summary>
        ///     Returns List of Persian name of Days of week
        /// </summary>
        /// <returns>
        ///     List of Persian name of Days of week
        /// </returns>
        public static List<string> GetDaysOfWeekList()
        {
            var list = new List<string>();
            list.AddRange(Enum.GetNames(typeof(DaysOfWeek)));
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Replace("_", " ");
            }
            return list;
        }
        
        /// <summary>
        ///     Returns list of Persian name of months
        /// </summary>
        /// <returns>
        ///     List of Persian name of months
        /// </returns>
        public static List<string> GetMonthsOfYearList()
        {
            var list = new List<string>();
            list.AddRange(Enum.GetNames(typeof(MonthsOfYear)));
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Replace("_", " ");
            }
            return list;
        }

        /// <summary>
        ///     parse string of Persian name of day in week (eg: شنبه) to enum of DaysOfWeek
        /// </summary>
        /// <param name="val">
        ///     string of Persian name of day in week (eg: شنبه)
        /// </param>
        /// <returns>
        ///     enum of DaysOfWeek
        /// </returns>
        public static DaysOfWeek ParseDaysOfWeek(string val)
        {
            val = val.Replace(" ", "_");
            return (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), val, true);
        }

        /// <summary>
        ///     parse string of Persian name of month (eg: فروردین) to enum of MonthsOfYear
        /// </summary>
        /// <param name="val">
        ///     string of Persian name of month (eg: فروردین)
        /// </param>
        /// <returns>
        ///     enum of MonthsOfYear
        /// </returns>
        public static MonthsOfYear ParseMonthsOfYear(string val)
        {
            val = val.Replace(" ", "_");
            return (MonthsOfYear)Enum.Parse(typeof(MonthsOfYear), val, true);
        }

        #endregion

        /// <summary>
        ///     The Equality operator determines whether two PersianDateTime values are equal 
        ///     by comparing their Gregorian DateTime.
        /// </summary>
        /// <param name="d1">
        ///     The first object to compare.
        /// </param>
        /// <param name="d2">
        ///     The second object to compare.
        /// </param>
        /// <returns>
        ///     true if d1 and d2 represent the same date and time; otherwise, false.
        /// </returns>
        public static bool operator ==(PersianDateTime d1, PersianDateTime d2)
        {
            if ((object)d1 == null && (object)d2 == null)
            {
                return true;
            }
            if ((object)d1 == null || (object)d2 == null)
            {
                return false;
            }
            return d1.DateTime == d2.DateTime;
        }

        /// <summary>
        ///     The Inequality operator determines whether two PersianDateTime values are not equal.
        /// </summary>
        /// <param name="d1">
        ///     The first object to compare.
        /// </param>
        /// <param name="d2">
        ///     The second object to compare.
        /// </param>
        /// <returns>
        ///     true if d1 and d2 do not represent the same date and time; otherwise, false.
        /// </returns>
 
        public static bool operator !=(PersianDateTime d1, PersianDateTime d2)
        {
            return !(d1 == d2);
        }

        /// <summary>
        ///     The type of comparison between the current instance and the obj parameter.
        /// </summary>
        /// <param name="obj">
        ///     The object to compare with the current object.
        /// </param>
        /// <returns>
        ///     true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            var item = obj as PersianDateTime;

            if (item == null)
            {
                return false;
            }

            return this.DateTime.Equals(item.DateTime);
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return this.DateTime.GetHashCode();
        }
    }
}