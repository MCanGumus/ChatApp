using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared
{
    public interface IRequestByServiceResult<T> : IRequest<ServiceResult<T>>;
    public interface IRequestByServiceResult : IRequest<ServiceResult>;

    public class ServiceResult
    {
        [JsonIgnore] public HttpStatusCode Status { get; set; }
        public ProblemDetails? Fail { get; set; }
        [JsonIgnore] public bool IsSuccess => Fail is null;
        [JsonIgnore] public bool IsFail => !IsSuccess;

        public static ServiceResult SuccessAsNoContent()
        {
            return new ServiceResult { Status = HttpStatusCode.NoContent };
        }

        public static ServiceResult ErrorAsNotFound()
        {
            return new ServiceResult
            {
                Fail = new ProblemDetails { Title = "Not Found", Detail = "The requested resource was not found." },
                Status = HttpStatusCode.NotFound
            };
        }

        public static ServiceResult ErrorFromProblemDetails(Refit.ApiException exception)
        {
            if (string.IsNullOrEmpty(exception.Content))
            {
                return new ServiceResult
                {
                    Fail = new ProblemDetails() { Title = exception.Message },
                    Status = exception.StatusCode,
                };
            }

            var problemDetails =
                JsonSerializer.Deserialize<ProblemDetails>(exception.Content, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });

            return new ServiceResult
            {
                Fail = problemDetails,
                Status = exception.StatusCode,
            };
        }

        public static ServiceResult Error(ProblemDetails problemDetails, HttpStatusCode statusCode)
        {
            return new ServiceResult
            {
                Fail = problemDetails,
                Status = statusCode,
            };
        }

        public static ServiceResult Error(string title, string description, HttpStatusCode statusCode)
        {
            return new ServiceResult
            {
                Fail = new ProblemDetails
                {
                    Title = title,
                    Detail = description,
                    Status = statusCode.GetHashCode()
                },
                Status = statusCode,
            };
        }

        public static ServiceResult Error(string title, HttpStatusCode statusCode)
        {
            return new ServiceResult
            {
                Fail = new ProblemDetails
                {
                    Title = title,
                    Status = statusCode.GetHashCode()
                },
                Status = statusCode,
            };
        }

        public static ServiceResult ErrorFromValidation(IDictionary<string, object> errors)
        {
            return new ServiceResult
            {
                Fail = new ProblemDetails
                {
                    Title = "Validation errors occured.",
                    Detail = "Please chect the errors property for more details",
                    Extensions = errors,
                    Status = HttpStatusCode.BadRequest.GetHashCode()
                },
                Status = HttpStatusCode.BadRequest,
            };
        }

    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }
        [JsonIgnore] public string? UrlAsCreated { get; set; }

        public static ServiceResult<T> SuccessAsOk(T data)
        {
            return new ServiceResult<T>
            {
                Status = HttpStatusCode.OK,
                Data = data
            };
        }

        public static ServiceResult<T> SuccessAsCreated(T data, string url)
        {
            ServiceResult<T> result = new ServiceResult<T>
            {
                Status = HttpStatusCode.Created,
                Data = data,
                UrlAsCreated = url,
            };

            return result;
        }

        public new static ServiceResult<T> ErrorFromProblemDetails(Refit.ApiException exception)
        {
            if (string.IsNullOrEmpty(exception.Content))
            {
                return new ServiceResult<T>
                {
                    Fail = new ProblemDetails() { Title = exception.Message },
                    Status = exception.StatusCode,
                };
            }

            var problemDetails =
                JsonSerializer.Deserialize<ProblemDetails>(exception.Content, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });

            return new ServiceResult<T>
            {
                Fail = problemDetails,
                Status = exception.StatusCode,
            };
        }

        public new static ServiceResult<T> Error(ProblemDetails problemDetails, HttpStatusCode statusCode)
        {
            return new ServiceResult<T>
            {
                Fail = problemDetails,
                Status = statusCode,
            };
        }

        public new static ServiceResult<T> Error(string title, string description, HttpStatusCode statusCode)
        {
            return new ServiceResult<T>
            {
                Fail = new ProblemDetails
                {
                    Title = title,
                    Detail = description,
                    Status = statusCode.GetHashCode()
                },
                Status = statusCode,
            };
        }

        public new static ServiceResult<T> Error(string title, HttpStatusCode statusCode)
        {
            return new ServiceResult<T>
            {
                Fail = new ProblemDetails
                {
                    Title = title,
                    Status = statusCode.GetHashCode()
                },
                Status = statusCode,
            };
        }

        public new static ServiceResult<T> ErrorFromValidation(IDictionary<string, object> errors)
        {
            return new ServiceResult<T>
            {
                Fail = new ProblemDetails
                {
                    Title = "Validation errors occured.",
                    Detail = "Please chect the errors property for more details",
                    Extensions = errors,
                    Status = HttpStatusCode.BadRequest.GetHashCode()
                },
                Status = HttpStatusCode.BadRequest,
            };
        }
    }
}