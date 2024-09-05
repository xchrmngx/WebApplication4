using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Numerics;
using System.Linq.Dynamic.Core;


namespace WebApplication4.Controllers
{
    public class DoctorsController : ApiController
    {
        private MedicalDBEntities db = new MedicalDBEntities();

        // Получение списка врачей с постраничной выборкой и сортировкой
        [HttpGet]
        public IHttpActionResult GetDoctors(int page = 1, int pageSize = 10, string sortBy = "FullName")
        {
            var doctors = db.Doctors
                            .OrderBy(sortBy)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(d => new DoctorListDto
                            {
                                Id = d.Id,
                                FullName = d.FullName,
                                OfficeNumber = d.OfficeId.ToString(),              // Номер кабинета
                                SpecializationName = d.SpecializationId.ToString(),  // Название специализации
                                UParticipantNumber = d.UParticipantId.ToString()   // Номер участка (если есть)
                            })
                            .ToList();

            return Ok(doctors);
        }

        // Получение врача для редактирования по Id
        [HttpGet]
        public IHttpActionResult GetDoctor(int id)
        {
            var doctor = db.Doctors
                           .Where(d => d.Id == id)
                           .Select(d => new DoctorEditDto
                           {
                               Id = d.Id,
                               FullName = d.FullName,
                               OfficeId = (int)d.OfficeId,
                               SpecializationId = (int)d.SpecializationId,
                               UParticipantId = (int)d.UParticipantId
                           })
                           .FirstOrDefault();

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        // Добавление врача
        [HttpPost]
        public IHttpActionResult AddDoctor(DoctorEditDto dto)
        {
            var doctor = new Doctors
            {
                FullName = dto.FullName,
                OfficeId = dto.OfficeId,
                SpecializationId = dto.SpecializationId,
                UParticipantId = dto.UParticipantId
            };

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return Ok();
        }

        // Редактирование врача
        [HttpPut]
        public IHttpActionResult EditDoctor(int id, DoctorEditDto dto)
        {
            var doctor = db.Doctors.Find(id);
            if (doctor == null) return NotFound();

            doctor.FullName = dto.FullName;
            doctor.OfficeId = dto.OfficeId;
            doctor.SpecializationId = dto.SpecializationId;
            doctor.UParticipantId = dto.UParticipantId;

            db.SaveChanges();
            return Ok();
        }

        // Удаление врача
        [HttpDelete]
        public IHttpActionResult DeleteDoctor(int id)
        {
            var doctor = db.Doctors.Find(id);
            if (doctor == null) return NotFound();

            db.Doctors.Remove(doctor);
            db.SaveChanges();
            return Ok();
        }
    }
}
