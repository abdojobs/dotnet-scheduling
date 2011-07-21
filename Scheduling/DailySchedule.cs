﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduling
{
    public class DailySchedule : Schedule
    {
        public override string Description
        {
            get
            {
                // Daily
                // Every 2 days
                return String.Concat(
                    String.Format(
                        Strings.Plural(Frequency, "Daily", "Every {0} days"),
                        Frequency),
                    (EndDate.HasValue ? String.Format(", ending {0}", EndDate.Value.ToString("d MMMM yyyy")) : ""));
            }
        }

        public DailySchedule()
            : base()
        {
            Period = Period.Day;
        }

        public override IList<DateTime> GetOccurences(DateTime start, DateTime end)
        {
            List<DateTime> list = new List<DateTime>();

            // check bounds to end immediately if necessary
            if (end < StartDate || (EndDate.HasValue && start > EndDate))
                return list;
            else if (start < StartDate)
                // modify start if the requested start is before the scheduled start
                start = StartDate;
            else
            {
                // the requested start is within the schedule, so calculate the corresponding start based on 
                // how far through two of the scheduled dates we are
                int rollover = ((int)Math.Floor((start - StartDate).TotalDays)) % Frequency;
                if (rollover > 0)
                    start = start.AddDays(Frequency - rollover);
            }

            // limit the end to either the parameter value or when this schedule ends
            if (EndDate.HasValue && EndDate < end)
                end = EndDate.Value;

            // add dates to the list until we reach the requested end
            DateTime cur = start;
            while (cur <= end)
            {
                list.Add(cur);
                cur = cur.AddDays(Frequency);
            }

            return list;
        }
    }
}