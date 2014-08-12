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
        IGenericDataRepository<Operation> OperationRepository {get;set;}
        public OperationController()
        {
            OperationRepository = new GenericDataRepository<Operation>();
            
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

    }
}
