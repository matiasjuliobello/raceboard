using System.Collections;
using System.Reflection;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Common.Exceptions;

namespace RaceBoard.Service.Filters
{
    public abstract class BaseCustomActionFilter
    {
        private enum PropertyOverrideAction
        {
            None = 0,
            ConvertToUtcTimeZone = 1,
            ConvertToUserTimeZone = 2
        }

        #region Private Fields

        private readonly ISessionHelper _sessionManager;
        private readonly IDateTimeHelper _dateTimeHelper;
        protected readonly IHttpHeaderHelper _httpHeaderHelper;
        private readonly ITranslator _translator;

        private readonly Type[] _dateTimeOffsetTypes = new Type[] { typeof(DateTimeOffset), typeof(DateTimeOffset?) };

        private RequestContext _currentRequestContext = null;
        private User _currentUser = null;
        private TimeZoneInfo _currentUserTimeZone = null;
        private TimeZoneInfo _utcTimeZone = null;

        #endregion

        #region Constructors

        protected BaseCustomActionFilter
            (
                IHttpHeaderHelper httpHeaderHelper,
                ISessionHelper sessionManager,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            )
        {
            _httpHeaderHelper = httpHeaderHelper;
            _sessionManager = sessionManager;
            _dateTimeHelper = dateTimeHelper;
            _translator = translator;
        }

        #endregion

        #region Protected Properties

        protected RequestContext CurrentRequestContext
        {
            get { return _currentRequestContext; }
            private set { _currentRequestContext = value; }
        }

        protected User CurrentUser
        {
            get { return _currentUser; }
            private set { _currentUser = value; }
        }

        #endregion

        #region Protected Methods

        protected void OnActionExecuting(ActionExecutingContext context)
        {
            string message = string.Empty;

            var authorizationHeader = this.GetValueFromHeaders(context.HttpContext.Request.Headers, CommonValues.HttpCustomHeaders.Authorization);
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                message = this.Translate("AuthorizationIsRequired");

                context.Result = this.CreateObjectResult(HttpStatusCode.Unauthorized, message);

                throw new FunctionalException(Common.Enums.ErrorType.Unauthorized, message);
            }

            RequestContext requestContext = _httpHeaderHelper.GetContext();
            if (requestContext == null)
            {
                context.Result = this.CreateObjectResult(HttpStatusCode.Unauthorized, this.Translate("CouldNotGetCurrentRequestContext"));
                return;
            }

            if (requestContext.Username == null)
            {
                context.Result = this.CreateObjectResult(HttpStatusCode.Unauthorized, this.Translate("MissingUsername"));
                return;
            }

            User user = _sessionManager.GetUser(requestContext.Username);
            if (user == null)
            {
                context.Result = this.CreateObjectResult(HttpStatusCode.Unauthorized, this.Translate("CouldNotFindUserForGivenUsername"));
                return;
            }

            this.CurrentUser = user;
            this.CurrentRequestContext = requestContext;

            this.ManipulateRequest(context);
        }

