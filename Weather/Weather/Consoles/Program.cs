using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;

using Weather.Models;
using Weather.Services;

namespace Weather.Consoles
{
    //Your can move your Console application Main here. Rename Main to myMain and make it NOT static and async

    class Program
    {
        #region used by the Console
        Views.ConsolePage theConsole;
        StringBuilder theConsoleString;
        public Program(Views.ConsolePage myConsole)
        {
            //used for the Console
            theConsole = myConsole;
            theConsoleString = new StringBuilder();
        }
        #endregion

        #region Console Demo program
        //This is the method you replace with your async method renamed and NON static Main
        public async Task myMain()
        {
            OpenWeatherService service = new OpenWeatherService();
            service.WeatherForecastAvailable += ReportWeatherDataAvailable;

            //Task<Forecast> t1 = null, t2 = null, t3 = null, t4 = null;
            List<Forecast> forecasts = new List<Forecast>();
            Exception exception = null;
            try
            {
                double latitude = 59.5086798659495;
                double longitude = 18.2654625932976;
                forecasts.Add(await service.GetForecastAsync(latitude, longitude));
                theConsole.WriteLine(forecasts[0].City);

                //Create the two tasks and wait for comletion
                //t1 = service.GetForecastAsync(latitude, longitude);
                //t2 = service.GetForecastAsync("Miami");



                //Task.WaitAll(t1, t2);
                //Thread.Sleep(20_000);
                //t3 = service.GetForecastAsync(latitude, longitude);
                //t4 = service.GetForecastAsync("Miami");


                ////Wait and confirm we get an event showing cahced data avaialable
                //Task.WaitAll(t3, t4);

            }
            catch (Exception ex)
            {
                //if exception write the message later
                exception = ex;
                theConsole.WriteLine(ex.Message);
            }

            theConsoleString.AppendLine("-----------------");
            foreach (var forecast in forecasts)
            {
                theConsoleString.AppendLine(".....");
                if (forecast != null)
                {
                    theConsoleString.AppendLine($"Weather forecast for {forecast.City}");
                    var GroupedList = forecast.Items.GroupBy(item => item.DateTime.Date);
                    foreach (var group in GroupedList)
                    {
                        theConsole.WriteLine(group.Key.Date.ToShortDateString());
                        foreach (var item in group)
                        {

                            theConsole.WriteLine($"   - {item.DateTime.ToShortTimeString()}: {item.Description}, temperature: {item.Temperature} degC, wind: {item.WindSpeed} m/s");
                        }
                    }
                }
                else
                {
                    theConsoleString.AppendLine($"Geolocation weather service error.");
                    theConsoleString.AppendLine($"Error: {exception.Message}");
                }
            }
            /*theConsole.WriteLine("Demo program output");

            //Write an output to the Console
            theConsole.Write("One ");
            theConsole.Write("Two ");
            theConsole.WriteLine("Three and end the line");

            //As theConsole.WriteLine return trips are quite slow in UWP, use instead of myConsoleString to build the the more complex output
            //string using several myConsoleString.AppendLine instead of several theConsole.WriteLine. 
            foreach (char c in "Hello World from my Console program")
            {
                theConsoleString.Append(c);
            }

            //Once the string is complete Write it to the Console
            theConsole.WriteLine(theConsoleString.ToString());

            theConsole.WriteLine("Wait for 2 seconds...");
            await Task.Delay(2000);

            //Finally, demonstrating getting some data async
            theConsole.WriteLine("Download from https://dotnet.microsoft.com/...");
            theConsoleString.Clear();
            using (var w = new WebClient())
            {
                string str = await w.DownloadStringTaskAsync("https://dotnet.microsoft.com/");
                theConsoleString.Append($"Nr of characters downloaded: {str.Length}");
            }
            theConsole.WriteLine(theConsoleString.ToString());*/
        }

        //If you have any event handlers, they could be placed here
        void myEventHandler(object sender, string message)
        {
            theConsole.WriteLine($"Event message: {message}"); //theConsole is a Captured Variable, don't use myConsoleString here
        }
        void ReportWeatherDataAvailable(object sender, string message)
        {
            theConsole.WriteLine($"Event message from weather service: {message}");
        }
        #endregion
    }
}
