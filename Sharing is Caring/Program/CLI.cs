﻿using Serilog;
using System;
using System.IO;

namespace Advent
{
    public class CLI
    {
        public CLI()
        {
            var debugLogger = new LoggerConfiguration()
                                     .MinimumLevel.Verbose()
                                     .WriteTo.Console()
                                     .CreateLogger();
            Log.Logger = debugLogger;
        }

        public void Process(string[] args)
        {
            if (args.Length == 1
                && string.Equals(args[0], "all", System.StringComparison.OrdinalIgnoreCase))
            {
                RunAll();
            }
            else if (args.Length == 1)
            {
                RunOne(args[0]);
            }
            else if (args.Length > 1)
            {
                RunMany(args);
            }
            else
            {
                NoArgs();
            }
        }

        private void RunAll()
        {
            var days = new AllDays();
            days.RunSolutions();
        }

        private void RunMany(string[] args)
        {
            foreach (var arg in args)
            {
                RunOne(arg);
            }
        }

        private void RunOne(string dayName)
        {
            SpecificDay specificDay;

            dayName = dayName.ToLower();
            dayName = dayName.Replace("day", "").Replace("_", "");

            try
            {
                specificDay = new SpecificDay(dayName);
            }
            catch (ArgumentException)
            {
                Log.Error("Could not find a day \"{dayName}\" in project.", dayName);
                return;
            }
            catch (Exception e)
            {
                Log.Error("Unexpected problem: {}", e.Message);
                return;
            }

            //run solutions
            Log.Information("Running '{ProblemName}'", specificDay.ProblemPart1.ProblemName);
            specificDay.ProblemPart1.Run();

            Log.Information("Running '{ProblemName}'", specificDay.ProblemPart2.ProblemName);
            specificDay.ProblemPart2.Run();
        }

        private void NoArgs()
        {
            var today = new LastDay();

            //run solutions
            Log.Information("Running '{ProblemName}'", today.ProblemPart1.ProblemName);
            today.ProblemPart1.Run();

            Log.Information("Running '{ProblemName}'", today.ProblemPart2.ProblemName);
            today.ProblemPart2.Run();
        }
    }
}