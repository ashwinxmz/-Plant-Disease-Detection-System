using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Blazorpdds.Data
{
    public class PddsModel
    {
        [Required(ErrorMessage = "Plant Name is required")]
        [StringLength(50, MinimumLength = 2,
            ErrorMessage =
            "Plant Name must be set and maximum of 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]+[ a-zA-Z0-9-_#,@:;+$()/.]*$", ErrorMessage = "Special characters are not allowed")]
        public string PlantName { get; set; }
        public int PlantID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class DiseaseModel
    {
        public string PlantName { get; set; }

        [Required(ErrorMessage = "Plant Name is required")]
        public int? PlantID { get; set; }
        
        [Required(ErrorMessage = "Disease Name is required")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage =
            "Disease Name must be set and maximum of 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]+[ a-zA-Z0-9-_#,@:;+$()/.]*$", ErrorMessage = "Special characters are not allowed")]
        public string DiseaseName { get; set; }

        [Required(ErrorMessage = "Symptoms are required")]
        //[RegularExpression(@"^[a-zA-Z0-9]+[ a-zA-Z0-9-_#,@:;+$()/.]*$", ErrorMessage = "Special characters are not allowed")]
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "Control Measures are required")]
      //  [RegularExpression(@"^[a-zA-Z0-9]+[ a-zA-Z0-9-_#,@:;+$()/.]*$", ErrorMessage = "Special characters are not allowed")]
        public string ControlMeasures { get; set; }
        public int DiseaseId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public byte [] Photo { get; set; }
    }
    public class AnalyzeModel
    {
        public string PlantName { get; set; }

        [Required(ErrorMessage = "Plant Name is required")]
        public int? PlantID { get; set; }       
        public string DiseaseName { get; set; }      
        public string Symptoms { get; set; }

        public string ControlMeasures { get; set; }
        public int DiseaseId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        public byte[] Photo { get; set; }
    }
    public class ModelOutput
    {        
        public uint Label { get; set; }        
        public byte[] ImageSource { get; set; }        
        public string PredictedLabel { get; set; }        
        public float[] Score { get; set; }
        public string DiseaseName { get; set; }

    }
}
