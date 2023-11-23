﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Classes
{
    public class SimpleResult
    {
        private string errorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; log.Log(value); }
        }

        public bool IsFailed => !string.IsNullOrEmpty(ErrorMessage);

        ErrorLog log = new ErrorLog();
    }
    public class Result<T> : SimpleResult
    {
        public T? Data { get; set; }
    }
    public class ErrorLog
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        string filename = "/Tekentracker_ErrorLog.txt";

        public void Log(string message)
        {
            File.AppendAllText(path + filename, DateTime.Now.ToShortTimeString() +": "+ message);
        }
    }
}