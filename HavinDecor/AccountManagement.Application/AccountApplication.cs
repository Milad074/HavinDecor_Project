using System;
using System.Collections.Generic;
using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Application
{
    public class AccountApplication :IAccountApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AccountApplication(IFileUploader fileUploader, IAccountRepository accountRepository, IPasswordHasher passwordHasher)
        {
            _fileUploader = fileUploader;
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }


        public OperationResult Create(CreateAccount command)
        {
            var operation = new OperationResult();

            if (_accountRepository.Exists(x => x.UserName == command.UserName || x.Mobile == command.Mobile))
                return operation.Failed(ApplicationMessage.DuplicatedRecord);

            var path = $"AccountPicture/{command.UserName}";
            var fileName = _fileUploader.Upload(command.ProfilePhoto, path);

            var password = _passwordHasher.Hash(command.Password);

            var account = new Account(command.FullName, command.UserName, password, command.Mobile,
                command.RoleId , fileName);

            _accountRepository.Create(account);

            _accountRepository.SaveChanges();

            return operation.Succedded();
        }

        public OperationResult Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.Get(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessage.RecordNotFound);

            if (_accountRepository.Exists(x => x.UserName == command.UserName
                  || x.Mobile == command.Mobile && x.Id != command.Id))
                return operation.Failed(ApplicationMessage.DuplicatedRecord);

            var path = $"AccountPicture/{command.UserName}";
            var fileName = _fileUploader.Upload(command.ProfilePhoto, path);

            account.Edit(command.FullName, command.UserName, command.Mobile,
                command.RoleId, fileName);

            _accountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();

            var account = _accountRepository.Get(command.Id);

            if (account == null)
                return operation.Failed(ApplicationMessage.RecordNotFound);

            if (command.Password != command.RePassword)
                return operation.Failed(ApplicationMessage.PasswordsNotMatch);

            var password = _passwordHasher.Hash(command.Password);

            account.ChangePassword(password);
            _accountRepository.SaveChanges();

            return operation.Succedded();
        }

        public EditAccount GetDetails(long id)
        {
            return _accountRepository.GetDetails(id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }
    }
}
