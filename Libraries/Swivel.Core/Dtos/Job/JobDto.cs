﻿using System;

namespace Swivel.Core.Dtos.Job
{
    public class JobDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Salary { get; set; }
        public string Experience { get; set; }
        public int Vacancies { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}