        protected void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                return;
            }

            this.ManipulateResponse(context);
        }

        protected string GetValueFromHeaders(IHeaderDictionary headers, string key)
        {
            var value = headers[key];
            if (value == StringValues.Empty)
                return string.Empty;

            return Convert.ToString(value);
        }

        protected ObjectResult CreateObjectResult(HttpStatusCode httpStatusCode, string description)
        {
            return new ObjectResult(description)
            {
                StatusCode = (int)httpStatusCode
            };
        }

        protected string Translate(string text, params object[] arguments)
        {
            if (_translator == null)
                return text;

            return _translator.Get(text, arguments);
        }

        #endregion

        #region Private Methods

        private void SetTimeZones()
        {
            _utcTimeZone = _dateTimeHelper.GetUtcTimeZone();

            var userSettings = _sessionManager.GetUserSettings(this.CurrentRequestContext.Username);
            if (userSettings == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, this.Translate("UserPreferencesNotFound"));

            _currentUserTimeZone = _dateTimeHelper.GetTimeZone(userSettings.TimeZone.Identifier);
        }

        private void ManipulateRequest(ActionExecutingContext context)
        {
            IDictionary<string, object?> arguments = context.ActionArguments;

            if (arguments.Count == 0)
                return;

            this.SetTimeZones();

            var actionsPerTypeDictionary = BuildPropertyTypeActionDictionary(_dateTimeOffsetTypes, GetSetOfActionsForRequestDateTimeOffsetProperties());

            foreach (var argument in arguments)
            {
                object? objectInstance = argument.Value;

                PerformActionOnObjectProperties(actionsPerTypeDictionary, objectInstance, false);
            }
        }

        private void ManipulateResponse(ActionExecutedContext context)
        {
            IActionResult? actionResult = context.Result;
            if (actionResult == null)
                return;

            if (actionResult is OkResult)
                return;

            if (actionResult is ObjectResult)
            {
                this.SetTimeZones();

                object? objectInstance = ((ObjectResult)actionResult).Value;

                var userSettings = _sessionManager.GetUserSettings(this.CurrentRequestContext.Username);
                string idTimeZone = userSettings.TimeZone.Identifier;

                var actionsPerTypeDictionary = BuildPropertyTypeActionDictionary(_dateTimeOffsetTypes, GetSetOfActionsForResponseDateTimeOffsetProperties());

                PerformActionOnObjectProperties(actionsPerTypeDictionary, objectInstance, false);
            }
        }
        private Dictionary<Type, PropertyOverrideAction[]> BuildPropertyTypeActionDictionary(Type[] types, PropertyOverrideAction[] actions)
        {
            var dictionary = new Dictionary<Type, PropertyOverrideAction[]>();

            foreach (var type in types)
            {
                dictionary.Add(type, actions);
            }

            return dictionary;
        }

        private PropertyOverrideAction[] GetSetOfActionsForRequestDateTimeOffsetProperties()
        {
            return new PropertyOverrideAction[] { PropertyOverrideAction.ConvertToUtcTimeZone };
        }

        private PropertyOverrideAction[] GetSetOfActionsForResponseDateTimeOffsetProperties()
        {
            return new PropertyOverrideAction[] { PropertyOverrideAction.ConvertToUserTimeZone };
        }

        private void PerformActionOnObjectProperties(Dictionary<Type, PropertyOverrideAction[]> actionsPerTypeDictionary, object? instance, bool isCollectionItem)
        {
            if (instance == null || IsNonComplexObject(instance))
                return;

            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties())
            {
                try
                {
                    if (propertyInfo.Name == "Date") // hack to avoid infinite loop while iterating over DateTime properties
                        return;

                    if (IsCollection(propertyInfo))
                    {
                        PerformActionOnObjectProperties(actionsPerTypeDictionary, propertyInfo.GetValue(instance), true);
                        return;
                    }

                    if (isCollectionItem)
                    {
                        foreach (var item in (IEnumerable)instance)
                        {
                            foreach (PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                            {
                                PerformActionOnPropertyIfCorresponds(actionsPerTypeDictionary, itemPropertyInfo, item);
                            }
                        }
                    }
                    else
                    {
                        PerformActionOnPropertyIfCorresponds(actionsPerTypeDictionary, propertyInfo, instance);
                    }
                }
                catch (Exception) { }
            }
        }

        private bool IsNonComplexObject(object instance)
        {
            // https://learn.microsoft.com/en-us/dotnet/api/system.type.isprimitive?view=net-8.0
            /*
                System.Boolean
                System.Byte
                System.SByte
                System.Int16
                System.UInt16
                System.Int32
                System.UInt32
                System.Int64
                System.UInt64
                System.IntPtr
                System.UIntPtr
                System.Char
                System.Double
                System.Single
            */

            var type = instance.GetType();

            return type.IsPrimitive || type.FullName == "System.String";
        }

        private bool IsCollection(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(String) || (propertyInfo.PropertyType == typeof(Char)))
                return false;

            bool implementsIEnumerable = propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IEnumerable));

            return implementsIEnumerable;
        }

        private void PerformActionOnPropertyIfCorresponds(Dictionary<Type, PropertyOverrideAction[]> actions, PropertyInfo propertyInfo, object? objectInstance)
        {
            bool hasPerformedAction = OverridePropertyValueIfCorresponds(actions, propertyInfo, objectInstance, ActionDispatcher);
            if (!hasPerformedAction)
                PerformActionOnObjectProperties(actions, propertyInfo.GetValue(objectInstance), false);
        }

        private bool OverridePropertyValueIfCorresponds
            (
                Dictionary<Type, PropertyOverrideAction[]> actions,
                PropertyInfo propertyInfo,
                object? objectInstance,
                Func<Dictionary<Type, PropertyOverrideAction[]>, PropertyInfo, object?, object?> overrideFunction
            )
        {
            var propertyValue = propertyInfo.GetValue(objectInstance);
            if (propertyValue == null)
                return false;

            if (!actions.ContainsKey(propertyInfo.PropertyType))
                return false;

            object? modifiedValue = overrideFunction(actions, propertyInfo, propertyValue);
            Type propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            object? safeValue = modifiedValue == null ? null : Convert.ChangeType(modifiedValue, propertyType);
            propertyInfo.SetValue(objectInstance, safeValue, null);

            return true;
        }

        private object? ActionDispatcher(Dictionary<Type, PropertyOverrideAction[]> actions, PropertyInfo propertyInfo, object? objectInstance)
        {
            PropertyOverrideAction[] actionList = actions[propertyInfo.PropertyType];

            foreach (var action in actionList)
            {
                switch (action)
                {
                    case PropertyOverrideAction.ConvertToUserTimeZone:
                    case PropertyOverrideAction.ConvertToUtcTimeZone:
                        objectInstance = PerformActionOnDateTimeOffsetProperty(objectInstance, action);
                        break;
                }
            }

            return objectInstance;
        }

        private object? PerformActionOnDateTimeOffsetProperty(object? propertyValue, PropertyOverrideAction action)
        {
            if (propertyValue == null)
                return null;

            var dateTimeOffset = (DateTimeOffset)propertyValue;

            switch (action)
            {
                default:
                case PropertyOverrideAction.None:
                    return propertyValue;

                case PropertyOverrideAction.ConvertToUserTimeZone:
                    return this.ConvertToTimeZone(dateTimeOffset, _currentUserTimeZone);

                case PropertyOverrideAction.ConvertToUtcTimeZone:
                    return this.ConvertToTimeZone(dateTimeOffset, _utcTimeZone);
            }
        }

        /// <summary>
        /// Applies a given TimeZone (offset) on provided Date and Time
        /// </summary>
        /// <param name="dateTimeOffset"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        private DateTimeOffset ConvertToTimeZone(DateTimeOffset dateTimeOffset, TimeZoneInfo timeZone)
        {
            return _dateTimeHelper.ApplyTimeZone(dateTimeOffset, timeZone);
        }

        #endregion
    }
}