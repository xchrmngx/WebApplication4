using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Linq.Dynamic.Core;

namespace WebApplication4.Controllers
{
    public class PatientsController : ApiController
    {
        private MedicalDBEntities db = new MedicalDBEntities();

        // Получение списка пациентов с постраничной выборкой и сортировкой
        [HttpGet]
        public IHttpActionResult GetPatients(int page = 1, int pageSize = 10, string sortBy = "LastName")
        {
            var patients = db.Patients
                             .OrderBy(sortBy)
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(p => new PatientListDto
                             {
                                 Id = p.Id,
                                 FullName = p.LastName + " " + p.FirstName + " " + p.MiddleName,
                                 Address = p.Address,
                                 Gender = p.Gender,
                                 BirthDate = p.BirthDate,
                                 UParticipantNumber = p.UParticipantId.ToString(),
                             })
                             .ToList();

            return Ok(patients);
        }

        // Получение пациента для редактирования по Id
        [HttpGet]
        public IHttpActionResult GetPatient(int id)
        {
            var patient = db.Patients
                            .Where(p => p.Id == id)
                            .Select(p => new PatientEditDto
                            {
                                Id = p.Id,
                                LastName = p.LastName,
                                FirstName = p.FirstName,
                                MiddleName = p.MiddleName,
                                Address = p.Address,
                                BirthDate = p.BirthDate,
                                Gender = p.Gender,
                                UParticipantId = (int)p.UParticipantId
                            })
                            .FirstOrDefault();

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // Добавление пациента
        [HttpPost]
        public IHttpActionResult AddPatient(PatientEditDto dto)
        {
            var patient = new Patients
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                Address = dto.Address,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                UParticipantId = dto.UParticipantId
            };

            db.Patients.Add(patient);
            db.SaveChanges();

            return Ok();
        }

        // Редактирование пациента
        [HttpPut]
        public IHttpActionResult EditPatient(int id, PatientEditDto dto)
        {
            var patient = db.Patients.Find(id);
            if (patient == null) return NotFound();

            patient.LastName = dto.LastName;
            patient.FirstName = dto.FirstName;
            patient.MiddleName = dto.MiddleName;
            patient.Address = dto.Address;
            patient.BirthDate = dto.BirthDate;
            patient.Gender = dto.Gender;
            patient.UParticipantId = dto.UParticipantId;

            db.SaveChanges();
            return Ok();
        }

        // Удаление пациента
        [HttpDelete]
        public IHttpActionResult DeletePatient(int id)
        {
            var patient = db.Patients.Find(id);
            if (patient == null) return NotFound();

            db.Patients.Remove(patient);
            db.SaveChanges();
            return Ok();
        }
    }
}
