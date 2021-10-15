using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Helpers;
using Newtonsoft.Json;

namespace InstagramApiSharp.GetMediaLikers
{
    public static class GetMediaLikers
    {
        public static async Task<IResult<Classes.Likes.MediaInfo>> GetMediaLikesAsync(this IInstaApi api, string code,
            PaginationParameters parameters)
        {
            UserAuthValidator.Validate(api.GetLoggedUser(), api.IsUserAuthenticated);
            try
            {
                parameters ??= PaginationParameters.MaxPagesToLoad(1);
                var list = new List<Classes.Likes.Edge>();
                Classes.Likes.Response data;
                do
                {
                    var uri = new Uri("https://www.instagram.com/graphql/query");
                    uri = uri.AddQueryParameterIfNotEmpty("query_hash", "d5d763b1e2acf209d62d22d184488e57");
                    string variables = $"{{\"shortcode\":\"{code}\",\"first\":50";
                    if (!string.IsNullOrEmpty(parameters.NextMaxId))
                        variables += $",\"after\":\"{parameters.NextMaxId}\"}}";
                    else variables += "}";
                    uri = uri.AddQueryParameterIfNotEmpty("variables", variables);
                    var result = await api.SendGetRequestAsync(uri);
                    if (!result.Succeeded)
                    {
                        if (result.Info.ResponseType == ResponseType.UnExpectedResponse)
                        {
                            return Result.UnExpectedResponse<Classes.Likes.MediaInfo>(
                                new HttpResponseMessage(HttpStatusCode.OK), result.Info.Message, result.Value);
                        }

                        return Result.Fail<Classes.Likes.MediaInfo>(result.Info.Exception, default);
                    }

                    data = JsonConvert.DeserializeObject<Classes.Likes.Response>(result.Value);


                    if (data.Status == "fail")
                        return Result.UnExpectedResponse<Classes.Likes.MediaInfo>(
                            new HttpResponseMessage(HttpStatusCode.OK), data.Message, String.Empty);
                    if (data.Info.ShortcodeMediaInfo == null)
                        return Result.Fail<Classes.Likes.MediaInfo>("Media was not found.", ResponseType.MediaNotFound,
                            default);


                    list.AddRange(data.Info.ShortcodeMediaInfo.Likes.Edges);
                    parameters.PagesLoaded++;
                    parameters.NextMaxId = data.Info.ShortcodeMediaInfo.Likes.NextPageInfo.NextMaxId;
                } while (!string.IsNullOrEmpty(parameters.NextMaxId)
                         && parameters.PagesLoaded < parameters.MaximumPagesToLoad);

                data.Info.ShortcodeMediaInfo.Likes.Edges = list;
                return Result.Success(data.Info.ShortcodeMediaInfo);
            }
            catch (Exception exception)
            {
                return Result.Fail<Classes.Likes.MediaInfo>(exception);
            }
        }


        public static async Task<IResult<Classes.Comments.MediaInfo>> GetMediaCommentsAsync(this IInstaApi api,
            string code,
            PaginationParameters parameters)
        {
            UserAuthValidator.Validate(api.GetLoggedUser(), api.IsUserAuthenticated);
            try
            {
                parameters ??= PaginationParameters.MaxPagesToLoad(1);
                var list = new List<Classes.Comments.Edge>();
                Classes.Comments.Response data;
                do
                {
                    var uri = new Uri("https://www.instagram.com/graphql/query");
                    uri = uri.AddQueryParameterIfNotEmpty("query_hash", "bc3296d1ce80a24b1b6e40b1e72903f5");
                    string variables = $"{{\"shortcode\":\"{code}\",\"first\":50";
                    if (!string.IsNullOrEmpty(parameters.NextMaxId))
                        variables += $",\"after\":\"{parameters.NextMaxId}\"}}";
                    else variables += "}";
                    uri = uri.AddQueryParameterIfNotEmpty("variables", variables);
                    var result = await api.SendGetRequestAsync(uri);
                    if (!result.Succeeded)
                    {
                        if (result.Info.ResponseType == ResponseType.UnExpectedResponse)
                        {
                            return Result.UnExpectedResponse<Classes.Comments.MediaInfo>(
                                new HttpResponseMessage(HttpStatusCode.OK), result.Info.Message, result.Value);
                        }

                        return Result.Fail<Classes.Comments.MediaInfo>(result.Info.Exception, default);
                    }

                    data = JsonConvert.DeserializeObject<Classes.Comments.Response>(result.Value);


                    if (data.Status == "fail")
                        return Result.UnExpectedResponse<Classes.Comments.MediaInfo>(
                            new HttpResponseMessage(HttpStatusCode.OK), data.Message, String.Empty);
                    if (data.Info.ShortcodeMediaInfo == null)
                        return Result.Fail<Classes.Comments.MediaInfo>("Media was not found.",
                            ResponseType.MediaNotFound,
                            default);


                    list.AddRange(data.Info.ShortcodeMediaInfo.Comments.Edges);
                    parameters.PagesLoaded++;
                    parameters.NextMaxId =
                        data.Info.ShortcodeMediaInfo.Comments.NextPageInfo.NextMaxId?.Replace("\"", "\\\"");
                } while (!string.IsNullOrEmpty(parameters.NextMaxId)
                         && parameters.PagesLoaded < parameters.MaximumPagesToLoad);

                data.Info.ShortcodeMediaInfo.Comments.Edges = list;
                return Result.Success(data.Info.ShortcodeMediaInfo);
            }
            catch (Exception exception)
            {
                return Result.Fail<Classes.Comments.MediaInfo>(exception);
            }
        }

        public static async Task<IResult<InstaMediaList>> GetMediaByIdsAsync(this IInstaApi api, List<string> ids)
        {
            IResult<InstaMediaList> finalResult = null;
            for (int i = 0; i < ids.Count; i += 100)
            {
                var result = await api.MediaProcessor.GetMediaByIdsAsync(ids.Skip(i).Take(100).ToArray());
                if (!result.Succeeded) return result;
                if (finalResult != null)
                {
                    result.Value.AddRange(finalResult.Value);
                }

                finalResult = result;
            }

            return finalResult;
        }
    }
}