/*
 * Author: OnnoRokom Software Ltd
 * Website: http://onnorokomsoftware.com
 * Copyright (c) onnorokomsoftware.com
 * License: Commercial
*/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.UmsBdResult.Services
{
    public class UmsBdResultApiService
    {
        private const string StudentExamController = "StudentExam";
        private const string LoadExamCourseFunc = "LoadCourse";
        private const string LoadExams = "LoadExams";
        private const string GetResult = "GetResult";
        private const string GetSolvedSheetPdf = "GetStudentSolveSheetPdfString";
        private const string GetSolvedSheetPdfByCode = "GetStudentSolveSheetPdfStringByCodeNo";


        public async Task<JsonResultResponse> LoadCourse(string baseUrl, string key, string orgBusinessId, string prn)
        {
            var dictionary = new Dictionary<string, string>
            {
                { "key", key },
                { "orgBusinessId", orgBusinessId },
                { "prnNo", prn }
            };
            var param = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");

            string url = baseUrl + StudentExamController + "/" + LoadExamCourseFunc;

            var jsonResponse = await HttpPostRequest(url, param);
            return jsonResponse;
        }

        public async Task<JsonResultResponse> LoadExam(string baseUrl, string key, string orgBusinessId, string courseId, string prnNo)
        {
            var dictionary = new Dictionary<string, string>
            {
                { "key", key },
                { "orgBusinessId", orgBusinessId },
                { "prnNo", prnNo },
                { "courseId", courseId }
            };
            var param = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");

            string url = baseUrl + StudentExamController + "/" + LoadExams;

            var jsonResponse = await HttpPostRequest(url, param);
            return jsonResponse;
        }

        public async Task<JsonResultResponse> GetStudentResult(string baseUrl, string key, string orgBusinessId, string courseId, string prnNo, string examId)
        {
            string param = "key=" + key + "&orgBusinessId=" + orgBusinessId + "&courseId=" + courseId + "&prnNo=" + prnNo + "&examId=" + examId;

            string url = baseUrl + StudentExamController + "/" + GetResult;

            var jsonResponse = await HttpGetRequest(url, param);
            return jsonResponse;
        }

        public async Task<JsonResultResponse> GetSolveSheetPdf(string baseUrl, string key, string orgBusinessId, string prnNo, string examId, string version, string ipAddress)
        {
            string param = "key=" + key + "&orgBusinessId=" + orgBusinessId + "&prnNo=" + prnNo + "&examId=" + examId + "&version=" + version + "&ipAddress=" + ipAddress;

            string url = baseUrl + StudentExamController + "/" + GetSolvedSheetPdf;

            var jsonResponse = await HttpGetRequest(url, param);
            return jsonResponse;
        }

        public async Task<JsonResultResponse> GetSolveSheetPdfByCode(string baseUrl, string key, string orgBusinessId, string codeNo, string ipAddress)
        {
            string param = "key=" + key + "&orgBusinessId=" + orgBusinessId + "&codeNo=" + codeNo + "&ipAddress=" + ipAddress;

            string url = baseUrl + StudentExamController + "/" + GetSolvedSheetPdfByCode;

            var jsonResponse = await HttpGetRequest(url, param);
            return jsonResponse;
        }

        #region Helper
        public async Task<JsonResultResponse> HttpGetRequest(string url, string param)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(url + "?" + param);

                    response.EnsureSuccessStatusCode();

                    string stringResults = await response.Content.ReadAsStringAsync();

                    var rawWeather = JsonConvert.DeserializeObject<JsonResultResponse>(stringResults);
                    return rawWeather;
                }
                catch (HttpRequestException httpRequestException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }

        public async Task<JsonResultResponse> HttpPostRequest(string url, StringContent param)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.PostAsync(url, param);

                    response.EnsureSuccessStatusCode();

                    string stringResults = await response.Content.ReadAsStringAsync();

                    var rawWeather = JsonConvert.DeserializeObject<JsonResultResponse>(stringResults);
                    return rawWeather;
                }
                catch (HttpRequestException httpRequestException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return null;
        }

        #endregion
    }

    public class CommonObjectApi
    {
        public virtual long Id { get; set; }
        public virtual String Name { get; set; }
    }

    public class JsonResultResponse
    {
        public bool IsAuthenticated { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }

        public JsonResultResponse()
        {
        }

        public JsonResultResponse(bool isAuthenticated, bool isSuccess, string message, dynamic data)
        {
            this.IsAuthenticated = isAuthenticated;
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Data = data;
        }
    }
}