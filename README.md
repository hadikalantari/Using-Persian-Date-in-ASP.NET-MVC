# Using-Persian-Date-in-ASP.NET-MVC
Persian Date (Jalali) to the Gregorian date conversion and vice versa.

Usage:
The main class is PersianDateTime in ~/Models/PersianDateTime.cs
You can use the class like this:

  //for convert string of persianDateTime to DateTime
    var persianDateTimeString = "1393/01/08";
    PersianDateTime persianDateTime = PersianDateTime.Parse(persianDateTimeString);
    DateTime gregorianDatetime = persianDateTime.DateTime;

  //for convert gregorian to PersianDateTime
    PersianDateTime pdt = new PersianDateTime(DateTime.Now);



PersianDatepicker from here:
https://github.com/behzadi/persianDatepicker/
