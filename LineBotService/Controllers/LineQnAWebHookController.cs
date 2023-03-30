using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace isRock.Template
{
    public class LineQnAWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        private readonly IConfiguration _config;
        public LineQnAWebHookController(IConfiguration config)
        {
            _config = config;
        }

        const string QnAEndpoint = "https://semanticanaly.cognitiveservices.azure.com/language/:query-knowledgebases?projectName=QnAProject&api-version=2021-10-01&deploymentName=production";
        const string QnAKey = "4326713418984f61ae647c14761e5a7a";
        const string UnknowAnswer = "不好意思，您可以換個方式問嗎? 我不太明白您的意思...";

        enum catIns
        {
            None,
            自我介紹,
            有什麼服務,
            想問治具,
            指令
        }

        [Route("api/TestQnA")]
        [HttpPost]
        public IActionResult POST()
        {
            var channelAccessToken = _config.GetSection("LINE-Bot-Setting:channelAccessToken");
            var AdminUserId = _config.GetSection("LINE-Bot-Setting:adminUserID");

            try
            {

                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = channelAccessToken.Value;
                //配合Line Verify
                if (ReceivedMessage.events == null || ReceivedMessage.events.Count() <= 0 ||
                    ReceivedMessage.events.FirstOrDefault().replyToken == "00000000000000000000000000000000") return Ok();
                //取得Line Event(範例，只取第一個)
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //回覆訊息
                if (LineEvent.type == "message")
                {
                    if (LineEvent.message.type == "text") //收到文字
                    {
                        //建立 MsQnAMaker Client
                        //var helper = new isRock.MsQnAMaker.Client(
                        //    new Uri(QnAEndpoint), QnAKey);
                        //var QnAResponse = helper.GetResponse(LineEvent.message.text.Trim());
                        //var ret = (from c in QnAResponse.answers
                        //           orderby c.score descending
                        //           select c
                        //        ).Take(1);

                        var responseText = UnknowAnswer;
                        //自訂回覆.................
                        string userReply = LineEvent.message.text.Trim();
                        var reKind = new catIns();
                        reKind = catIns.None;
                        if (userReply.Contains("#"))
                        {
                            responseText = "";
                            if (userReply.Contains("自我介紹"))
                            {
                                reKind = catIns.自我介紹;
                            }
                            if (userReply.Contains("有什麼服務"))
                            {
                                reKind = catIns.有什麼服務;
                            }
                            if (userReply.Contains("想問治具"))
                            {
                                reKind = catIns.想問治具;
                            }
                            if (userReply.Contains("告訴我指令"))
                            {
                                reKind = catIns.指令;
                            }

                            switch (reKind)
                            {
                                case catIns.指令:
                                    foreach (string el in Enum.GetNames(typeof(catIns)))
                                    {
                                        responseText += el + "\r\n";
                                    }
                                    break;
                                case catIns.自我介紹:
                                    responseText = "您好，我是AI軟爛貓~目前在台林上班。\r\n很高興為您服務٩(◦`꒳´◦)۶";
                                    break;
                                case catIns.有什麼服務:
                                    //responseText = "我問我媽...抓頭";
                                    this.ReplyMessage(LineEvent.replyToken, new Uri("https://i.imgur.com/H9hnFox.png"));
                                    break;
                                case catIns.想問治具:
                                    responseText = "可以幫您查詢治具庫存相關資訊.";
                                    break;
                                default:
                                    if (userReply.Contains("軟爛貓好"))
                                    {
                                        responseText = "您好~";
                                    }
                                    else
                                    {
                                        responseText = "喵喵...喵喵喵喵喵~~";
                                    }
                                    break;
                            }
                            if (responseText == "")
                            {
                                
                            }
                            else
                            {
                                this.ReplyMessage(LineEvent.replyToken, responseText);
                            }
                        }
                        else
                        {
                            //if (ret.FirstOrDefault().score > 0)
                            //    responseText = ret.FirstOrDefault().answer;
                            ////回覆
                            //this.ReplyMessage(LineEvent.replyToken, responseText);
                        }

                    }
                    if (LineEvent.message.type == "sticker") //收到貼圖
                        this.ReplyMessage(LineEvent.replyToken, 1, 2);
                }
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤，傳訊息給Admin
                this.PushMessage(AdminUserId.Value, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
    }
}