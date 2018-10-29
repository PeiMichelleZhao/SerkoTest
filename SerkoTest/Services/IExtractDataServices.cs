using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerkoTest.Models;

namespace SerkoTest.Services
{

    public interface IExtractDataServices
    {
        string ExtractData(string text);
    }

}
