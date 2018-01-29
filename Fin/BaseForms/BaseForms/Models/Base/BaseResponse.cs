using System;
namespace BaseForms.Models.Base
{
    public class BaseResponse
    {
        public string Mensage { get; set; }
        public bool IsCorrect { get; set; }
        public BaseResponse()
        {
            Mensage = "No se ha podido realizar la operación";
            IsCorrect = false;
        }

    }
}
