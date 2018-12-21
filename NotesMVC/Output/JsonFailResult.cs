using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NotesMVC.Output {

    public class ErrorsHandler {

        [JsonProperty("errors")]
        public IDictionary<string, string> Errors = new Dictionary<string, string>();

    }

    public class JsonFailResult : JsonResult {

        const int STATUS_CODE = 400;

        public JsonFailResult(IDictionary<string, string> errors) : base(null) {

            this.StatusCode = STATUS_CODE;
            this.Value = new ErrorsHandler { Errors = errors };

        }

        public JsonFailResult(string error) : base(null) {

            this.StatusCode = STATUS_CODE;

            var errorsHandler = new ErrorsHandler();
            errorsHandler.Errors.Add("error", error);

            this.Value = errorsHandler;


        }

        public JsonFailResult(ModelStateDictionary modelState) : base(null) {

            var errorsHandler = new ErrorsHandler();

            foreach (var keyValuePair in modelState) {

                if (keyValuePair.Value.Errors.Count > 1) {

                    for(var i = 0; i < keyValuePair.Value.Errors.Count; i++) {
                        errorsHandler.Errors.Add(keyValuePair.Key + $"_{i}", keyValuePair.Value.Errors[i].ErrorMessage);
                    }

                } else {
                    errorsHandler.Errors.Add(keyValuePair.Key, keyValuePair.Value.Errors[0].ErrorMessage);
                }

            }

            this.Value = errorsHandler;
            this.StatusCode = STATUS_CODE;

        }

    }
}
