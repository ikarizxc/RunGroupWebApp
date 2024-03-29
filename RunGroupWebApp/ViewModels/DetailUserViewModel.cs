﻿namespace RunGroupWebApp.ViewModels
{
    public class DetailUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? Pace { get; set; }
        public int? Kilometers { get; set; }
        public string ProfileImageUrl { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
	}
}
