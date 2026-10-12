# Calendar .NET

## .NET 8 Windows Forms build

The library and demo now target `net8.0-windows`. Build on Windows with the
.NET 8 SDK or a newer SDK that supports this target:

```powershell
dotnet build CalendarNet.sln --configuration Release
dotnet run --project CalendarNetDemo --configuration Release
```

See [the migration notes](docs/NET8-MIGRATION.md) for project changes and
deployment requirements. The original usage article below predates this
migration; its .NET Framework requirements refer to the original release.
## Introduction
Recently I had a need for a calendar control for an application I was writing. I could not find a good one that suited my needs (one of those needs was to be free or open source). So I decided to write my own control. Calendar .NET is a free calendar control that can render itself as either a month-to-month calendar or as a day-to-day calendar. In addition, events can be added to the calendar. These events can be one time only events or recurring events. The calendar has several built in recurring frequencies, as well as the ability to create your own frequencies. Finally, the calendar also comes preloaded with all Federal Holidays, which can be disabled if you wish.
## Using the code
Before diving into how the calendar's internal code works, I'd first like to discuss how to use the calendar. I've always tried to subscribe to the KISS (Keep It Simple Stupid) principle and in doing that, I feel I made the calendar control pretty easy and intuitive to use. The calendar control requires the .NET Framework, version 3.5 or higher. To start things off, all you need to do is create a new Windows Form Application in Visual Studio, making sure to target .NET 3.5 or higher. Then add a reference to the calendar control. After doing that, the calendar control should appear in your designer toolbox. Simply drag the calendar control onto your form and you should have a calendar appear on your form! The calendar control will render at almost any size and will even dynamically resize if it needs to.

The calendar control exposes many properties and I will describe each on in detail right now:

**AllowEditingEvents** - By default, Calendar .NET allows events to be right-clicked with the mouse and allows the event to be edited during runtime. This property is a bool property that can be set to  **true** if you want to allow events to be edited at runtime by the end user. Setting this property to  **false** will disable this feature and prevent the event from being edited by the end user.

**CalendarDate** - This property is a DateTime property that tells the calendar control what date to render. If the calendar control is rendering a month-to-month view, then the time component, as well as the day component of the DateTime value is ignored. Setting this property will cause the calendar control to instantly change dates to the date specified.

**CalendarView** - This property is an enumerated property that determines if the calendar should render itself as a month-to-month calendar or a day-to-day calendar. To make the calendar render as a month-to-month calendar, you would set this value to CalendarViews.Month. If you want a day-to-day view, you would set this value to CalendarViews.Day.

**DateHeaderFont** - This property takes a Font object and it uses this font to render the CalendarDate in the upper right hand corner. Displaying this header can be disabled using the ShowDateInHeader property described below.

**DayOfWeekFont** - This property also take a Font object and it uses this font to render the days of the week in month-to-month view (e.g. Sun, Mon, Tue, Wed, Thu, Fri, Sat).

**DaysFont** - This is, yet another Font object that is used to render the actual days in the month-to-month calendar view.

**DayViewTimeFont** - This property is another Font object that is used to render the times in the day-to-day calendar view.

**DimDisabledEvents** - It is possible to disable events in the Calendar .NET control so that they do not render on the calendar. It is also possible to force these events to render on the calendar even if they are disabled by using the ShowDisabledEvents property described below. If disabled events are set to show up, setting this property to  **true**  will make disabled events show up as "dimmed" to give the feel that they are disabled. Setting this property to  **false** will make disabled events show up and look just like normal events.

