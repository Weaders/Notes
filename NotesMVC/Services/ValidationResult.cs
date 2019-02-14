using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace NotesMVC.Services {

    public interface IValidationResult {

        bool IsSuccess { get; set; }
        Dictionary<string, string> Errors { get; set; }

        void ErrorsToModelState(ModelStateDictionary modelState);
    }

    public class ValidationResult : IValidationResult {

        public bool IsSuccess { get; set; } = true;
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();

        public void ErrorsToModelState(ModelStateDictionary modelState) {

            foreach (var error in Errors) {
                modelState.AddModelError(error.Key, error.Value);
            }

        }

    }

}
