namespace RaceBoard.Domain
{
    public class DateTimeRange
    {
        public enum IntersectionType
        {
            /// <summary>
            /// No Intersection
            /// </summary>
            None = -1,
            /// <summary>
            /// Given range ends inside the range
            /// </summary>
            EndsInRange,
            /// <summary>
            /// Given range starts inside the range
            /// </summary>
            StartsInRange,
            /// <summary>
            /// Both ranges are equaled
            /// </summary>
            RangesEqualed,
            /// <summary>
            /// Given range contained in the range
            /// </summary>
            ContainedInRange,
            /// <summary>
            /// Given range contains the range
            /// </summary>
            ContainsRange,
        }

        #region Construction

        public DateTimeRange()
        {
        }

        public DateTimeRange(DateTimeOffset start, DateTimeOffset end)
        {
            if (start > end)
            {
                throw new DateTimeOffsetRangeInvalidException();
            }

            _start = start;
            _end = end;
        }

        public DateTimeRange(DateTimeOffset? start, DateTimeOffset? end)
        {
            DateTimeOffset rangeStart = DateTimeOffset.MinValue;
            DateTimeOffset rangeEnd = DateTimeOffset.MaxValue;

            if (!start.HasValue && !end.HasValue)
            {
                rangeStart = DateTimeOffset.MinValue;
                rangeEnd = DateTimeOffset.MaxValue;
            }
            else if (!start.HasValue && end.HasValue)
            {
                rangeStart = DateTimeOffset.MinValue;
                rangeEnd = end.Value;
            }
            else if (start.HasValue & !end.HasValue)    // use of non-compound boolean expression to avoid Partially Covered Lines on Code Coverage
            {
                rangeStart = start.Value;
                rangeEnd = DateTimeOffset.MaxValue;
            }
            else if (start.HasValue & end.HasValue)     // use of non-compound boolean expression to avoid Partially Covered Lines on Code Coverage
            {
                rangeStart = start.Value;
                rangeEnd = end.Value;
            }

            if (start > end)
            {
                throw new DateTimeOffsetRangeInvalidException();
            }

            _start = rangeStart;
            _end = rangeEnd;
        }

        #endregion

        #region Properties

        private DateTimeOffset _start;

        public DateTimeOffset Start
        {
            get { return _start; }
            set { _start = value; }
        }

        private DateTimeOffset _end;

        public DateTimeOffset End
        {
            get { return _end; }
            set { _end = value; }
        }

        #endregion

        #region Operators

        public static bool operator ==(DateTimeRange range1, DateTimeRange range2)
        {
            return range1.Equals(range2);
        }

        public static bool operator !=(DateTimeRange range1, DateTimeRange range2)
        {
            return !(range1 == range2);
        }

        public override bool Equals(object obj)
        {
            if (obj is DateTimeRange)
            {
                var range1 = this;
                var range2 = (DateTimeRange)obj;
                return range1.Start == range2.Start && range1.End == range2.End;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Querying

        public bool Intersects(DateTimeRange range)
        {
            var type = GetIntersectionType(range);
            return type != IntersectionType.None;
        }

        public bool IsInRange(DateTimeOffset date)
        {
            return (date >= this.Start) && (date <= this.End);
        }

        public IntersectionType GetIntersectionType(DateTimeRange range)
        {
            if (this == range)
            {
                return IntersectionType.RangesEqualed;
            }
            else if (IsInRange(range.Start) && IsInRange(range.End))
            {
                return IntersectionType.ContainedInRange;
            }
            else if (IsInRange(range.Start))
            {
                return IntersectionType.StartsInRange;
            }
            else if (IsInRange(range.End))
            {
                return IntersectionType.EndsInRange;
            }
            else if (range.IsInRange(this.Start) && range.IsInRange(this.End))
            {
                return IntersectionType.ContainsRange;
            }
            return IntersectionType.None;
        }

        public DateTimeRange GetIntersection(DateTimeRange range)
        {
            var type = this.GetIntersectionType(range);
            if (type == IntersectionType.RangesEqualed | type == IntersectionType.ContainedInRange) // use of non-compound boolean expression to avoid Partially Covered Lines on Code Coverage
            {
                return range;
            }
            else if (type == IntersectionType.StartsInRange)
            {
                return new DateTimeRange(range.Start, this.End);
            }
            else if (type == IntersectionType.EndsInRange)
            {
                return new DateTimeRange(this.Start, range.End);
            }
            else if (type == IntersectionType.ContainsRange)
            {
                return this;
            }
            else
            {
                return default(DateTimeRange);
            }
        }

        #endregion

        public override string ToString()
        {
            return Start.ToString() + " - " + End.ToString();
        }
    }

    [Serializable]
    public class DateTimeOffsetRangeInvalidException : Exception
    {
        public override string Message
        {
            get
            {
                return "Invalid date|time range";
            }
        }
    }
}