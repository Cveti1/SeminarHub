﻿namespace SeminarHub.Models.Seminar
{
    public class SeminarViewDetailModel : SeminarViewShortModel
    {
        public int Duration { get; set; }
        public string Details { get; set; } = null!;
        

    }
}
