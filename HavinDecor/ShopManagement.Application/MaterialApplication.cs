using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Material;
using ShopManagement.Domain.MaterialAgg;

namespace ShopManagement.Application
{
    public class MaterialApplication : IMaterialApplication
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialApplication(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public OperationResult Create(CreateMaterial command)
        {
            var operation = new OperationResult();

            if (_materialRepository
                .Exists(x=> x.MaterialName == command.MaterialName 
                  && x.Panel == command.Panel))
            {
                return operation.Failed(ApplicationMessage.DuplicatedRecord);
            }

            var material = new Material(command.MaterialName, command.Price, command.Panel, command.RingColor);

            _materialRepository.Create(material);
            _materialRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditMaterial command)
        {
            var operation = new OperationResult();

            var material = _materialRepository.Get(command.Id);

            if (material == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            if (_materialRepository
                .Exists(x => x.MaterialName == command.MaterialName
                             && x.Panel == command.Panel && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessage.DuplicatedRecord);
            }

            material.Edit(command.MaterialName ,command.Price , command.Panel , command.RingColor);

            _materialRepository.SaveChanges();

            return operation.Succedded();
        }

        public EditMaterial GetDetails(long id)
        {
            return _materialRepository.GetDetails(id);
        }

        public List<MaterialViewModel> ViewModel()
        {
            return _materialRepository.ViewModel();
        }
    }
}
