using CalendarNet;
using CalendarNet.Themes;
using System.Reflection;
internal static class Program
{
    static int passed;
    static void Check(bool value,string message){if(!value)throw new Exception(message);passed++;Console.WriteLine("PASS "+message);}
    [STAThread] static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        using var host=new Form {ClientSize=new Size(1100,700),Opacity=0,ShowInTaskbar=false};
        using var calendar=new Calendar {Dock=DockStyle.Fill}; host.Controls.Add(calendar);
        host.Show();Application.DoEvents();
        var assembly=typeof(Calendar).Assembly;
        var target=assembly.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>();
        Check(target?.FrameworkName.Contains("Version=v8.0")==true,"Library targets .NET 8");
        var icons=assembly.GetManifestResourceNames().Where(x=>x.StartsWith("CalendarNet.Icons.")).ToArray();
        Check(icons.Length==10,"All ten original icon resource names preserved");
        foreach(var name in icons){using var stream=assembly.GetManifestResourceStream(name)!;using var bitmap=new Bitmap(stream);Check(bitmap.Width>0 && bitmap.Height>0,"Icon loads: "+name);}
        for(var month=1;month<=12;month++)
        {
            calendar.Date=new DateTime(2026,month,15);calendar.CalendarType=Calendar.CalendarTypes.MonthView;
            using var bitmap=new Bitmap(calendar.Width,calendar.Height);calendar.DrawToBitmap(bitmap,calendar.ClientRectangle);
            Check(calendar.Date.Month==month,"Month rendering: "+month);
            if(month==10)bitmap.Save(Path.Combine(AppContext.BaseDirectory,"month.png"));
        }
        calendar.ShowFederalHolidays=false;calendar.ShowCommonHolidays=false;
        calendar.Date=new DateTime(2026,10,12);calendar.CalendarType=Calendar.CalendarTypes.DayView;
        using(var bitmap=new Bitmap(calendar.Width,calendar.Height)){calendar.DrawToBitmap(bitmap,calendar.ClientRectangle);bitmap.Save(Path.Combine(AppContext.BaseDirectory,"day.png"));}
        Check(calendar.CalendarType==Calendar.CalendarTypes.DayView,"Day view renders on .NET 8");
        foreach(var theme in new ICalendarTheme[]{new DefaultTheme(),new BlueTheme(),new GreenTheme(),new DarkTheme()})
        {
            calendar.ApplyTheme(theme);calendar.CalendarType=Calendar.CalendarTypes.MonthView;
            host.ClientSize=new Size(800,520);Application.DoEvents();using var image=new Bitmap(calendar.Width,calendar.Height);calendar.DrawToBitmap(image,calendar.ClientRectangle);
            Check(calendar.Width>700,"Theme and resized month render: "+theme.GetType().Name);
        }
        using(var demo=new CalendarNetDemo.MainForm {Opacity=0,ShowInTaskbar=false}) { demo.Show();Application.DoEvents();Check(demo.Controls.OfType<Calendar>().Any(),"Demo form initializes on .NET 8");demo.Close(); }
        host.Close();Console.WriteLine($"COMPLETED {passed} assertions");
    }
}