**HighlightCurrentDay** - This is a bool property that, if set to  **true**, will highlight the current day (like today's date, not the current day set in the CalendarDate property). By default, this is  **true**. If set to  **false**, it will not highlight the current day.

**LoadPresetHolidays** - This is a bool property that determines if the preset Federal Holidays should be automatically loaded into the calendar. This property should be set early on and keep in mind that setting this property to  **true** or  **false** will wipe out any other events you might have programmed into the calendar instance, so its best to set this property early, before any events are added to the calendar. By default, this property is set to  **true**.

**ShowArrowControls** - The calendar can render arrow buttons above the calendar. These arrow buttons can be used to navigate to a previous month or navigate to a future month. By default, this property is set to  **true**. Setting it to  **false** will hide these controls.

**ShowDashedBorderOnDisabledEvents**  - Disabled events that are forced to render via the ShowDisabledEvents property can be set to be displayed with a dashed border around the event. If this property is set to  **true**, then disabled events will have a dashed border around them.

**ShowDateInHeader**  - Normally Calendar .NET will show the calendar's date about the calendar, justified to the right. By setting this property to  **false**, Calendar .NET will not render this date. By default, this property is set to  **true**.

**ShowDisabledEvents** - Events can be set to be disabled so they do not show up on the calendar. By setting this property to  **true**, events that are disabled will be rendered anyway. By default, this property is set to  **false**.

**ShowEventTooltips**  - When the mouse is hovered over an event, a tool tip is displayed. By setting this property to  **false**, it will disable these tool tips. Note that this property globally turns off all tool tips. It is possible to disable tool tips for certain or specific events. I will show you how later.

**ShowTodayButton** - Next to the navigation buttons, a "today button" normally is rendered. This button will take the user to today's date instantly. Setting this property to  **false** will hide this button.

**TodayFont** - This is a property that takes a Font object used to render the text on the "Today button.

As you can see, there is a lot of customization that can be done to the Calendar control. If you haven't already done so, drag a Calendar .NET control onto your empty form, then add the following code:

        public Form1()
        {
            InitializeComponent();
    
            calendar1.CalendarDate = DateTime.Now;
            calendar1.CalendarView = CalendarViews.Month;
        }
If you run your application now, you should see a month-to-month calendar, along with a today button and navigational controls. Also, if you navigate to a month that has a holiday, you should see that holiday on the calendar. For example, navigating to July, you should see an event for the 4th of July. If you hover your mouse over the event, you can also see a tool tip appear. Notice that right clicking on a holiday event will do nothing because, by default, you can not edit holiday events (since holiday events should not need editing anyway).
### Events
Events in Calendar .NET are easy to create. All events are simply classes that implement the  `IEvent` interface. Calendar .NET comes with classes that define two different kinds of events,  `HolidayEvent`  and  `CustomEvent`. A  `HolidayEvent` is a type of event that describes a holiday, such as Thanksgiving or Christmas. All of the preloaded Federal Holidays that come with Calendar .NET are instances of this class.  `CustomEvent` describes custom events that can be pretty much anything. Most of your events will be instances of this class. Of course you are also free to create your own kinds of event classes by simply implementing the  `IEvent`  interface.

All events, since they implement  `IEvent`, have the following properties:

**Date** - This property is the date and time that the event occurs.

**Enabled** - This property determines if the event is enabled. If set to  **false**, then the event is disabled and it will not be rendered on the calendar (unless the calendar is configured to show disabled events)

**EventColor** - This property is a  `Color` object that determines the background color used to render the event on the calendar.

**EventFont**  - This property is a  `Font` object that determines the font used to render the event.

**EventLengthInHours** - This is the length of the event, in hours. If the event is two hours long, then you'd set this to  **2.0**. If the event was a half hour long, you'd set this to  **0.5**. If the event is an hour and 15 minutes long, you'd set this to  **1.25**. This property is ignored if the  `IgnoreTimeComponent` property is set to true.

**EventText** - This is a description of the event. This is the text that will be displayed on the calendar.

**EventTextColor** - This is a  `Color` property that will determine the color to render the text in.

**IgnoreTimeComponent**  - This is a  `bool` value that, if set to  **true**, will ignore the time component of the  **Date** property. This is useful for events that last all day and have no fixed time.

**Rank**  - This property is an integer property that determines the order in which events that occur on the same day will be rendered in a month-to-month view. The smaller the number, the more precedence the event will have. Holiday events, by default, have a rank of 1, which means they will be rendered first on that day. Custom events have a rank of 2, which means they render after the holiday events. If two or more events have the same rank, then the order rendered is determined by the earliest event, in order to the latest. By giving an event a rank of -1, you can force the event to render ahead of everything else.

**ReadOnlyEvent**  - If this is set to  **true**, then the event can not be modified by the end user by right clicking on it. By default, this is set to  **false**, unless its a  `HolidayEvent`.

**RecurringFrequency**  - This property determines how often the event should render. Calendar .NET comes with several built in recurring frequencies, as well as the option to define your own. This property can be set to any value in the  `RecurringFrequencies` enumeration. Valid values are:

-   **None** - Indicates that this event is a one shot event and should only be rendered on one day.
-   **Daily** - Indicates that this event is a daily event and should be rendered every single day.
-   **EveryWeekday** - Indicates that this is an event that occurs every weekday (Mon-Fri), excluding weekends (Sat & Sun).
-   **EveryMonWedFri** - Indicates that this event will be rendered every Monday, Wednesday and Friday.
-   **EveryTueThurs** - Indicates that this event will be rendered every Tuesday and Thursday.
-   **Weekly** - Indicates that this event will be rendered once a week on the day of the week specified by the date.
-   **Monthly** - Indicates that this event will be rendered once a month on the day specified by the calendar date.
-   **Yearly** - Indicates that this event will be rendered once a year on the day and month specified by the calendar date.
-   **EveryWeekend** - Indicates the event will be rendered every Saturday and Sunday.
-   **Custom** - Indicates that the event has a recurring frequency that is unique. I will show you later an example of how to implement this.

**CustomRecurringFunction** - This is a property that takes a delegate function of type  `CustomRecurringFrequenciesHandler`. This function will accept the event and a render date and should return a  `bool` indicating if the event should be rendered on this day. An example of this will be coming up soon. This property is ignored unless the  `RecurringFrequency` property is set to  `Custom`.

**ThisDayForwardOnly** - This property, if set to  **true**, will cause the event to render only if the date is later or equal to the Calendar date. If set to  **false**, then the event, if its recurring, will show up in past days on the calendar too.

**TooltipEnabled**  - If set to  **true**, a tool tip will be rendered when the mouse is hovered over the event.

Just to summarize the difference between a  `HolidayEvent` and a  `CustomEvent`, here are the default properties of each (any or all of these properties can be overriden of course):

##### HolidayEvent

-   The  `EventColor` will be a light blue
-   The  `EventFont` will be Arial 8pt Bold
-   The  `EventTextColor` is white
-   The  `EventLengthInHours` is set to 24 (since holidays are usually all day events)
-   The  `ReadOnlyEvent` is set to  **true**, meaning you can't edit them
-   The  `IgnoreTimeComponent` is set to  **true**, again because they are all day events
-   The  `TooltipEnabled` is set to  **true**, meaning a tool tip will be shown when hovered over
-   The  `ThisDayForwardOnly` is set to  **false** since holidays occur not just now, but last year too
-   The  `Rank` is set to 1, meaning they are rendered on the calendar first, ahead of  `CustomEvents`

##### CustomEvent

-   The  `EventColor` will be a red
-   The  `EventFont` will be Arial 8pt Bold
-   The  `EventTextColor` is white
-   The  `EventLengthInHours` is set to 1 hour.
-   The  `ReadOnlyEvent` is set to  **false**, meaning the end user can edit the event.
-   The  `IgnoreTimeComponent` is set to  **false**
-   The  `ThisDayForwardOnly` is set to  **true** since most events won't need repeating in the past.
-   The  `Rank` is set to 2, meaning they are rendered after  `HolidayEvents`.

## Creating a Custom Holiday Event
Creating holiday events are very easy. Lets say we wanted to create a calendar event for Groundhog Day, which occurs every February 2nd. Add this code after the code you previously added in `Form1`'s constructor:

        var groundhogEvent = new HolidayEvent
                                         {
                                             Date = new DateTime(2012, 2, 2),
                                             EventText = "Groundhog Day",
                                             RecurringFrequency = RecurringFrequencies.Yearly
                                         };
        calendar1.AddEvent(groundhogEvent);
This code creates a new `HolidayEvent`. It sets the date to Feb 2nd 2012. Since this is a yearly recurring event, the year doesn't matter. It will render every year. We set the text to be "Groundhog Day" and then we set the `RecurringFrequency` to be yearly. Finally, the event is added to the calendar by calling the `AddEvent` method. The `AddEvent` method takes an `IEvent` parameter and adds the `IEvent` to the calendar. If you run the program now and navigate to February of any year, you should see the Groundhog Day event on the calendar. Congratulations! You just added your first event! Feel free to modify the event by changing some of the properties, then seeing the result.

## Creating a Custom Event

Creating custom events are just as easy as creating holiday events. In this example, we are going to create an event to remind us to exercise every Monday, Wednesday and Friday. Add this code after your Groundhog day event:

        var exerciseEvent = new CustomEvent
                                    {
                                        Date = DateTime.Now,
                                        RecurringFrequency = RecurringFrequencies.EveryMonWedFri,
                                        EventText = "Time for Exercise!"
                                    };
    
        calendar1.AddEvent(exerciseEvent);
his code creates a  `CustomEvent` object and sets the date for today and sets the  `RecurringFrequency` to  `EveryMonWedFri` and finally sets the text to "Time for Exercise!". If you run the program now, you will see that the event now shows up every Monday, Wednesday and Friday. Something else to notice, which differs from our  `HolidayEvent`, is that by default, the  `ThisDayForwardOnly`  property is set to  **true**. This means that the event will not show up prior to today (prior to  `DateTime.Now`). If you change this property to false then rerun the program, you will see that our event will show up every Monday, Wednesday and Friday, even if we go back a year from now.

One other thing I'd like to show you. If you right click on the event, you will get a menu that says "Properties". If you click on this, a dialog pops up that allows you to modify the event in real time. This functionality can be disabled by setting the  `ReadOnlyEvent` property in the event itself to  **true** or by setting the  `AllowEditingEvents` property in the calendar control to  **false**.
## Creating Events With Custom Recurring Frequencies

One of the cool things about Calendar .NET is that it if none of the prepackaged recurring frequencies meet your bill, you can create your own. To demonstrate this, we are going to create an event that reoccurs every Monday and Friday only. This particular event needs to occur only through June. It should not occur past June 2012. To do this, add this code in the constructor after your exercise event:

        var rehabEvent = new CustomEvent
                    {
                        Date = DateTime.Now,
                        RecurringFrequency = RecurringFrequencies.Custom,
                        EventText = "Rehab Therapy",
                        Rank = 3,
                        CustomRecurringFunction = RehabDays
                    };
        calendar1.AddEvent(rehabEvent);
This event is set up almost like our custom exercise event. The only major differences are, we are setting our `RecurringFrequency` property to `Custom` and we are setting a `CustomRecurringFunction` called `RehabDays`. The magic of when this event is rendered comes from the RehabDays function. Add this function to our `Form1` class:

        [CustomRecurringFunction("RehabDates", "Calculates which days I should be getting rehab therapy")]
        private bool RehabDays(IEvent evnt, DateTime day)
        {
            if (day.DayOfWeek == DayOfWeek.Monday || day.DayOfWeek == DayOfWeek.Friday)
            {
                if (day.Ticks >= (new DateTime(2012, 7, 1)).Ticks)
                    return false;
                return true;
            }
            return false;
        }


Setting up a function like this is easy. First, I added an attribute tag to the function called  `CustomRecurringFunction` . This attribute isn't required and doesn't really do anything, but I have plans for it in a future version of this control, so I recommend added them. Custom recurring functions always receive two parameters and return a bool. All custom recurring functions must have this signature. How this works is, when Calendar .NET starts to render a calendar and detects an event that has a custom recurring frequency, Calendar .NET basically calls your custom function and says "I am about to render this particular day on the calendar. Should this event be rendered?". Then its up to your function to decide if it should be rendered and return true if it should be or false if it shouldn't be. The  `IEvent` parameter is the event to be rendered. The  `DateTime` parameter is the day that is to be rendered. What we are doing in this function is first, deciding, is it Monday or Friday. If it is, then we are deciding if its July 1st 2012 or past that date. If it is, we return false because we do not want to render this event on or past July 1st. If its not then we return true, indicating it should be rendered. Hopefully this all makes sense :)

If you run the program now, you will see our new event and you will see that it is, indeed, rendering every Monday and Friday. In addition, if you navigate past June, you will see the event is no longer on the calendar.
## Making The Calendar Day-to-Day

Everything we've done so far has shown the power of Calendar .NET as a month-to-month calendar. Calendar .NET can also render itself as a day-to-day calendar, showing events on an hour-to-hour basis. To make the calendar render itself as a day-to-day calendar, you just need to need to change the line in the constructor that reads `calendar1.CalendarView = CalendarViews.Month;`  and change it to `calendar1.CalendarView = CalendarViews.Day;`. That's all there is to it. Now we just need to create an event to demonstrate. Add this code after our Rehab event in the constructor:

    var ce2 = new CustomEvent
                {
                    IgnoreTimeComponent = false,
                    EventText = "Dinner",
                    Date = new DateTime(2012, 5, 15, 18, 0, 0),
                    EventLengthInHours = 2f,
                    RecurringFrequency = RecurringFrequencies.None,
                    EventFont = new Font("Verdana", 12, FontStyle.Regular),
                    Enabled = true,
                    EventColor = Color.FromArgb(120, 255, 120),
                    EventTextColor = Color.Black,
                    ThisDayForwardOnly=false
                };
    calendar1.AddEvent(ce2);
This code will create a new custom event reminding us to have dinner on May 15th, 2012 at 6pm. We do not want to ignore the time component on this event since this event happens at a specific time. Dinner will be a two hour event and it will not be a recurring event. We are changing the color of this event to a green color so the event stands out from all the other events. If you run the application now, you will see the calendar looks quite different from before. The calendar is now showing events on a specific day. Each day shows a 24 hour period, starting at midnight and ending at 11:59pm that night. If you can not see all the days on the calendar, click anywhere on the calendar with your left mouse button and drag up or down to scroll the calendar. If you navigate to May 15th and scroll down to 6pm, you should see our dinner event. Also notice that the event is occupying two hours of our time. The green block for that event starts at the 6pm line and ends at the 8pm line.

