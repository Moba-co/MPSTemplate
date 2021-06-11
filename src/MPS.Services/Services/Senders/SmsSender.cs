using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MPS.Common.DTOs;
using MPS.Services.Interfaces.Senders;
using MPS.Common.Helpers;
using MPS.Common.ViewModels.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nancy.Json;
using System.Web;
using System.Collections.Specialized;

namespace MPS.Services.Services.Senders
{
    public class SmsSender : ISender
    {
        private readonly IOptionsSnapshot<SiteSettings> _siteSetting;
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;
        public SmsSender(IOptionsSnapshot<SiteSettings> siteSetting,ILogger logger,IHttpClientFactory clientFactory)
        {
            _siteSetting = siteSetting;
            _logger = logger;
            _clientFactory = clientFactory;
        }
        public async Task<ReturnMessageDto> SendManyAsync(IEnumerable<string> to, string subject, string message, bool isMessageHtml = false)
        {
            object input = new
            {
                PhoneNumber = _siteSetting.Value.SMSInfo.SenderNumber, // شماره اختصاصی
                Message = message, //    ارساالی
                UserGroupID = Guid.NewGuid(), // شماره پیگیری
                Mobiles = to, // لیست شماره موبایل ها
                SendDateInTimeStamp = DateTimeHelper.GetTimeStamp(DateTime.Now, DateTimeKind.Local) // تاریخ ارسال به صورت timestamp
            };
            string inputJson = new JavaScriptSerializer().Serialize(input);
            var res =  await PostData(inputJson);
            return new ReturnMessageDto("با موفقیت ارسال شد",true,0);
        }

        public async Task<ReturnMessageDto> FastSendAsync(string to,string message)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://raygansms.com/SendMessageWithCode.ashx?Username={_siteSetting.Value.SMSInfo.UserName}&Password={_siteSetting.Value.SMSInfo.Password}&Mobile={to}&Message={message}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return new ReturnMessageDto("پیام با موفقیت ارسال شد",true,0);
        }

        public async Task<ReturnMessageDto> SendAsync(string to, string subject, string message, bool isMessageHtml = false)
        {
            object input = new
            {
                PhoneNumber = _siteSetting.Value.SMSInfo.SenderNumber, // شماره اختصاصی
                Message = message, //    ارساالی
                UserGroupID = Guid.NewGuid(), // شماره پیگیری
                Mobiles = new List<string>(){to}, // لیست شماره موبایل ها
                SendDateInTimeStamp = DateTimeHelper.GetTimeStamp(DateTime.Now, DateTimeKind.Local) // تاریخ ارسال به صورت timestamp
            };
            string inputJson = new JavaScriptSerializer().Serialize(input);
            var res =  await PostData(inputJson);
            return new ReturnMessageDto("با موفقیت ارسال شد",true,0);
        }

        private async Task<string> PostData(string inputJson)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "http://smspanel.trez.ir/api/smsAPI/SendMessage/");
            //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var token = Encoders.Base64Encode(_siteSetting.Value.SMSInfo.UserName + ":" + _siteSetting.Value.SMSInfo.Password);
            request.Headers.Add("Authorization", $"Basic {token}");
            request.Content = new StringContent(inputJson, Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return result;
            
        }
    }
}