using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
    {

    static async Task Main(string[] args)
        {
        
        //авторизация wialon и получение данных
        using var Login = new HttpClient();
        var JsonForLogin = "svc=token/login&params={\"token\":" +
            "\"047321555f8e58d28d5be54e4240f6d8C5F86CEB81E8FC2FBE6BE338156C9FA658B0DC43\",\"fl\":\"1\"}";
        var Content = new StringContent(JsonForLogin, Encoding.UTF8, "application/x-www-form-urlencoded");
        var URL = "https://hst-api.wialon.com/wialon/ajax.html";  
        var response = await Login.PostAsync(URL, Content);
        string result = response.Content.ReadAsStringAsync().Result;

        //получение из ответа на запрос авторизации ID сессии
        var eid = JObject.Parse(result).SelectToken("eid");
        
        //получение ID объектов
        /*var GetIDofElements = "svc=core/search_items&params={\"spec\":{\"itemsType\":\"avl_unit\",\"propName\":\"sys_name\",\"propValueMask\":\"*\",\"sortType\":\"sys_name\"},\"force\":1,\"flags\":\"1\",\"from\":0,\"to\":0}&sid=" + eid;
        var Content1 = new StringContent(GetIDofElements, Encoding.UTF8, "application/x-www-form-urlencoded");
        var response1 = await Login.PostAsync(URL, Content1);
        string result1 = response1.Content.ReadAsStringAsync().Result;*/
       
        bool running = true;
        while (running)
           {
            //получение информации о машине
            var GetParamsofElement = "svc=core/search_item&params={\"id\":16508345," +
                "\"flags\":4194304}&sid=" + eid;
            var Content1 = new StringContent(GetParamsofElement, Encoding.UTF8, "application" +
                "/x-www-form-urlencoded");
            var response1 = await Login.PostAsync(URL, Content1);
            string InfoAboutCar = response1.Content.ReadAsStringAsync().Result;

            //запись координат в переменные
            var x = JObject.Parse(InfoAboutCar).SelectToken("item.pos.x")?.ToObject<string>();
            var y = JObject.Parse(InfoAboutCar).SelectToken("item.pos.y")?.ToObject<string>();



        StreamWriter sr = new StreamWriter("coord.js", false);
        sr.WriteLine("var x = " + x + ";");
        sr.WriteLine("var y = " + y + ";");
        sr.Close();

            //Console.WriteLine("x: " + result);
            //Console.WriteLine("y: " + y);

            //Console.ReadKey();
        }
    }

    }
