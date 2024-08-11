﻿namespace RaceBoard.Domain
{
    public class CreditSpreadsheet : Spreadsheet.Abstract.Spreadsheet
    {
        public class Item
        {
            public string Project { get; set; }
            public int EpisodeNumber { get; set; }
            public string Script { get; set; }
            public string Character { get; set; }
            public string FullName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string LicenseNumber { get; set; }
        }

        public List<CreditSpreadsheet.Item> Items { get; set; }
    }
}
