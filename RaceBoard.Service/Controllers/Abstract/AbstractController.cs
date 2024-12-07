using AutoMapper;
using RaceBoard.Domain;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;
using System.Net;
using RaceBoard.DTOs.Competition.Request;
using System.Security;

namespace RaceBoard.Service.Controllers.Abstract
{
    public class AbstractController<T> : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger<T> _logger;
        protected readonly ITranslator _translator;
        protected readonly ISessionHelper _sessionHelper;
        private readonly IRequestContextHelper? _requestContextHelper;
        private readonly LogLevel[] _failureLogLevels = new LogLevel[]
        {
            LogLevel.Error,
            LogLevel.Critical
        };

        public AbstractController
            (
                IMapper mapper,
                ILogger<T> logger,
                ITranslator translator,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            )
        {
            _logger = logger;
            _translator = translator;
            _mapper = mapper;
            _sessionHelper = sessionHelper;
            _requestContextHelper = requestContextHelper;
        }

        protected User GetUserFromRequestContext()
        {
            return _requestContextHelper.GetUser();
        }

        protected UserSettings GetUserSettings(string username)
        {
            return _sessionHelper.GetUserSettings(username);
        }

        protected void Log(LogLevel logLevel, string message, Exception? e = null)
        {
            if (_failureLogLevels.Contains(logLevel))
                _logger.LogError(e, message + Environment.NewLine);
            else
                _logger.Log(logLevel, message);
        }

        protected ObjectResult ReturnBadRequestResponse(string message)
        {
            return new ObjectResult(this.Translate(message)) 
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        protected string Translate(string text, params object[] arguments)
        {
            if (_translator == null)
                return text;

            return _translator.Get(text, arguments);
        }

        protected Domain.File CreateFileInstance(FileUpload uploadedFile)
        {
            var currentUser = this.GetUserFromRequestContext();
            var currentDate = DateTimeOffset.UtcNow;

            return new Domain.File()
            {
                CreationPerson = null,
                CreationUser = currentUser,
                CreationDate = currentDate,
                Content = uploadedFile.Content,
                Name = uploadedFile.UniqueFilename
            };
        }
    }
}
