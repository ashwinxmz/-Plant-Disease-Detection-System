using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazorpdds.Data
{
   public interface Ipddsservice
    {
        Task<bool> CreatePlants(PddsModel pmodel);
        Task<bool> UpdatePlants(PddsModel pmodel);
        Task<IEnumerable<PddsModel>> GetPlants();
        Task<PddsModel> GetPlantsByID(int plantID);
        Task<PddsModel> DeletePlantsByID(int plantID);
        Task<bool> CreateDisease(DiseaseModel pmodel);
        Task<bool> UpdateDisease(DiseaseModel pmodel);
        Task<IEnumerable<DiseaseModel>> GetDiseases();
        Task<DiseaseModel> GetDiseaseByID(int diseaseId);
        Task<PddsModel> DeleteDiseaseByID(int diseaseId);
        Task<ModelOutput> SaveScanData(AnalyzeModel pmodel);
        Task<AnalyzeModel> GetAnalyzedPlantsByID(int? plantID, string analyzedData);

    }
}
