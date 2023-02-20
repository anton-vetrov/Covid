using Microsoft.AspNetCore.Http;
using System;

namespace CovidService.Services.Github
{
    public class GithubException : BaseException
    {
        public GithubException() : base("Github is unavailable. Please check connection or CovidCasesUrl setting", StatusCodes.Status400BadRequest)
        {
        }
    }
}
