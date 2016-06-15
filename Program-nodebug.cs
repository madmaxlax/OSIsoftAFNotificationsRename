using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PISDK;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace NotificationsRename
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            Console.WriteLine("Set up output file " + destination);
			//connect to AF
            string pisysname = "PISystemName"; //edit with your server name!
            string Afdbname = "AFDB Name"; // edit with your server name!
            Console.WriteLine("Connecting to AF DB " + Afdbname + " on " + pisysname);
            PISystem PISys = new PISystems()[pisysname];
            AFDatabase AFDb = PISys.Databases[Afdbname];
            Console.WriteLine("Connected to AF DB " + Afdbname + " on " + pisysname);
			//refresh and check in, just in case
            AFDb.ApplyChanges();
            AFDb.CheckIn();
            AFDb.Refresh();
			
			//start
            Console.WriteLine("Ready to begin? Press any key");
            Console.ReadKey();

            foreach(OSIsoft.AF.Notification.AFNotification curr in AFDb.Notifications)
            {
			//only change name of notifications coming from templates (which have a name)
                if (! (curr.Template == null) )
                {
					//this is the format I chose for the naming: [Template name] [parent name]-[element name]
					string newname = curr.Template.Name + " " + ((OSIsoft.AF.Asset.AFElement)(curr.Target)).Parent.Name + "-" + curr.Target.ToString();
                    Console.WriteLine("Renaming this notification: " + curr.Name + " to: "+ newname);
                    //rename the notification
                    try
                    {
                        curr.Name = newname;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("*Unable to rename: " + e.Message);
                    }
                }
                Console.WriteLine("Name: " + curr.Name);
            }
            

            Console.WriteLine("Checking in changes...");
            AFDb.ApplyChanges();
            AFDb.CheckIn();
            AFDb.Refresh();
            Console.WriteLine("Press enter to quit");
            Console.ReadLine();

        }
    }
}
