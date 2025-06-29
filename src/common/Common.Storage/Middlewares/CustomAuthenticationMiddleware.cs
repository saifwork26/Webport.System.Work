﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Common.Storage.Middlewares;

public class CustomAuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["AuthState"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader))
        {
            await next(context);
        }
        else
        {
            var error = new ProblemDetails()
            {
                Title = "No authentication Header Found",
                Status = StatusCodes.Status404NotFound,
                Detail = "No authentication Header [AuthState] found"
            };
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(error));
        }
    }
}
