using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.Model;
using DentistApp.DAL.DAL;

namespace DentistApp.BL
{
    public class OperationController
    {
        PatientController PatientController { get; set; }

        IGenericDataRepository<Operation> OperationRepository {get;set;}
        public OperationController()
        {
            OperationRepository = new GenericDataRepository<Operation>();
            PatientController = new PatientController();
        }

        public OperationController(IGenericDataRepository<Operation> operationRepository)
        {
            OperationRepository = operationRepository;
        }

        public void SaveOperation(Operation operation)
        {
            if (operation.OperationId == 0)
            {
                OperationRepository.Add(operation);
            }
            else
            {
                OperationRepository.Update(operation);
            }

        }

        public IEnumerable<Operation> List(int operationId = 0)
        {
            if (operationId == 0)
            {
                return OperationRepository.GetAll();
            }
            return OperationRepository.GetList(d => d.OperationId == operationId);

        }


        public void Delete(Operation operation)
        {

            OperationRepository.Remove(operation);

        }


        public IEnumerable<Operation> GetOperationsOfPatientAndTooth(int patientId, int toothId)
        {
            var patient = PatientController.Get(patientId);
            var SelectedToothAppointments = patient.Appointments.Select(d => d.Teeth.Where(t => t.Teeth.ToothId == toothId)).SelectMany(i => i).ToList();
            
            return SelectedToothAppointments.Select(o => o.Appointment.Operation.Select(i => i.Operation)).SelectMany(i => i).Where(o=>o.IsDeleted==false);

        }



    }
}
