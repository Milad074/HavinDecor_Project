using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Application
{
    public class SlideApplication :ISlideApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly ISlideRepository _slideRepository;

        public SlideApplication(IFileUploader fileUploader, ISlideRepository slideRepository)
        {
            _fileUploader = fileUploader;
            _slideRepository = slideRepository;
        }

        public OperationResult Create(CreateSlide command)
        {
            var operation = new OperationResult();

            var fileName = _fileUploader.Upload(command.Picture, "Slides");

            var slide = new Slide(fileName, command.PictureAlt, command.PictureTitle, command.Heading,
                command.Title, command.Text, command.BtnText, command.BtnColor, command.Link);

            _slideRepository.Create(slide);

            _slideRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditSlide command)
        {
            var operation = new OperationResult();

            var slide = _slideRepository.Get(command.Id);

            if (slide == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            var fileName = _fileUploader.Upload(command.Picture, "Slides");

            slide.Edit(fileName, command.PictureAlt, command.PictureTitle, command.Heading,
                command.Title, command.Text, command.BtnText, command.BtnColor , command.Link);

            _slideRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Remove(long id)
        {
            var operation = new OperationResult();

            var slide = _slideRepository.Get(id);

            if (slide == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            slide.Remove();

            _slideRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Restore(long id)
        {
            var operation = new OperationResult();

            var slide = _slideRepository.Get(id);

            if (slide == null)
            {
                return operation.Failed(ApplicationMessage.RecordNotFound);
            }

            slide.Restore();

            _slideRepository.SaveChanges();

            return operation.Succedded();
        }

        public EditSlide GetDetails(long id)
        {
            return _slideRepository.GetDetails(id);
        }

        public List<SlideViewModel> Search(SlideSearchModel searchModel)
        {
            return _slideRepository.Search(searchModel);
        }
    }
}
