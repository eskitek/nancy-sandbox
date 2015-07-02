/*
 * Example code for my blog post on function shims for NancyFx request handlers
 * Blog post is here: http://anthonysteele.co.uk/more-patterns-for-web-services-in-nancyfx
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Validation;

namespace NancyFxPlayground.MyApi
{
    public static class ModuleExtensions
    {
        public static void GetHandler(this NancyModule module, string path, Func<object> handler)
        {
            module.Get[path] = _ => RunHandler(module, handler);
        }

        public static void GetHandler<TIn>(this NancyModule module, string routeName, string routePath, Func<TIn, object> routeHandler)
        {
            module.Get[routeName, routePath] = _ => RunHandler(module, routeHandler);
        }

        public static void GetHandlerAsync<TIn>(this NancyModule module, string path, Func<TIn, Task<object>> handler)
        {
            module.Get[path, true] = async (x, ctx) => await RunHandlerAsync(module, handler);
        }

        public static void GetHandlerAsync(this NancyModule module, string path, Func<Task<object>> handler)
        {
            module.Get[path, true] = async (x, ctx) => await RunHandlerAsync(module, handler);
        }


        public static object RunHandler(this NancyModule module, Func<object> handler)
        {
            return handler();
        }

        public static async Task<object> RunHandlerAsync(this NancyModule module, Func<Task<object>> handler)
        {
            return await handler();
        }

        public static object RunHandler<TIn>(this NancyModule module, Func<TIn, object> handler)
        {
            TIn model;
            try
            {
                model = module.BindAndValidate<TIn>();
                if (!module.ModelValidationResult.IsValid)
                {
                    return module.Negotiate.RespondWithValidationFailure(module.ModelValidationResult);
                }
            }
            catch (ModelBindingException)
            {
                return module.Negotiate.RespondWithValidationFailure("Model binding failed");
            }

            return handler(model);
        }

        public static async Task<object> RunHandlerAsync<TIn>(this NancyModule module, Func<TIn, Task<object>> handler)
        {
            TIn model;
            try
            {
                model = module.BindAndValidate<TIn>();
                if (!module.ModelValidationResult.IsValid)
                {
                    return module.Negotiate.RespondWithValidationFailure(module.ModelValidationResult);
                }
            }
            catch (ModelBindingException)
            {
                return module.Negotiate.RespondWithValidationFailure("Model binding failed");
            }

            return await handler(model);
        }

        public static Negotiator RespondWithValidationFailure(this Negotiator negotiate, ModelValidationResult validationResult)
        {
            var model = new ValidationFailedResponse(validationResult);

            return negotiate
                .WithModel(model)
                .WithStatusCode(HttpStatusCode.BadRequest);
        }

        public static object RespondWithValidationFailure(this Negotiator negotiate, string message)
        {
            var model = new ValidationFailedResponse(message);

            return negotiate
                .WithModel(model)
                .WithStatusCode(HttpStatusCode.BadRequest);
        }
    }


    public class ValidationFailedResponse
    {
        public List<string> Messages { get; set; }

        public ValidationFailedResponse()
        { }

        public ValidationFailedResponse(ModelValidationResult validationResult)
        {
            Messages = new List<string>();
            ErrorsToStrings(validationResult);
        }

        public ValidationFailedResponse(string message)
        {
            Messages = new List<string>
            {
                message
            };
        }

        private void ErrorsToStrings(ModelValidationResult validationResult)
        {
            foreach (var errorGroup in validationResult.Errors)
            {
                foreach (var error in errorGroup.Value)
                {
                    Messages.Add(error.ErrorMessage);
                }
            }
        }
    }
}