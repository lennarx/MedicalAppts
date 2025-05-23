﻿using MediatR;
using MedicalAppts.Core;

namespace MedicalAptts.UseCases.Appointment.GetAppointmentsPerPatient
{
    public class GetAppointmentsPerPatient : IRequest<Result<IEnumerable<AppointmentDTO>, Error>>
    {
        public int PatientId { get; }
        public DateTime? AppointmentDate { get; }
        public GetAppointmentsPerPatient(int patientId, DateTime? appointmentDate)
        {
            AppointmentDate = appointmentDate;
            PatientId = patientId;
        }
    }
}
