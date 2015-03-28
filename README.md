# Using-Persian-Date-in-ASP.NET-MVC
Persian Date (Jalali) to the Gregorian date conversion and vice versa.

##Usage:
The main class is PersianDateTime in ~/Models/PersianDateTime.cs
You can use the class like this:

####for convert string of persianDateTime to DateTime
```sh
    var persianDateTimeString = "1393/01/08";
    PersianDateTime persianDateTime = PersianDateTime.Parse(persianDateTimeString);
    DateTime gregorianDatetime = persianDateTime.DateTime;
```
####for convert gregorian to PersianDateTime
```sh
    PersianDateTime pdt = new PersianDateTime(DateTime.Now);
```

PersianDatepicker from here:
https://github.com/behzadi/persianDatepicker/

License
----
**Free Software**

##Screenshots
>Using PersianDatepicker
![Using PersianDatepicker](https://cloud.githubusercontent.com/assets/5028035/6880932/ac28c95e-d565-11e4-8349-097ff88af949.JPG)
>Converting PersianDateTime to GregorianDateTime
![Converting PersianDateTime to GregorianDateTime](https://cloud.githubusercontent.com/assets/5028035/6880933/b02322ac-d565-11e4-80ce-9b5e61a83581.JPG)
>Convert Gregorian to PersianDateTime
![Convert Gregorian to PersianDateTime](https://cloud.githubusercontent.com/assets/5028035/6880934/b295fef6-d565-11e4-8750-a7c0aa02c03d.JPG)
>Fields of PersianDateTime object
![Fields of PersianDateTime object](https://cloud.githubusercontent.com/assets/5028035/6880956/b38206b0-d566-11e4-9520-1dd5f1eb1555.JPG)
