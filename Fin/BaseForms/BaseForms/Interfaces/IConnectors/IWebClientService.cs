using System;
using System.Net.Http;

namespace BaseForms.Interfaces.IConnectors
{
    public interface IWebClientService
    {
        HttpClient Client();   
    }
}
