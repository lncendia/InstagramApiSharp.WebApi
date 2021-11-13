using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes.ResponseWrappers;
using InstagramApiSharp.GetMediaLikers.Enums;
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
                        if (!result.Succeeded)
                        {
                            return result.Info.ResponseType == ResponseType.InternalException
                                ? Result.Fail<Classes.Likes.MediaInfo>(result.Info.Exception)
                                : Result.Fail<Classes.Likes.MediaInfo>(result.Info.Message);
                        }
                    }

                    data = JsonConvert.DeserializeObject<Classes.Likes.Response>(result.Value.Replace("\"id\"", "\"pk\""));


                    if (data is not { Status: "ok" })
                    {
                        return Result.Fail<Classes.Likes.MediaInfo>(data?.Message ?? "Failed to deserialize the object");
                    }
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
                        if (!result.Succeeded)
                        {
                            return result.Info.ResponseType == ResponseType.InternalException
                                ? Result.Fail<Classes.Comments.MediaInfo>(result.Info.Exception)
                                : Result.Fail<Classes.Comments.MediaInfo>(result.Info.Message);
                        }
                    }

                    data = JsonConvert.DeserializeObject<Classes.Comments.Response>(result.Value.Replace("\"id\"", "\"pk\""));


                    if (data is not { Status: "ok" })
                    {
                        return Result.Fail<Classes.Comments.MediaInfo>(data?.Message ?? "Failed to deserialize the object");
                    }
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

        public static async Task<IResult<InstaUserListShortResponse>> GetUserFriendshipsAsync(this IInstaApi api, string username,
            FriendshipStatus status, int countPerPage,
            PaginationParameters parameters, string query = "")
        {
            UserAuthValidator.Validate(api.GetLoggedUser(), api.IsUserAuthenticated);
            try
            {
                var user = await api.UserProcessor.GetUserAsync(username);
                if (!user.Succeeded) return Result.Fail(user.Info, default(InstaUserListShortResponse));
                if (user.Value.FriendshipStatus.IsPrivate && user.Value.UserName != api.GetLoggedUser().UserName &&
                    !user.Value.FriendshipStatus.Following)
                    return Result.Fail("You must be a follower of private accounts to be able to get user's followers",
                        default(InstaUserListShortResponse));

                return await api.GetUserFriendshipsByIdAsync(user.Value.Pk, status, countPerPage, parameters, query);
            }
            catch (Exception exception)
            {
                return Result.Fail(exception, default(InstaUserListShortResponse));
            }
        }

        public static async Task<IResult<InstaUserListShortResponse>> GetUserFriendshipsByIdAsync(this IInstaApi api, long pk,
            FriendshipStatus status, int countPerPage,
            PaginationParameters parameters, string query = "")
        {
            UserAuthValidator.Validate(api.GetLoggedUser(), api.IsUserAuthenticated);
            try
            {
                parameters ??= PaginationParameters.MaxPagesToLoad(1);

                string type = status switch
                {
                    FriendshipStatus.Followers => "followers",
                    FriendshipStatus.Following => "following",
                    _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
                };
                var list = new List<InstaUserShortResponse>();
                InstaUserListShortResponse data;
                do
                {
                    var uri = new Uri($"https://i.instagram.com/api/v1/friendships/{pk}/{type}/");
                    uri = uri.AddQueryParameterIfNotEmpty("count", countPerPage.ToString());
                    uri = uri.AddQueryParameterIfNotEmpty("query", query);
                    uri = uri.AddQueryParameterIfNotEmpty("max_id", parameters.NextMaxId);
                    var result = await api.SendGetRequestAsync(uri);
                    if (!result.Succeeded)
                    {
                        if (!result.Succeeded)
                        {
                            return result.Info.ResponseType == ResponseType.InternalException
                                ? Result.Fail<InstaUserListShortResponse>(result.Info.Exception)
                                : Result.Fail<InstaUserListShortResponse>(result.Info.Message);
                        }
                    }

                    data = JsonConvert.DeserializeObject<InstaUserListShortResponse>(result.Value);


                    if (data is not { Status: "ok" })
                    {
                        return Result.UnExpectedResponse<InstaUserListShortResponse>(new HttpResponseMessage(),
                            result.Value);
                    }

                    list.AddRange(data.Items);
                    parameters.PagesLoaded++;
                    parameters.NextMaxId =
                        data.NextMaxId?.Replace("\"", "\\\"");
                } while (!string.IsNullOrEmpty(parameters.NextMaxId)
                         && parameters.PagesLoaded < parameters.MaximumPagesToLoad);

                data.Items = list;
                return Result.Success(data);
            }
            catch (HttpRequestException httpException)
            {
                return Result.Fail(httpException, default(InstaUserListShortResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                return Result.Fail(exception, default(InstaUserListShortResponse));
            }
        }

        public static async Task<IResult<InstaSectionMediaListResponse>> GetReelsHashtagMediaListAsync(this IInstaApi api,
            string tag,
            PaginationParameters parameters)
        {
            UserAuthValidator.Validate(api.GetLoggedUser(), api.IsUserAuthenticated);
            // try
            // {
                parameters ??= PaginationParameters.MaxPagesToLoad(1);
                List<InstaSectionMediaResponse> list = new List<InstaSectionMediaResponse>();
                InstaSectionMediaListResponse data;
                do
                {
                    var uri = new Uri($"https://i.instagram.com/api/v1/tags/{tag}/sections/");
                    Dictionary<string, string> dictionary = new Dictionary<string, string>
                    {
                        { "include_persistent", "false" },
                        { "tab", "clips" }
                    };
                    var result = await api.SendPostRequestAsync(uri, dictionary);
                    if (!result.Succeeded)
                    {
                        return result.Info.ResponseType == ResponseType.InternalException
                            ? Result.Fail<InstaSectionMediaListResponse>(result.Info.Exception)
                            : Result.Fail<InstaSectionMediaListResponse>(result.Info.Message);
                    }

                    data = JsonConvert.DeserializeObject<InstaSectionMediaListResponse>(result.Value);
                    if (data is not { Status: "ok" })
                    {
                        return Result.UnExpectedResponse<InstaSectionMediaListResponse>(new HttpResponseMessage(),
                            result.Value);
                    }
                    list.AddRange(data.Sections);
                    parameters.PagesLoaded++;
                    parameters.NextMaxId = data.NextMaxId;
                } while (!string.IsNullOrEmpty(parameters.NextMaxId)
                         && parameters.PagesLoaded < parameters.MaximumPagesToLoad);

                data.Sections = list;
                var x = Type.GetType("InstagramApiSharp.Converters.ConvertersFabric, InstagramApiSharp", false, true)?.GetProperty("Instance")?.GetGetMethod()?.Invoke(null, null);
                var medias = x.GetType().GetMethod("GetHashtagMediaListConverter").Invoke(x, new[] { (object)data });
                InstaSectionMedia media = (InstaSectionMedia)medias.GetType().GetMethod("Convert").Invoke(medias, null);
                media.Medias.ForEach(a=>Console.WriteLine(a.Code));
                return Result.Success(data);
                // }
            // catch (HttpRequestException httpException)
            // {
            //     return Result.Fail(httpException, default(InstaSectionMediaListResponse), ResponseType.NetworkProblem);
            // }
            // catch (Exception exception)
            // {
            //     return Result.Fail(exception, default(InstaSectionMediaListResponse));
            // }
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