﻿using RaceBoard.Common.Helpers.Interfaces;

namespace RaceBoard.Common.Helpers
{
    public class DateTimeHelper : IDateTimeHelper
    {
        private readonly string _UTC_TIMEZONE_ID = TimeZoneInfo.Utc.Id.ToString();
        private readonly string _DEFAULT_TIMEZONE_ID;

        public DateTimeHelper()
        {
            _DEFAULT_TIMEZONE_ID = _UTC_TIMEZONE_ID;
        }

        #region IDateTimeHelper implementation

        public DateTimeOffset GetCurrentTimestamp(DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            switch (dateTimeKind)
            {
                case DateTimeKind.Utc:
                    return DateTimeOffset.UtcNow;

                case DateTimeKind.Unspecified:
                case DateTimeKind.Local:
                default:
                    return DateTimeOffset.Now;
            }
        }
        
        public TimeZoneInfo GetUtcTimeZone()
        {
            return this.GetTimeZone(_UTC_TIMEZONE_ID);
        }

        public TimeZoneInfo GetTimeZone(string id)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (Exception)
            {
                return TimeZoneInfo.FindSystemTimeZoneById(_DEFAULT_TIMEZONE_ID);
            }
        }

        public DateTimeOffset ApplyTimeZone(DateTimeOffset dateTimeOffset, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTimeOffset, timeZone);
        }

        #endregion
    }
}
