using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class DoctorListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string OfficeNumber { get; set; }  // Номер кабинета вместо Id
        public string SpecializationName { get; set; }  // Название специализации вместо Id
        public string UParticipantNumber { get; set; }  // Номер участка
    }
}