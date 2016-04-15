﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Logic.Models;

namespace Logic.Controllers
{
    public class Utility
    {
        /// <summary>
        /// Takes an object as input which can either be of the type Project and Task. 
        /// Sanitizes everything within given object and returns a safe to execute to sql version
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Project or Task which is safe to execute variables against sql server</returns>
        public object Sanitizer(object input)
        {

            foreach (PropertyInfo p in input.GetType().GetProperties())
            {
                if (p.PropertyType == typeof(string))
                {
                    //if (!string.IsNullOrEmpty(p.GetValue(input, null).ToString()))
                    //if(p.CanRead && p.CanWrite)
                    if (Nullable.GetUnderlyingType(p.PropertyType) != null)
                    {
                        p.SetValue(input, Sanitize(p.GetValue(input, null).ToString()));
                    }
                }
            }
            return input;
        }

        public object Desanitizer(object input)
        {
            foreach (PropertyInfo p in input.GetType().GetProperties())
            {
                if (p.PropertyType == typeof(string))
                {
                    if (Nullable.GetUnderlyingType(p.PropertyType) != null)
                    {
                        p.SetValue(input, Desanitize(p.GetValue(input, null).ToString()));
                    }
                }
            }
            return input;
        }

        public static string Sanitize(string s)
        {
            if (s.Contains("'"))
                return s.Replace("'", "''");
            return s;
        }

        public static string Desanitize(string s)
        {
            if (s.Contains("''"))
                return s.Replace("''", "'");
            return s;
        }

        public bool StringLength50(string s)
        {
            if (s.Length <= 50)
                return true;
            return false;
        }

        /// <summary>
        /// Splits a n object of the type DateTime into date and time seperately. 
        /// </summary>
        /// <param name="dateTime">The DateTime object to split</param>
        /// <returns>An array of the type string. slot 0 is date, slot 1 is time</returns>
        public static string[] SplitDateTime(DateTime dateTime)
        {
            string date = dateTime.ToShortDateString();
            string time = dateTime.ToLongTimeString();
            return new[] { date, time };
        }

        /// <summary>
        /// Parses a string of date and a string of time to an object of the type DateTime
        /// </summary>
        /// <param name="date">The date</param>
        /// <param name="time">The time</param>
        /// <returns>The DateTime object of the given date and time</returns>
        public static DateTime ParseDateTime(string date, string time)
        {
            return DateTime.Parse(date + " " + time);
        }
    }
}
