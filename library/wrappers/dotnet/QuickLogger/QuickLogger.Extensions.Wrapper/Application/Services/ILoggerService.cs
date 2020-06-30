﻿using System;
using System.Collections.Generic;
using System.Text;

namespace QuickLogger.Extensions.Wrapper.Application.Services
{
    public interface ILoggerService
    {
        void Info(string className, string msg);
        void Success(string className, string msg);
        void Warning(string className, string msg);
        void Error(string className, string msg);
        void Debug(string className, string msg);
        void Trace(string className, string msg);
        void Critical(string className, string msg);
        void Exception(Exception exception);
        void Exception(Exception exception, string msg);
    }
}
