using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Blazorpdds.Data
{
    public class pddsservice:Ipddsservice
    {
        private readonly SqlConnectionConfiguration _configuration;
        public pddsservice(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> CreatePlants(PddsModel pmodel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", pmodel.PlantName, DbType.String);           

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync("Add_Plants", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return true;
        }
        public async Task<ModelOutput> SaveScanData(AnalyzeModel pmodel)
        {           
            var parameters = new DynamicParameters();
            parameters.Add("Planid", pmodel.PlantID, DbType.String);
            parameters.Add("Photo", pmodel.Photo, DbType.Binary);
            ModelOutput model = null;
            string jsonres = "";
            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    var res = await conn.ExecuteScalarAsync("SaveScanPlants", parameters, commandType: CommandType.StoredProcedure);
                    
                    jsonres=CallAPI(Convert.ToInt32(res));
                     model = JsonConvert.DeserializeObject<ModelOutput>(jsonres);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return model;
        }
        private string CallAPI(int Id)
        {
            string jsonres = "";
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            var configSection = configuration.GetSection("AppSettings:APIURL");
            if (configSection != null)
            {                
                var client = new RestClient(configSection.Value);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = @"{" + "\n" +
    @"     ""Id"":" + Id + "}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                jsonres= response.Content;
            }               
            return jsonres;
        }
        public async Task<bool> UpdatePlants(PddsModel pmodel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", pmodel.PlantName, DbType.String);
            parameters.Add("plantid", pmodel.PlantID, DbType.String);


            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync("Update_Plants", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return true;
        }
        public async Task<IEnumerable<PddsModel>> GetPlants()
        {
            IEnumerable<PddsModel> plants = null;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    plants = await conn.QueryAsync<PddsModel>("Get_Plants", commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                  //  Helper.LogMessage("Server", "GetCustomers", ex.ToString());
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return plants;
        }
        public async Task<PddsModel> GetPlantsByID(int plantID)
        {
           PddsModel plant = null;
            var parameters = new DynamicParameters();
            parameters.Add("plantid", plantID, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    plant = await conn.QueryFirstOrDefaultAsync<PddsModel>("Get_PlantById", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //  Helper.LogMessage("Server", "GetCustomers", ex.ToString());
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return plant;
        }
        public async Task<AnalyzeModel> GetAnalyzedPlantsByID(int? plantID,string analyzedData)
        {
            AnalyzeModel plant = null;
            var parameters = new DynamicParameters();
            parameters.Add("plantid", plantID, DbType.Int32);
            parameters.Add("analysedData", analyzedData, DbType.String);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    plant = await conn.QueryFirstOrDefaultAsync<AnalyzeModel>("Get_AnalyzedData", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //  Helper.LogMessage("Server", "GetCustomers", ex.ToString());
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return plant;
        }

        public async Task<PddsModel> DeletePlantsByID(int plantID)
        {
            PddsModel plant = null;
            var parameters = new DynamicParameters();
            parameters.Add("plantid", plantID, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync("Delete_Plants", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //  Helper.LogMessage("Server", "GetCustomers", ex.ToString());
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return plant;
        }

        public async Task<bool> CreateDisease(DiseaseModel pmodel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("PlandID", pmodel.PlantID, DbType.String);
            parameters.Add("Name", pmodel.DiseaseName, DbType.String);
            parameters.Add("Symptoms", pmodel.Symptoms, DbType.String);
            parameters.Add("ControlMeasure", pmodel.ControlMeasures, DbType.String);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync("Add_Disease", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return true;
        }

        public async Task<bool> UpdateDisease(DiseaseModel pmodel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", pmodel.DiseaseName, DbType.String);
            parameters.Add("plantid", pmodel.PlantID, DbType.String);
            parameters.Add("Diseaseid", pmodel.DiseaseId, DbType.String);
            parameters.Add("Symptoms", pmodel.Symptoms, DbType.String);
            parameters.Add("ControlMeasure", pmodel.ControlMeasures, DbType.String);


            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync("Update_Disease", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return true;
        }
        public async Task<IEnumerable<DiseaseModel>> GetDiseases()
        {
            IEnumerable<DiseaseModel> diseases = null;

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    diseases = await conn.QueryAsync<DiseaseModel>("Get_Diseases", commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //  Helper.LogMessage("Server", "GetCustomers", ex.ToString());
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return diseases;
        }
        public async Task<DiseaseModel> GetDiseaseByID(int diseaseId)
        {
            DiseaseModel disease = null;
            var parameters = new DynamicParameters();
            parameters.Add("diseaseid", diseaseId, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    disease = await conn.QueryFirstOrDefaultAsync<DiseaseModel>("Get_DiseaseById", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //  Helper.LogMessage("Server", "GetCustomers", ex.ToString());
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return disease;
        }

        public async Task<PddsModel> DeleteDiseaseByID(int diseaseId)
        {
            PddsModel plant = null;
            var parameters = new DynamicParameters();
            parameters.Add("diseaseid", diseaseId, DbType.Int32);

            using (var conn = new SqlConnection(_configuration.Value))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                try
                {
                    await conn.ExecuteAsync("Delete_Disease", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    //  Helper.LogMessage("Server", "GetCustomers", ex.ToString());
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return plant;
        }
    }
}
