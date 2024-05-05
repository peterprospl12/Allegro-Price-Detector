using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Allegro_Price_Detector.Models;

public class Auction
{
    public Auction(string link)
    {
        Link = link;
    }

    public string Link { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string IconUrl { get; set; }


    public async Task<dynamic> GetAuctionPriceAsync(Client client)
    {
        var url = "https://api.allegro.pl.allegrosandbox.pl/offers/listing";
        var pattern = @"\/oferta\/([^\/]+)$";
        var match = Regex.Match(Link, pattern);
        if (!match.Success)
        {
            Console.WriteLine("Couldn't find match");
            return null;
        }

        var extractedFragment = match.Groups[1].Value.Replace('-', ' ').Split();
        var itemId = extractedFragment[^1];
        var itemName = string.Join(" ", extractedFragment[..^1]);

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("Authorization", $"Bearer {client.getToken()}");
        request.Headers.Add("Accept", "application/vnd.allegro.public.v1+json");
        var parameters = $"phrase={itemName}&marketplaceId=allegro-pl";
        request.RequestUri = new Uri($"{request.RequestUri}?{parameters}");
        var response = await client.getHttpClient().SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonString);
            var json = await response.Content.ReadAsStringAsync();
            if (data.items != null)
            {
                foreach (var category in data.items)
                {
                    foreach (var item in category)
                    {
                        foreach (var it in item)
                        {
                            if (itemId == it["id"].ToString())
                            {
                                Name = it["name"];
                                Price = it["sellingMode"]["price"]["amount"];
                                IconUrl = it["images"][0]["url"].ToString();
                                Console.WriteLine(IconUrl);
                                return item;
                            }

                        }
                    }

                }
            }
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }

        return null;
    }
}
