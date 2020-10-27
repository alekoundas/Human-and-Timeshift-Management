﻿using System;

namespace Bussiness.Service
{
    public sealed class DateRange
    {

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public DateRange(DateTime startOn, DateTime endOn)
        {
            this.StartTime = startOn;
            this.EndTime = endOn;
        }
        private double _Seconds { get; set; }

        public double TotalHours
        {
            get => this._Seconds / 60 / 60;
        }
        public double TotalSeconds
        {
            get => this._Seconds;
        }


        public DateRange ConvertToDayWork()
        {
            this._Seconds = 0;
            var currentStartDate = this.StartTime;
            var currentEndDate = this.EndTime;

            if (this.StartTime.Day == this.EndTime.Day)
            {
                if (this.StartTime <= this.StartTime.Date.Add(EndNightHours))
                    currentStartDate = this.StartTime.Date.Add(EndNightHours);

                if (this.EndTime.Date.Add(StartNightHours) <= this.EndTime)
                    currentEndDate = this.EndTime.Date.Add(StartNightHours);

                if (!(this.StartTime.Date.Add(StartNightHours) <= this.StartTime ||
                    this.EndTime <= this.EndTime.Date.Add(EndNightHours)))

                    this._Seconds = currentEndDate.Subtract(currentStartDate).TotalSeconds;
            }
            else
            {
                currentEndDate = this.EndTime.AddDays(-1).Date.Add(StartNightHours);

                if (this.StartTime <= this.StartTime.Date.Add(EndNightHours))
                    currentStartDate = this.StartTime.Date.Add(EndNightHours);


                if (!(this.StartTime.Date.Add(StartNightHours) <= this.StartTime))
                    this._Seconds = currentEndDate.Subtract(currentStartDate).TotalSeconds;

                currentStartDate = this.StartTime.AddDays(1).Date.Add(StartNightHours);
                currentEndDate = this.EndTime;

                if (this.EndTime.Date.Add(StartNightHours) <= this.EndTime)
                    currentEndDate = this.EndTime.Date.Add(StartNightHours);

                if (!(this.EndTime <= this.EndTime.Date.Add(EndNightHours)))
                    this._Seconds += currentEndDate.Subtract(currentStartDate).TotalSeconds;

            }

            return this;
        }

        public DateRange ConvertToNightWork()
        {

            this._Seconds = 0;
            var currentStartDate = this.StartTime;
            var currentEndDate = this.EndTime;

            if (this.StartTime.Day == this.EndTime.Day)
            {
                if (this.StartTime <= this.StartTime.Date.Add(EndNightHours))
                {
                    if (this.EndTime.Date.Add(EndNightHours) <= this.EndTime)
                        currentEndDate = this.EndTime.Date.Add(EndNightHours);

                    this._Seconds = currentEndDate.Subtract(currentStartDate).TotalSeconds;
                }
                currentEndDate = this.EndTime;
                if (this.EndTime.Date.Add(StartNightHours) <= this.EndTime)
                {
                    if (this.StartTime <= this.StartTime.Date.Add(StartNightHours))
                        currentStartDate = this.StartTime.Date.Add(StartNightHours);

                    this._Seconds += currentEndDate.Subtract(currentStartDate).TotalSeconds;
                }
            }
            else
            {
                if (this.StartTime <= this.StartTime.Date.Add(EndNightHours))
                {
                    if (this.EndTime.AddDays(-1).Date.Add(EndNightHours) <= this.EndTime.AddDays(-1))
                        currentEndDate = this.EndTime.AddDays(-1).Date.Add(EndNightHours);

                    this._Seconds = currentEndDate.Subtract(currentStartDate).TotalSeconds;
                }
                currentEndDate = this.EndTime;
                if (this.EndTime.AddDays(-1).Date.Add(StartNightHours) <= this.EndTime)
                {
                    if (this.EndTime.Date.Add(EndNightHours) <= this.EndTime)
                        currentEndDate = this.EndTime.Date.Add(EndNightHours);

                    if (this.StartTime <= this.StartTime.Date.Add(StartNightHours))
                        currentStartDate = this.StartTime.Date.Add(StartNightHours);

                    this._Seconds += currentEndDate.Subtract(currentStartDate).TotalSeconds;
                }


            }
            return this;
        }


        private TimeSpan StartNightHours { get => new TimeSpan(22, 0, 0); }
        private TimeSpan EndNightHours { get => new TimeSpan(6, 0, 0); }


        //private static bool IsWorkday(DateTime date)
        //{
        //    // Weekend
        //    //if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        //    //    return false;

        //    //Maybe TODO later

        //    return true;
        //}
    }
}
